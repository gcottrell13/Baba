using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.SaveFormats
{
    internal static class LoadSaveFiles
    {
        private static string _getSaveDirectory()
        {
            var a = Directory.GetParent(Directory.GetCurrentDirectory());
            while (a.Name != "Baba")
            {
                a = a.Parent;
            }
            var b = a.ToString() + $"/editorSaves/";
            return b;
        }

        public static IEnumerable<string> GetSaveFiles()
        {
            foreach (var file in Directory.EnumerateFiles(_getSaveDirectory()))
            {
                var n = Path.GetFileName(file);
                yield return n[..n.IndexOf('.')];
            }
        }

        public static SaveFormat[] LoadAllWorlds()
        {
            return GetSaveFiles().Select(LoadWorld).ToArray();
        }

        public static SaveFormat LoadWorld(string worldName)
        {
            var text = File.ReadAllText(_getSaveDirectory() + $"{worldName}.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SaveFormat>(text) ?? throw new NullReferenceException("could not load");
        }

        public static void SaveAll(string worldName, SaveFormat save)
        {
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(save);
            File.WriteAllText(_getSaveDirectory() + $"{worldName}.json", text);
        }
    }
}
