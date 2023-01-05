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
        private readonly Region region;
        private Text titleText = new();

        public RegionEditorScreen(ScreenStack stack, Region region)
        {
            this.region = region;
            var bg = new RectangleSprite();

            bg.SetColor(ThemeInfo.GetThemeBackgroundColor(region.theme));
            bg.xscale = ScreenWidth;
            bg.yscale = ScreenHeight;

            stateMachine = new StateMachine<EditorStates, KeyPress>("region editor")
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
                        _ => EditorStates.RenamingWorld,
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
                    c => EditorStates.None
                )
                .State(
                    EditorStates.SelectRegionTheme,
                    c => themePickerScreen!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.RegionEditor,
                        PickerState.ClosePick => pickTheme(bg),
                        _ => EditorStates.SelectRegionTheme,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            themePickerScreen = new(region.theme);
                            stack.Add(themePickerScreen);
                        })
                );

            Graphics.AddChild(bg);
            AddChild(titleText);
            refreshText();
        }


        public void init()
        {
            SetCommands(new()
            {
                { "n", "rename region" },
                { "t", "set theme" },
                { "c", "edit region layer" },
                { CommonStrings.ESCAPE, "go back" },
            });
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
            titleText.SetText(string.Join("\n\n", lines));
        }

        private EditorStates pickTheme(RectangleSprite r)
        {
            if (themePickerScreen?.Selected != null)
            {
                region.theme = themePickerScreen.Selected;
                r.SetColor(ThemeInfo.GetThemeBackgroundColor(region.theme));
                refreshText();
            }
            return EditorStates.RegionEditor;
        }


        public override EditorStates Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
        {
            EditorStates.WorldEditor => EditorStates.WorldEditor,
            _ => EditorStates.RegionEditor,
        };

        protected override void OnDispose()
        {
        }
    }
}
