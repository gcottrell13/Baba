using Core.Content;
using Core.Engine;
using Core.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Engine;


public class RuleDict : Dictionary<ObjectTypeId, List<Rule<ObjectData>>>
{
    public new List<Rule<ObjectData>> this[ObjectTypeId key]
    {
        get => this.ConstructDefaultValue(key); set => TryAdd(key, value);
    }
}


public class MapSimulator
{
    private readonly BabaWorld world;
    public readonly RegionData? region;

    private readonly MapData map;

    private MapSimulator? north;
    private MapSimulator? south;
    private MapSimulator? east;
    private MapSimulator? west;

    private Dictionary<int, int> convertToNorth = new();
    private Dictionary<int, int> convertToSouth = new();
    private Dictionary<int, int> convertToEast = new();
    private Dictionary<int, int> convertToWest = new();

    private RuleDict allRules = new();
    private RuleDict cachedRulesFromAbove = new();
    private RuleDict cachedParsedRules = new();

    public short? NorthNeighbor => north?.MapId;
    public short? EastNeighbor => east?.MapId;
    public short? WestNeighbor => west?.MapId;
    public short? SouthNeighbor => south?.MapId;

    public short Width => map.width;
    public short Height => map.height;


    public MapSimulator(BabaWorld world, short mapId)
	{
        this.world = world;
        MapId = mapId;
        map = world.MapDatas[mapId];
        region = world.Regions.TryGetValue(map.region, out var r) ? r : null;
    }

    public void GetNeighbors()
    {
        if (world.Simulators.TryGetValue(map.northNeighbor, out north)) setupConvertCoordinates(map.width, north.map.width, out convertToNorth);
        if (world.Simulators.TryGetValue(map.southNeighbor, out south)) setupConvertCoordinates(map.width, south.map.width, out convertToSouth);
        if (world.Simulators.TryGetValue(map.westNeighbor, out west)) setupConvertCoordinates(map.height, west.map.height, out convertToWest);
        if (world.Simulators.TryGetValue(map.eastNeighbor, out east)) setupConvertCoordinates(map.height, east.map.height, out convertToEast);
    }

    public bool HasNeighbor(short mapId)
    {
        return north?.MapId == mapId || south?.MapId == mapId || west?.MapId == mapId || east?.MapId == mapId;
    }

    /// <summary>
    /// cache the math needed to move objects between maps
    /// </summary>
    /// <param name="thisDim"></param>
    /// <param name="otherDim"></param>
    /// <param name="dict"></param>
    private void setupConvertCoordinates(int thisDim, int otherDim, out Dictionary<int, int> dict)
    {
        dict = new();
        for (var i = 0; i < thisDim; i++)
        {
            dict[i] = (int)(otherDim * (float)i / thisDim);
        }
    }

    public void doMovement(Direction input, ObjectTypeId playerNumber)
    {
        var movingObjects = new List<(ObjectData obj, int dx, int dy)>();

        if (input != Direction.None)
        {
            var you = findObjectsThatAre(playerNumber);
            var (dx, dy) = DirectionExtensions.DeltaFromDirection(input);
            movingObjects.AddRange(you.Select(i => (i, dx, dy)));
        }
        else
        {
            movingObjects.AddRange(findObjectsThatAre(ObjectTypeId.chill).Select(i => (i, 1, 0)));
        }

        foreach (var (obj, dx, dy) in movingObjects)
        {
            if (push(obj.x, obj.y, dx, dy))
                pull(obj.x, obj.y, dx, dy);
        }
    }

    public void transform()
    {

    }

    public void moveblock()
    {

    }

    public void fallblock()
    {

    }

    public void statusblock()
    {

    }

    public void interactionblock()
    {

    }

    public void collisionCheck()
    {

    }

    public void setAllRules(RuleDict rules)
    {
        allRules = rules;
    }

