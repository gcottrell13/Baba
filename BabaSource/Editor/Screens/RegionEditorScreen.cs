using Core.Content;
using Core.Screens;
using Core.UI;
using Core.Utils;
using Editor.SaveFormats;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class RegionEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine;
        private RenameScreen? renameScreen;
        private ThemePickerScreen? themePickerScreen;
        private RectangleSprite bg = new RectangleSprite();
        private readonly Region region;
        private MapLayerEditorDisplay? layerDisplay;
        private Text titleText = new();
        private MapLayerEditorScreen? layerEditor;

        public RegionEditorScreen(ScreenStack stack, Region region)
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
                    c => layerEditor!.Handle(c),
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            // the edit functions make a new screen object
                            var theme = region.theme;
                            layerEditor = new("layer 1", stack, region.regionObjectLayer, EditorStates.RegionEditor, theme);
                            stack.Add(layerEditor);
                            layerEditor.init();
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
                                OnSelect=pickTheme,
                            };
                            stack.Add(themePickerScreen);
                        })
                );

            Graphics.AddChild(bg);
            AddChild(titleText);
            refreshText();
        }


        public void init()
        {
            stateMachine.Initialize(EditorStates.RegionEditor);
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

            RemoveChild(layerDisplay);
            layerDisplay = new MapLayerEditorDisplay("region", region.regionObjectLayer, null, region.theme);
            layerDisplay.Graphics.y = 75;
            AddChild(layerDisplay);

            SetCommands(new()
            {
                { "n", "rename region" },
                { "t", "set theme" },
                { "c", "edit region layer" },
                { CommonStrings.ESCAPE, "go back" },
            });
        }

        private void pickTheme(string theme)
        {
            region.theme = theme;
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
}
