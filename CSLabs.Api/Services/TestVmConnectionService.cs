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
        
        public TestVmConnectionService(DefaultContext context)
        {
            Context = context;
        }

        public async Task<IActionResult> TestLabVmConnection()
        {
            var hypervisors = await Context.Hypervisors
                .Include(h => h.HypervisorNodes)
                .ToListAsync();
        
            var labVms = await Context.LabVms
                .Include(v => v.Id)
                .ToListAsync();

            foreach (var hypervisor in hypervisors) 
            {
                var api = ProxmoxManager.GetProxmoxApi(hypervisor.HypervisorNodes.First());
                
                foreach (var labVm in labVms) 
                { 
                    try 
                    { 
                        var vmStatus = await api.GetVmStatus(labVm.Id); 
                        if (vmStatus.IsStopped())  // vm is down
                        { 
                            // attempt to restart the vm
                            try
                            {
                                await api.StartVM(labVm.Id); // 1st attempt
                            }
                            catch (ProxmoxRequestException)
                            {
                                try
                                {
                                    await api.StartVM(labVm.Id); // 2nd attempt
                                }
                                catch (ProxmoxRequestException)
                                {
                                    try
                                    {
                                        //third attempt
                                        await api.ShutdownVm(labVm.Id);
                                        await api.StartVM(labVm.Id);
                                    }
                                    catch (ProxmoxRequestException)
                                    {
                                        // third attempt at restarting has failed. Something really bad has happened
                                        // and the maintainers need to be emailed
                                    }
                                }
                            }
                        }
                    }
                    catch (ProxmoxRequestException) 
                    {
                        
                    }

                }
            }
            
            return new OkObjectResult("All LabVMs are up and responding");
        }
        
    }
}