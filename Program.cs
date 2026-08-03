// Based on https://github.com/joshbrunty/Indexer/blob/6d8cbfd15d3853b482e6a49f2d875ded9188b721/indexer.py

using Indexer.NET;
using Microsoft.Extensions.FileSystemGlobbing;

var options = Options.Parse(args);

var root = Path.GetFullPath(options.Root);

var matcher = new Matcher();
matcher.AddIncludePatterns(options.Includes);
matcher.AddExcludePatterns(options.Excludes);

var sort = new Sort(options.Sort, options.SortReverse);

var factory = new DirectoryFactory(options.Root, matcher, sort);

void Index(string dir)
{
    var result = DirectoryResult.Empty;

    try
    {
        result = factory.For(dir);
        var url = $"/{dir.Replace(Path.DirectorySeparatorChar, '/')}";

        new DirectoryIndexer(result, url).Create();
    }
    catch(Exception ex)
    {
        Console.WriteLine($"There was an error while indexing {dir}, skipping: {ex}");
    }

    if(options.Recursive)
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

namespace Indexer.NET;

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
