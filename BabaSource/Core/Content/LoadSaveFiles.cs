using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Content
{
    public static class LoadSaveFiles
    {
        private static string editorFilesDirectiory =
#if DEBUG
            "../../../../../editorSaves/";
#else
            "./editorSaves/";
#endif

        private static ReadonlySavesList files = new();

        public static void AddNewSave(SaveFormat saveFormat)
        {
            files.Add(saveFormat);
        }

        public static IEnumerable<string> GetSaveFiles()
        {
            foreach (var file in Directory.EnumerateFiles(editorFilesDirectiory))
            {
                var n = Path.GetFileName(file);
                yield return n[..n.IndexOf('.')];
            }
        }

        public static ReadonlySavesList LoadAllWorlds()
        {
            var alist = new List<SaveFormat>();
            foreach (var f in GetSaveFiles().Select(LoadWorld))
            {
                if (f == null) continue;
                if (alist.Any(s => s.fileName == f.fileName)) continue;
                alist.Add(f);
            }
            files.AddRange(alist);
            return files;
        }

        public static SaveFormat? LoadWorld(string saveFileName)
        {
            try
            {
                var text = File.ReadAllText(editorFilesDirectiory + $"{saveFileName}.json");
                var d = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveFormat>(text) ?? throw new NullReferenceException("could not load");
                d.fileName = saveFileName;
                return d;
            }
            catch 
            {
                return null;
            }
        }

        public static void SaveAll(SaveFormat save)
        {
            save.fileName ??= save.worldName;
            var text = SaveFormatSerializer.Serialize(save);
            File.WriteAllText(editorFilesDirectiory + $"{save.fileName}.json", text);
        }
    }

    public class ReadonlySavesList : IEnumerable<SaveFormat>
    {
        private readonly List<SaveFormat> saves = new();

        public void Add(SaveFormat save)
        {
            saves.Add(save);
        }

        public void AddRange(IEnumerable<SaveFormat> saves)
        {
            this.saves.AddRange(saves);
        }

        public IEnumerator GetEnumerator()
        {
            return saves.GetEnumerator();
        }

        IEnumerator<SaveFormat> IEnumerable<SaveFormat>.GetEnumerator()
        {
            return saves.GetEnumerator();
        }

        public int Count => saves.Count;

        public ReadOnlyCollection<SaveFormat> ToList()
        {
            return new ReadOnlyCollection<SaveFormat>(saves);
        }
    }
}
