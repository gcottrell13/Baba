using Core.Content;
using Core.Screens;
using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework.Input;
using Editor.Saves;
using System.Linq;

namespace Editor.Screens;

internal class RegionEditorScreen : BaseScreen<EditorStates>
{
    private StateMachine<EditorStates, KeyPress> stateMachine;
    private RenameScreen? renameScreen;
    private ThemePickerScreen? themePickerScreen;
    private RectangleSprite bg = new RectangleSprite();
    private readonly SaveRegion region;
    private readonly SaveFormatWorld world;
    private Text pickedMapsDisplay = new();
    private Text titleText = new();
    private MapInstancePickerScreen? instancePickerScreen;
    private MusicPickerScreen? musicPickerScreen;

    public RegionEditorScreen(ScreenStack stack, SaveRegion region, SaveFormatWorld world)
    {
        this.region = region;
        bg.SetColor(ThemeInfo.GetThemeBackgroundColor(region.theme));
        bg.xscale = ScreenWidth;
        bg.yscale = ScreenHeight;

        stateMachine = new StateMachine<EditorStates, KeyPress>("region editor", EditorStates.None)
            .State(
                EditorStates.RegionEditor,
                c => c switch
                {
                    KeyPress { KeyPressed: Keys.Escape } => EditorStates.WorldEditor,
                    KeyPress { Text: 'n' } => EditorStates.EditRegionName,
                    KeyPress { Text: 'c' } => EditorStates.EditRegionWordLayer,
                    KeyPress { Text: 't' } => EditorStates.SelectRegionTheme,
                    KeyPress { Text: 'm' } => EditorStates.SelectRegionMusic,
                    _ => EditorStates.None,
                }
            )
            .State(
                EditorStates.EditRegionName,
                c => renameScreen!.Handle(c) switch
                {
                    RenameScreen.RenameStates.Cancel => EditorStates.RegionEditor,
                    RenameScreen.RenameStates.Save => EditorStates.RegionEditor,
                    _ => EditorStates.None,
                },
                def => def
                    .AddOnLeave(() =>
                    {
                        if (renameScreen?.Text != null)
                        {
                            region.name = renameScreen.Text;
                            refreshText();
                        }
                        stack.Pop().Dispose();
                    })
                    .AddOnEnter(() =>
                    {
                        renameScreen = new(region.name, "name region: {}");
                        stack.Add(renameScreen);
                    })
            )
            .State(
                EditorStates.EditRegionWordLayer,
                c => instancePickerScreen!.Handle(c) switch
                {
                    PickerState.CloseCancel => EditorStates.RegionEditor,
                    _ => EditorStates.None,
                },
                def => def
                    .AddOnLeave(() => stack.Pop().Dispose())
                    .AddOnEnter(() =>
                    {
                        var theme = region.theme;
                        instancePickerScreen = new(world.WorldLayout, 10, world, x => region.regionObjectInstanceIds.Contains(x.instanceId))
                        {
                            OnSelect = onAddInstanceToRegion,
                        };
                        stack.Add(instancePickerScreen);
                    })
            )
            .State(
                EditorStates.SelectRegionTheme,
                c => themePickerScreen!.Handle(c) switch
                {
                    PickerState.CloseCancel => EditorStates.RegionEditor,
                    PickerState.ClosePick => EditorStates.RegionEditor,
                    _ => EditorStates.None,
                },
                def => def
                    .AddOnLeave(() => stack.Pop().Dispose())
                    .AddOnEnter(() =>
                    {
                        themePickerScreen = new(region.theme)
                        {
                            OnSelect = pickTheme,
                        };
                        stack.Add(themePickerScreen);
                    })
            )
            .State(
                EditorStates.SelectRegionMusic,
                c => musicPickerScreen!.Handle(c) switch
                {
                    PickerState.CloseCancel => EditorStates.RegionEditor,
                    PickerState.ClosePick => EditorStates.RegionEditor,
                    _ => EditorStates.None,
                },
                def => def
                    .AddOnLeave(() => stack.Pop().Dispose())
                    .AddOnEnter(() =>
                    {
                        musicPickerScreen = new(region.musicName)
                        {
                            OnSelect = pickMusic,
                        };
                        stack.Add(musicPickerScreen);
                    })
            );

        Graphics.AddChild(bg);
        AddChild(titleText);
        refreshText();
        this.world = world;
    }


    public void init()
    {
        stateMachine.Initialize(EditorStates.RegionEditor);
    }

    public void onAddInstanceToRegion(SaveMapInstance saveMapInstance)
    {
        if (region.regionObjectInstanceIds.Contains(saveMapInstance.instanceId))
        {
            region.regionObjectInstanceIds.Remove(saveMapInstance.instanceId);
        }
        else
        {
            region.regionObjectInstanceIds.Add(saveMapInstance.instanceId);
        }
    }

    private void refreshText()
    {
        var lines = new[]
        {
            $"Region: {region.name}",
            $"Theme: {region.theme}",
            $"Id: {region.id}",
        };
        titleText.SetText(string.Join("\n", lines));

        bg.SetColor(ThemeInfo.GetThemeBackgroundColor(region.theme));

        RemoveChild(pickedMapsDisplay);
        pickedMapsDisplay.SetText(string.Join("\n", region.regionObjectInstanceIds.Select(world.SaveMapDataByInstanceId).Select(map => map?.name)));
        pickedMapsDisplay.Graphics.y = 75;
        AddChild(pickedMapsDisplay);

        SetCommands(new()
        {
            { "n", "rename region" },
            { "t", "set theme" },
            { "c", "edit region layer" },
            { "m", "set music" },
            { CommonStrings.ESCAPE, "go back" },
        });
    }

    private void pickTheme(string theme)
    {
        region.theme = theme;
        refreshText();
    }

    private void pickMusic(string name)
    {
        region.musicName = name;
        PlaySound.PlayMusic(name);
        refreshText();
    }

    public override EditorStates Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
    {
        EditorStates.WorldEditor => EditorStates.WorldEditor,
        _ => EditorStates.None,
    };

    protected override void OnDispose()
    {
    }
}
