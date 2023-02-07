using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.UI;

public enum PickerState
{
    None,
    Selecting,
    Filtering,
    CloseAdd,
    ClosePick,
    CloseCancel,
    CloseEdit,
    CloseRemove,
}

public class ListDisplay<T> : GameObject
    where T : class
{

    public StateMachine<PickerState, KeyPress> statemachine { get; }

    public T? Selected { get; private set; }
    private readonly List<T> items;
    private readonly Func<T, string>? filterBy;
    private readonly Func<T, string> display;
    protected int maxDisplay;

    public Action<T>? OnEdit;
    public Action? OnAdd;
    public Action<T>? OnRemove;
    public Action<T>? OnSelect;
    public Action<string>? OnFilter;

    private readonly List<Text> texts = new();
    private readonly SpriteContainer itemDisplay = new();
    private List<T> filteredChildren = new();
    private Color highlightColor = Color.Brown;

    private Text countDisplay = new();

    private const string baseFilterDisplayText = "select";
    private readonly TextInputBox filterDisplay = new() { TextFilterRegex = new System.Text.RegularExpressions.Regex(@"^[^\n\[\]\t]{0,10}$") };

    public string Filter => filterDisplay.Text;

    public ListDisplay(
        IEnumerable<T> items,
        int maxDisplay,
        Func<T, string> display,
        Func<T, string>? filterBy = null,
        T? currentValue = null)
    {
        this.display = display;
        this.items = items.ToList();
        this.filterBy = filterBy ?? display;
        filteredChildren = items.ToList();
        this.maxDisplay = maxDisplay;
        filterDisplay.SetOptions(new() { background = Color.Black });
        _setSelected(Math.Max(0, filteredChildren.IndexOf(currentValue!)));

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
        AddChild(countDisplay, false);
        itemDisplay.AddChild(countDisplay.Graphics);
        SetDisplayTypeName("", false);
        SetFilter("");

        statemachine = new StateMachine<PickerState, KeyPress>("filter modal", PickerState.None)
            .State(
                PickerState.Selecting,
                c => c.KeyPressed switch
                {
                    Keys.F => PickerState.Filtering,
                    Keys.Up => Up(),
                    Keys.Down => Down(),
                    Keys.Escape => Cancel(),
                    Keys.Enter => Pick(),
                    Keys.E => Edit(),
                    Keys.A => Add(),
                    Keys.D => Remove(),
                    _ => 0,
                }
            ).State(
                PickerState.Filtering,
                c => c switch
                {
                    KeyPress { KeyPressed: Keys.Escape } => PickerState.Selecting,
                    KeyPress { KeyPressed: Keys.Enter } => Pick(),
                    KeyPress k => addCharToFilter(k),
                }
            ).State(
                PickerState.CloseCancel,
                c => throwClosedError()
            ).State(
                PickerState.ClosePick,
                c => throwClosedError()
            ).State(
                PickerState.CloseEdit,
                c => throwClosedError()
            ).State(
                PickerState.CloseAdd,
                c => throwClosedError()
            ).State(
                PickerState.CloseRemove,
                c => throwClosedError()
            );
        statemachine.Initialize(PickerState.Selecting);
    }

    private PickerState throwClosedError() => throw new InvalidOperationException("Must create a new filter modal. This one is closed");

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

    private int? getSelectedItemIndex()
    {
        if (Selected == null) return null;
        return filteredChildren.IndexOf(Selected);
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
            var isSelectedItem = object.Equals(t, Selected);
            var color = isSelectedItem ? highlightColor : Color.Black;
            var pre = isSelectedItem ? "[arrow] " : "  ";
            child.SetText(pre + display(t), new() { background = color });
            child.Graphics.y = y;
            child.Graphics.alpha = 1f;
            y += 24;
        }

        countDisplay.SetText($"{sid + 1}/{filteredChildren.Count}");
        countDisplay.Graphics.y = 24 * (maxDisplay + 1);
    }

    private PickerState Cancel()
    {
        Selected = null;
        return PickerState.CloseCancel;
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
        if (Selected == null)
            return PickerState.None;
        if (OnSelect != null)
            OnSelect(Selected);
        return PickerState.ClosePick;
    }

    private PickerState Edit()
    {
        if (Selected == null)
            return PickerState.None;
        if (OnEdit != null)
        {
            OnEdit(Selected);
            return PickerState.CloseEdit;
        }
        return PickerState.None;
    }

    private PickerState Add()
    {
        if (OnAdd != null)
        {
            OnAdd();
            return PickerState.CloseAdd;
        }
        return PickerState.None;
    }

    private PickerState Remove()
    {
        if (Selected == null)
            return PickerState.None;
        if (OnRemove != null)
        {
            OnRemove(Selected);
            return PickerState.CloseRemove;
        }
        return PickerState.None;
    }

    public PickerState Handle(KeyPress ev) => statemachine.SendAction(ev);

    private PickerState addCharToFilter(KeyPress c)
    {
        filterDisplay.HandleInput(c);
        _afterSetFilter();
        return PickerState.Filtering;
    }

    public void SetFilter(string f)
    {
        filterDisplay.SetText(f);
        _afterSetFilter();
    }

    private void _afterSetFilter()
    {
        if (filterBy == null) return;

        filteredChildren = items.Where(x => filterBy(x).Contains(Filter)).ToList();
        _setSelected(0);
        _drawChildren();
    }

    public void SetHighlightColor(Color color)
    {
        highlightColor = color;
        _drawChildren();
    }

    public void SetDisplayTypeName(string typeName, bool withBackground)
    {
        var pre = $"{baseFilterDisplayText} {typeName}: {{0}} :\n".Trim();
        
        if (withBackground)
        {
            var name = "textboxoutline";
            var c = ChildByName(name);
            if (c is not TextWithBoxOutline outline)
            {
                RemoveChild(c);
                outline = new TextWithBoxOutline() { Name = name };
                var longestDisplay = items.Count > 0 ? items.Select(x => display(x)).MaxBy(s => s.Length) ?? "" : "";

                var maxSize = Text.ParseText(longestDisplay).TextCharLength() + 2;
                outline.SetText(" ".Repeat(maxSize) + "\n".Repeat(maxDisplay + 2 + 1), new() { backgroundColor = Color.Black });
                outline.Graphics.x = -Text.DEFAULT_LINE_HEIGHT;
                AddChild(outline, index: 0);
                itemDisplay.y = Text.DEFAULT_LINE_HEIGHT * 3;
                filterDisplay.Graphics.y = Text.DEFAULT_LINE_HEIGHT;
            }
        }
        else
        {
            itemDisplay.y = Text.DEFAULT_LINE_HEIGHT * 2;
            filterDisplay.Graphics.y = 0;
        }

        filterDisplay.SetFormat(pre);
        _afterSetFilter();
    }
}
