using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CSLabs.Api.Config;
using CSLabs.Api.Services;
using CSLabs.ConsoleUtil.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.ConsoleUtil
{
    class Program
    {

        public static List<Type> Commands = new List<Type>
        {
            typeof(AddHypervisorCommand),
            typeof(AddHypervisorNodeCommand),
            typeof(ChangeHypervisorPasswordCommand),
            typeof(ChangeUserPasswordCommand),
            typeof(ListHypervisorsCommand),
            typeof(EncryptCommand),
            typeof(DecryptCommand)
        };
        
        
        private static void ConfigureServices(ServiceCollection services, AppSettings appSettings)
        {
            services.ConfigureDatabase(appSettings.ConnectionStrings.DefaultConnection);
            services.AddSingleton(appSettings);
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
        
        static async Task Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            var mainProjectDir = $"../{typeof(Api.Program).Namespace}";
            if (Directory.Exists(mainProjectDir))
            {
                dir = Path.GetFullPath(mainProjectDir);
            }
            var builder = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile("appsettings.json", false, true);
            IConfiguration configuration = builder.Build();
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            var services = new ServiceCollection();
            ConfigureServices(services, appSettings);
            await ExecuteCommands(Commands, services, args);
        }
        
        private static async Task ExecuteCommands(List<Type> commands, IServiceCollection collection, string[] args)
        {
            foreach (var command in commands)
                collection.AddSingleton(command);

            var provider = collection.BuildServiceProvider();
            var optionsWithCommand = commands.Select(command => (command.BaseType.GetGenericArguments()[0], command)).ToArray();
            var options = optionsWithCommand.Select(oCommand =>
                {
                    var (o, _) = oCommand;
                    return o;
                })
                .ToArray();
            var parsedResult = Parser.Default.ParseArguments(args, options.ToArray());
            if (parsedResult is Parsed<object> parsed)
            {
                foreach (var optionWithCommand in optionsWithCommand)
                {
                    var (option, command) = optionWithCommand;

                    if (parsed.Value.GetType() == option)
                    {
                        var instance = provider.GetService(command);
                        var returnVal = command.GetMethod("Run").Invoke(instance, new[] {parsed.Value});
                        if (returnVal is Task task)
                        {
                            await task;
                        }
                    }
                }
            }
        }
    }
}