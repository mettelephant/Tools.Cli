namespace Tools.Cli.Logging.Help;

public static class Help
{
    public static IReadOnlyDictionary<HelpExplanation, string> HelpMapping { get; } =
        new Dictionary<HelpExplanation, string>
        {
            [HelpExplanation.FileNotFound] = "https://wiki.com/FileNotFound"
        };
}
