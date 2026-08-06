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

var queue = new Queue<string>();
queue.Enqueue("");

while(queue.Count > 0)
{
    var dir = queue.Dequeue();

    var result = DirectoryResult.Empty;

    try
    {
        result = factory.For(dir);
        var url = $"/{dir.Replace(Path.DirectorySeparatorChar, '/')}";

        new DirectoryIndexer(result, url).Create();
        Console.WriteLine($"Indexed {result.FullName}");
    }
    catch(Exception ex)
    {
        Console.WriteLine($"There was an error while indexing {result.FullName}, skipping: {ex}");
    }

    if(options.Recursive)
    {
        foreach(var subDir in result.Directories) queue.Enqueue(Path.Join(dir, subDir.Name));
    }
}
