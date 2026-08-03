namespace Indexer.NET;

public readonly record struct DirectorySort(DirectorySort.SortMethod Method, bool IsReverse)
{
    public enum SortMethod
    {
        Name,
        Mtime
    }
}
