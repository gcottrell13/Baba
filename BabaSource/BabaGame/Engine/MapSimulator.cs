using BabaGame.Events;
using BabaGame.Objects;
using Core.Content;
using Core.Engine;
using Core.Utils;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BabaGame.Engine;


public class RuleDict : Dictionary<ObjectTypeId, List<Rule<BabaObject>>>
{
    public new List<Rule<BabaObject>> this[ObjectTypeId key]
    {
        get => this.ConstructDefaultValue(key); 
    }
}


public class MapSimulator
{
    public readonly BabaWorld world;
    public readonly RegionData? region;

    private readonly BabaMap map;
    private readonly BabaMap? upLayer;

    private MapSimulator? north;
    private MapSimulator? south;
    private MapSimulator? east;
    private MapSimulator? west;

    private Dictionary<int, int> convertToNorth = new();
    private Dictionary<int, int> convertToSouth = new();
    private Dictionary<int, int> convertToEast = new();
    private Dictionary<int, int> convertToWest = new();

    public RuleDict allRules = new();
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
        upLayer = world.MapDatas.TryGetValue(map.upLayer, out var layer) ? layer : null;
        region = world.Regions.TryGetValue(map.region, out var r) ? r : null;
    }

    public void OnUnload()
    {
        // look for LEVEL IS TRANS
        // call map.ResetToOriginalState()
    }

    public void GetNeighbors()
    {
        if (world.Simulators.TryGetValue(map.northNeighbor, out north)) setupConvertCoordinates(map.width, north.map.width, out convertToNorth);
        if (world.Simulators.TryGetValue(map.southNeighbor, out south)) setupConvertCoordinates(map.width, south.map.width, out convertToSouth);
        if (world.Simulators.TryGetValue(map.westNeighbor, out west)) setupConvertCoordinates(map.height, west.map.height, out convertToWest);
        if (world.Simulators.TryGetValue(map.eastNeighbor, out east)) setupConvertCoordinates(map.height, east.map.height, out convertToEast);
    }

    public bool HasNeighbor(short mapId, out Direction dir)
    {
        dir = Direction.None;
        if (north?.MapId == mapId) dir = Direction.Up;
        if (south?.MapId == mapId) dir = Direction.Down;
        if (west?.MapId == mapId) dir = Direction.Left;
        if (east?.MapId == mapId) dir = Direction.Right;
        return dir != Direction.None;
    }

    public void MarkVisited()
    {
        map.Visited = true;
        if (upLayer != null) upLayer.Visited = true;
    }

    public bool TryRemoveObject(BabaObject obj)
    {
        map.RemoveObject(obj);
        return true;
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

    private List<MapMovementStackItem> tryMoveObject(BabaObject obj, int dx, int dy)
    {
        var moves = new List<MapMovementStackItem>();
        if (moveObjectTo(obj, obj.x + dx, obj.y + dy) is MapMovementStackItem m)
        {
            moves.Add(m);
            var p = push(obj.x, obj.y, dx, dy, m);
            if (p != null) moves.AddRange(p);
        }
        return moves;
    }

    public IEnumerable<MapMovementStackItem> moveYou(Direction input)
    {
        var movingObjects = new List<MapMovementStackItem>();

        if (input != Direction.None)
        {
            var you = findObjectsThatAre(ObjectTypeId.you).ToList();
            var (dx, dy) = DirectionExtensions.DeltaFromDirection(input);
            foreach (var obj in you)
            {
                obj.Facing = input;
                movingObjects.AddRange(tryMoveObject(obj, dx, dy));
            }
        }
        else
        {
            foreach (var obj in findObjectsThatAre(ObjectTypeId.idle).ToList())
            {
                var (dx, dy) = DirectionExtensions.DeltaFromDirection(obj.Facing);
                movingObjects.AddRange(tryMoveObject(obj, dx, dy));
            };
        }

        return movingObjects;
    }

    public IEnumerable<MapMovementStackItem> doMovement()
    {
        var movingObjects = new List<MapMovementStackItem>();

        // chill has a low chance of moving on any turn, and will move in a random direction
        movingObjects.AddRange(findObjectsThatAre(ObjectTypeId.chill).ToList().SelectMany(i =>
        {
            // TODO: randomly pick CHILL objects to move in a random direction
            return tryMoveObject(i, 1, 0);
        }));
        // Find all MOVE objects and add them with the delta corresponding to their direction
        movingObjects.AddRange(findObjectsThatAre(ObjectTypeId.move).ToList().SelectMany(i =>
        {
            var (dx, dy) = DirectionExtensions.DeltaFromDirection(i.Facing);
            return tryMoveObject(i, dx, dy);
        }));

        return movingObjects;
    }

    public void transform()
    {

    }

    public IEnumerable<MapMovementStackItem> moveblock()
    {
        return new List<MapMovementStackItem>();
    }

    public IEnumerable<MapMovementStackItem> fallblock()
    {
        return new List<MapMovementStackItem>();
    }

    public void statusblock()
    {

    }

    public void interactionblock()
    {
        this.Hot();
        this.Save();
        this.Need();
    }

    public bool collisionCheck(BabaObject n)
    {
        return objectsAt(n.x, n.y).Any(at => isObject(at, ObjectTypeId.stop) && at != n);
    }

    public void removeDuplicatesInSamePosition()
    {
        void dup(BabaMap thismap)
        {
            var positions = new Dictionary<ObjectTypeId, List<(int, int, Direction)>>();
            foreach (var item in thismap.WorldObjects.ToList())
            {
                var list = positions.ConstructDefaultValue(item.Name);
                if (list.Contains((item.x, item.y, item.Facing)))
                    thismap.RemoveObject(item);
                else
                    list.Add((item.x, item.y, item.Facing));
            }
        }
        dup(map);
        //if (upLayer != null) dup(upLayer);
    }

    public void doJoinables()
    {
        foreach (var type in SheetMap.JoinableObjects)
        {
            var objects = findObjectsThatAre(type).ToList();
            if (objects.Count == 0) continue;

            var d = objects.Select(x => (x.x, x.y)).ToHashSet();

            foreach (var obj in objects)
            {
                var state = Direction.None;
                if (d.Contains((obj.x - 1, obj.y))) state |= Direction.Left;
                if (d.Contains((obj.x + 1, obj.y))) state |= Direction.Right;
                if (d.Contains((obj.x, obj.y - 1))) state |= Direction.Up;
                if (d.Contains((obj.x, obj.y + 1))) state |= Direction.Down;
                obj.Facing = state;
            }
        }
    }

    public void findSpecialPropertiesToDisplay()
    {
        var p = new Dictionary<ObjectTypeId, ObjectStatesToDisplay>()
        {
            { ObjectTypeId.@float, ObjectStatesToDisplay.Float },
            { ObjectTypeId.big, ObjectStatesToDisplay.Big },
            { ObjectTypeId.sleep, ObjectStatesToDisplay.Sleep },
            { ObjectTypeId.powered, ObjectStatesToDisplay.Powered },
            { ObjectTypeId.powered2, ObjectStatesToDisplay.Powered2 },
            { ObjectTypeId.powered3, ObjectStatesToDisplay.Powered3 },
            { ObjectTypeId.party, ObjectStatesToDisplay.Party },
            { ObjectTypeId.pet, ObjectStatesToDisplay.Pet },
            { ObjectTypeId.phantom, ObjectStatesToDisplay.Phantom },
            { ObjectTypeId.best, ObjectStatesToDisplay.Best },
            { ObjectTypeId.broken, ObjectStatesToDisplay.Broken },
            { ObjectTypeId.sad, ObjectStatesToDisplay.Sad },
            { ObjectTypeId.tele, ObjectStatesToDisplay.Tele },
            { ObjectTypeId.win, ObjectStatesToDisplay.Win },
            { ObjectTypeId.hide, ObjectStatesToDisplay.Hidden },
        };

        var props = p.Where(i => allRules.ContainsKey(i.Key)).ToList();

        foreach (var obj in map.WorldObjects)
        {
            obj.state = 0;
            foreach (var (type, display) in props)
            {
                if (isObject(obj, type)) obj.state |= display;
            }
        }
    }

    public Dictionary<(int x, int y), List<BabaObject>> ToGrid(IEnumerable<BabaObject> objects)
    {
        var d = new Dictionary<(int x, int y), List<BabaObject>>();
        foreach (var obj in objects)
        {
            d.ConstructDefaultValue((obj.x, obj.y)).Add(obj);
        }
        return d;
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
            RuleDict parseMap(BabaMap thismap)
            {
                var texts = thismap.WorldObjects.Where(x => x.Kind == ObjectKind.Text).ToList();
                var rules = SemanticFilter.FindRulesAndFilterInvalid(texts);

                foreach (var t in texts)
                    t.Active = false;
                foreach (var rule in rules)
                {
                    if (rule.Verb.Name == ObjectTypeId.need)
                    {
                        parseMultiplier(rule.GetSentenceMembers().ToList());
                    }
                    foreach (var member in rule.GetSentenceMembers())
                        member.Active = true;
                }

                var dict = new RuleDict();
                addRules(dict, rules);
                return dict;
            }
            dict = parseMap(map);
            if (upLayer != null)
            {
                var upLayerRules = parseMap(upLayer);
                addRules(dict, upLayerRules.Values.SelectMany(x => x).ToList());
            }
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
        foreach (var (key, value) in rulesFromAbove)
        {
            dict[key].AddRange(value);
        }
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
            if (rule.Verb.Name == ObjectTypeId.need && rule.LHS is NounAdjective<BabaObject> lhs && lhs.Value.Name == name && rule.RHS is NounAdjective<BabaObject> na)
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

    public IEnumerable<MapMovementStackItem>? push(int x, int y, int dx, int dy, MapMovementStackItem instigator)
    {
        var movingObjects = new List<MapMovementStackItem>();
        var allObjects = new List<BabaObject>();
        while (x >= 0 && x < map.width && y >= 0 && y < map.height)
        {
            var inFront = objectsAt(x + dx, y + dy);
            if (inFront.Any(x => x.Name == mapBorderTypeId)) return null;
            inFront = inFront.Where(x => isObject(x, ObjectTypeId.push)).ToArray();
            x += dx;
            y += dy;
            if (inFront.Length == 0) break;
            allObjects.AddRange(inFront);
        }

        if (x < 0 && west?.push(west.map.width - 1, convertToWest[y], dx, dy, instigator) is IEnumerable<MapMovementStackItem> westMoved) movingObjects.AddRange(westMoved);
        if (y < 0 && north?.push(convertToNorth[x], north.map.height - 1, dx, dy, instigator) is IEnumerable<MapMovementStackItem> northMoved) movingObjects.AddRange(northMoved);
        if (x >= map.width && east?.push(0, convertToEast[y], dx, dy, instigator) is IEnumerable<MapMovementStackItem> eastMoved) movingObjects.AddRange(eastMoved);
        if (y >= map.height && south?.push(convertToSouth[x], 0, dx, dy, instigator) is IEnumerable<MapMovementStackItem> southMoved) movingObjects.AddRange(southMoved);

        foreach (var obj in allObjects)
        {
            if (moveObjectTo(obj, obj.x + dx, obj.y + dy) is MapMovementStackItem m)
                movingObjects.Add(m with { dependentOn=instigator });
        }
        return movingObjects;
    }

    public MapMovementStackItem? moveObjectTo(BabaObject obj, int x, int y)
    {
        if (x < 0 && west != null)
        {
            return moveObjectToMap(west, obj, west.map.width + x, convertToWest[y]);
        }
        if (y < 0 && north != null)
        {
            return moveObjectToMap(north, obj, convertToNorth[x], north.map.height + y);
        }
        if (x >= map.width && east != null)
        {
            return moveObjectToMap(east, obj, x - map.width, convertToEast[y]);
        }
        if (y >= map.height && south != null)
        {
            return moveObjectToMap(south, obj, convertToSouth[x], y - map.height);
        }
        var retval = new MapMovementStackItem(obj, MapId, obj.x, obj.y, null);
        obj.x = x;
        obj.y = y;
        return retval;
    }

    public bool addObjectAt(BabaObject obj, int x, int y)
    {
        map.AddObject(obj);
        obj.x = x;
        obj.y = y;
        return true;
    }

    private MapMovementStackItem moveObjectToMap(MapSimulator m, BabaObject obj, int x, int y)
    {
        var retVal = new MapMovementStackItem(obj, MapId, obj.x, obj.y, null);
        map.RemoveObject(obj);
        m.addObjectAt(obj, x, y);
        return retVal;
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
        var theX = objectsAt(x, y).FirstOrDefault();

        if (theX == null || theX.Name != ObjectTypeId.x)
            return 1;

        x += dx;
        y += dy;
        var current = objectsAt(x, y).FirstOrDefault();
        var multiplier = 0;
        while (current != null)
        {
            var count = current.Name - ObjectTypeId.zero;
            if (count < 0 || count > 9) 
                break;
            current.Active = true;

            multiplier *= 10;
            multiplier += count;
            x += dx;
            y += dy;
            current = objectsAt(x, y).FirstOrDefault();
        }
        if (multiplier > 1) 
            theX.Active = true;
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
        { ObjectTypeId.stop, /* implied by */ new[] { ObjectTypeId.you, ObjectTypeId.you2, mapBorderTypeId } },
        { ObjectTypeId.push, new[] { ObjectTypeId.text } },  
    };
}
