using BabaGame.Events;
using Core.Content;
using Core.Engine;
using Core.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BabaGame.Engine;


public class RuleDict : Dictionary<ObjectTypeId, List<Rule<BabaObject>>>
{
    public new List<Rule<BabaObject>> this[ObjectTypeId key]
    {
        get => this.ConstructDefaultValue(key); set => TryAdd(key, value);
    }
}


public class MapSimulator
{
    private readonly BabaWorld world;
    public readonly RegionData? region;

    private readonly BabaMap map;

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


    public const ObjectTypeId mapBorderTypeId = ObjectTypeId.nnope;
    private static readonly BabaObject[] mapBorder = new[] { new BabaObject() { Name = mapBorderTypeId } };

    public short MapId { get; }


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

    public bool doMovement(Direction input, ObjectTypeId playerNumber)
    {
        var movingObjects = new List<(BabaObject obj, int dx, int dy)>();

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

        var didAnyMove = false;

        foreach (var (obj, dx, dy) in movingObjects)
        {
            if (push(obj.x, obj.y, dx, dy))
            {
                pull(obj.x, obj.y, dx, dy);
                moveObjectTo(obj, obj.x + dx, obj.y + dy);
                if (obj.Present == false)
                {
                    // switch map
                    EventChannels.MapChange.SendAsyncMessage(new() { MapId = obj.CurrentMapId });
                }
                didAnyMove = true;
            }
            if (input != Direction.None)
                obj.Facing = input;
        }
        return didAnyMove;
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
            var texts = map.WorldObjects.Where(x => x.Kind == ObjectKind.Text).ToList();
            var rules = SemanticFilter.FindRulesAndFilterInvalid(texts);

            foreach (var t in texts)
                t.Active = false;
            foreach (var rule in rules)
                foreach (var member in rule.GetSentenceMembers())
                    member.Active = true;

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

    public static void addRules(RuleDict dict, List<Rule<BabaObject>> rules)
    {
        foreach (var rule in rules)
        {
            var na = rule.LHS as NounAdjective<BabaObject>;
            na ??= (rule.LHS as NA_WithRelationship<BabaObject>)!.Target;

            dict[na.Value.Name].Add(rule);
            if (rule.RHS is NounAdjective<BabaObject> rhs)
            {
                dict[rhs.Value.Name].Add(rule);
            }
        }
    }

    public IEnumerable<BabaObject> findObjectsThatAre(ObjectTypeId property)
    {
        var rules = allRules[property];
        if (rules.Count == 0) return Array.Empty<BabaObject>();

        return map.WorldObjects.Where(x => isObject(x, property));
    }

    public bool isObject(BabaObject obj, ObjectTypeId property)
    {
        // all text objects are treated as the same kind of object
        var name = obj.Kind == ObjectKind.Text ? ObjectTypeId.text : obj.Name;

        if (name == property) return true;
        if (obj.Present == false || obj.Deleted) return false;

        // rule checking
        foreach (var rule in allRules[property])
        {
            if (rule.Verb.Name != ObjectTypeId.@is) continue;

            if (rule.LHS is NounAdjective<BabaObject> na && (na.Value.Name == name) != na.Not)
            {
                if ((na.Value.Name == ObjectTypeId.text) != (obj.Kind == ObjectKind.Text)) continue;
                if (rule.RHS is NounAdjective<BabaObject> rhs) 
                    return (rhs.Value.Name == property) != rhs.Not;
            }
            else if (rule.LHS is NA_WithRelationship<BabaObject> wr && (wr.Target.Value.Name == name) != wr.Target.Not)
            {
                if (relation(obj, wr)) return true;
            }
        }

        if (impliedBy.ContainsKey(property))
        {
            if (impliedBy[property].Any(otherProp => isObject(obj, otherProp))) 
                return true;
        }
        return false;
    }

    public Dictionary<ObjectTypeId, int> doesObjectNeedAnything(BabaObject obj)
    {
        var name = obj.Name;
        if (obj.Kind == ObjectKind.Text) name = ObjectTypeId.text;

        var dict = new Dictionary<ObjectTypeId, int>();

        foreach (var rule in allRules[name])
        {
            if (rule.Verb.Name == ObjectTypeId.need && rule.RHS is NounAdjective<BabaObject> na)
            {
                var count = parseMultiplier(rule.GetSentenceMembers().ToList());
                if (dict.ContainsKey(na.Value.Name)) dict[na.Value.Name] += count;
                else dict[na.Value.Name] = count;
            }
        }

        return dict;
    }

    private bool relation(BabaObject obj, NA_WithRelationship<BabaObject> wr)
    {
        if (wr.Relation.Name == ObjectTypeId.on)
        {
            if (wr.RelatedTo is NA_WithRelationship<BabaObject> relatedTo)
                return map.WorldObjects.Any(other => other.index != obj.index && (other.X == obj.X && other.Y == obj.Y) != wr.Not && relation(other, relatedTo) != relatedTo.Not);
            else if (wr.RelatedTo is NounAdjective<BabaObject> na) 
                return map.WorldObjects.Any(other => other.index != obj.index && (other.X == obj.X && other.Y == obj.Y) != wr.Not && isObject(other, na.Value.Name) != na.Not);
        }
        else if (wr.Relation.Name == ObjectTypeId.feeling)
        {
            if (wr.RelatedTo is NounAdjective<BabaObject> relatedToNa)
            {
                return isObject(obj, relatedToNa.Value.Name) != relatedToNa.Not;
            }
        }
        throw new Exception();
    }

    public bool push(int x, int y, int dx, int dy)
    {
        var allObjects = new List<BabaObject>();
        while (x >= 0 && x < map.width && y >= 0 && y < map.height)
        {
            var inFront = objectsAt(x + dx, y + dy);
            if (inFront.Any(x => x.Name == mapBorderTypeId || isObject(x, ObjectTypeId.stop))) return false;
            inFront = inFront.Where(x => isObject(x, ObjectTypeId.push)).ToArray();
            x += dx;
            y += dy;
            if (inFront.Length == 0) break;
            allObjects.AddRange(inFront);
        }

        var canMove = true;
        if (x < 0) canMove = west!.push(west.map.width - 1, convertToWest[y], dx, dy);
        if (y < 0) canMove = north!.push(convertToNorth[x], north.map.height - 1, dx, dy);
        if (x >= map.width) canMove = east!.push(0, convertToEast[y], dx, dy);
        if (y >= map.height) canMove = south!.push(convertToSouth[x], 0, dx, dy);

        if (!canMove) return false;

        var dir = DirectionExtensions.DirectionFromDelta((dx, dy));
        foreach (var obj in allObjects)
        {
            obj.Facing = dir;
            moveObjectTo(obj, obj.x + dx, obj.y + dy);
        }
        return true;
    }

    public void pull(int x, int y, int dx, int dy)
    {
        var allObjects = new List<BabaObject>();
        while (x >= dx && x < map.width + dx && y >= dy && y < map.height + dy)
        {
            var pullObjects = objectsAt(x - dx, y - dy).Where(x => isObject(x, ObjectTypeId.pull)).ToList();
            if (pullObjects.Count == 0) break;
            allObjects.AddRange(pullObjects);
            x -= dx;
            y -= dy;
        }

        var dir = DirectionExtensions.DirectionFromDelta((dx, dy));
        foreach (var obj in allObjects)
        {
            obj.Facing = dir;
            moveObjectTo(obj, obj.x + dx, obj.y + dy);
        }

        x -= dx;
        y -= dy;

        if (x < 0) west?.pull(west.map.width, convertToWest[y], dx, dy);
        if (y < 0) north?.pull(convertToNorth[x], north.map.height, dx, dy);
        if (x >= map.width) east?.pull(0, convertToEast[y], dx, dy);
        if (y >= map.height) south?.pull(convertToSouth[x], 0, dx, dy);
    }

    public bool moveObjectTo(BabaObject obj, int x, int y)
    {
        if (x < 0)
        {
            obj.Present = false;
            return west != null && moveObjectToMap(west, obj, west.map.width + x, convertToWest[y]);
        }
        if (y < 0)
        {
            obj.Present = false;
            return north != null && moveObjectToMap(north, obj, convertToNorth[x], north.map.height + y);
        }
        if (x >= map.width)
        {
            obj.Present = false;
            return moveObjectToMap(east, obj, x - map.width, convertToEast[y]);
        }
        if (y >= map.height)
        {
            obj.Present = false;
            return moveObjectToMap(south, obj, convertToSouth[x], y - map.height);
        }

        var dx = x - obj.x;
        var dy = y - obj.y;
        if (Math.Abs(dx) > Math.Abs(dy))
        {
            obj.Facing = dx < 0 ? Direction.Left : Direction.Right;
        }
        else
        {
            obj.Facing = dy < 0 ? Direction.Up : Direction.Down;
        }

        obj.x = x;
        obj.y = y;
        return true;
    }

    public bool addObjectAt(BabaObject obj, int x, int y)
    {
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
            index=-1,
        });
        return true;
    }

    private bool moveObjectToMap(MapSimulator? m, BabaObject obj, int x, int y)
    {
        if (m is MapSimulator otherMap && otherMap.addObjectAt(obj, x, y))
        {
            this.map.RemoveObject(obj);
            obj.CurrentMapId = otherMap.MapId;
            return true;
        }
        return false;
    }

    public BabaObject[] objectsAt(int x, int y)
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

    public int parseMultiplier(List<BabaObject> ruleMembers)
    {
        var dx = Math.Sign(ruleMembers.Last().x - ruleMembers.First().x);
        var dy = Math.Sign(ruleMembers.Last().y - ruleMembers.First().y);

        if (dx == dy) return 1; // this shouldn't happen though

        var x = ruleMembers.Select(item => item.x).Max();
        var y = ruleMembers.Select(item => item.y).Max();

        x += dx;
        y += dy;
        var current = objectsAt(x, y).FirstOrDefault();

        if (current == null || current.Name != ObjectTypeId.x)
            return 1;

        x += dx;
        y += dy;
        current = objectsAt(x, y).FirstOrDefault();
        var multiplier = 0;
        while (current != null)
        {
            var count = current.Name - ObjectTypeId.zero;
            if (count < 0 || count > 9) return multiplier;

            multiplier *= 10;
            multiplier += count;
            x += dx;
            y += dy;
            current = objectsAt(x, y).FirstOrDefault();
        }
        return multiplier;
    }

    public IEnumerable<BabaObject> GetNeighbors(BabaObject obj)
    {
        foreach (var north in objectsAt(obj.x, obj.y - 1)) yield return north;
        foreach (var south in objectsAt(obj.x, obj.y + 1)) yield return south;
        foreach (var east in objectsAt(obj.x + 1, obj.y)) yield return east;
        foreach (var west in objectsAt(obj.x - 1, obj.y)) yield return west;
    }

    private static Dictionary<ObjectTypeId, ObjectTypeId[]> impliedBy = new()
    {
        { ObjectTypeId.stop, /* implied by */ new[] { ObjectTypeId.you, ObjectTypeId.you2 } },
        { ObjectTypeId.push, new[] { ObjectTypeId.text } },  
    };
}
