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
    private readonly RegionData? region;

    private readonly MapData map;


    public MapSimulator(BabaWorld world, short mapId)
	{
        this.world = world;
        map = world.Maps[mapId];
        region = world.Regions.TryGetValue(map.region, out var r) ? r : null;
    }

    public void Step(Direction input, List<Rule<ObjectData>> rulesFromAbove, int playerNumber)
    {
        var all = new HashSet<ObjectTypeId>(map.WorldObjects.Select(x => x.Name));

        var rules = parseRules(rulesFromAbove);

        doMovement(input, rules, playerNumber);

    }

    private void doMovement(Direction input, RuleDict allRules, int playerNumber)
    {
        if (input != Direction.None)
        {
            // you and you2
            var youRule = playerNumber switch { 
                1 => ObjectTypeId.you, 
                2 => ObjectTypeId.you2, 
                _ => throw new InvalidOperationException(), 
            };
            findObjectsThatAre(youRule, allRules);

        }

    }

    public RuleDict parseRules(List<Rule<ObjectData>> rulesFromAbove)
    {
        var mapRules = SemanticFilter.FindRulesAndFilterInvalid(map.WorldObjects.Where(x => x.Kind == ObjectKind.Text).ToList());
        var dict = new RuleDict();

        addRules(dict, rulesFromAbove);
        addRules(dict, mapRules);

        void addRules(RuleDict dict, List<Rule<ObjectData>> rules)
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

        return dict;
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
}
