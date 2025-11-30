// exemples
// Download todays input: dotnet run -- download
// Download input from 2024 day 1: dotnet run -- download -e 2024 -d 1
// Cookie must be stored in user environment variable AOC
// The -- part separates dotnet command switches from our switches


using CLI.Download;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<DownloadCommand>("download")
          .WithDescription("Download the Advent of Code input");
});

return await app.RunAsync(args);

