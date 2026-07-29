// Based on https://github.com/joshbrunty/Indexer/blob/6d8cbfd15d3853b482e6a49f2d875ded9188b721/indexer.py

using System.CommandLine;
using DirectoryIndexer;
using Microsoft.Extensions.FileSystemGlobbing;

var includesOption = new Option<string[]>("--filter", "-f")
{
    DefaultValueFactory = (_) => ["**"]
};
var excludesOption = new Option<string[]>("--ignore", "-i");
var recursiveOption = new Option<bool>("--recursive", "-r");
var rootArgument = new Argument<string>("root")
{
    Arity = ArgumentArity.ExactlyOne
    //DefaultValueFactory = (_) => Directory.GetCurrentDirectory()
};

var rootCommand = new RootCommand();
rootCommand.Options.Add(includesOption);
rootCommand.Options.Add(excludesOption);
rootCommand.Options.Add(recursiveOption);
rootCommand.Arguments.Add(rootArgument);

var parseResult = rootCommand.Parse(args);
if(parseResult.Errors.Count > 0)
{
    foreach(var error in parseResult.Errors) Console.Error.WriteLine(error.Message);
    Environment.Exit(1);
}

var root = Path.GetFullPath(parseResult.GetValue(rootArgument) ?? Directory.GetCurrentDirectory());

var matcher = new Matcher();

var includes = parseResult.GetValue(includesOption);
if(includes != null) matcher.AddIncludePatterns(includes);

var excludes = parseResult.GetValue(excludesOption);
if(excludes != null) matcher.AddExcludePatterns(excludes);

var recursive = parseResult.GetValue(recursiveOption);

var factory = new DirectoryFactory(root, matcher);

void Index(string dir)
{
    var result = factory.For(dir);
    var url = $"/{dir.Replace(Path.DirectorySeparatorChar, '/')}";

    new Indexer(result, url).Create();

    if(recursive)
    {
        foreach(var subDir in result.Directories) Index(Path.Join(dir, subDir.Name));
    }
}

Index("");



//public class Indexer(string rootDir, Matcher matcher, bool recursive)
//{
//    private readonly DirectoryFactory factory = new(rootDir, matcher);

//    public void Index() => Index("");
//    private void Index(string dir)
//    {
//        var directory = factory.For(dir);
//        var entries = EntryFactory.For(directory);
//        var url = $"/{dir.Replace(Path.DirectorySeparatorChar, '/')}";

//        new Generator(directory.FullName, url, entries).Generate();

//        if(recursive)
//        {
//            foreach (var subDir in directory.Directories) Index(Path.Join(dir, subDir.Name));
//        }
//    }
//}

/*
 using Microsoft.Extensions.FileSystemGlobbing;

namespace DirectoryIndexer;

public class Indexer(string rootDir, Matcher matcher, bool recursive)
{
    private readonly DirectoryFactory factory = new(rootDir, matcher);

    public void Index() => Index("");

    public void Index(string dir)
    {
        var directory = factory.For(dir);
        var entries = EntryFactory.For(directory);
        var url = $"/{dir}";

        var result = new List<string>
        {
            $"Index of {url}:"
        };
        foreach (var entry in entries) result.Add(entry.ToString());
        Console.WriteLine(string.Join("\n", result));

        if(recursive)
        {
            foreach (var subDir in directory.Directories)
            {
                //Index($"{dir}{subDir.Name}/");
                Index(dir == "" ? subDir.Name : $"{dir}/{subDir.Name}");
                //Index(Path.Join(dir, subDir.Name), $"{url}{subDir.Name}/");
            }
        }
    }
}
*/
/*
public class Indexer(string rootDir, Matcher matcher, bool recursive)
{
    private readonly DirectoryFactory factory = new(rootDir, matcher);

    public void Index(params ReadOnlySpan<string> segments)
    {
        var directory = factory.For(Path.Join(segments));
        var entries = EntryFactory.For(directory);
        var url = $"/{string.Join('/', segments)}";

        var result = new List<string>
        {
            $"Index of {url}:"
        };
        foreach (var entry in entries) result.Add(entry.ToString());
        Console.WriteLine(string.Join("\n", result));

        if(recursive)
        {
            foreach (var subDir in directory.Directories)
            {
                Index([..segments, subDir.Name]);
                //Index($"{dir}{subDir.Name}/");
                //Index(dir == "" ? subDir.Name : $"{dir}/{subDir.Name}");
                //Index(Path.Join(dir, subDir.Name), $"{url}{subDir.Name}/");
            }
        }
    }
}
*/
/*
public class Indexer(string rootDir, Matcher matcher, bool recursive)
{
    private readonly DirectoryFactory factory = new(rootDir, matcher);

    public void Index() => Index("", "/");
    private void Index(string dir, string url)
    {
        var directory = factory.For(dir);
        var entries = EntryFactory.For(directory);

        var result = new List<string>
        {
            $"Index of {url}:"
        };
        foreach (var entry in entries) result.Add(entry.ToString());
        Console.WriteLine(string.Join("\n", result));

        if(recursive)
        {
            foreach (var subDir in directory.Directories)
            {
                //Index([..segments, subDir.Name]);
                //Index($"{dir}{subDir.Name}/");
                //Index(dir == "" ? subDir.Name : $"{dir}/{subDir.Name}");
                Index(Path.Join(dir, subDir.Name), $"{url}{subDir.Name}/");
            }
        }
    }
}
*/
/*
public class Indexer(string rootDir, Matcher matcher, bool recursive)
{
    private readonly DirectoryFactory factory = new(rootDir, matcher);

    public void Index() => Index("");
    private void Index(string dir)
    {
        var directory = factory.For(dir);
        var entries = EntryFactory.For(directory);
        var url = $"{dir}/";

        var result = new List<string>
        {
            $"Index of {url}:"
        };
        foreach (var entry in entries) result.Add(entry.ToString());
        Console.WriteLine(string.Join("\n", result));

        if(recursive)
        {
            foreach (var subDir in directory.Directories) Index($"{dir}/{subDir.Name}");
        }
    }
}

*/
/*
public class Indexer(string rootDir, Matcher matcher, bool recursive)
{
    private readonly DirectoryFactory factory = new(rootDir, matcher);

    public void Index() => Index("");
    private void Index(string dir)
    {
        var directory = factory.For(dir);
        var entries = EntryFactory.For(directory);
        var url = $"{dir}/";

        var result = new List<string>
        {
            $"Index of {url}:"
        };
        foreach (var entry in entries) result.Add(entry.ToString());
        Console.WriteLine(string.Join("\n", result));

        if(recursive)
        {
            foreach (var subDir in directory.Directories) Index(dir == "" ? subDir.Name : $"{dir}/{subDir.Name}");
        }
    }
}
*/
/*
public class Indexer(string rootDir, Matcher matcher, bool recursive)
{
    private readonly DirectoryFactory factory = new(rootDir, matcher);

    public void Index() => Index("");
    private void Index(string dir)
    {
        var directory = factory.For(dir);
        var entries = EntryFactory.For(directory);
        var url = $"/{dir}";

        var result = new List<string>
        {
            $"Index of {url}:"
        };
        foreach (var entry in entries) result.Add(entry.ToString());
        Console.WriteLine(string.Join("\n", result));

        if(recursive)
        {
            foreach (var subDir in directory.Directories) Index($"{dir}{subDir.Name}/");
        }
    }
}
*/