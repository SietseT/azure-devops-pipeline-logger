using CommandLine;

namespace AdoPipelines.Logger.Sample;

public class Options
{
    [Option('t', "type", Required = true, HelpText = "Log type to simulate: logs, group, issue, command or progress")]
    public string Type { get; set; } = null!;
}