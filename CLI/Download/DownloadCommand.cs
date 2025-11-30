using Spectre.Console;
using Spectre.Console.Cli;
using System.Net;

namespace CLI.Download
{
    public class DownloadCommand : AsyncCommand<DownloadSettings>
    {
        private const string AdventOfCodeUri = "https://adventofcode.com";

        public override async Task<int> ExecuteAsync(CommandContext context, DownloadSettings settings, CancellationToken cancellationToken)
        {
            var cookieValue = Environment.GetEnvironmentVariable("AOC", EnvironmentVariableTarget.User);

            if (cookieValue is null)
            {
                Console.WriteLine($"Cookie not found");

                return Exit.FromErrorMode(ErrorMode.CookieNotFound);
            }

            Console.WriteLine("Cookie loaded");

            var uri = new Uri(settings.Url ?? AdventOfCodeUri);
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer()
            };
            handler.CookieContainer.Add(new Uri($"{uri.Scheme}://{uri.Host}"), new Cookie("session", cookieValue));

            using var client = new HttpClient(handler)
            {
                BaseAddress = uri,
            };

            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            string endpoint = $"{settings.Event}/day/{settings.Day}/input";

            Console.WriteLine($"Downloading {uri}{endpoint}...");
            var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var normalizedContent = content.TrimEnd();

                string targetFile = Path.Combine("..", "Solutions", $"Event{settings.Event}", "Input", $"Day{settings.Day:D2}.txt");

                await File.WriteAllTextAsync(targetFile, normalizedContent);

                AnsiConsole.MarkupLine($"[green]Success:[/] Input downloaded to: {targetFile}");

                return Exit.Success;
            }
            else
            {
                string message = $"{(int)response.StatusCode} ({response.ReasonPhrase})";
                AnsiConsole.MarkupLine($"[red]Error:[/] Download failed with status code: {message}");

                return Exit.FromHttpStatus(response.StatusCode);
            }
        }
    }
}
