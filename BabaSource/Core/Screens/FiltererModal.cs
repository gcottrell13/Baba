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

        private bool canFilter;

        protected ListDisplay<T> listDisplay;

        private CallbackCollector<PickerState> callbackCollector = new(PickerState.Open);
        private readonly bool canCancel;

        public FiltererModal(
            IEnumerable<T> items,
            int maxDisplay,
            Func<T, string> display,
            Func<T, string>? filterBy = null,
            T? currentValue = null,
            bool canCancel = true)
        {
            listDisplay = new(
                items,
                maxDisplay,
                display,
                filterBy,
                currentValue
            )
            {
                OnAdd=callbackCollector.cb(onAdd),
                OnEdit=callbackCollector.cb<T>(onEdit),
                OnRemove=callbackCollector.cb<T>(onRemove),
                OnSelect=callbackCollector.cb<T>(onSelect),
            };
            canFilter = filterBy != null;
            this.canCancel = canCancel;
            AddChild(listDisplay);
        }

        public override PickerState Handle(KeyPress ev)
        {
            var current = listDisplay.statemachine.CurrentState;
            var r = listDisplay.Handle(ev);
            refreshCommands();

            if (current == ListState.Selecting && ev.KeyPressed == Keys.Escape)
            {
                return PickerState.CloseCancel;
            }

            return callbackCollector.latestReturn;
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
            };
            if (canFilter) d.Add("f", "filter");
            if (OnSelect != null) d.Add(CommonStrings.ENTER, "select");
            if (OnEdit != null) d.Add("e", "edit");
            if (OnAdd != null) d.Add("a", "add");
            if (OnRemove != null) d.Add("d", "remove");
            if (canCancel) d.Add(CommonStrings.ESCAPE, "go back");
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

        private PickerState onAdd()
        {
            OnAdd?.Invoke();
            refreshCommands();
            return PickerState.CloseAdd;
        }

        private PickerState onEdit(T item)
        {
            OnEdit?.Invoke(item);
            refreshCommands();
            return PickerState.CloseEdit;
        }

        private PickerState onRemove(T item)
        {
            OnRemove?.Invoke(item);
            refreshCommands();
            return PickerState.CloseRemove;
        }

        private PickerState onSelect(T item)
        {
            OnSelect?.Invoke(item);
            refreshCommands();
            return PickerState.ClosePick;
        }


        protected override void OnDispose()
        {
        }

        public void SetDisplayTypeName(string typeName)
        {
            listDisplay.SetShowBackground(Transparent);
            listDisplay.SetDisplayTypeName(typeName);
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