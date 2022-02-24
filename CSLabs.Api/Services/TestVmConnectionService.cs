using System;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Services
{
    public class TestVmConnectionService
    {
        private DefaultContext Context { get; }

        private ProxmoxManager ProxmoxManager;
        
        public TestVmConnectionService(DefaultContext context, ProxmoxManager proxmoxManager)
        {
            Context = context;
            ProxmoxManager = proxmoxManager;
        }
        
        // Recursive helper function
        public async void AttemptStart(int attempt, int labId, ProxmoxApi api)
        {
            try
            {
                if (attempt != 3) // first or second attempt
                {
                    await api.StartVM(labId);
                }
                else // third attempt
                {
                    await api.StopVM(labId);
                    await api.StartVM(labId);
                }
                
            }
            catch (ProxmoxRequestException)
            {
                if (attempt != 3)
                {
                    AttemptStart(attempt + 1, labId, api);
                }
                else
                {
                    Console.WriteLine("Uh-Oh");
                    // TODO
                    // third attempt at restarting has failed. Something really bad has happened
                    // and the maintainers need to be emailed
                }
            }
        }
        
        public async Task<IActionResult> TestLabVmConnection()
        {
            try {
                Console.WriteLine("In TestLabVMConnection()");
                var hypervisors = await Context.Hypervisors
                    .Include(h => h.HypervisorNodes)
                    .ToListAsync();
                Console.WriteLine("Got hypervisors context");
                var labVms = await Context.LabVms
                    .ToListAsync();
                Console.WriteLine("Got labVms context");

                var userLabVms = await Context.UserLabVms
                .ToListAsync();
                Console.WriteLine("Got all the database info");
                foreach (var hypervisor in hypervisors)
                {
                    var node = hypervisor.HypervisorNodes.First();
                    var api = ProxmoxManager.GetProxmoxDBApi(node);
                    Console.WriteLine("Got the API");
                
                    foreach (var labVm in labVms) 
                    { 
                        Console.WriteLine("Test2");
                        try {
                        
                            //value stored in database
                            var userLab = userLabVms.Find(x => x.LabVmId.Equals(labVm.Id));
                            var userLabStatus = userLab.Running;
                            Console.WriteLine("Got VM status in DB");
                        
                            //actual status of VM
                            var vmStatus = await api.GetVmStatus(labVm.Id); 
                            Console.WriteLine("Got VM status");

                            // check if actual status is opposite what is stored in the database
                            if (vmStatus.IsStopped() != userLabStatus)  
                            {
                                Console.WriteLine("VM status is not what is stored in DB, attempting to fix...");
                                AttemptStart(1, labVm.Id, api);
                            }
                        }
                        //catch (ProxmoxRequestException)
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            // something went wrong getting the vm status
                        }

                    }
                
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            return new OkObjectResult("All LabVMs are up and responding");
        }
        
    }
}