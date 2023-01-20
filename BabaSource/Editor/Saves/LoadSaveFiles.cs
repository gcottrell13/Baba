using Core.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Saves
{
    public static class LoadSaveFiles
    {
        private const string editorFilesDirectory = ContentDirectory.contentDirectory + "/editorSaves/";

        private static ReadonlySavesList files = new();

        public static void AddNewSave(SaveFormatWorld saveFormat)
        {
            files.Add(saveFormat);
        }

        public static IEnumerable<string> GetSaveFiles()
        {
            foreach (var file in Directory.EnumerateFiles(editorFilesDirectory))
            {
                var n = Path.GetFileName(file);
                yield return n[..n.IndexOf('.')];
            }
        }

        public static ReadonlySavesList LoadAllWorlds()
        {
            var alist = new List<SaveFormatWorld>();
            foreach (var f in GetSaveFiles().Select(LoadWorld))
            {
                if (f == null) continue;
                if (alist.Any(s => s.fileName == f.fileName)) continue;
                alist.Add(f);
            }
            files.AddRange(alist);
            return files;
        }

        public static SaveFormatWorld? LoadWorld(string saveFileName)
        {
            try
            {
                var text = File.ReadAllText(editorFilesDirectory + $"{saveFileName}.json");
                var d = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveFormatWorld>(text) ?? throw new NullReferenceException("could not load");
                d.fileName = saveFileName;
                return d;
            }
            catch 
            {
                return null;
            }
        }

        public static void SaveAll(SaveFormatWorld save)
        {
            save.fileName ??= save.worldName;
            var text = SaveFormatSerializer.Serialize(save);
            File.WriteAllText(editorFilesDirectory + $"{save.fileName}.json", text);
        }
    }

    public class ReadonlySavesList : IEnumerable<SaveFormatWorld>
    {
        private readonly List<SaveFormatWorld> saves = new();

        public void Add(SaveFormatWorld save)
        {
            saves.Add(save);
        }

        public void AddRange(IEnumerable<SaveFormatWorld> saves)
        {
            this.saves.AddRange(saves);
        }

        public IEnumerator GetEnumerator()
        {
            return saves.GetEnumerator();
        }

        IEnumerator<SaveFormatWorld> IEnumerable<SaveFormatWorld>.GetEnumerator()
        {
            return saves.GetEnumerator();
        }

        public int Count => saves.Count;

        public ReadOnlyCollection<SaveFormatWorld> ToList()
        {
            return new ReadOnlyCollection<SaveFormatWorld>(saves);
        }
    }
}
