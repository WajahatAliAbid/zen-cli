using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using TextCopy;
using Zen.Cli.Commands;
using Zen.Cli.Commands.Git;
using Zen.Cli.Commands.Information;
using Zen.Cli.Commands.Misc;
using Zen.Core.Serializers;
using Zen.Core.Services;
using Zen.Core.Services.Anime;
using Zen.Core.SpectreConsole;

namespace Zen.Cli
{
    public delegate void CommandGroup(string name, IConfigurator<CommandSettings> configurator);
    public class Startup : BaseStartup, ISpectreConfiguration
    {
        public void ConfigureCommandApp(in IConfigurator configurator)
        {
            configurator.SetApplicationName("zen");
            configurator.CaseSensitivity(CaseSensitivity.None);
            configurator.AddCommand<MainCommand>("logo")
                .WithDescription("Displays cli logo");
            configurator.AddCommand<GenerateGuidCommand>("uuidgen")
                    .WithDescription("Generates Guid and copies to clipboard")
                    .WithAliases("guid", "uuid", "guidgen")
                    .WithExample("uuidgen")
                    .WithExample("guidgen")
                    .WithExample("guid")
                    .WithExample("uuid");
            configurator.AddBranch("git", options =>
            {
                options.SetDescription("Commands for working with git repositories");
                options.AddCommand<GitSearchCommand>("search")
                    .WithDescription("Finds git repositories in a directory")
                    .WithExample("git", "search", "~/projects/github");
            });
            configurator.AddBranch("getinfo", options =>
            {
                options.SetDescription("Gets information about various things");
                options.AddCommand<GetIPCommand>("ip")
                    .WithDescription("Gets public ip of the system")
                    .WithAliases("myip", "public-ip")
                    .WithExample("getinfo", "ip")
                    .WithExample("getinfo", "myip")
                    .WithExample("getinfo", "public-ip");
                options.AddCommand<GetNetworkInterfacesCommand>("nic")
                    .WithDescription("Gets list of network interfaces")
                    .WithAlias("network-interfaces")
                    .WithExample("getinfo", "net-interfaces")
                    .WithExample("getinfo", "nic");
                options.AddCommand<EndOfLifeCommand>("eol")
                    .WithDescription("Gets information about end of life for a tool or product")
                    .WithAliases("end-of-life", "endoflife")
                    .WithExample("eol")
                    .WithExample("eol", "--prefix", "dotnet");
            });

            configurator.AddCommand<GitIgnoreCommand>("gitignore")
                .WithDescription("Utility for gitignore.io")
                .WithAlias("giio")
                .WithExample("gitignore")
                .WithExample("giio")
                .WithExample("giio", "-q", "visual")
                .WithExample("giio", "--query", "visual", "--destination", "/home/user/projects/my-app/")
                .WithExample("giio", "--query", "visual");

            configurator.AddBranch("misc", options =>
            {
                options.SetDescription("Miscalaneous commands");
                options.AddCommand<GenerateMD5Command>("md5")
                    .WithDescription("Generates MD5 hash value")
                    .WithExample("misc", "md5", "\"Hello World\"");
                options.AddCommand<AnimeQuoteCommand>("anime-quote")
                    .WithDescription("Displays a random anime quote")
                    .IsHidden()
                    .WithExample("misc", "anime-quote")
                    .WithExample("misc", "anime-quote", "-q", "naruto, one piece");

                options.AddCommand<GenerateInsultCommand>("insult")
                    .WithDescription("Displays a random insult")
                    .WithAliases("generate-insult", "insult-me")
                    .IsHidden()
                    .WithExample("misc", "insult")
                    .WithExample("misc", "generate-insult");
                options.AddCommand<GenerateAdviseCommand>("advise")
                    .WithDescription("Displays a random advise")
                    .WithAliases("generate-advise", "advise-me")
                    .IsHidden()
                    .WithExample("misc", "advise")
                    .WithExample("misc", "generate-advise");
            });
        }

        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.InjectClipboard();
            FlurlHttp.Configure(setting =>
            {
                setting.JsonSerializer = new SystemTextJsonSerialzier();
            });
            services.AddSingleton<IGitIgnoreService, GitIgnoreService>();
            services.AddSingleton<IAnimeChanService, AnimeChanService>();
            services.AddSingleton<MiscApiService>();
        }
    }
}