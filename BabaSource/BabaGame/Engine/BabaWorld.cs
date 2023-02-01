using Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Engine;

public class BabaWorld
{

	public Dictionary<short, MapData> Maps;
	public Dictionary<short, RegionData> Regions;
	public MapData GlobalWords;

	public BabaWorld(WorldData data)
	{
		Maps = data.Maps.ToDictionary(map => map.MapId);
		Regions = data.Regions.ToDictionary(r => r.RegionId);
		GlobalWords = Maps[data.GlobalWordMapId];
	}
}
