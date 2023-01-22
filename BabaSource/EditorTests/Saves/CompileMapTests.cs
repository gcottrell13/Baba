using Core.Engine;
using Core.Utils;
using Editor.Saves;
using NUnit.Framework;

namespace EditorTests.Saves;


[TestFixture]
public class CompileMapTests
{
    [Test]
    public void Compile_ShouldWork()
    {
        var editorFormat = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveFormatWorld>(saveFile)!;
        var compiledTest = CompileMap.CompileWorld(editorFormat);

        Assert.AreEqual(expectedCompiledMap.ToString(), compiledTest.ToString());
        Assert.AreEqual(expectedCompiledMap, compiledTest);
    }

    private static WorldData expectedCompiledMap = new WorldData()
    {
        GlobalWordMapId = 1,
        Name = "new [baba] city",
        Maps = new() {
            new MapData(new ObjectData[] {
                new ObjectData() {
                    Occupied = false,
                    Color = 33,
                    ObjectId = 168,
                    Facing = 0,
                    x = 4,
                    y = 5,
                    Name = "text_baba",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 3,
                    ObjectId = 297,
                    Facing = 0,
                    x = 5,
                    y = 5,
                    Name = "text_is",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 33,
                    ObjectId = 473,
                    Facing = 0,
                    x = 6,
                    y = 5,
                    Name = "text_you",
                } }) {
                MapId = 1,
                Name = "global",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 0,
                upLayer = 0,
                region = 0,
            },
            new MapData(new ObjectData[] {
                new ObjectData() {
                    Occupied = false,
                    Color = 49,
                    ObjectId = 13,
                    Facing = 0,
                    x = 2,
                    y = 2,
                    Name = "boat",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 49,
                    ObjectId = 13,
                    Facing = 0,
                    x = 5,
                    y = 4,
                    Name = "boat",
                } }) {
                MapId = 2,
                Name = "region 0 - autumnregion",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 0,
                upLayer = 0,
                region = 0,
            },
            new MapData(new ObjectData[] {  }) {
                MapId = 3,
                Name = "region 1 - region 2",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 0,
                upLayer = 0,
                region = 0,
            },
            new MapData(new ObjectData[] {  }) {
                MapId = 4,
                Name = "region 2 - new region 2",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 0,
                upLayer = 0,
                region = 0,
            },
            new MapData(new ObjectData[] {  }) {
                MapId = 5,
                Name = "region 3 - starting region",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 0,
                upLayer = 0,
                region = 0,
            },
            new MapData(new ObjectData[] {
                new ObjectData() {
                    Occupied = false,
                    Color = 42,
                    ObjectId = 443,
                    Facing = 0,
                    x = 0,
                    y = 0,
                    Name = "text_tree",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 3,
                    ObjectId = 297,
                    Facing = 0,
                    x = 1,
                    y = 0,
                    Name = "text_is",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 41,
                    ObjectId = 429,
                    Facing = 0,
                    x = 2,
                    y = 0,
                    Name = "text_stop",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 42,
                    ObjectId = 444,
                    Facing = 0,
                    x = 0,
                    y = 1,
                    Name = "text_trees",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 3,
                    ObjectId = 297,
                    Facing = 0,
                    x = 1,
                    y = 1,
                    Name = "text_is",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 41,
                    ObjectId = 429,
                    Facing = 0,
                    x = 2,
                    y = 1,
                    Name = "text_stop",
                } }) {
                MapId = 7,
                Name = "6 uplayer - start",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 0,
                upLayer = 0,
                region = 0,
            },
            new MapData(new ObjectData[] {
                new ObjectData() {
                    Occupied = false,
                    Color = 42,
                    ObjectId = 480,
                    Facing = 0,
                    x = 0,
                    y = 0,
                    Name = "tree",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 42,
                    ObjectId = 4,
                    Facing = 0,
                    x = 1,
                    y = 0,
                    Name = "baba",
                } }) {
                MapId = 6,
                Name = "start",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 8,
                westNeighbor = 0,
                upLayer = 7,
                region = 3,
            },
            new MapData(new ObjectData[] {
                new ObjectData() {
                    Occupied = false,
                    Color = 42,
                    ObjectId = 443,
                    Facing = 0,
                    x = 2,
                    y = 1,
                    Name = "text_tree",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 3,
                    ObjectId = 297,
                    Facing = 0,
                    x = 3,
                    y = 1,
                    Name = "text_is",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 41,
                    ObjectId = 429,
                    Facing = 0,
                    x = 4,
                    y = 1,
                    Name = "text_stop",
                } }) {
                MapId = 9,
                Name = "8 uplayer - start2",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 0,
                upLayer = 0,
                region = 0,
            },
            new MapData(new ObjectData[] {
                new ObjectData() {
                    Occupied = false,
                    Color = 42,
                    ObjectId = 480,
                    Facing = 0,
                    x = 0,
                    y = 0,
                    Name = "tree",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 42,
                    ObjectId = 1,
                    Facing = 0,
                    x = 1,
                    y = 0,
                    Name = "amongi",
                } }) {
                MapId = 8,
                Name = "start2",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 10,
                westNeighbor = 1,
                upLayer = 9,
                region = 3,
            },
            new MapData(new ObjectData[] {
                new ObjectData() {
                    Occupied = false,
                    Color = 50,
                    ObjectId = 31,
                    Facing = 0,
                    x = 7,
                    y = 6,
                    Name = "cat",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 20,
                    ObjectId = 6,
                    Facing = 0,
                    x = 7,
                    y = 10,
                    Name = "banana",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 17,
                    ObjectId = 161,
                    Facing = 0,
                    x = 3,
                    y = 3,
                    Name = "text_amongi",
                } }) {
                MapId = 11,
                Name = "10 uplayer - doggos cattos",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 0,
                upLayer = 0,
                region = 0,
            },
            new MapData(new ObjectData[] {
                new ObjectData() {
                    Occupied = false,
                    Color = 2,
                    ObjectId = 44,
                    Facing = 0,
                    x = 2,
                    y = 1,
                    Name = "dog",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 2,
                    ObjectId = 44,
                    Facing = 0,
                    x = 4,
                    y = 4,
                    Name = "dog",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 17,
                    ObjectId = 44,
                    Facing = 0,
                    x = 1,
                    y = 7,
                    Name = "dog",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 3,
                    ObjectId = 493,
                    Facing = 0,
                    x = 9,
                    y = 7,
                    Name = "worm",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 17,
                    ObjectId = 44,
                    Facing = 0,
                    x = 0,
                    y = 5,
                    Name = "dog",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 17,
                    ObjectId = 44,
                    Facing = 0,
                    x = 0,
                    y = 10,
                    Name = "dog",
                },
                new ObjectData() {
                    Occupied = false,
                    Color = 17,
                    ObjectId = 1,
                    Facing = 0,
                    x = 6,
                    y = 2,
                    Name = "amongi",
                } }) {
                MapId = 10,
                Name = "doggos cattos",
                northNeighbor = 0,
                southNeighbor = 0,
                eastNeighbor = 0,
                westNeighbor = 8,
                upLayer = 11,
                region = 1,
            } },
        Regions = new() {
            new RegionData() {
                RegionId = 0,
                WordLayerId = 2,
                Theme = "autumn",
                Music = "editorsong",
                Name = "autumnregion"
            },
            new RegionData() {
                RegionId = 1,
                WordLayerId = 3,
                Theme = "garden",
                Music = "default",
                Name = "region 2"
            },
            new RegionData() {
                RegionId = 2,
                WordLayerId = 4,
                Theme = "swamp",
                Music = "default",
                Name = "new region 2"
            },
            new RegionData() {
                RegionId = 3,
                WordLayerId = 5,
                Theme = "garden",
                Music = "default",
                Name = "starting region"
            } }
    };

