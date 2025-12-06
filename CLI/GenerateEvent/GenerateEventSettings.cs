using Spectre.Console.Cli;
using System.ComponentModel;

namespace CLI.Download
{
    public class GenerateEventSettings : CommandSettings
    {
        [Description("The Event for which to generate new files (optional, defaults to today's year)")]
        [CommandOption("-e|--event")]
        public int Event { get; set; } = DateTime.Now.Year;
    }
}
