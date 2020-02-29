using System.Linq;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models
{
    public static class QueryExtensions
    {
        public static IQueryable<Module> IncludeRelations(this IQueryable<Module> query)
        {
            return query
                .Include(m => m.Labs)
                .ThenInclude(l => l.LabVms)
                .ThenInclude(v => v.VmTemplate)
                .ThenInclude(v => v.HypervisorVmTemplates)
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
                .ThenInclude(l => l.BridgeTemplates)
                .Include(l => l.Lab)
                .ThenInclude(n => n.LabVms)
                .ThenInclude(l => l.VmTemplate)
                .ThenInclude(l => l.HypervisorVmTemplates)
                .ThenInclude(vt => vt.HypervisorNode)
                .ThenInclude(hn => hn.Hypervisor)
                .Include(l => l.Lab)
                .ThenInclude(n => n.LabVms)
                .ThenInclude(n => n.TemplateInterfaces);
        }
        
        public static IQueryable<UserLab> IncludeHypervisor(this IQueryable<UserLab> query)
        {
            return query
                .Include(vt => vt.HypervisorNode)
                .ThenInclude(hn => hn.Hypervisor);
        }

        public static IQueryable<UserLabVm> WhereIncludesUser(this IQueryable<UserLabVm> query, User user)
        {
            return query.Where(ulv => ulv.UserLab.UserModule.UserUserModules.Any(uum => uum.UserId == user.Id));
        }
        
        public static IQueryable<UserLab> WhereIncludesUser(this IQueryable<UserLab> query, User user)
        {
            return query.Where(ul => ul.UserModule.UserUserModules.Any(uum => uum.UserId == user.Id));
        }
        
        public static IQueryable<UserModule> WhereIncludesUser(this IQueryable<UserModule> query, User user)
        {
            return query.Where(um => um.UserUserModules.Any(uum => uum.UserId == user.Id));
        }
    }
}