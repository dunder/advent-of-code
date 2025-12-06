using Spectre.Console;
using Spectre.Console.Cli;
using System.Net;

namespace CLI.Download
{
    public class GenerateEventCommand : AsyncCommand<GenerateEventSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, GenerateEventSettings settings, CancellationToken cancellationToken)
        {
            // https://adventofcode.com/2025/about
            int days = settings.Event < 2025 ? 25 : 12;

            foreach (var day in Enumerable.Range(1, days))
            {
                string problemFile = Path.Combine("..", "Solutions", $"Event{settings.Event}", $"Day{day:D2}.cs");

                if (!File.Exists(problemFile))
                {
                    AnsiConsole.MarkupLine($"[yellow]File already exists[/]:{problemFile}");
                }
                else
                {
                    await File.WriteAllTextAsync(problemFile, RenderClass(settings.Event, day));

                    AnsiConsole.MarkupLine($"[green]Created[/]:{problemFile}");
                }
            }

            return Exit.Success;
        }

        private static string RenderClass(int @event, int day)
        {
            string dayString = day.ToString().PadLeft(2, '0');

            return $$"""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using Xunit;
            using Xunit.Abstractions;
            using static Solutions.InputReader;


            namespace Solutions.Event2025
            {
                // --- Day X: Phrase ---
                public class Day{{dayString}}
                {
                    private readonly ITestOutputHelper output;

                    public Day{{dayString}}(ITestOutputHelper output)
                    {
                        this.output = output;
                    }

                    private static int Problem1(IList<string> input)
                    {
                        return 0;
                    }

                    private static int Problem2(IList<string> input)
                    {
                        return 0;
                    }

                    [Fact]
                    [Trait("Event", "{{@event}}")]
                    public void FirstStarTest()
                    {
                        var input = ReadLineInput();

                        Assert.Equal(-1, Problem1(input));
                    }

                    [Fact]
                    [Trait("Event", "{{@event}}")]
                    public void SecondStarTest()
                    {
                        var input = ReadLineInput();

                        Assert.Equal(-1, Problem2(input));
                    }

                    [Fact]
                    [Trait("Example", "{{@event}}")]
                    public void FirstStarExample()
                    {
                        var exampleInput = ReadExampleLineInput("Example");

                        Assert.Equal(-1, Problem1(exampleInput));
                    }

                    [Fact]
                    [Trait("Example", "{{@event}}")]
                    public void SecondStarExample()
                    {
                        var exampleInput = ReadExampleLineInput("Example");

                        Assert.Equal(-1, Problem2(exampleInput));
                    }
                }
            }
            """;
        }
    }
}
