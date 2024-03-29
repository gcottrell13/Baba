﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorTests;

public static class WorldDataJson
{
    public const string saveFile = """
        {
        	"WorldLayout": [
        		{"x":0,"y":0,"mapDataId":4},
        		{"x":1,"y":0,"mapDataId":5},
        		{"x":2,"y":0,"mapDataId":3},
        		{"x":3,"y":0,"mapDataId":1}
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
        			"regionObjectInstanceIds": [3]
        		},
        		{
        			"id": 1,
        			"name": "region 2",
        			"theme": "garden",
        			"musicName": "default",
        			"regionObjectInstanceIds": []
        		},
        		{
        			"id": 2,
        			"name": "new region 2",
        			"theme": "swamp",
        			"musicName": "default",
        			"regionObjectInstanceIds": []
        		},
        		{
        			"id": 3,
        			"name": "starting region",
        			"theme": "garden",
        			"musicName": "default",
        			"regionObjectInstanceIds": []
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
        					{"x":8,"y":6,"name":"text_dog","state":0,"color":2,"text":"","original":null},
        					{"x":10,"y":6,"name":"text_two","state":0,"color":2,"text":"","original":null},
        				],
        				"width": 15,
        				"height": 15
        			},
        			"layer2": {
        				"objects": [
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
        					{"x":0,"y":0,"name":"tree","state":0,"color":42,"text":"","original":{"name":"wall","state":1,"color":9}},
        					{"x":1,"y":0,"name":"baba","state":1,"color":42,"text":"","original":{"name":"wall","state":1,"color":9}},
        				],
        				"width": 15,
        				"height": 15
        			},
        			"layer2": {
        				"objects": [
        					{"x":0,"y":0,"name":"text_tree","state":0,"color":42,"text":"","original":null},
        					{"x":1,"y":0,"name":"text_is","state":0,"color":3,"text":"","original":null},
        					{"x":2,"y":0,"name":"text_stop","state":0,"color":41,"text":"","original":null},
        					{"x":0,"y":1,"name":"text_trees","state":0,"color":42,"text":"","original":null},
        					{"x":1,"y":1,"name":"text_is","state":0,"color":3,"text":"","original":null},
        					{"x":2,"y":1,"name":"text_stop","state":0,"color":41,"text":"","original":null}
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
        					{"x":0,"y":0,"name":"tree","state":0,"color":42,"text":"","original":{"name":"tree","state":1,"color":42}},
        					{"x":1,"y":0,"name":"amongi","state":1,"color":42,"text":"","original":{"name":"tree","state":1,"color":42}},
        				],
        				"width": 15,
        				"height": 15
        			},
        			"layer2": {
        				"objects": [
        					{"x":2,"y":1,"name":"text_tree","state":0,"color":42,"text":"","original":null},
        					{"x":3,"y":1,"name":"text_is","state":0,"color":3,"text":"","original":null},
        					{"x":4,"y":1,"name":"text_stop","state":0,"color":41,"text":"","original":null}
        				],
        				"width": 15,
        				"height": 15
        			}
        		}
        	],
        	"globalObjectInstanceIds": [ 1 ],
        	"startMapX": 0,
        	"startMapY": 0,
        	"worldName": "new [baba] city"
        }
        """;
}
