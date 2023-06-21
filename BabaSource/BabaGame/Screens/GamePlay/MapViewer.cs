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
	private Dictionary<int, ObjectSprite> sprites = new();
    private readonly BabaWorld world;
    private readonly MapSimulator simulator;
    private readonly TextOverlay textOverlay = new();
    private string theme;

    public BabaMap MapData { get; }

    public MapViewer(BabaMap mapData, BabaWorld world, MapSimulator simulator)
    {
        MapData = mapData;
        this.world = world;
        this.simulator = simulator;
        textOverlay.Graphics.zindex = 1000;
        theme = world.Regions.TryGetValue(MapData.region, out var region) ? region.Theme ?? "default" : "default";
        AddChild(textOverlay);

        var bgColor = PaletteInfo.Palettes[theme][0];

        var rect = new RectangleSprite();
        rect.xscale = mapData.width;
        rect.yscale = mapData.height;
        rect.SetColor(bgColor);
        Graphics.AddChild(rect);

        foreach (var (obj, index) in mapData.WorldObjects.Select((x, i) => (x, i)))
        {
            if (obj == null) continue;
            addSprite(index);
        }
    }

    public void Load()
    {
        simulator.doJoinables();
        ensureSprites();
        foreach (var data in MapData.WorldObjects)
		{
            var sprite = sprites[data.index];
			sprite.OnMove(data, false, false);
		}
	}

	public void onMove()
	{
        ensureSprites();
        foreach (var data in MapData.WorldObjects)
        {
            var sprite = sprites[data.index];
            sprite.OnMove(data, false, true);
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
        var indexesToRemove = sprites.Keys.ToList();

        foreach (var obj in MapData.WorldObjects)
        {
            if (obj.index >= 0 && !sprites.ContainsKey(obj.index))
            {
                addSprite(obj.index);
            }
            indexesToRemove.Remove(obj.index);
        }

        foreach (var index in indexesToRemove)
        {
            RemoveChild(sprites[index]);
            sprites.Remove(index);
        }
    }

	private void addSprite(int index)
	{
        var sprite = new ObjectSprite(theme);
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
                var hasColor = hasCount >= count ? "[44,ff,44]" : "[gray]";

                message += $"\n {activeColor}{reagent} [{reagent}] {hasColor}{hasCount}/{count}";
            }
        }
        if (!string.IsNullOrWhiteSpace(message))
        {
            textOverlay.AddText(message.Trim(), obj.x, obj.y, 1f / 60);
        }
    }
}
