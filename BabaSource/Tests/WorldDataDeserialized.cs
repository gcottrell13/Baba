using Core.Content;
using Core.Engine;
using Core.Utils;
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
        GlobalWordMapIds = new short[] { 3 },
        Name = "new [baba] city",
        Maps = new() {
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.None,
                x = 0, y = 0, Name = ObjectTypeId.@tree, Kind = ObjectKind.Text,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 3, Facing = Direction.None,
                x = 1, y = 0, Name = ObjectTypeId.@is, Kind = ObjectKind.Text,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 41, Facing = Direction.None,
                x = 2, y = 0, Name = ObjectTypeId.@stop, Kind = ObjectKind.Text,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.None,
                x = 0, y = 1, Name = ObjectTypeId.@trees, Kind = ObjectKind.Text,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 3, Facing = Direction.None,
                x = 1, y = 1, Name = ObjectTypeId.@is, Kind = ObjectKind.Text,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 41, Facing = Direction.None,
                x = 2, y = 1, Name = ObjectTypeId.@stop, Kind = ObjectKind.Text,
                Text = "",
            } }) {
            MapId = 2,
            Name = "1 uplayer - start",
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
                Deleted = false, Color = 42, Facing = Direction.None,
                x = 0, y = 0, Name = ObjectTypeId.@tree, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.Right,
                x = 1, y = 0, Name = ObjectTypeId.@baba, Kind = ObjectKind.Object,
                Text = "",
            } }) {
            MapId = 1,
            Name = "start",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 3,
            westNeighbor = 0,
            upLayer = 2,
            region = 3,
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.None,
                x = 2, y = 1, Name = ObjectTypeId.@tree, Kind = ObjectKind.Text,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 3, Facing = Direction.None,
                x = 3, y = 1, Name = ObjectTypeId.@is, Kind = ObjectKind.Text,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 41, Facing = Direction.None,
                x = 4, y = 1, Name = ObjectTypeId.@stop, Kind = ObjectKind.Text,
                Text = "",
            } }) {
            MapId = 4,
            Name = "3 uplayer - start2",
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
                Deleted = false, Color = 42, Facing = Direction.None,
                x = 0, y = 0, Name = ObjectTypeId.@tree, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.Right,
                x = 1, y = 0, Name = ObjectTypeId.@amongi, Kind = ObjectKind.Object,
                Text = "",
            } }) {
            MapId = 3,
            Name = "start2",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 5,
            westNeighbor = 1,
            upLayer = 4,
            region = 3,
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false, Color = 50, Facing = Direction.Right,
                x = 7, y = 6, Name = ObjectTypeId.@cat, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 20, Facing = Direction.Right,
                x = 7, y = 10, Name = ObjectTypeId.@banana, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 17, Facing = Direction.Right,
                x = 3, y = 3, Name = ObjectTypeId.@amongi, Kind = ObjectKind.Text,
                Text = "",
            } }) {
            MapId = 6,
            Name = "5 uplayer - doggos cattos",
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
                Deleted = false, Color = 2, Facing = Direction.Right,
                x = 2, y = 1, Name = ObjectTypeId.@dog, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 2, Facing = Direction.Left,
                x = 4, y = 4, Name = ObjectTypeId.@dog, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 17, Facing = Direction.Right,
                x = 1, y = 7, Name = ObjectTypeId.@dog, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 3, Facing = Direction.Right,
                x = 9, y = 7, Name = ObjectTypeId.@worm, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 17, Facing = Direction.Right,
                x = 0, y = 5, Name = ObjectTypeId.@dog, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 17, Facing = Direction.Left,
                x = 0, y = 10, Name = ObjectTypeId.@dog, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 17, Facing = Direction.Right,
                x = 6, y = 2, Name = ObjectTypeId.@amongi, Kind = ObjectKind.Object,
                Text = "",
            } }) {
            MapId = 5,
            Name = "doggos cattos",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 7,
            westNeighbor = 3,
            upLayer = 6,
            region = 1,
            width = 15,
            height = 15,
        },
        new MapData(new ObjectData[] {
            new ObjectData() {
                Deleted = false, Color = 17, Facing = Direction.Left,
                x = 0, y = 0, Name = ObjectTypeId.@skull, Kind = ObjectKind.Object,
                Text = """
				heloo
				there [baba]
				ssslfsjd
				""",
            },
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.Right,
                x = 4, y = 2, Name = ObjectTypeId.@bolt, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.Left,
                x = 5, y = 2, Name = ObjectTypeId.@bolt, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.Down,
                x = 4, y = 4, Name = ObjectTypeId.@bolt, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.Right,
                x = 6, y = 4, Name = ObjectTypeId.@bolt, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 42, Facing = Direction.Down,
                x = 5, y = 4, Name = ObjectTypeId.@bolt, Kind = ObjectKind.Object,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 2, Facing = Direction.None,
                x = 8, y = 6, Name = ObjectTypeId.@dog, Kind = ObjectKind.Text,
                Text = "",
            },
            new ObjectData() {
                Deleted = false, Color = 2, Facing = Direction.None,
                x = 10, y = 6, Name = ObjectTypeId.@two, Kind = ObjectKind.Text,
                Text = "",
            } }) {
            MapId = 7,
            Name = "single [skull:8] and [algae]",
            northNeighbor = 0,
            southNeighbor = 0,
            eastNeighbor = 0,
            westNeighbor = 5,
            upLayer = 0,
            region = 2,
            width = 15,
            height = 15,
        } },
        Regions = new() {
        new RegionData() {
            RegionId = 0,
            WordLayerIds = new Int16[] { 7 },
            Theme = "autumn",
            Music = "editorsong",
            Name = "autumnregion"
        },
        new RegionData() {
            RegionId = 1,
            WordLayerIds = new Int16[] {  },
            Theme = "garden",
            Music = "default",
            Name = "region 2"
        },
        new RegionData() {
            RegionId = 2,
            WordLayerIds = new Int16[] {  },
            Theme = "swamp",
            Music = "default",
            Name = "new region 2"
        },
        new RegionData() {
            RegionId = 3,
            WordLayerIds = new Int16[] {  },
            Theme = "garden",
            Music = "default",
            Name = "starting region"
        } },
        Inventory = new() {
                { ObjectTypeId.key, 10 },
                { ObjectTypeId.fish, 22 }
         },
    };
}
