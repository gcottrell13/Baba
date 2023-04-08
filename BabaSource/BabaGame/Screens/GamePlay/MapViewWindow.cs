using BabaGame.Engine;
using Core;
using Core.Engine;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;

/// <summary>
/// controls the display of the world.
/// manages animations between map segments, scale factor, displaying neighboring map segments
/// </summary>
internal class MapViewWindow : GameObject
{
    private readonly BabaWorld babaWorld;
    private Dictionary<short?, MapViewer> mapViewers = new();
    private short currentMapId = 0;

    public MapViewWindow(BabaWorld babaWorld)
    {
        this.babaWorld = babaWorld;
        
        foreach (var (mapId, map) in babaWorld.MapDatas)
        {
            var mv = new MapViewer(map);
            mapViewers[mapId] = mv;
        }
    }

    public void LoadMap(short mapId)
    {
        _loadMap(mapId);
        if (babaWorld.Simulators.ContainsKey(currentMapId) == false) 
        {
            // no animation
        }
        else if (babaWorld.Simulators[currentMapId].HasNeighbor(mapId))
        {
            // animate transition
        }
        else
        {
            // no animation
        }
    }


    private void _loadMap(short mapId)
    {
        RemoveAllChildren();
        var sim = babaWorld.Simulators[mapId];
        //if (_tryGetMapViewer(sim.NorthNeighbor, out var neighbor)) AddChild(neighbor);
        //if (_tryGetMapViewer(sim.SouthNeighbor, out neighbor)) AddChild(neighbor);
        //if (_tryGetMapViewer(sim.WestNeighbor, out neighbor)) AddChild(neighbor);
        //if (_tryGetMapViewer(sim.EastNeighbor, out neighbor)) AddChild(neighbor);
        if (_tryGetMapViewer(mapId, out var mp))
        {
            AddChild(mp);
            mp.Load();
            mp.Graphics.xscale = 24;
            mp.Graphics.yscale = 24;
        }
    }

    private bool _tryGetMapViewer(short? mapId, out MapViewer mapViewer)
    {
        if (mapId == null)
        {
            mapViewer = null;
            return false;
        }
        return mapViewers.TryGetValue(mapId, out mapViewer);
    }
}
