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
    public class FiltererModal<T> : BaseScreen<PickerState>
        where T : class
    {
        public StateMachine<PickerState, KeyPress> statemachine { get; }

        public T? Selected { get; private set; }
        private readonly List<T> items;
        private readonly Func<T, string> filterBy;
        private readonly Func<T, string> display;
        protected int maxDisplay;

        public string Filter => filterDisplay.Text;

        private bool _add;
        private bool _edit;

        protected bool Add { get => _add; set { _add = value; refreshCommands(); } }
        protected bool Edit { get => _edit; set { _edit = value; refreshCommands(); } }

        private readonly List<Text> texts = new();
        private readonly SpriteContainer itemDisplay = new();
        private List<T> filteredChildren = new();
        public Color HighlightColor = Color.Brown;

        private const string baseFilterDisplayText = "select";
        private readonly TextInputBox filterDisplay = new() { DisallowedCharacters = "\n[]\t" };

        public FiltererModal(
            List<T> items,
            int maxDisplay,
            Func<T, string> filterBy,
            Func<T, string>? display = null)
        {
            this.display = display ?? filterBy;
            this.items = items;
            this.filterBy = filterBy;
            filteredChildren = items.ToList();
            this.maxDisplay = maxDisplay;

            statemachine = new StateMachine<PickerState, KeyPress>("filter modal")
                .State(
                    PickerState.Selecting,
                    c => c.KeyPressed switch
                    {
                        Keys.F => PickerState.Filtering,
                        Keys.Up => Up(),
                        Keys.Down => Down(),
                        Keys.Escape => PickerState.CloseCancel,
                        Keys.P => Pick(),
                        Keys.E => OnEdit(),
                        Keys.A => OnAdd(),
                        _ => 0,
                    },
                    def => def
                        .AddOnEnter(selectingCommands)
                ).State(
                    PickerState.Filtering,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => PickerState.Selecting,
                        KeyPress { KeyPressed: Keys.Enter } => Pick(),
                        KeyPress k => addCharToFilter(k),
                    },
                    def => def
                        .AddOnEnter(filteringCommands)
                ).State(
                    PickerState.CloseCancel,
                    c => PickerState.Selecting
                ).State(
                    PickerState.ClosePick,
                    c => PickerState.Selecting
                ).State(
                    PickerState.CloseEdit,
                    c => PickerState.Selecting
                ).State(
                    PickerState.CloseAdd,
                    c => PickerState.Selecting
                );
            statemachine.Initialize(PickerState.Selecting);

            for (var i = 0; i < maxDisplay; i++)
            {
                var t = new Text("", new() { background = Color.Black });
                texts.Add(t);
                AddChild(t, false);
                itemDisplay.AddChild(t.Graphics);
            }

            itemDisplay.y = Text.DEFAULT_LINE_HEIGHT * 2;
            Graphics.AddChild(itemDisplay);
            AddChild(filterDisplay);
            SetDisplayTypeName("");
            SetFilter("");
        }

        private int? getSelectedItemIndex()
        {
            if (Selected == null) return null;
            return filteredChildren.IndexOf(Selected);
        }

        private void refreshCommands()
        {
            switch (statemachine.CurrentState)
            {
                case PickerState.Filtering: { filteringCommands(); break; }
                case PickerState.Selecting: { selectingCommands(); break; }
            };
        }

        protected void selectingCommands()
        {
            var d = new Dictionary<string, string>()
            {
                { CommonStrings.UD_ARROW, "move cursor" },
                { "f", "filter" },
                { "p", "pick" },
                { CommonStrings.ESCAPE, "go back" },
            };
            if (_edit) d.Add("e", "edit");
            if (_add) d.Add("a", "add");
            SetCommands(d);
        }

        protected void filteringCommands()
        {
            SetCommands(new()
            {
                { CommonStrings.NAME_CHARS, "type a name" },
                { CommonStrings.ESCAPE, "stop filtering" },
                { CommonStrings.ENTER, "pick first" },
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
            foreach (var (t, child) in filteredChildren.Skip(startIndex).Take(maxDisplay).Zip(texts))
            {
                var color = object.Equals(t, Selected) ? HighlightColor : Color.Black;
                child.SetText(display(t), new() { background = color });
                child.Graphics.y = y;
                child.Graphics.alpha = 1f;
                y += 24;
            }
        }

        private PickerState Up()
        {
            var sid = getSelectedItemIndex() ?? 0;
            _setSelected(Math.Max(sid - 1, 0));
            return PickerState.Selecting;
        }

        private PickerState Down()
        {
            var sid = getSelectedItemIndex() ?? 0;
            _setSelected(Math.Min(sid + 1, filteredChildren.Count - 1));
            return PickerState.Selecting;
        }

        private PickerState Pick()
        {
            if (Selected != null)
                return PickerState.ClosePick;
            return PickerState.Selecting;
        }

        private PickerState OnEdit()
        {
            if (Selected == null)
                return PickerState.Selecting;
            return _edit ? PickerState.CloseEdit : PickerState.Selecting;
        }

        private PickerState OnAdd()
        {
            return _add ? PickerState.CloseAdd : PickerState.Selecting;
        }

        public override PickerState Handle(KeyPress ev) => statemachine.SendAction(ev);

        private PickerState addCharToFilter(KeyPress c)
        {
            filterDisplay.HandleInput(c);
            _setSelected(0);
            _afterSetFilter();
            return PickerState.Filtering;
        }

        public void SetFilter(string f)
        {
            filterDisplay.SetText(f);
            _setSelected(0);
            _afterSetFilter();
        }

        private void _afterSetFilter()
        {
            filteredChildren = items.Where(x => filterBy(x).Contains(Filter)).ToList();
            _drawChildren();
        }

        protected void SetDisplayTypeName(string typeName)
        {
            var pre = $"{baseFilterDisplayText} {typeName}: {{0}} :\n".Trim();
            filterDisplay.SetFormat(pre);
            _afterSetFilter();
        }
    }

    public enum PickerState
    {
        Selecting,
        Filtering,
        CloseAdd,
        ClosePick,
        CloseCancel,
        CloseEdit,
    }
}
