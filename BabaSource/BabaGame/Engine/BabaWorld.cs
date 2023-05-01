using Core.Content;
using Core.Engine;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Engine;

public class BabaWorld
{

	public Dictionary<short, MapData> MapDatas;
	public Dictionary<short, RegionData> Regions;
	public Dictionary<short, MapSimulator> Simulators;
	public short[] GlobalWordMapIds;

	public BabaWorld(WorldData data)
	{
		MapDatas = data.Maps.ToDictionary(map => map.MapId);
		Regions = data.Regions.ToDictionary(r => r.RegionId);
        GlobalWordMapIds = data.GlobalWordMapIds;
		Simulators = data.Maps.ToDictionary(map => map.MapId, map => new MapSimulator(this, map.MapId));

		foreach (var sim in Simulators.Values)
		{
			sim.GetNeighbors();
        }
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

		foreach (var id in mapIds)
		{
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
        var sims = mapIds.Select(id => Simulators[id]);
        parseMapRules(mapIds);

        var didAnyMove = false;
        foreach (var map in sims)
        {
            if (map.doMovement(map.MapId == currentMap ? direction : Direction.None, playerNumber)) 
                didAnyMove = true;
        }

        if (!didAnyMove) return;

        parseMapRules(mapIds);
        foreach (var map in sims) map.transform();
        parseMapRules(mapIds);
        foreach (var map in sims) map.moveblock();
        parseMapRules(mapIds);
        foreach (var map in sims) map.fallblock();
        parseMapRules(mapIds);
        foreach (var map in sims) map.statusblock();
        parseMapRules(mapIds);
        foreach (var map in sims) map.interactionblock();
        parseMapRules(mapIds);
        foreach (var map in sims) map.collisionCheck();
        parseMapRules(mapIds);
    }
}
