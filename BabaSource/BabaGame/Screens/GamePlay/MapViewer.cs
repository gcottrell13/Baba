using BabaGame.Objects;
using Core;
using Core.Engine;
using Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;

internal class MapViewer : GameObject
{
	private Dictionary<int, ObjectSprite> sprites = new();

	public MapViewer(MapData mapData)
	{
		foreach (var (obj, index) in mapData.WorldObjects.Select((x, i) => (x, i)))
		{
			if (obj == null) continue;
			obj.index = index;
			var sprite = new ObjectSprite(obj);
            sprites.Add(index, sprite);
			AddChild(sprite);
            sprite.Graphics.xscale = 1f / 24;
            sprite.Graphics.yscale = 1f / 24;
		}
        MapData = mapData;
    }

    public MapData MapData { get; }

    public void Load()
	{
		foreach (var sprite in sprites.Values)
		{
			sprite.MoveSpriteNoAnimate();
		}
	}

	public void onMove()
	{
		foreach (var sprite in sprites.Values)
		{
			sprite.OnMove(false);
		}
	}
}
