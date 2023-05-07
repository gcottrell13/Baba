using BabaGame.Engine;
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
	private Dictionary<uint, ObjectSprite> sprites = new();

	public MapViewer(BabaMap mapData)
	{
		foreach (var (obj, index) in mapData.WorldObjects.Select((x, i) => (x, i)))
		{
			if (obj == null) continue;
			addSprite((uint)index);
        }
        MapData = mapData;
    }

    public BabaMap MapData { get; }

    public void Load()
	{
        ensureSprites();
        foreach (var (index, sprite) in sprites)
		{
			sprite.MoveSpriteNoAnimate(MapData.WorldObjects[(int)index]);
		}
	}

	public void onMove()
	{
        ensureSprites();
        foreach (var (index, sprite) in sprites)
        {
            sprite.OnMove(MapData.WorldObjects[(int)index], false);
        }
    }

	private void ensureSprites()
    {
        foreach (var obj in MapData.WorldObjects)
        {
            if (obj.index >= 0 && !sprites.ContainsKey((uint)obj.index))
            {
                addSprite((uint)obj.index);
            }
        }

        for (var i = MapData.WorldObjects.Count; i < sprites.Count; i++)
        {
            RemoveChild(sprites[(uint)i]);
            sprites.Remove((uint)i);
        }
    }

	private void addSprite(uint index)
	{
        var sprite = new ObjectSprite();
        sprites.Add(index, sprite);
        AddChild(sprite);
    }
}
