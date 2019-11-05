using System.Collections.Generic;
using CSLabsBackend.Models;
using CSLabsBackend.Models.Enums;
using CSLabsBackend.Models.ModuleModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabsBackend
{
    public static class Seeder
    {
        public static void Seed(DefaultContext context)
        {
            if (!context.Modules.Any())
            {
                context.Modules.Add(new Module()
                {
                    Description = "Test Pilot",
                    Name = "Dr. Sexton's Test Pilot",
                    Published = false,
                    Labs = new List<Lab>()
                    {
                        new Lab()
                        {
                            Name = "Test Pilot Lab",
                            EstimatedCpusUsed = 1,
                            EstimatedMemoryUsedMb = 4096,
                            LabType = LabTypes.Permanent,
                            LabDifficulty = 1,
                            LabVms = new List<LabVm>()
                            {
                                new LabVm()
                                {
                                    Name = "Test Pilot VM",
                                    TemplateProxmoxVmId = 100
                                }
                            }
                        }
                    }
                });
                context.SaveChanges();
            }
        }
        
        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DefaultContext>();
                Seed(context);
            }
            return host;
        }
    }
}