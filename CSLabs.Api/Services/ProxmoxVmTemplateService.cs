using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using CSLabs.Api.Models;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Proxmox;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using ConnectionInfo = Renci.SshNet.ConnectionInfo;

namespace CSLabs.Api.Services
{
    public class ProxmoxVmTemplateService
    {
        public class OVFConfig
        {
            public int DiskSizeGb { get; set; }
            public string Name { get; set; } 
            public int MemorySizeMb { get; set; }
        }
        public ProxmoxManager ProxmoxManager { get;}
        
        public ProxmoxVmTemplateService(ProxmoxManager manager)
        {
            ProxmoxManager = manager;
        }
        
        public async Task UploadTemplate(DefaultContext context, string name, User user, Stream stream, long length,  Action<double> callback = null)
        {
            await context.Database.BeginTransactionAsync();
            var hypervisor = await context.Hypervisors.FirstOrDefaultAsync();
            context.Entry(user).State = EntityState.Unchanged;
            var vmTemplate = new VmTemplate
            {
                Name = name,
                Owner = user,
                IsCoreRouter = false,
            };
            context.Add(vmTemplate);
            await context.SaveChangesAsync();
            var templateId = await UploadVmTemplate(
                name, stream, length, hypervisor, vmTemplate, callback);
            var primaryHypervisorNode = await ProxmoxManager.GetPrimaryHypervisorNode(hypervisor);
            vmTemplate.HypervisorVmTemplates = new List<HypervisorVmTemplate>
            {
                new HypervisorVmTemplate
                {
                    HypervisorNode = primaryHypervisorNode,
                    TemplateVmId = templateId
                }
            };
            await context.SaveChangesAsync();
            context.Database.CommitTransaction();
        }
        
        public async Task<int> UploadVmTemplate(
            string name, 
            Stream fileStream, 
            long length,
            Hypervisor hypervisor, 
            VmTemplate vmTemplate, 
            Action<double> callback = null)
        {
            var safeFileName = vmTemplate.Id + ".ova";
            const string baseDir = "/root/uploads";
            var dirName  = "VmTemplate_" + vmTemplate.Id;
            var dirPath  = string.Join("/", baseDir, dirName);
            var filePath = string.Join("/", dirPath, safeFileName);
            var node = await ProxmoxManager.GetPrimaryHypervisorNode(hypervisor);
            var api = ProxmoxManager.GetProxmoxApi(node);
            int vmId;
            // Upload File
            using (var sftp = new SftpClient(GetConnectionInfoFromHypervisor(hypervisor)))
            {
                sftp.Connect();
                try
                {
                    sftp.CreateDirectory(dirPath);
                    sftp.ChangeDirectory(dirPath);
                    sftp.UploadFile(fileStream, filePath, true, progress =>
                    {
                        if (callback != null)
                        {
                            callback((double) progress / length * 100);
                        }
                    });
                    using (var ssh = new SshClient(GetConnectionInfoFromHypervisor(hypervisor)))
                    {
                        ssh.Connect();
                        await ExtractOva(ssh, filePath, dirPath);
                        vmId = await CreateVmAndImportDisk(name, ssh, sftp, api, dirPath);
                        var unusedDisk = await api.GetVmUnusedDisk(vmId);
                        await api.SetVmScsi0(vmId, unusedDisk);
                        await api.ConvertVmToTemplate(vmId);
                        ssh.Disconnect();
                    }
                    Cleanup(sftp, dirPath);
                }
                catch (Exception)
                {
                    Cleanup(sftp, dirPath);
                    throw;
                }
                sftp.Disconnect();
            }

            return vmId;
        }

        public async Task TestConnect(Hypervisor hypervisor)
        {
            using (var ssh = new SshClient(GetConnectionInfoFromHypervisor(hypervisor)))
            {
                ssh.Connect();
                ssh.Disconnect();
            }

        }

        private void Cleanup(SftpClient sftp, string path)
        {
            var files = sftp.ListDirectory(path);
            foreach (var file in files)
            {
                if (!file.IsDirectory)
                {
                    sftp.DeleteFile(file.FullName);
                }
            }
            sftp.DeleteDirectory(path);
        }

        public ConnectionInfo GetConnectionInfoFromHypervisor(Hypervisor hypervisor)
        {
            var host = hypervisor.Host;
            var username = hypervisor.UserName;
            var password = ProxmoxManager.GetProxmoxPassword(hypervisor);
            return new ConnectionInfo(host, username, new PasswordAuthenticationMethod(username, password));
        }

        public async Task ExtractOva(SshClient ssh, string filePath, string dirPath)
        {
            var result = ssh.RunCommand($"tar -oxf {filePath} -C {dirPath}");
            Console.WriteLine("Extract Error: " + result.Error);
            if (result.Error != "")
            {
                throw new Exception("Extraction process failed! Error: " + result.Error);
            }
        }
        public async Task<int> CreateVmAndImportDisk(string name, SshClient ssh, SftpClient sftp, ProxmoxApi api, string dirPath)
        {
            var files = sftp.ListDirectory(dirPath).ToList();
            SftpFile vmdk = files.First(f => f.GetExtension() == "vmdk");
            SftpFile ovfFile = files.First(f => f.GetExtension() == "ovf");
            Debug.Assert(vmdk != null, "vmdk != null");
            Debug.Assert(ovfFile != null, "ovfFile != null");
            await using var stream = new MemoryStream();
            sftp.DownloadFile(ovfFile.FullName, stream);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            var ovf = ParseOvf(await reader.ReadToEndAsync());
            
            var vmId = await api.CreateVm(name.ToSafeId(), ovf.MemorySizeMb);
            var result = ssh.RunCommand($"qm importdisk {vmId} {vmdk.FullName} nasapp -format qcow2");
            if (result.Error != "")
            {
                throw new Exception("Failed to import disk! Error: " + result.Error);
            }

            return vmId;
        }
        
       
        public static OVFConfig ParseOvf(string data) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);  
            var root = doc.DocumentElement;
            var attributeCount = root.Attributes.Count;
            for (int i = 0; i < attributeCount; i++)
            {
                var attribute = root.Attributes.Item(i);
                if (attribute.Name.Contains("xmlns:"))
                {
                    var ns = attribute.Name.Substring(attribute.Name.IndexOf(":") + 1);
                    nsmgr.AddNamespace(ns,  attribute.Value);  
                }
            }

            var vm = doc.DocumentElement.ChildNodes.ToIEnumerable().First(node => node.Name == "VirtualSystem");
            var name = vm.Attributes["ovf:id"].Value;
            var memoryStr = vm.ChildNodes.ToIEnumerable()
                .First(node => node.Name == "VirtualHardwareSection").ChildNodes.ToIEnumerable()
                .Where(item => item.Name == "Item")
                .First(item => item.ChildNodes.ToIEnumerable().First(child => child.Name == "rasd:ResourceType").InnerText == "4")
                .ChildNodes
                .ToIEnumerable()
                .First(node => node.Name == "rasd:VirtualQuantity")
                .InnerText;
            var memorySizeMb = int.Parse(memoryStr);
            var diskStr = doc.DocumentElement.ChildNodes.ToIEnumerable()
                .First(node => node.Name == "DiskSection").ChildNodes.ToIEnumerable()
                .First(node => node.Name == "Disk")
                .Attributes["ovf:capacity"].Value;
            var diskSizeGb = (int)(long.Parse(diskStr) / Math.Pow(1024, 3));
            return new OVFConfig
            {
                Name = name,
                DiskSizeGb = diskSizeGb,
                MemorySizeMb = memorySizeMb
            };
        }
    }
    
}