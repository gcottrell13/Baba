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


    public MapSimulator(BabaWorld world, short mapId)
	{
        this.world = world;
        map = world.MapDatas[mapId];
        region = world.Regions.TryGetValue(map.region, out var r) ? r : null;
    }

    public void GetNeighbors()
    {
        world.Simulators.TryGetValue(map.northNeighbor, out north);
        world.Simulators.TryGetValue(map.southNeighbor, out south);
        world.Simulators.TryGetValue(map.westNeighbor, out west);
        world.Simulators.TryGetValue(map.eastNeighbor, out east);
    }

    public void Step(Direction input, RuleDict rulesFromAbove, int playerNumber)
    {
        var all = new HashSet<ObjectTypeId>(map.WorldObjects.Select(x => x.Name));

        var rules = parseRules(rulesFromAbove);

        doMovement(input, rules, playerNumber);

    }

    private void doMovement(Direction input, RuleDict allRules, int playerNumber)
    {
        var movingObjects = new List<(ObjectData obj, int dx, int dy)>();

        if (input != Direction.None)
        {
            // you and you2
            var youRule = playerNumber switch { 
                1 => ObjectTypeId.you, 
                2 => ObjectTypeId.you2, 
                _ => throw new InvalidOperationException(), 
            };
            var you = findObjectsThatAre(youRule, allRules);
            var (dx, dy) = DirectionExtensions.DeltaFromDirection(input);
            movingObjects.AddRange(you.Select(i => (i, dx, dy)));
        }
        else
        {
            movingObjects.AddRange(findObjectsThatAre(ObjectTypeId.chill, allRules).Select(i => (i, 1, 0)));
        }

        foreach (var (obj, dx, dy) in movingObjects)
        {
            if (push(obj.x, obj.y, dx, dy, allRules))
                pull(obj.x, obj.y, dx, dy, allRules);
        }
    }

    public RuleDict parseRules(RuleDict rulesFromAbove)
    {
        var mapRules = SemanticFilter.FindRulesAndFilterInvalid(map.WorldObjects.Where(x => x.Kind == ObjectKind.Text).ToList());
        var dict = new RuleDict();

        foreach (var rule in rulesFromAbove) dict.Add(rule.Key, rule.Value);

        addRules(dict, mapRules);

        return dict;
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

    public IEnumerable<ObjectData> findObjectsThatAre(ObjectTypeId property, RuleDict allRules)
    {
        var rules = allRules[property];
        if (rules.Count == 0) return Array.Empty<ObjectData>();

        return map.WorldObjects.Where(x => isObject(x, property, allRules));
    }

    public bool isObject(ObjectData obj, ObjectTypeId property, RuleDict allRules)
    {
        if (obj.Name == property) return true;

        foreach (var rule in allRules[property])
        {
            if (rule.Verb.Name != ObjectTypeId.@is) continue;

            if (rule.LHS is NounAdjective<ObjectData> na && (na.Value.Name == obj.Name) != na.Not)
            {
                if (rule.RHS is NounAdjective<ObjectData> rhs) 
                    return (rhs.Value.Name == property) != rhs.Not;
            }
            else if (rule.LHS is NA_WithRelationship<ObjectData> wr && (wr.Target.Value.Name == obj.Name) != wr.Target.Not)
            {
                if (relation(obj, wr, allRules)) return true;
            }
        }
        return false;
    }

    private bool relation(ObjectData obj, NA_WithRelationship<ObjectData> wr, RuleDict allRules)
    {
        if (wr.Relation.Name == ObjectTypeId.on)
        {
            if (wr.RelatedTo is NA_WithRelationship<ObjectData> relatedTo)
                return map.WorldObjects.Any(other => other.index != obj.index && (other.X == obj.X && other.Y == obj.Y) != wr.Not && relation(other, relatedTo, allRules) != relatedTo.Not);
            else if (wr.RelatedTo is NounAdjective<ObjectData> na) 
                return map.WorldObjects.Any(other => other.index != obj.index && (other.X == obj.X && other.Y == obj.Y) != wr.Not && isObject(other, na.Value.Name, allRules) != na.Not);
        }
        else if (wr.Relation.Name == ObjectTypeId.feeling)
        {
            if (wr.RelatedTo is NounAdjective<ObjectData> relatedToNa)
            {
                return isObject(obj, relatedToNa.Value.Name, allRules) != relatedToNa.Not;
            }
        }
        throw new Exception();
    }

    public bool push(int x, int y, int dx, int dy, RuleDict allRules)
    {
        var allObjects = new List<ObjectData>();
        ObjectData[] inFront;
        while ((inFront = objectsAt(x + dx, y + dy)).Length > 0)
        {
            if (inFront.Any(x => x.Name == mapBorderTypeId || isObject(x, ObjectTypeId.stop, allRules))) return false;
            allObjects.AddRange(inFront);
            x += dx;
            y += dy;
        }
        foreach (var obj in allObjects)
        {
            moveObjectTo(obj, obj.x + dx, obj.y + dy);
        }
        return true;
    }

    public void pull(int x, int y, int dx, int dy, RuleDict allRules)
    {
        var allObjects = new List<ObjectData>();
        ObjectData[] behind;
        while ((behind = objectsAt(x - dx, y - dy)).Length > 0)
        {
            var pullObjects = behind.Where(x => isObject(x, ObjectTypeId.pull, allRules)).ToList();
            if (pullObjects.Count == 0) break;
            allObjects.AddRange(pullObjects);
            x -= dx;
            y -= dy;
        }
        foreach (var obj in allObjects)
        {
            moveObjectTo(obj, obj.x + dx, obj.y + dy);
        }
    }

    public bool moveObjectTo(ObjectData obj, int x, int y)
    {
        if (x < 0)
        {
            obj.Present = false;
            return west?.addObjectAt(obj, west.map.width + x, (int)(y * west.map.height / (float)map.height)) ?? false;
        }
        if (y < 0)
        {
            obj.Present = false;
            return north?.addObjectAt(obj, (int)(x * north.map.width / (float)map.width), north.map.height + y) ?? false;
        }
        if (x >= map.width)
        {
            obj.Present = false;
            return east?.addObjectAt(obj, x - map.width, (int)(y * east.map.height / (float)map.height)) ?? false;
        }
        if (y >= map.height)
        {
            obj.Present = false;
            return south?.addObjectAt(obj, (int)(x * south.map.width / (float)map.width), y - map.height) ?? false;
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

    public ObjectData[] objectsAt(int x, int y)
    {
        if (x < 0)
        {
            return west?.objectsAt(west.map.width + x, (int)(y * west.map.height / (float)map.height)) ?? mapBorder;
        }
        if (y < 0)
        {
            return north?.objectsAt((int)(x * north.map.width / (float)map.width), north.map.height + y) ?? mapBorder;
        }
        if (x >= map.width)
        {
            return east?.objectsAt(x - map.width, (int)(y * east.map.height / (float)map.height)) ?? mapBorder;
        }
        if (y >= map.height)
        {
            return south?.objectsAt((int)(x * south.map.width / (float)map.width), y - map.height) ?? mapBorder;
        }
        return map.WorldObjects.Where(obj => obj.x == x && obj.y == y).ToArray();
    }

}
