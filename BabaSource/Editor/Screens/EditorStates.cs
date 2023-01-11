using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal enum EditorStates
    {
        None,
        WorldEditor,  // editor for laying out maps in the world 

        // In the Map Editor: 
        MapEditor, // editor for a section of the world; a single screen 
        ChangeObjectColor, // bring up a modal to select a color for a specific object 
        AddingTextToObject, // bring up a modal for adding text to a specific object 
        ObjectPicker, // modal for selecting a new object 
        EditMapLayer1,
        EditMapLayer2,
        RenamingMap,
        ResizeMapLayerWidth,
        ResizeMapLayerHeight,
        SelectMapRegion, // view the map's current region, and select/add/edit a region 
                         // MapPicker, // select which map we are editing // just go back to the world map and pick another map 

        // Region Editor 
        RegionEditor, // add a new region, or edit an existing one 
        EditRegionName,
        EditRegionWordLayer,
        SelectRegionTheme,
        SelectRegionMusic,

        // World Editor 
        WorldEditorPickMap, // Choose which map to place in the world 
        WorldEditorWarps,
        WorldEditorWarpPlacingPoint2, // edit warp points 
        SelectingWorld,
        RenamingWorld,
        ViewingWorld,
        WorldEditorGlobalLayer,

    }
}
