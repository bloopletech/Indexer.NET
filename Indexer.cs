using NeoSmart.PrettySize;

namespace DirectoryIndexer;

public class Indexer(DirectoryResult directory, string url)
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
                <div class="listing">
                    <table aria-describedby="summary">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Name</th>
                                <th>Size</th>
                                <th class="hideable">Modified</th>
                                <th class="hideable"></th>
                            </tr>
                        </thead>
                        <tbody>{RenderEntries()}</tbody>
                    </table>
                </div>
                </main>
            </body>
        </html>
        """;

    private string RenderEntries() => string.Join("", [..RenderOthers(), ..RenderDirectories(), ..RenderFiles()]);

    private IEnumerable<string> RenderOthers() => url == "/" ? [] : [RenderParent()];

    private IEnumerable<string> RenderDirectories() => directory.Directories
        .OrderBy(d => d.Name, StringComparer.CurrentCultureIgnoreCase)
        .Select(RenderDirectory);

    private IEnumerable<string> RenderFiles() => directory.Files
        .OrderBy(f => f.Name, StringComparer.CurrentCultureIgnoreCase)
        .Where(f => f.Name != IndexFileName)
        .Select(RenderFile);

    private static string RenderParent() => $"""
        <tr class="clickable">
            <td></td>
            <td><a href="../">⬆️ <span class="name">..</span></a></td>
            <td data-order="-1">-</td>
            <td class="hideable">-</td>
            <td class="hideable"></td>
        </tr>
        """;

    private static string RenderDirectory(LinkAwareDirectoryInfo info) => $"""
        <tr class="file">
            <td></td>
            <td><a href="{UrlUtility.EncodeUrlPath(info.Name + "/")}">📂 <span class="name">{info.Name}</span></a></td>
            <td data-order="-1">-</td>
            <td class="hideable"><time datetime="{info.LastModified:O}">{info.LastModified:F}</time></td>
            <td class="hideable"></td>
        </tr>
        """;

    private static string RenderFile(LinkAwareFileInfo info) => $"""
        <tr class="file">
            <td></td>
            <td><a href="{UrlUtility.EncodeUrlPath(info.Name)}">📄 <span class="name">{info.Name}</span></a></td>
            <td data-order="{info.Length}">{PrettySize.Bytes(info.Length)}</td>
            <td class="hideable"><time datetime="{info.LastModified:O}">{info.LastModified:F}</time></td>
            <td class="hideable"></td>
        </tr>
        """;

    private static readonly string Styles = """
        * { padding: 0; margin: 0; }
        body {
            font-family: sans-serif;
            text-rendering: optimizespeed;
            background-color: #ffffff;
        }
        a {
            color: #006ed3;
            text-decoration: none;
        }
        a:hover,
        h1 a:hover {
            color: #319cff;
        }
        header,
        #summary {
            padding-left: 5%;
            padding-right: 5%;
        }
        th:first-child,
        td:first-child {
            width: 5%;
        }
        th:last-child,
        td:last-child {
            width: 5%;
        }
        header {
            padding-top: 25px;
            padding-bottom: 15px;
            background-color: #f2f2f2;
        }
        h1 {
            font-size: 20px;
            font-weight: normal;
            white-space: nowrap;
            overflow-x: hidden;
            text-overflow: ellipsis;
            color: #999;
        }
        h1 a {
            color: #000;
            margin: 0 4px;
        }
        h1 a:hover {
            text-decoration: underline;
        }
        h1 a:first-child {
            margin: 0;
        }
        main {
            display: block;
        }
        .meta {
            font-size: 12px;
            font-family: Verdana, sans-serif;
            border-bottom: 1px solid #9C9C9C;
            padding-top: 10px;
            padding-bottom: 10px;
        }
        .meta-item {
            margin-right: 1em;
        }
        #filter {
            padding: 4px;
            border: 1px solid #CCC;
        }
        table {
            width: 100%;
            border-collapse: collapse;
        }
        tr {
            border-bottom: 1px dashed #dadada;
        }
        tbody tr:hover {
            background-color: #ffffec;
        }
        th,
        td {
            text-align: left;
            padding: 10px 0;
        }
        th {
            padding-top: 15px;
            padding-bottom: 15px;
            font-size: 16px;
            white-space: nowrap;
        }
        th a {
            color: black;
        }
        th svg {
            vertical-align: middle;
        }
        td {
            white-space: nowrap;
            font-size: 14px;
        }
        td:nth-child(2) {
            width: 80%;
        }
        td:nth-child(3) {
            padding: 0 20px 0 20px;
        }
        th:nth-child(4),
        td:nth-child(4) {
            text-align: right;
        }
        td:nth-child(2) svg {
            position: absolute;
        }
        td .name {
            margin-left: 1.75em;
            word-break: break-all;
            overflow-wrap: break-word;
            white-space: pre-wrap;
        }
        td .goup {
            margin-left: 1.75em;
            padding: 0;
            word-break: break-all;
            overflow-wrap: break-word;
            white-space: pre-wrap;
        }
        .icon {
            margin-right: 5px;
        }
        tr.clickable { 
            cursor: pointer; 
        } 
        tr.clickable a { 
            display: block; 
        } 
        @media (max-width: 600px) {
            * {
                font-size: 1.06rem;
            }
            .hideable {
                display: none;
            }
            td:nth-child(2) {
                width: auto;
            }
            th:nth-child(3),
            td:nth-child(3) {
                padding-right: 5%;
                text-align: right;
            }
            h1 {
                color: #000;
            }
            h1 a {
                margin: 0;
            }
            #filter {
                max-width: 100px;
            }
        }
        """;

}
