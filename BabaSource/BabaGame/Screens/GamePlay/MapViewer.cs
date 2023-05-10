using BabaGame.Engine;
using BabaGame.Objects;
using Core;
using Core.Content;
using Core.Engine;
using Core.UI;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;

internal class MapViewer : GameObject
{
	private Dictionary<uint, ObjectSprite> sprites = new();
    private readonly BabaWorld world;
    private readonly MapSimulator simulator;
    private readonly TextOverlay textOverlay = new();

    public BabaMap MapData { get; }

    public MapViewer(BabaMap mapData, BabaWorld world, MapSimulator simulator)
	{
		foreach (var (obj, index) in mapData.WorldObjects.Select((x, i) => (x, i)))
		{
			if (obj == null) continue;
			addSprite((uint)index);
        }
        MapData = mapData;
        this.world = world;
        this.simulator = simulator;

        textOverlay.Graphics.zindex = 1000;
        AddChild(textOverlay);
    }

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

        textOverlay.RemoveAllText();
        foreach (var you in simulator.findObjectsThatAre(ObjectTypeId.you))
        {
            foreach (var neighbor in simulator.GetNeighbors(you))
            {
                doTextOverlay(neighbor);
            }
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

    private void doTextOverlay(BabaObject obj)
    {
        var message = "";
        if (!string.IsNullOrWhiteSpace(obj.Text)) 
            message = obj.Text;

        if (simulator.isObject(obj, ObjectTypeId.disk))
        {
            message += $"\nStep on this [{obj.Name}]\nsave your game";
        }

        if (simulator.doesObjectNeedAnything(obj) is Dictionary<ObjectTypeId, int> needs && needs.Count > 0)
        {
            var objColor = PaletteInfo.Palettes["default"][obj.Color].ToHexTriple();
            message += $"\n{objColor}[{obj.Name}:{(int)obj.Facing}][white] Requires:\n";
            foreach (var (reagent, count) in needs)
            {
                var color = ThemeInfo.GetColorsByKind(reagent, ObjectKind.Text);
                var activeColor = PaletteInfo.Palettes["default"][color.colorActive].ToHexTriple();

                world.Inventory.TryGetValue(reagent, out int hasCount);
                var hasColor = hasCount >= count ? "[44,ff,44]" : "[white]";

                message += $"\n {activeColor}{reagent} [{reagent}] {hasColor}{hasCount}/{count}";
            }
        }
        if (!string.IsNullOrWhiteSpace(message))
        {
            textOverlay.AddText(message.Trim(), obj.x, obj.y, 1f / 60);
        }
    }
}
