using Microsoft.Extensions.FileSystemGlobbing;

namespace Indexer.NET;

public class DirectoryFactory(string rootDir, Matcher matcher)
{
    public DirectoryResult For(string dir)
    {
        var info = new DirectoryInfo(Path.Join(rootDir, dir));

        var entries = info.EnumerateFileSystemInfos().Where(Filter);
        var directories = entries.OfType<DirectoryInfo>().Select(d => new LinkAwareDirectoryInfo(d));
        var files = entries.OfType<FileInfo>().Select(f => new LinkAwareFileInfo(f));

        return new(info.FullName, directories, files);
    }

    private bool Filter(FileSystemInfo info) => IsMatch(info) && !IsSensitive(info);

    private bool IsMatch(FileSystemInfo info) => matcher.Match(rootDir, info.FullName).HasMatches;

    // Based on https://github.com/dotnet/runtime/blob/ba10a6ee8aa80fce1bb3e40b57b2b941102107c9/src/libraries/Microsoft.Extensions.FileProviders.Physical/src/Internal/FileSystemInfoHelper.cs#L12
    private static bool IsSensitive(FileSystemInfo info)
    {
        if(info.Name.StartsWith('.')) return false;

        if((info.Attributes & FileAttributes.Hidden) != 0 || (info.Attributes & FileAttributes.System) != 0)
        {
            return true;
        }

        return false;
    }
}
