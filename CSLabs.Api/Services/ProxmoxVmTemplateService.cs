using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Proxmox;
using CSLabs.Api.Util;
using Microsoft.AspNetCore.Http;
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
        
        public async Task<int> UploadVmTemplate(IFormFile ovaFile, Hypervisor hypervisor, VmTemplate vmTemplate)
        {
            var safeFileName = vmTemplate.Id + ".ova";
            var baseDir = "/root/uploads";
            var dirName = "VmTemplate_" + vmTemplate.Id;
            var dirPath = string.Join("/", baseDir, dirName);
            var filePath =string.Join("/", dirPath, safeFileName);
            var node = await ProxmoxManager.GetPrimaryHypervisorNode(hypervisor);
            var api = ProxmoxManager.GetProxmoxApi(node);
            int vmId;
            // Upload File
            using (var sftp = new SftpClient(GetConnectionInfoFromHypervisor(hypervisor)))
            {
                sftp.Connect();
                sftp.CreateDirectory(dirPath);
                sftp.ChangeDirectory(dirPath);
                using (var uploadFileStream =  ovaFile.OpenReadStream()){
                    sftp.UploadFile(uploadFileStream, filePath, true);
                }
                
                using (var ssh = new SshClient(GetConnectionInfoFromHypervisor(hypervisor)))
                {
                    ssh.Connect();
                    await ExtractOva(ssh, filePath, dirPath);
                    vmId = await CreateVmAndImportDisk(ssh, sftp, api, dirPath);
                    var unusedDisk = await api.GetVmUnusedDisk(vmId);
                    await api.AddDiskToVm(vmId, unusedDisk);
                    await api.ConvertVmToTemplate(vmId);
                    ssh.Disconnect();
                }

                Cleanup(sftp, dirPath);
                sftp.Disconnect();
            }

            return vmId;
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
        public async Task<int> CreateVmAndImportDisk(SshClient ssh, SftpClient sftp, ProxmoxApi api, string dirPath)
        {
            var files = sftp.ListDirectory(dirPath);
            SftpFile vmdk = null;
            SftpFile ovfFile = null;
            foreach (var sftpFile in files)
            {
                var extension = sftpFile.GetExtension();
                switch (extension)
                {
                    case "vmdk": vmdk = sftpFile; break;
                    case "ovf": ovfFile = sftpFile; break;
                }
            }
            Stream stream = new MemoryStream();
            sftp.DownloadFile(ovfFile.FullName, stream);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            var ovf = ParseOvf(await reader.ReadToEndAsync());
            
            var vmId = await api.CreateVm(ovf.Name, ovf.MemorySizeMb);
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