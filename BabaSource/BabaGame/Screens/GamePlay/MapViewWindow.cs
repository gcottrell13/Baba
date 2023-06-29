using BabaGame.Engine;
using BabaGame.Events;
using Core;
using Core.Content;
using Core.Engine;
using Core.Events;
using Core.Screens;
using Core.Utils;
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
    private float currentScale = 1f;

    private List<short> visibleMaps = new();

    public MapViewWindow(BabaWorld babaWorld, int viewportWidth, int viewportHeight)
    {
        this.babaWorld = babaWorld;
        this.viewportWidth = viewportWidth;
        this.viewportHeight = viewportHeight;
        foreach (var (mapId, map) in babaWorld.MapDatas)
        {
            var mv = new MapViewer(map, babaWorld, babaWorld.Simulators[mapId]);
            mapViewers[mapId] = mv;
        }
    }

    public async Task LoadMap(short mapId)
    {
        var newScale = _getMapScale(mapId);

        if (babaWorld.Simulators.TryGetValue(currentMapId, out var sim) == false)
        {
            _loadMap(mapId);
            // no animation
            Graphics.xscale = newScale;
            Graphics.yscale = newScale;
            Graphics.x = newScale;
            Graphics.y = newScale;
            EventChannels.CharacterControl.SendMessage(true);
        }
        else if (sim.HasNeighbor(mapId, out var dir))
        {
            var shortAnimation = visibleMaps.Contains(mapId);
            var (dx, dy) = DirectionExtensions.DeltaFromDirection(dir);

            if (shortAnimation)
            {
                // TODO: fix flickering! (possibly only when going from a smaller scale to a larger one)
                _loadMap(mapId);
                // animate
                // TODO: add something for when we are going left/up to ensure that the right/bottom side is visible the whole time
                var x = dx * sim.Width * currentScale + currentScale;
                var y = dy * sim.Height * currentScale + currentScale;
                Graphics.x = x;
                Graphics.y = y;
                EventChannels.CharacterControl.SendMessage(false);
                var time = 0.25f;
                await Task.WhenAll(
                    AnimateHelper.AnimateCubic(x, time, newScale, f => Graphics.x = f),
                    AnimateHelper.AnimateCubic(y, time, newScale, f => Graphics.y = f)
                );
                if (currentScale != newScale)
                {
                    await Task.WhenAll(
                        AnimateHelper.AnimateCubic(Graphics.xscale, time, newScale, f => Graphics.xscale = f),
                        AnimateHelper.AnimateCubic(Graphics.yscale, time, newScale, f => Graphics.yscale = f)
                        // TODO: add another animation for x and y (again) if we are going left/up
                    );
                }
                EventChannels.CharacterControl.SendMessage(true);
            }
            else
            {
                // animate
                // THEN load
                _loadMap(mapId);
            }
            //Graphics.xscale = scale;
            //Graphics.yscale = scale;
            //Graphics.x = scale;
            //Graphics.y = scale;
        }
        else
        {
            _loadMap(mapId);
            // no animation
            Graphics.xscale = newScale;
            Graphics.yscale = newScale;
            Graphics.x = newScale;
            Graphics.y = newScale;
        }
        currentScale = newScale;
    }


    private void _loadMap(short mapId)
    {
        RemoveAllChildren();
        var sim = babaWorld.Simulators[mapId];

        var previousVisibleMaps = visibleMaps.ToList();
        visibleMaps.Clear();

        currentMapId = mapId;

        if (_tryGetMapViewer(sim.NorthNeighbor, out var neighbor) && neighbor.MapData.width == sim.Width) _addMapAndScale(neighbor, 0, -neighbor.MapData.height);
        if (_tryGetMapViewer(sim.SouthNeighbor, out neighbor) && neighbor.MapData.width == sim.Width) _addMapAndScale(neighbor, 0, sim.Height);
        if (_tryGetMapViewer(sim.WestNeighbor, out neighbor) && neighbor.MapData.height == sim.Height) _addMapAndScale(neighbor, -neighbor.MapData.width, 0);
        if (_tryGetMapViewer(sim.EastNeighbor, out neighbor) && neighbor.MapData.height == sim.Height) _addMapAndScale(neighbor, sim.Width, 0);

        if (_tryGetMapViewer(mapId, out var mp)) _addMapAndScale(mp, 0, 0);
        sim.MarkVisited();

        var removedMaps = previousVisibleMaps.Except(visibleMaps);
        //var addedMaps = visibleMaps.Except(previousVisibleMaps);

        foreach (var removedMapId in removedMaps)
        {
            babaWorld.Simulators[removedMapId].OnUnload();
        }
    }

    private void _addMapAndScale(MapViewer mp, int x, int y)
    {
        AddChild(mp);
        mp.Load();
        visibleMaps.Add(mp.MapData.MapId);
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

    public short[] GetVisibleMaps()
    {
        return visibleMaps.ToArray();
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

    public void DisplayItemGetToast(ObjectTypeId obj, int addedCount, int newTotal)
    {
        var toast = new ItemAddedToast(obj, addedCount, newTotal);
        toast.Graphics.xscale = 1f/24;
        toast.Graphics.yscale = 1f/24;
        AddChild(toast);
    }
}
