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
    private readonly int viewportWidth;
    private readonly int viewportHeight;
    private Dictionary<short?, MapViewer> mapViewers = new();
    private short currentMapId = 0;

    public MapViewWindow(BabaWorld babaWorld, int viewportWidth, int viewportHeight)
    {
        this.babaWorld = babaWorld;
        this.viewportWidth = viewportWidth;
        this.viewportHeight = viewportHeight;
        foreach (var (mapId, map) in babaWorld.MapDatas)
        {
            var mv = new MapViewer(map);
            mapViewers[mapId] = mv;
        }
    }

    public void LoadMap(short mapId)
    {
        _loadMap(mapId);
        var scale = _getMapScale(mapId);

        if (babaWorld.Simulators.ContainsKey(currentMapId) == false) 
        {
            // no animation
            Graphics.xscale = scale;
            Graphics.yscale = scale;
            Graphics.x = scale;
            Graphics.y = scale;
        }
        else if (babaWorld.Simulators[currentMapId].HasNeighbor(mapId))
        {
            // TODO: animate transition
            Graphics.xscale = scale;
            Graphics.yscale = scale;
            Graphics.x = scale;
            Graphics.y = scale;
        }
        else
        {
            // no animation
            Graphics.xscale = scale;
            Graphics.yscale = scale;
            Graphics.x = scale;
            Graphics.y = scale;
        }
    }


    private void _loadMap(short mapId)
    {
        RemoveAllChildren();
        var sim = babaWorld.Simulators[mapId];
        if (_tryGetMapViewer(mapId, out var mp)) _addMapAndScale(mp, 0, 0);

        if (_tryGetMapViewer(sim.NorthNeighbor, out var neighbor)) _addMapAndScale(neighbor, 0, -neighbor.MapData.height);
        if (_tryGetMapViewer(sim.SouthNeighbor, out neighbor)) _addMapAndScale(neighbor, 0, sim.Height);
        if (_tryGetMapViewer(sim.WestNeighbor, out neighbor)) _addMapAndScale(neighbor, -neighbor.MapData.width, 0);
        if (_tryGetMapViewer(sim.EastNeighbor, out neighbor)) _addMapAndScale(neighbor, sim.Width, 0);
        currentMapId = mapId;
    }

    private void _addMapAndScale(MapViewer mp, int x, int y)
    {
        AddChild(mp);
        mp.Load();
        mp.Graphics.x = x;
        mp.Graphics.y = y;
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

    private float _getMapScale(short mapId)
    {
        var map = babaWorld.Simulators[mapId];

        var xscale = viewportWidth / (map.Width + 2);
        var yscale = viewportHeight / (map.Height + 2);

        var smallerSide = Math.Min(xscale, yscale);
        return smallerSide;
    }

    public void OnMove(short[] mapIds)
    {
        foreach (var mapId in mapIds)
        {
            if (_tryGetMapViewer(mapId, out var mp))
            {
                mp.onMove();
            }
        }
    }
}
