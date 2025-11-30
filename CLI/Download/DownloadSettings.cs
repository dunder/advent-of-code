using Spectre.Console.Cli;
using System.ComponentModel;

namespace CLI.Download
{
    public class DownloadSettings : CommandSettings
    {
        [Description("The URL for the Advent of Code web site")]
        [CommandArgument(0, "[url]")]
        public string? Url { get; set; }

        [Description("The Event for which the input should be downloaded (optional, defaults to today's year)")]
        [CommandOption("-e|--event")]
        public int Event { get; set; } = DateTime.Now.Year;

        [Description("The day for which the input should be downloaded (optional, defaults to today's date")]
        [CommandOption("-d|--day")]
        public int Day { get; set; } = DateTime.Now.Day;
    }
}
