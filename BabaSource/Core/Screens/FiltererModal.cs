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
        public string Filter => listDisplay.Filter;

        private Action<T>? _onEdit;
        private Action? _onAdd;
        private Action<T>? _onRemove;
        private Action<T>? _onSelect;

        public Action<T>? OnEdit { get => _onEdit; set { _onEdit = value; refreshCommands(); } }
        public Action? OnAdd { get => _onAdd; set { _onAdd = value; refreshCommands(); } }
        public Action<T>? OnRemove { get => _onRemove; set { _onRemove = value; refreshCommands(); } }
        public Action<T>? OnSelect { get => _onSelect; set { _onSelect = value; refreshCommands(); } }

        protected ListDisplay<T> listDisplay;

        private PickerState state = PickerState.Open;

        public FiltererModal(
            IEnumerable<T> items,
            int maxDisplay,
            Func<T, string> display,
            Func<T, string>? filterBy = null,
            T? currentValue = null)
        {
            listDisplay = new(
                items,
                maxDisplay,
                display,
                filterBy,
                currentValue
            )
            {
                OnAdd=onAdd,
                OnEdit=onEdit,
                OnRemove=onRemove,
                OnSelect=onSelect,
            };
            AddChild(listDisplay);
        }

        public override PickerState Handle(KeyPress ev)
        {
            var r = listDisplay.Handle(ev);
            refreshCommands();

            if (r == ListState.Selecting && ev.KeyPressed == Keys.Escape)
            {
                return PickerState.CloseCancel;
            }

            return state;
        }

        protected Dictionary<string, string> GetCommands() => listDisplay.statemachine.CurrentState switch
        {
            ListState.Filtering => filteringCommands(),
            ListState.Selecting => selectingCommands(),
            _ => new(),
        };

        protected Dictionary<string, string> selectingCommands()
        {
            var d = new Dictionary<string, string>()
            {
                { CommonStrings.UD_ARROW, "move cursor" },
                { "f", "filter" },
                { CommonStrings.ESCAPE, "go back" },
            };
            if (OnSelect != null) d.Add(CommonStrings.ENTER, "select");
            if (OnEdit != null) d.Add("e", "edit");
            if (OnAdd != null) d.Add("a", "add");
            if (OnRemove != null) d.Add("d", "remove");
            return d;
        }

        protected Dictionary<string, string> filteringCommands()
        {
            var d = new Dictionary<string, string>()
            {
                { CommonStrings.NAME_CHARS, "type a filter" },
                { CommonStrings.ESCAPE, "stop filtering" },
            };
            if (OnSelect != null) d.Add(CommonStrings.ENTER, "select first");
            return d;
        }

        protected void refreshCommands()
        {
            SetCommands(GetCommands());
        }

        private void onAdd()
        {
            state = PickerState.CloseAdd;
            OnAdd?.Invoke();
            refreshCommands();
        }

        private void onEdit(T item)
        {
            state = PickerState.CloseEdit;
            OnEdit?.Invoke(item);
            refreshCommands();
        }

        private void onRemove(T item)
        {
            state = PickerState.CloseRemove;
            OnRemove?.Invoke(item);
            refreshCommands();
        }

        private void onSelect(T item)
        {
            state = PickerState.ClosePick;
            OnSelect?.Invoke(item);
            refreshCommands();
        }


        protected override void OnDispose()
        {
        }

        public void SetDisplayTypeName(string typeName)
        {
            listDisplay.SetDisplayTypeName(typeName, withBackground: Transparent);
        }
    }

}


public enum PickerState
{
    Open,
    CloseAdd,
    ClosePick,
    CloseCancel,
    CloseEdit,
    CloseRemove,
}