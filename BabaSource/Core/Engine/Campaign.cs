using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Engine;

public class Campaign
{
    public WorldData InitialContent;
    public Dictionary<string, WorldData> SaveFiles;
    public string Name { get; }

    public Campaign(string name, WorldData initial, Dictionary<string, WorldData> saves)
    {
        Name = name;
        InitialContent = initial;
        SaveFiles = saves;
    }


    public void SetSave(WorldData save)
    {
        SaveFiles[save.Name] = save;
    }
}