    private const string saveFile = """
        {
        	"WorldLayout": [
        		{"x":0,"y":0,"mapDataId":4},
        		{"x":1,"y":0,"mapDataId":5},
        		{"x":2,"y":0,"mapDataId":3}
        	],
        	"Warps": [
        		{"x1":0,"y1":2,"x2":4,"y2":0,"name":"","r":234,"g":168,"b":229}
        	],
        	"Regions": [
        		{
        			"id": 0,
        			"name": "autumnregion",
        			"theme": "autumn",
        			"musicName": "editorsong",
        			"regionObjectLayer": {
        				"objects": [
        					{"x":2,"y":2,"name":"boat","state":4,"color":49,"text":"","original":null},
        					{"x":5,"y":4,"name":"boat","state":1,"color":49,"text":"","original":null}
        				],
        				"width": 10,
        				"height": 10
        			}
        		},
        		{
        			"id": 1,
        			"name": "region 2",
        			"theme": "garden",
        			"musicName": "default",
        			"regionObjectLayer": {
        				"objects": [],
        				"width": 18,
        				"height": 18
        			}
        		},
        		{
        			"id": 2,
        			"name": "new region 2",
        			"theme": "swamp",
        			"musicName": "default",
        			"regionObjectLayer": {
        				"objects": [],
        				"width": 18,
        				"height": 18
        			}
        		},
        		{
        			"id": 3,
        			"name": "starting region",
        			"theme": "garden",
        			"musicName": "default",
        			"regionObjectLayer": {
        				"objects": [],
        				"width": 15,
        				"height": 15
        			}
        		}
        	],
        	"MapDatas": [
        		{
        			"id": 1,
        			"name": "single [skull:8] and [algae]",
        			"regionId": 2,
        			"resetWhenInactive": false,
        			"layer1": {
        				"objects": [
        					{"x":0,"y":0,"name":"skull","state":4,"color":17,"text":"heloo\nthere [baba]\nssslfsjd","original":{"name":"arm","state":0,"color":25}},
        					{"x":4,"y":2,"name":"bolt","state":1,"color":42,"text":"","original":null},
        					{"x":5,"y":2,"name":"bolt","state":4,"color":42,"text":"","original":null},
        					{"x":4,"y":4,"name":"bolt","state":8,"color":42,"text":"","original":null},
        					{"x":6,"y":4,"name":"bolt","state":1,"color":42,"text":"","original":null},
        					{"x":5,"y":4,"name":"bolt","state":8,"color":42,"text":"","original":null},
        					{"x":8,"y":6,"name":"text_dog","state":1,"color":2,"text":"","original":null}
        				],
        				"width": 15,
        				"height": 15
        			},
        			"layer2": {
        				"objects": [
        					{"x":2,"y":2,"name":"text_baba","state":1,"color":33,"text":"","original":null}
        				],
        				"width": 15,
        				"height": 15
        			}
        		},
        		{
        			"id": 2,
        			"name": "neskdf",
        			"regionId": 0,
        			"resetWhenInactive": false,
        			"layer1": {
        				"objects": [
        					{"x":3,"y":2,"name":"pants","state":1,"color":26,"text":"","original":null},
        					{"x":4,"y":5,"name":"pants","state":1,"color":26,"text":"","original":null},
        					{"x":0,"y":3,"name":"pants","state":1,"color":26,"text":"","original":null},
        					{"x":0,"y":5,"name":"pants","state":1,"color":26,"text":"","original":null}
        				],
        				"width": 10,
        				"height": 10
        			},
        			"layer2": {
        				"objects": [
        					{"x":3,"y":5,"name":"text_baba","state":0,"color":33,"text":"","original":null},
        					{"x":4,"y":5,"name":"text_is","state":0,"color":3,"text":"","original":null},
        					{"x":5,"y":5,"name":"text_you","state":0,"color":33,"text":"","original":null}
        				],
        				"width": 18,
        				"height": 18
        			}
        		},
        		{
        			"id": 3,
        			"name": "doggos cattos",
        			"regionId": 1,
        			"resetWhenInactive": false,
        			"layer1": {
        				"objects": [
        					{"x":2,"y":1,"name":"dog","state":1,"color":2,"text":"","original":null},
        					{"x":4,"y":4,"name":"dog","state":4,"color":2,"text":"","original":null},
        					{"x":1,"y":7,"name":"dog","state":1,"color":17,"text":"","original":null},
        					{"x":9,"y":7,"name":"worm","state":1,"color":3,"text":"","original":null},
        					{"x":0,"y":5,"name":"dog","state":1,"color":17,"text":"","original":null},
        					{"x":0,"y":10,"name":"dog","state":4,"color":17,"text":"","original":null},
        					{"x":6,"y":2,"name":"amongi","state":1,"color":17,"text":"","original":null}
        				],
        				"width": 15,
        				"height": 15
        			},
        			"layer2": {
        				"objects": [
        					{"x":7,"y":6,"name":"cat","state":1,"color":50,"text":"","original":null},
        					{"x":7,"y":10,"name":"banana","state":1,"color":20,"text":"","original":null},
        					{"x":3,"y":3,"name":"text_amongi","state":1,"color":17,"text":"","original":null}
        				],
        				"width": 15,
        				"height": 15
        			}
        		},
        		{
        			"id": 4,
        			"name": "start",
        			"regionId": 3,
        			"resetWhenInactive": false,
        			"layer1": {
        				"objects": [
        					{"x":0,"y":0,"name":"tree","state":1,"color":42,"text":"","original":{"name":"wall","state":1,"color":9}},
        					{"x":1,"y":0,"name":"baba","state":1,"color":42,"text":"","original":{"name":"wall","state":1,"color":9}},
        				],
        				"width": 15,
        				"height": 15
        			},
        			"layer2": {
        				"objects": [
        					{"x":0,"y":0,"name":"text_tree","state":1,"color":42,"text":"","original":null},
        					{"x":1,"y":0,"name":"text_is","state":1,"color":3,"text":"","original":null},
        					{"x":2,"y":0,"name":"text_stop","state":1,"color":41,"text":"","original":null},
        					{"x":0,"y":1,"name":"text_trees","state":1,"color":42,"text":"","original":null},
        					{"x":1,"y":1,"name":"text_is","state":1,"color":3,"text":"","original":null},
        					{"x":2,"y":1,"name":"text_stop","state":1,"color":41,"text":"","original":null}
        				],
        				"width": 15,
        				"height": 15
        			}
        		},
        		{
        			"id": 5,
        			"name": "start2",
        			"regionId": 3,
        			"resetWhenInactive": false,
        			"layer1": {
        				"objects": [
        					{"x":0,"y":0,"name":"tree","state":1,"color":42,"text":"","original":{"name":"tree","state":1,"color":42}},
        					{"x":1,"y":0,"name":"amongi","state":1,"color":42,"text":"","original":{"name":"tree","state":1,"color":42}},
        				],
        				"width": 15,
        				"height": 15
        			},
        			"layer2": {
        				"objects": [
        					{"x":2,"y":1,"name":"text_tree","state":1,"color":42,"text":"","original":null},
        					{"x":3,"y":1,"name":"text_is","state":1,"color":3,"text":"","original":null},
        					{"x":4,"y":1,"name":"text_stop","state":1,"color":41,"text":"","original":null}
        				],
        				"width": 15,
        				"height": 15
        			}
        		}
        	],
        	"globalObjectLayer": {
        		"objects": [
        			{"x":4,"y":5,"name":"text_baba","state":1,"color":33,"text":"","original":null},
        			{"x":5,"y":5,"name":"text_is","state":1,"color":3,"text":"","original":null},
        			{"x":6,"y":5,"name":"text_you","state":1,"color":33,"text":"","original":null}
        		],
        		"width": 18,
        		"height": 18
        	},
        	"startMapX": 0,
        	"startMapY": 0,
        	"worldName": "new [baba] city"
        }
        """;
}