    public RuleDict parseRules(RuleDict rulesFromAbove)
    {
        var dict = allRules;
        if (hasChanged())
        {
            var rules = SemanticFilter.FindRulesAndFilterInvalid(map.WorldObjects.Where(x => x.Kind == ObjectKind.Text).ToList());
            dict = new RuleDict();
            addRules(dict, rules);
            cachedParsedRules = dict;
            goto applyRulesFromAbove;
        }

        if (rulesFromAbove.Equals(cachedRulesFromAbove))
        {
            goto theReturn;
        }
        else
        {
            dict = cachedParsedRules;
            goto applyRulesFromAbove;
        }

        applyRulesFromAbove:
        cachedRulesFromAbove = rulesFromAbove;
        foreach (var rule in rulesFromAbove) dict[rule.Key] = dict.ContainsKey(rule.Key) ? dict[rule.Key].Concat(rule.Value).ToList() : rule.Value;
        setAllRules(dict);

        theReturn:
        return dict;
    }

    public bool hasChanged()
    {
        // TODO: implement
        // use some sort of hashing function on the contents of this map
        return true;
    }

    public static void addRules(RuleDict dict, List<Rule<ObjectData>> rules)
    {
        foreach (var rule in rules)
        {
            var na = rule.LHS as NounAdjective<ObjectData>;
            na ??= (rule.LHS as NA_WithRelationship<ObjectData>)!.Target;

            dict[na.Value.Name].Add(rule);
            if (rule.RHS is NounAdjective<ObjectData> rhs)
            {
                dict[rhs.Value.Name].Add(rule);
            }
        }
    }

    public IEnumerable<ObjectData> findObjectsThatAre(ObjectTypeId property)
    {
        var rules = allRules[property];
        if (rules.Count == 0) return Array.Empty<ObjectData>();

        return map.WorldObjects.Where(x => isObject(x, property));
    }

    public bool isObject(ObjectData obj, ObjectTypeId property)
    {
        if (obj.Name == property && obj.Kind == ObjectKind.Object) return true;

        foreach (var rule in allRules[property])
        {
            if (rule.Verb.Name != ObjectTypeId.@is) continue;

            if (rule.LHS is NounAdjective<ObjectData> na && (na.Value.Name == obj.Name) != na.Not)
            {
                if ((na.Value.Name == ObjectTypeId.text) != (obj.Kind == ObjectKind.Text)) continue;
                if (rule.RHS is NounAdjective<ObjectData> rhs) 
                    return (rhs.Value.Name == property) != rhs.Not;
            }
            else if (rule.LHS is NA_WithRelationship<ObjectData> wr && (wr.Target.Value.Name == obj.Name) != wr.Target.Not)
            {
                if (relation(obj, wr)) return true;
            }
        }
        return false;
    }

    private bool relation(ObjectData obj, NA_WithRelationship<ObjectData> wr)
    {
        if (wr.Relation.Name == ObjectTypeId.on)
        {
            if (wr.RelatedTo is NA_WithRelationship<ObjectData> relatedTo)
                return map.WorldObjects.Any(other => other.index != obj.index && (other.X == obj.X && other.Y == obj.Y) != wr.Not && relation(other, relatedTo) != relatedTo.Not);
            else if (wr.RelatedTo is NounAdjective<ObjectData> na) 
                return map.WorldObjects.Any(other => other.index != obj.index && (other.X == obj.X && other.Y == obj.Y) != wr.Not && isObject(other, na.Value.Name) != na.Not);
        }
        else if (wr.Relation.Name == ObjectTypeId.feeling)
        {
            if (wr.RelatedTo is NounAdjective<ObjectData> relatedToNa)
            {
                return isObject(obj, relatedToNa.Value.Name) != relatedToNa.Not;
            }
        }
        throw new Exception();
    }

