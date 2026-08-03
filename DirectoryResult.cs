namespace Indexer.NET;

public readonly record struct DirectoryResult(
    string FullName,
    IEnumerable<LinkAwareDirectoryInfo> Directories,
    IEnumerable<LinkAwareFileInfo> Files)
{
    public static readonly DirectoryResult Empty = new("", [], []);
}
