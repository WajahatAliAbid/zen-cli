﻿using System.Threading.Tasks;
using Spectre.Console.Cli;
using Zen.SpectreConsole.Extensions;

namespace Zen.CLI
{
    class Program
    {
        public static async Task<int> Main(string[] args) =>
            await SpectreConsoleHost
                .WithStartup<Startup>(args)
                .UseConfigurator<Startup>()
                .RunAsync(args);
    }
}
