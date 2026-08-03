using System.CommandLine;

namespace Indexer.NET;

public readonly record struct Options(
    string[] Includes,
    string[] Excludes,
    SortMethod Sort,
    bool SortReverse,
    bool Recursive,
    string Root)
{
    public static Options Parse(string[] args)
    {
        var includesOption = new Option<string[]>("--filter", "-f")
        {
            DefaultValueFactory = (_) => ["**"]
        };
        var excludesOption = new Option<string[]>("--ignore", "-i");
        var sortMethodOption = new Option<SortMethod>("--sort", "-s")
        {
            DefaultValueFactory = (_) => SortMethod.Name
        };
        var sortReverseOption = new Option<bool>("--sort-reverse", "-sr");
        var recursiveOption = new Option<bool>("--recursive", "-r");
        var rootArgument = new Argument<string>("root")
        {
            DefaultValueFactory = (_) => Directory.GetCurrentDirectory()
        };

        var rootCommand = new RootCommand();
        rootCommand.Options.Add(includesOption);
        rootCommand.Options.Add(excludesOption);
        rootCommand.Options.Add(sortMethodOption);
        rootCommand.Options.Add(sortReverseOption);
        rootCommand.Options.Add(recursiveOption);
        rootCommand.Arguments.Add(rootArgument);

        var parseResult = rootCommand.Parse(args);
        if(parseResult.Errors.Count > 0)
        {
            foreach(var error in parseResult.Errors) Console.Error.WriteLine(error.Message);
            Environment.Exit(1);
        }

        return new(
            parseResult.GetRequiredValue(includesOption),
            parseResult.GetValue(excludesOption) ?? [],
            parseResult.GetRequiredValue(sortMethodOption),
            parseResult.GetValue(sortReverseOption),
            parseResult.GetValue(recursiveOption),
            parseResult.GetRequiredValue(rootArgument)
        );
    }
}
