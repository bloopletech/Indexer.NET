using System.CommandLine;
using System.CommandLine.Parsing;

namespace Indexer.NET;

public readonly record struct Options(
    string[] Includes,
    string[] Excludes,
    DirectorySort Sort,
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
        var sortMethodOption = new Option<string>("--sort", "-s")
        {
            Required = false,
            DefaultValueFactory = (_) => "name"
        };
        sortMethodOption.Validators.Add(ValdiateEnumOption<DirectorySort.SortMethod>);
        var sortReverseOption = new Option<bool>("--sort-reverse", "-sr");
        var recursiveOption = new Option<bool>("--recursive", "-r");
        var rootArgument = new Argument<string>("root")
        {
            //Arity = ArgumentArity.ExactlyOne
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

        var sortMethod = ParseEnum<DirectorySort.SortMethod>(parseResult.GetRequiredValue(sortMethodOption));
        var sortReverse = parseResult.GetValue(sortReverseOption);
        var sort = new DirectorySort(sortMethod, sortReverse);

        return new(
            parseResult.GetRequiredValue(includesOption),
            parseResult.GetValue(excludesOption) ?? [],
            sort,
            parseResult.GetValue(recursiveOption),
            Path.GetFullPath(parseResult.GetRequiredValue(rootArgument))
        );
    }

    private static void ValdiateEnumOption<T>(OptionResult result) where T : struct, Enum
    {
        if(result.Implicit) return;

        var value = result.Tokens.Single().Value.ToLowerInvariant();
        var validValues = Enum.GetNames<T>().Select(s => s.ToLowerInvariant());

        if(!validValues.Contains(value))
        {
            result.AddError($"Argument '{value}' not recognized. Must be one of: {string.Join(", ", validValues)}");
        }
    }

    private static T ParseEnum<T>(string value) where T : struct, Enum => Enum.Parse<T>(value, true);
}
