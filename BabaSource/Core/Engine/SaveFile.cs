using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Engine;

public class SaveFile
{
    public WorldData InitialContent;
    public Dictionary<string, WorldData> SaveFiles;

    public SaveFile(WorldData initial, Dictionary<string, WorldData> saves)
    {
        InitialContent = initial;
        SaveFiles = saves;
    }
}
