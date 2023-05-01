using Core.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils;

public static class SettingsFileManager
{
    private const string filesDirectory = ContentDirectory.contentDirectory + "/settings/";

    private static void ensureDirectory()
    {
        if (!Directory.Exists(filesDirectory))
        {
            Directory.CreateDirectory(filesDirectory);
        }
    }

    public static T GetSettings<T>(string name) where T : class, new()
    {
        ensureDirectory();
        try
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(File.ReadAllText(filesDirectory + "/" + name + ".json"));
        }
        catch {
            return new T();
        }
    }

    public static void SaveSettings<T>(T file, string name) where T : class
    {
        ensureDirectory();
        File.WriteAllText(Newtonsoft.Json.JsonConvert.SerializeObject(file), filesDirectory + "/" + name + ".json");
    }
}
