using System.Collections.Generic;
using System.Linq;
using CSLabs.Api.Config;
using CSLabs.Api.Models;
using CSLabs.Api.Models.Enums;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.ModuleModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api
{
    public static class Seeder
    {
        public static void Seed(DefaultContext context, AppSettings settings)
        {
            var anyModulesExist = context.Modules.Any();
            if (!anyModulesExist)
            {
                context.Modules.Add(new Module
                {
                    Description = "Test Pilot",
                    Name = "Sexton's Test Pilot",
                    Published = false,
                    SpecialCode = settings.ModuleSpecialCode,
                    Difficulty = 1,
                    ModuleTags = new List<ModuleTag>
                    {
                      new ModuleTag
                      {
                          ModuleId = 1,
                          TagId = 1,
                          Tag = new Tag
                          {
                              Id = 1,
                              Name = "Test"
                          }
                      }
                    },
                    Labs = new List<Lab>
                    {
                        new Lab
                        {
                            Name = "Test Pilot Lab",
                            EstimatedCpusUsed = 1,
                            EstimatedMemoryUsedMb = 4096,
                            Type = ELabType.Permanent,
                            LabDifficulty = 1,
                            LabVms = new List<LabVm>
                            {
                                new LabVm
                                {
                                    Name = "Test Pilot VM",
                                    VmTemplate = new VmTemplate
                                    {
                                        HypervisorVmTemplates = new List<HypervisorVmTemplate>
                                        {
                                            new HypervisorVmTemplate
                                            {
                                                HypervisorNodeId = 1,
                                                TemplateVmId = 109
                                            }
                                        }
                                    }
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
                var appSettings = scope.ServiceProvider.GetService<AppSettings>();
                Seed(context, appSettings);
            }
            return host;
        }
    }
}