using Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BabaGame.src.Resources
{
    class Load
    {
        public static Dictionary<string, ResourceHandle<T>> loadAll<T>(string path, Func<string, ResourceHandle<T>> act)
        {
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory + @$"\Content\{path}\");
            var dict = new Dictionary<string, ResourceHandle<T>>();

            foreach (var file in d.GetFiles("*.xnb"))
            {
                var name = file.Name.Split('.').First();
                dict.Add(name, act($"{path}/{name}"));
            }

            return dict;
        }

        public static T loadJson<T>(string name)
        {
            var file = File.Open($"Content/json/{name}.json", FileMode.Open);
            var buffer = new byte[file.Length];
            file.Read(buffer);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.ASCII.GetString(buffer));
        }
    }
}
