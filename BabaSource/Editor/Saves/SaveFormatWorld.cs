using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Saves;

public class SaveFormatWorld
{
    public List<SaveScreenInstance> WorldLayout { get; set; } = new();

    public List<SaveWarp> Warps { get; set; } = new();

    public List<SaveRegion> Regions { get; set; } = new();

    public List<SaveScreenData> ScreenDatas { get; set; } = new();

    public List<int> globalObjectInstanceIds { get; set; } = new();

    public int width = 30;
    public int height = 30;

    public string worldName = string.Empty;

    public string? fileName = null;

    public SaveScreenData? SaveScreenDataByInstanceId(int instanceId)
    {
        var instance = WorldLayout.FirstOrDefault(x => x.instanceId == instanceId);
        if (instance == null)
            return null;
        return ScreenDatas.FirstOrDefault(x => x.id == instance.screenDataId);
    }
}

public class SaveScreenInstance
{
    public int x = 0;
    public int y = 0;
    public int screenDataId = 0;
    public int instanceId => (x + y * 30) + 1;
}

public class SaveWarp
{
    public int x1 = 0;
    public int y1 = 0;
    public int x2 = 0;
    public int y2 = 0;
    public string name = string.Empty;

    public int r;
    public int g;
    public int b;
}

public class SaveRegion
{
    public int id = 0;
    public string name = string.Empty;
    public string theme = "default";
    public List<int> regionObjectInstanceIds { get; set; } = new();

    public string musicName = "default";
}

public class SaveScreenData
{
    public int id = 0;
    public string name = string.Empty;
    public int regionId = 0;
    public SaveMapLayer layer1 { get; set; } = new();

    public bool resetWhenInactive = false;

}

public class SaveMapLayer
{
    public List<SaveObjectData> objects { get; set; } = new();
    public int width = 15;
    public int height = 15;
}

public class SaveObjectData
{
    public int x = 0;
    public int y = 0;
    public string name = string.Empty;
    public uint state = 0;
    public int color;
    public string text = string.Empty;
    public SaveObjectData? original = null;

    public SaveObjectData copy()
    {
        return new SaveObjectData { x = x, y = y, color = color, state = state, name = name, original = original, text = text };
    }
}
