using NeoSmart.PrettySize;

namespace Indexer.NET;

public class Indexer(DirectoryResult directory, DirectorySort sort, string url)
{
    private const string IndexFileName = "index.html";

    public void Create()
    {
        File.WriteAllText(Path.Join(directory.FullName, IndexFileName), Render());
    }

    public string Render() => $"""
        <!DOCTYPE html>
        <html>
            <head>
                <meta charset="utf-8">
                <title>Index of {url}</title>
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <style>{Styles}</style>
            </head>
            <body>
                <header>
                    <h1>Index of {url}</h1>
                </header>
                <main>
                    <table>
                        <thead>
                            <tr>
                                <th class="name">Name</th>
                                <th class="size">Size</th>
                                <th class="modified hideable">Modified</th>
                            </tr>
                        </thead>
                        <tbody>{RenderEntries()}</tbody>
                    </table>
                </main>
            </body>
        </html>
        """;

    private string RenderEntries() => string.Join("", [..RenderOthers(), ..RenderDirectories(), ..RenderFiles()]);

    private IEnumerable<string> RenderOthers() => url == "/" ? [] : [RenderParent()];

    private IEnumerable<string> RenderDirectories() => Sort(directory.Directories).Select(RenderDirectory);

    private IEnumerable<string> RenderFiles() => Sort(directory.Files).Where(f => f.Name != IndexFileName).Select(RenderFile);

    private IEnumerable<T> Sort<T>(IEnumerable<T> items) where T : IFileInfo
    {
        var ordered = sort.Method switch
        {
            DirectorySort.SortMethod.Name => items.OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase),
            DirectorySort.SortMethod.Mtime => items.OrderBy(f => f.LastModified),
            _ => items,
        };

        return sort.IsReverse ? ordered.Reverse() : ordered;
    }

    private static string RenderParent() => $"""
        <tr>
            <td class="name"><a href="../">⬆️ <span>..</span></a></td>
            <td class="size">—</td>
            <td class="modified hideable">—</td>
        </tr>
        """;

    private static string RenderDirectory(LinkAwareDirectoryInfo info) => $"""
        <tr>
            <td class="name"><a href="{UrlUtility.EncodeUrlPath(info.Name + "/")}">📂 <span>{info.Name}</span></a></td>
            <td class="size">—</td>
            <td class="modified hideable"><time datetime="{info.LastModified:O}">{info.LastModified:d MMM yyy h:mm:ss tt}</time></td>
        </tr>
        """;

    private static string RenderFile(LinkAwareFileInfo info) => $"""
        <tr>
            <td class="name"><a href="{UrlUtility.EncodeUrlPath(info.Name)}">📄 <span>{info.Name}</span></a></td>
            <td class="size">{PrettySize.Bytes(info.Length)}</td>
            <td class="modified hideable"><time datetime="{info.LastModified:O}">{info.LastModified:d MMM yyy h:mm:ss tt}</time></td>
        </tr>
        """;

    private static readonly string Styles = """
        * {
            -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
            box-sizing: border-box;
            font-family: system-ui, sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol", "Noto Color Emoji";
            line-height: 1.4;
        }

        body {
            margin: 0;
            padding: 0;
            min-height: 100vh;
            font-size: 16px;
            background-color: #fff;
            color: #000;
        }
        a {
            color: #006ed3;
            text-decoration: none;
        }
        a:hover {
            color: #319cff;
        }
        header {
            padding: 25px 25px 15px 25px;
            background-color: #f2f2f2;
        }
        h1 {
            max-width: 1200px;
            margin: 0 auto;
            font-size: 20px;
            font-weight: normal;
        }
        main {
            padding: 0 25px 25px 25px;
        }
        table {
            width: 100%;
            max-width: 1200px;
            margin: 0 auto;
            border-collapse: collapse;
        }
        tr {
            border-bottom: 1px dashed #dadada;
        }
        tbody tr:hover {
            background-color: #ffffec;
        }
        th {
            padding: 15px 5px;
            text-align: left;
        }
        td {
            padding: 10px 5px;
        }
        th:first-child, td:first-child {
            padding-left: 0;
        }
        th:last-child, td:last-child {
            padding-right: 0;
        }
        td.name {
            overflow-wrap: anywhere;
            white-space: nowrap;
        }
        td.name span {
            white-space: normal;
        }
        th.size, td.size {
            width: 100px;
        }
        th.modified, td.modified {
            width: 195px;
            text-align: right;
        }

        @media (max-width: 600px) {
            .hideable {
                display: none;
            }
        }
        """;

}
