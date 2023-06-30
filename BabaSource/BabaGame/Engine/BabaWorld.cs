using BabaGame.Events;
using Core.Content;
using Core.Engine;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Engine;


public record MapMovementStackItem(BabaObject obj, short mapId, int x, int y, MapMovementStackItem? dependentOn);

public class BabaWorld
{

	public Dictionary<short, BabaMap> MapDatas;
	public Dictionary<short, RegionData> Regions;
	public Dictionary<short, MapSimulator> Simulators;
	public short[] GlobalWordMapIds;
    private string name;
    public readonly Dictionary<ObjectTypeId, int> Inventory;

	public BabaWorld(WorldData data)
	{
        name = data.Name;
		MapDatas = data.Maps.ToDictionary(map => map.MapId, map => (BabaMap)map);
		Regions = data.Regions.ToDictionary(r => r.RegionId);
        GlobalWordMapIds = data.GlobalWordMapIds;
		Simulators = data.Maps.ToDictionary(map => map.MapId, map => new MapSimulator(this, map.MapId));
        Inventory = data.Inventory;

		foreach (var sim in Simulators.Values)
		{
			sim.GetNeighbors();
        }
	}

    public WorldData ToWorldData()
    {
        var data = new WorldData
        {
            Maps = MapDatas.Values.Select(x => x.ToMapData()).ToList(),
            Regions = Regions.Values.ToList(),
            GlobalWordMapIds = GlobalWordMapIds,
            Name = name,
            Inventory = Inventory,
        };
        return data;
    }


    public short[] mapsWithYou(ObjectTypeId you)
    {
        parseMapRules(Simulators.Values.Select(s => s.MapId).ToArray());
        var mapIds = new List<short>();
        foreach (var sim in Simulators.Values)
        {
            var objs = sim.findObjectsThatAre(you).ToList();
            if (objs.Any())
            {
                mapIds.Add(sim.MapId);
            }
        }
        return mapIds.ToArray();
    }

	private void parseMapRules(short[] mapIds)
	{
		var dict = new Dictionary<short, RuleDict>();

        var globalRules = _parseAndApplyToAll(new RuleDict(), GlobalWordMapIds);

        var regionRules = Regions.ToDictionary(s => s.Key, s => _parseAndApplyToAll(globalRules, s.Value.WordLayerIds));

        var alreadyProcessed = GlobalWordMapIds.Concat(Regions.SelectMany(x => x.Value.WordLayerIds)).ToList();

		foreach (var id in mapIds)
		{
            if (alreadyProcessed.Contains(id)) continue;

			var map = Simulators[id];
			var rules = map.region != null ? regionRules[map.region.RegionId] : globalRules;
            dict[id] = map.parseRules(rules);
		}
	}

    RuleDict _parseAndApplyToAll(RuleDict startingRules, short[] ids)
    {
        var rules = startingRules;
        foreach (var id in ids)
        {
            rules = Simulators[id].parseRules(rules);
        }
        foreach (var id in ids)
        {
            Simulators[id].setAllRules(rules);
        }
        return rules;
    }

    public void Step(short currentMap, short[] mapIds, Direction direction, ObjectTypeId playerNumber)
    {
        var sims = mapIds.Select(id => Simulators[id]).ToList();
        var movementStack = new Stack<MapMovementStackItem>();

        parseMapRules(mapIds);
        foreach (var map in sims) movementStack.PushMany(map.moveYou(map.MapId == currentMap ? direction : Direction.None));

        if (movementStack.Count == 0) return;

        foreach (var map in sims) movementStack.PushMany(map.doMovement());
        parseMapRules(mapIds);
        foreach (var map in sims) map.transform();
        parseMapRules(mapIds);
        foreach (var map in sims) movementStack.PushMany(map.moveblock());
        parseMapRules(mapIds);
        foreach (var map in sims) movementStack.PushMany(map.fallblock());
        parseMapRules(mapIds);
        foreach (var map in sims) map.statusblock();
        parseMapRules(mapIds);
        foreach (var map in sims) map.interactionblock();
        parseMapRules(mapIds);

        // collisionCheck:
        {
            // pop items off the stack to check if objects should be reverted to their previous position.
            // items higher on the stack should represent the farthest each item can move.

            var revertedMovements = new HashSet<MapMovementStackItem>();

            void revert(BabaObject obj, MapMovementStackItem previous)
            {
                if (obj.CurrentMapId != previous.mapId)
                {
                    Simulators[obj.CurrentMapId].TryRemoveObject(obj);
                    Simulators[previous.mapId].addObjectAt(obj, previous.x, previous.y);
                }
                else
                {
                    obj.x = previous.x;
                    obj.y = previous.y;
                }
                revertedMovements.Add(previous);
            }

            while (movementStack.Count > 0)
            {
                var previous = movementStack.Pop();
                var (obj, mapId, x, y, instigator) = previous;
                if (obj.Deleted || !obj.Present) continue;

                if (instigator != null && revertedMovements.Contains(instigator))
                {
                    revert(obj, previous);
                }
                // if true, then there is a collision, and this object should be moved back.
                else if (Simulators[obj.CurrentMapId].collisionCheck(obj))
                {
                    revert(obj, previous);
                    if (Simulators[obj.CurrentMapId].isObject(obj, ObjectTypeId.move))
                    {
                        obj.Facing = DirectionExtensions.Opposite(obj.Facing);
                    }
                }
                else if (obj.CurrentMapId != mapId && mapId == currentMap && Simulators[mapId].isObject(obj, ObjectTypeId.you))
                {
                    EventChannels.MapChange.SendAsyncMessage(new() { MapId = obj.CurrentMapId });
                }
            }
        }

        parseMapRules(mapIds);

        foreach (var map in sims)
        {
            map.removeDuplicatesInSamePosition();
            map.doJoinables();
        }

        Simulators[currentMap].findSpecialPropertiesToDisplay();
    }

    public bool TestInventory(Dictionary<ObjectTypeId, int> needs)
    {
        foreach (var (reagent, count) in needs)
        {
            if (!Inventory.TryGetValue(reagent, out var has) || has < count)
                return false;
        }
        return true;
    }

    public bool ConsumeFromInventory(Dictionary<ObjectTypeId, int> needs)
    {
        if (!TestInventory(needs)) return false;

        foreach (var (reagent, count) in needs)
        {
            Inventory[reagent] -= count;
        }
        return true;
    }

    public bool AddToInventory(ObjectTypeId id, int amount)
    {
        Inventory[id] = Inventory.TryGetValue(id, out var current) ? current + amount : amount;
        return true;
    }
}
