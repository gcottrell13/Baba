using Core.Content;
using Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

public class WorldDataDeserialized
{
    public static WorldData expectedCompiledMap = new WorldData()
    {
        GlobalWordMapId = 1,
        Name = "new [baba] city",
        Maps = new() {
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false,
                Color = 33,
                Name = ObjectTypeId.baba,
                Facing = 0,
                x = 4,
                y = 5,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 3,
                Name = ObjectTypeId.@is,
                Facing = 0,
                x = 5,
                y = 5,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 33,
                Name = ObjectTypeId.you,
                Facing = 0,
                x = 6,
                y = 5,
                Kind = ObjectKind.Text,
            } }) {
            MapId = 1,
            Name = "global",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 0,
            westNeighbor = 0,
            upLayer = 0,
            region = 0,
            width = 18, 
            height = 18,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false,
                Color = 49,
                Name = ObjectTypeId.boat,
                Facing = 0,
                x = 2,
                y = 2,
                Kind = ObjectKind.Object,
                Text = """
                this is a boat
                """,
            },
            new ObjectData() {
                Deleted = false,
                Color = 49,
                Name = ObjectTypeId.boat,
                Facing = 0,
                x = 5,
                y = 4,
                Kind = ObjectKind.Object,
            } }) {
            MapId = 2,
            Name = "region 0 - autumnregion",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 0,
            westNeighbor = 0,
            upLayer = 0,
            region = 0,
            width = 10,
            height = 10,
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
            width = 18,
            height = 18,
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
            width = 18,
            height = 18,
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
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false,
                Color = 42,
                Name = ObjectTypeId.tree,
                Facing = 0,
                x = 0,
                y = 0,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 3,
                Name = ObjectTypeId.@is,
                Facing = 0,
                x = 1,
                y = 0,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 41,
                Name = ObjectTypeId.stop,
                Facing = 0,
                x = 2,
                y = 0,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 42,
                Name = ObjectTypeId.trees,
                Facing = 0,
                x = 0,
                y = 1,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 3,
                Name = ObjectTypeId.@is, Facing = 0, x = 1, y = 1,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 41,
                Name = ObjectTypeId.stop, Facing = 0, x = 2, y = 1,
                Kind = ObjectKind.Text,
            } }) {
            MapId = 7,
            Name = "6 uplayer - start",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 0,
            westNeighbor = 0,
            upLayer = 0,
            region = 0,
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false,
                Color = 42,
                Name = ObjectTypeId.tree, Facing = 0, x = 0, y = 0,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 42,
                Name = ObjectTypeId.baba, Facing = 0, x = 1, y = 0,
                Kind = ObjectKind.Object,
            } }) {
            MapId = 6,
            Name = "start",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 8,
            westNeighbor = 0,
            upLayer = 7,
            region = 3,
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false,
                Color = 42,
                Name = ObjectTypeId.tree, Facing = 0, x = 2, y = 1,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 3,
                Name = ObjectTypeId.@is, Facing = 0, x = 3, y = 1,
                Kind = ObjectKind.Text,
            },
            new ObjectData() {
                Deleted = false,
                Color = 41,
                Name = ObjectTypeId.stop, Facing = 0, x = 4, y = 1,
                Kind = ObjectKind.Text,
            } }) {
            MapId = 9,
            Name = "8 uplayer - start2",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 0,
            westNeighbor = 0,
            upLayer = 0,
            region = 0,
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false,
                Color = 42,
                Name = ObjectTypeId.tree, Facing = 0, x = 0, y = 0,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 42,
                Name = ObjectTypeId.amongi, Facing = 0, x = 1, y = 0,
                Kind = ObjectKind.Object,
            } }) {
            MapId = 8,
            Name = "start2",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 10,
            westNeighbor = 1,
            upLayer = 9,
            region = 3,
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false,
                Color = 50,
                Name = ObjectTypeId.cat, Facing = 0, x = 7, y = 6,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 20,
                Name = ObjectTypeId.banana, Facing = 0, x = 7, y = 10,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 17,
                Name = ObjectTypeId.amongi, Facing = 0, x = 3, y = 3,
                Kind = ObjectKind.Text,
            } }) {
            MapId = 11,
            Name = "10 uplayer - doggos cattos",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 0,
            westNeighbor = 0,
            upLayer = 0,
            region = 0,
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false,
                Color = 2,
                Name = ObjectTypeId.dog, Facing = 0, x = 2, y = 1,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 2,
                Name = ObjectTypeId.dog, Facing = 0, x = 4, y = 4,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 17,
                Name = ObjectTypeId.dog, Facing = 0, x = 1, y = 7,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 3,
                Name = ObjectTypeId.worm, Facing = 0, x = 9, y = 7,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 17,
                Name = ObjectTypeId.dog, Facing = 0, x = 0, y = 5,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 17,
                Name = ObjectTypeId.dog, Facing = 0, x = 0, y = 10,
                Kind = ObjectKind.Object,
            },
            new ObjectData() {
                Deleted = false,
                Color = 17,
                Name = ObjectTypeId.amongi, Facing = 0, x = 6, y = 2,
                Kind = ObjectKind.Object,
            } }) {
            MapId = 10,
            Name = "doggos cattos",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 0,
            westNeighbor = 8,
            upLayer = 11,
            region = 1,
            width = 15,
            height = 15,
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
}
