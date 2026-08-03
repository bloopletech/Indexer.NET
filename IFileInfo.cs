using System;
using System.Collections.Generic;
using System.Text;

namespace Indexer.NET;

// Based on https://github.com/dotnet/runtime/blob/c8acea22626efab11c13778c028975acdc34678f/src/libraries/Microsoft.Extensions.FileProviders.Abstractions/src/IFileInfo.cs
public interface IFileInfo
{
    bool Exists { get; }
    bool IsDirectory { get; }
    DateTimeOffset LastModified { get; }
    long Length { get; }
    string Name { get; }
    string PhysicalPath { get; }
}
