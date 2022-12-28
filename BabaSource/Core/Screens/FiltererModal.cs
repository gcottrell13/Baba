using Core.Events;
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
        public StateMachine<PickerState, int> statemachine { get; }
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
            this.items = items;
            this.filterBy = filterBy;
            this.maxDisplay = maxDisplay;
            this.add = add;
            this.edit = edit;

            statemachine = new StateMachine<PickerState, int>()
                .State(
                    PickerState.Selecting,
                    c => c switch
                    {
                        Keys.F => 1,
                        Keys.Up => Up(),
                        Keys.Down => Down(),
                        Keys.Escape => Escape(),
                        Keys.P => Pick(),
                        Keys.E => Edit(),
                        Keys.A => Add(),
                        _ => 0,
                    },
                    def => def
                        // we have to go here first because the events give us KeyEvent and then an equivalent TextInput right after
                        .Change(1, PickerState.StartFilter) 
                        .Change(-1, PickerState.CloseCancel)
                        .Change(2, PickerState.ClosePick)
                        .Change(3, PickerState.CloseEdit)
                        .Change(4, PickerState.CloseAdd)
                        .AddOnEnter(selectingCommands)
                ).State(
                    PickerState.Filtering,
                    c => c switch
                    {
                        Keys.Escape => -1,
                        (char)8 => backspace(),
                        char f => addCharToFilter(f),
                        _ => 0,
                    },
                    def => def
                        .Change(-1, PickerState.Selecting)
                        .AddOnEnter(filteringCommands)
                ).State(
                    PickerState.CloseCancel,
                    c => 1,
                    def => def.Change(1, PickerState.Selecting)
                ).State(
                    PickerState.ClosePick,
                    c => 1,
                    def => def.Change(1, PickerState.Selecting)
                ).State(
                    PickerState.CloseEdit,
                    c => 1,
                    def => def.Change(1, PickerState.Selecting)
                ).State(
                    PickerState.CloseAdd,
                    c => 1,
                    def => def.Change(1, PickerState.Selecting)
                ).State(
                    PickerState.StartFilter,
                    c => 1,
                    def => def.Change(1, PickerState.Filtering)
                );
            statemachine.Initialize(PickerState.Selecting);

            foreach (var item in items)
            {
                var t = new Text(this.display(item), new() { background = Color.Black });
                texts.Add(t);
                filteredChildren.Add(item);
                AddChild(t, false);
                itemDisplay.AddChild(t.Graphics);
            }

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
                { "a-z0-9_", "type a name" },
                { "[text_escape]", "stop filtering" },
            });
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

        private int Up()
        {
            var sid = getSelectedItemIndex() ?? 0;
            _setSelected(Math.Max(sid - 1, 0));
            return 0;
        }

        private int Down()
        {
            var sid = getSelectedItemIndex() ?? 0;
            _setSelected(Math.Min(sid + 1, filteredChildren.Count - 1));
            return 0;
        }

        private int Escape()
        {
            return -1;
        }

        private int Pick()
        {
            if (Selected != null)
                return 2;
            return 0;
        }

        private int Edit()
        {
            if (Selected == null)
                return 0;
            return edit ? 3 : 0;
        }

        private int Add()
        {
            return add ? 4 : 0;
        }

        public int RecieveKey(Keys key) => statemachine.SendAction(key) switch
        {
            PickerState.CloseCancel => -1,
            PickerState.ClosePick => 1,
            PickerState.CloseEdit => 2,
            PickerState.CloseAdd => 3,
            _ => 0,
        };

        public int RecieveText(char c)
        {
            if (statemachine.CurrentState == PickerState.Selecting)
            {
                if (c == 'f')
                {
                    statemachine.SendAction(Keys.F);
                }
                return 0;
            }
            statemachine.SendAction(c);
            return 0;
        }

        private int addCharToFilter(char c)
        {
            if (c == '[' || c == ']')
            {
                
            }
            else if (!string.IsNullOrWhiteSpace(c.ToString().Trim()))
            {
                // should filter out newlines
                SetFilter(filter + c);
            }

            return 0;
        }

        private int backspace()
        {
            SetFilter(filter[..^1]);
            return 0;
        }

        public void SetFilter(string f)
        {
            filter = f;
            filterDisplay.SetText(baseFilterDisplayText + filter, new() { background = Color.Black });
            filteredChildren = items.Where(x => filterBy(x).Contains(filter)).ToList();
            _setSelected(0);
        }

        public enum PickerState
        {
            None = 0,
            Selecting,
            StartFilter,
            Filtering,
            CloseCancel,
            ClosePick,
            CloseEdit,
            CloseAdd,
        }
    }
}
