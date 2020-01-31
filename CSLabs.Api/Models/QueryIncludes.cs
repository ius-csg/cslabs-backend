using System.Linq;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models
{
    public static class QueryIncludes
    {
        public static IQueryable<Module> IncludeRelations(this IQueryable<Module> query)
        {
            return query
                .Include(m => m.Labs)
                .ThenInclude(l => l.LabVms)
                .ThenInclude(v => v.VmTemplates)
                .ThenInclude(t => t.HypervisorNode)
                .ThenInclude(n => n.Hypervisor);
        }
        
        public static IQueryable<UserLab> IncludeRelations(this IQueryable<UserLab> query)
        {
            return query
                .Include(l => l.HypervisorNode)
                .ThenInclude(n => n.Hypervisor)
                .Include(l => l.UserLabVms);
        }
        
        public static IQueryable<UserLab> IncludeLabHypervisor(this IQueryable<UserLab> query)
        {
            return query
                .Include(l => l.Lab)
                .ThenInclude(n => n.LabVms)
                .ThenInclude(l => l.VmTemplates)
                .ThenInclude(vt => vt.HypervisorNode)
                .ThenInclude(hn => hn.Hypervisor);
        }
        
        public static IQueryable<UserLab> IncludeHypervisor(this IQueryable<UserLab> query)
        {
            return query
                .Include(vt => vt.HypervisorNode)
                .ThenInclude(hn => hn.Hypervisor);
        }
    }
}