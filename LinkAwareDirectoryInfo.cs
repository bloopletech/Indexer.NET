namespace Indexer.NET;

// Based on https://github.com/dotnet/runtime/blob/c8acea22626efab11c13778c028975acdc34678f/src/libraries/Microsoft.Extensions.FileProviders.Physical/src/PhysicalDirectoryInfo.cs
public class LinkAwareDirectoryInfo
{
    private readonly DirectoryInfo _info;
    private readonly DirectoryInfo _resolvedInfo;

    public LinkAwareDirectoryInfo(DirectoryInfo info)
    {
        _info = info;

        try
        {
            var targetInfo = _info.ResolveLinkTarget(true) as DirectoryInfo;
            _resolvedInfo = targetInfo ?? _info;
        }
        catch(IOException)
        {
            _resolvedInfo = _info;
        }
    }

    public bool Exists => _resolvedInfo.Exists;

    public long Length => -1;

    public string PhysicalPath => _info.FullName;

    public string Name => _info.Name;

    public DateTimeOffset LastModified => _resolvedInfo.LastWriteTimeUtc;

    public bool IsDirectory => true;
}
