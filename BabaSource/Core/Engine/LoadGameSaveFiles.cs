using Core.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Engine;

public class LoadGameSaveFiles
{
    private const string filesDirectory = ContentDirectory.contentDirectory + "/saves/";

    public static void SaveCompiledMap(WorldData worldData, string saveFileName, string version)
    {
        if (!Directory.Exists(filesDirectory)) 
        { 
            Directory.CreateDirectory(filesDirectory);
        }

        File.WriteAllText(filesDirectory + $"{saveFileName.Replace(".", "_")}.{version.Replace(".", "_")}.sav", worldData.Serialize());
    }

    public static IEnumerable<string> GetCompiledMaps()
    {
        if (!Directory.Exists(filesDirectory))
        {
            Directory.CreateDirectory(filesDirectory);
        }

        foreach (var file in Directory.EnumerateFiles(filesDirectory))
        {
            var n = Path.GetFileName(file);
            yield return n;
        }
    }

    public static Dictionary<string, SaveFile> LoadAllCompiledMaps()
    {
        var regexp = new Regex(@"([^.]+)\.([^.]+)\.sav");
        var groups = GetCompiledMaps().GroupBy(x => regexp.Match(x) is Match m ? m.Groups[1].Value : "");
        var d = new Dictionary<string, SaveFile>();
        foreach (var group in groups)
        {
            if (group.Key == "") 
                continue;
            var saveFileNames = group.ToDictionary(name => regexp.Match(name).Groups[2].Value);
            var savesFiles = saveFileNames.ToDictionary(k => k.Key, k => WorldData.Deserialize(File.ReadAllText(filesDirectory + k.Value)));
            var initial = savesFiles.Remove("0", out var zero) ? zero : new();
            d[group.Key] = new SaveFile(group.Key, initial, savesFiles);
        }

        return d;
    }
}