    public bool push(int x, int y, int dx, int dy)
    {
        var allObjects = new List<ObjectData>();
        while (x >= 0 && x < map.width && y >= 0 && y < map.height)
        {
            var inFront = objectsAt(x + dx, y + dy);
            if (inFront.Any(x => x.Name == mapBorderTypeId || isObject(x, ObjectTypeId.stop))) return false;
            allObjects.AddRange(inFront);
            x += dx;
            y += dy;
        }

        var canMove = true;
        if (x < 0) canMove = west!.push(west.map.width, convertToWest[y], dx, dy);
        if (y < 0) canMove = north!.push(convertToNorth[x], north.map.height, dx, dy);
        if (x >= map.width) canMove = east!.push(0, convertToEast[y], dx, dy);
        if (y >= map.height) canMove = south!.push(convertToSouth[x], 0, dx, dy);

        if (!canMove) return false;

        foreach (var obj in allObjects)
        {
            moveObjectTo(obj, obj.x + dx, obj.y + dy);
        }
        return true;
    }

    public void pull(int x, int y, int dx, int dy)
    {
        var allObjects = new List<ObjectData>();
        while (x >= dx && x < map.width + dx && y >= dy && y < map.height + dy)
        {
            var pullObjects = objectsAt(x - dx, y - dy).Where(x => isObject(x, ObjectTypeId.pull)).ToList();
            if (pullObjects.Count == 0) break;
            allObjects.AddRange(pullObjects);
            x -= dx;
            y -= dy;
        }
        foreach (var obj in allObjects)
        {
            moveObjectTo(obj, obj.x + dx, obj.y + dy);
        }

        x -= dx;
        y -= dy;

        if (x < 0) west?.pull(west.map.width, convertToWest[y], dx, dy);
        if (y < 0) north?.pull(convertToNorth[x], north.map.height, dx, dy);
        if (x >= map.width) east?.pull(0, convertToEast[y], dx, dy);
        if (y >= map.height) south?.pull(convertToSouth[x], 0, dx, dy);
    }

    public bool moveObjectTo(ObjectData obj, int x, int y)
    {
        if (x < 0)
        {
            obj.Present = false;
            return west?.addObjectAt(obj, west.map.width + x, convertToWest[y]) ?? false;
        }
        if (y < 0)
        {
            obj.Present = false;
            return north?.addObjectAt(obj, convertToNorth[x], north.map.height + y) ?? false;
        }
        if (x >= map.width)
        {
            obj.Present = false;
            return east?.addObjectAt(obj, x - map.width, convertToEast[y]) ?? false;
        }
        if (y >= map.height)
        {
            obj.Present = false;
            return south?.addObjectAt(obj, convertToSouth[x], y - map.height) ?? false;
        }
        obj.x = x;
        obj.y = y;
        return true;
    }

    public bool addObjectAt(ObjectData obj, int x, int y)
    {
        obj.x = x;
        obj.y = y;
        map.AddObject(new()
        {
            Name = obj.Name,
            Present =true,
            y=y,
            x=x,
            Color=obj.Color,
            Facing=obj.Facing,
            Kind=obj.Kind,
            OriginX = obj.OriginX,
            OriginY = obj.OriginY,
            MapOfOrigin =obj.MapOfOrigin,
        });
        return true;
    }

    public bool removeObject(ObjectData obj)
    {
        obj.Deleted = true;
        return true;
    }

    public const ObjectTypeId mapBorderTypeId = ObjectTypeId.nnope;
    private static readonly ObjectData[] mapBorder = new[] { new ObjectData() { Name = mapBorderTypeId } };

    public short MapId { get; }

    public ObjectData[] objectsAt(int x, int y)
    {
        if (x < 0)
        {
            return west?.objectsAt(west.map.width + x, convertToWest[y]) ?? mapBorder;
        }
        if (y < 0)
        {
            return north?.objectsAt(convertToNorth[x], north.map.height + y) ?? mapBorder;
        }
        if (x >= map.width)
        {
            return east?.objectsAt(x - map.width, convertToEast[y]) ?? mapBorder;
        }
        if (y >= map.height)
        {
            return south?.objectsAt(convertToSouth[x], y - map.height) ?? mapBorder;
        }
        return map.WorldObjects.Where(obj => obj.x == x && obj.y == y).ToArray();
    }

}
