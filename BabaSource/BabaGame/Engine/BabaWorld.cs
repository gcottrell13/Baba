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
	public MapData GlobalWords;

	public BabaWorld(WorldData data)
	{
		MapDatas = data.Maps.ToDictionary(map => map.MapId);
		Regions = data.Regions.ToDictionary(r => r.RegionId);
		GlobalWords = MapDatas[data.GlobalWordMapId];
		Simulators = data.Maps.ToDictionary(map => map.MapId, map => new MapSimulator(this, map.MapId));

		foreach (var sim in Simulators.Values)
		{
			sim.GetNeighbors();
        }
	}


    public short[] mapsWithYou()
    {
		// where would we be without YOU?
		return Array.Empty<short>();
    }

    public void Step(short[] mapIds, Direction direction, int playerNumber)
	{
		var globalRules = Simulators[GlobalWords.MapId].parseRules(new());
		foreach (var id in mapIds)
		{
			var map = Simulators[id];

			if (map.region != null)
                globalRules = Simulators[map.region.RegionId].parseRules(globalRules);

            map.Step(direction, globalRules, playerNumber);
		}
	}
}
