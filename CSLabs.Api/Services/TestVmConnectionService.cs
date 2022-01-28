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
                    // TODO
                    // third attempt at restarting has failed. Something really bad has happened
                    // and the maintainers need to be emailed
                }
            }
        }
        
        public async Task<IActionResult> TestLabVmConnection()
        {
            var hypervisors = await Context.Hypervisors
                .Include(h => h.HypervisorNodes)
                .ToListAsync();
        
            var labVms = await Context.LabVms
                .Include(v => v.Id)
                .ToListAsync();

            var userLabVms = await Context.UserLabVms
                .Include(v => v.Id)
                .ToListAsync();

            foreach (var hypervisor in hypervisors) 
            {
                var api = ProxmoxManager.GetProxmoxDBApi(hypervisor.HypervisorNodes.First());
                
                foreach (var labVm in labVms) 
                { 
                    try 
                    { 
                        //actual status of VM
                        var vmStatus = await api.GetVmStatus(labVm.Id); 
                        
                        //value stored in database
                        var userLab = userLabVms.Find(x => x.LabVmId.Equals(labVm.Id));
                        var userLabStatus = userLab.Running;

                        // check if actual status is opposite what is stored in the database
                        if (vmStatus.IsStopped() != userLabStatus)  
                        {
                            AttemptStart(1, labVm.Id, api);
                        }
                    }
                    catch (ProxmoxRequestException) 
                    {
                        // something went wrong getting the vm status
                    }

                }
            }
            
            return new OkObjectResult("All LabVMs are up and responding");
        }
        
    }
}