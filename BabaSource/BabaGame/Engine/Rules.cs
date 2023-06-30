using BabaGame.Events;
using Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Engine;

public static class Rules
{
    public static void Hot(this MapSimulator sim)
    {
        if (sim.allRules.ContainsKey(ObjectTypeId.hot) && sim.allRules.ContainsKey(ObjectTypeId.melt))
        {
            var hotObjectsGrid = sim.ToGrid(sim.findObjectsThatAre(ObjectTypeId.hot));
            var meltObjectsGrid = sim.ToGrid(sim.findObjectsThatAre(ObjectTypeId.melt));
            foreach (var (position, melters) in meltObjectsGrid)
            {
                if (hotObjectsGrid.ContainsKey(position))
                {
                    foreach (var m in melters) sim.TryRemoveObject(m);
                }
            }
        }
    }

    public static void Save(this MapSimulator sim)
    {
        var yous = sim.findObjectsThatAre(ObjectTypeId.you).ToList();
        foreach (var save in sim.findObjectsThatAre(ObjectTypeId.save).ToList())
        {
            foreach (var you in yous)
            {
                if (you.x == save.x && you.y == save.y)
                {
                    EventChannels.SaveGame.SendMessage(1, async: true);
                }
            }
        }
    }

    public static void Grab(this MapSimulator sim)
    {
        var yous = sim.findObjectsThatAre(ObjectTypeId.you).ToList();
        foreach (var grab in sim.findObjectsThatAre(ObjectTypeId.grab).ToList())
        {
            foreach (var you in yous)
            {
                if (you.x == grab.x && you.y == grab.y)
                {
                    EventChannels.SoundPlay.SendMessage(new() { TrackName = "get" }, async: true);
                    EventChannels.AddItemsToInventory.SendMessage((grab.Name, 1), async: true);
                    sim.TryRemoveObject(grab);
                }
            }
        }
    }
}
