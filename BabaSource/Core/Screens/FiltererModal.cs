using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Screens
{
    public class FiltererModal<T> : BaseScreen
        where T : class
    {
        private StateMachine<PickerState, int> smachine;
        private string filter;
        public T? Selected { get; private set; }
        private readonly List<T> items;
        private readonly Func<T, string> filterBy;
        private readonly Func<T, string> display;
        private readonly int maxDisplay;
        private readonly bool add;
        private readonly bool edit;
        private readonly List<Text> texts = new();
        private readonly SpriteContainer itemDisplay = new();
        private List<T> filteredChildren = new();
        public Color HighlightColor = Color.Brown;

        private const string baseFilterDisplayText = "filter:\n";
        private readonly Text filterDisplay = new(baseFilterDisplayText);

        public FiltererModal(
            List<T> items, 
            Func<T, string> filterBy, 
            Func<T, string>? display = null,
            int maxDisplay = 5, 
            bool add = false, bool edit = false)
        {
            this.display = display ?? filterBy;
            filter = string.Empty;

            selectingCommands();
            smachine = new StateMachine<PickerState, int>()
                .State(
                    PickerState.Selecting,
                    c => c switch
                    {
                        Keys.F => 1,
                        _ => 0,
                    },
                    def => def
                        .Change(1, PickerState.Filtering)
                        .AddOnEnter(selectingCommands)
                ).State(
                    PickerState.Filtering,
                    c => c switch
                    {
                        Keys.Escape => -1,
                        _ => 0,
                    },
                    def => def
                        .Change(-1, PickerState.Selecting)
                        .AddOnEnter(filteringCommands)
                );
            smachine.Initialize(PickerState.Selecting);

            foreach (var item in items)
            {
                var t = new Text(this.display(item), new() { background = Color.Black });
                texts.Add(t);
                filteredChildren.Add(item);
                AddChild(t, false);
                itemDisplay.AddChild(t.Graphics);
            }

            this.items = items;
            this.filterBy = filterBy;
            this.maxDisplay = maxDisplay;
            this.add = add;
            this.edit = edit;
            itemDisplay.y = Text.DEFAULT_LINE_HEIGHT * 2;
            Graphics.AddChild(itemDisplay);
            AddChild(filterDisplay);
            Selected = items[0];
            _drawChildren();
        }

        private int? getSelectedItemIndex()
        {
            if (Selected == null) return null;
            return filteredChildren.IndexOf(Selected);
        }

        private void selectingCommands()
        {
            var d = new Dictionary<string, string>()
            {
                { "[arrow:2][arrow:8]", "move cursor" },
                { "f", "filter" },
                { "p", "pick" },
                { "[text_escape]", "go back" },
            };
            if (edit) d.Add("e", "edit");
            if (add) d.Add("a", "add");
            SetCommands(d);
        }

        private void filteringCommands()
        {
            SetCommands(new()
            {
                { "a-z0-9", "type a name" },
                { "[text_escape]", "stop filtering" },
            });
        }

        public int RecieveText(char c)
        {
            if (smachine.CurrentState == PickerState.Selecting)
            {
                if (c == 'f') {
                    smachine.SendAction(Keys.F);
                }
                return 0;
            }

            if (c == 8)
            {
                SetFilter(filter[..^1]);
            }
            else if (c == '[' || c == ']')
            {
                return 0;
            }
            else if (!string.IsNullOrWhiteSpace(c.ToString().Trim()))
            {
                // should filter out newlines
                SetFilter(filter + c);
            }

            return 0;
        }

        private void _setSelected(int newSelected)
        {
            if (filteredChildren.Count > 0)
            {
                Selected = filteredChildren[newSelected];
            }
            else
            {
                Selected = null;
            }

            _drawChildren();
        }

        private void _drawChildren()
        {
            foreach (var child in texts)
            {
                if (child == null) continue;
                child.Graphics.alpha = 0f; // hide them all, we will reveal the appropriate ones below
            }

            var sid = getSelectedItemIndex() ?? 0;
            var startIndex = Math.Max(0, Math.Min(sid - maxDisplay / 2, filteredChildren.Count - maxDisplay));

            var y = 0;
            foreach (var (t, child) in filteredChildren.Skip(startIndex).Take(maxDisplay).Zip(texts.Take(maxDisplay)))
            {
                var color = object.Equals(t, Selected) ? HighlightColor : Color.Black;
                child.SetText(display(t), new() { background = color });
                child.Graphics.y = y;
                child.Graphics.alpha = 1f;
                y += 25;
            }
        }

        public int RecieveKey(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    {
                        var sid = getSelectedItemIndex() ?? 0;
                        _setSelected(Math.Max(sid - 1, 0));
                        return 0;
                    }
                case Keys.Down:
                    {
                        var sid = getSelectedItemIndex() ?? 0;
                        _setSelected(Math.Min(sid + 1, filteredChildren.Count - 1));
                        return 0;
                    }
                case Keys.Escape:
                    {
                        var r = smachine.CurrentState == PickerState.Filtering ? 0 : -1;
                        smachine.SendAction(key);
                        return r;
                    }
                case Keys.P:
                    {
                        // pick
                        if (Selected != null) 
                            return 1;
                        return 0;
                    }
                case Keys.E:
                    {
                        // edit
                        if (Selected == null)
                            return 0;
                        return edit ? 2 : 0;
                    }
            }
            return 0;
        }

        public void SetFilter(string f)
        {
            filter = f;
            filterDisplay.SetText(baseFilterDisplayText + filter, new() { background = Color.Black });
            filteredChildren = items.Where(x => filterBy(x).Contains(filter)).ToList();
            _setSelected(0);
        }

        private enum PickerState
        {
            Selecting,
            Filtering
        }
    }
}
