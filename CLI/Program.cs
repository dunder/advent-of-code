// CLI for managing Advent of Code
//
// The -- (double dash) in the commands separates dotnet command switches from
// our switches
//
// Download examples
// ============================================================================
//
// Cookie must be stored in user environment variable AOC
//
// Download todays input:
//
//     dotnet run -- download
//
// Download input from 2024 day 1:
//
//     dotnet run -- download -e 2024 -d 1
//
// Generate event examples
// ============================================================================
//
// Generate this years problem files:
//
//     dotnet run -- generate
//
// Generate problem files for the event of year 2024:
//
//     dotnet run -- generate -e 2024

using CLI.Download;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config
    .AddCommand<DownloadCommand>("download")
    .WithDescription("Download the Advent of Code input");

    config
    .AddCommand<GenerateEventCommand>("generate")
    .WithDescription("Generate all .cs-files for an event");
});

return await app.RunAsync(args);

