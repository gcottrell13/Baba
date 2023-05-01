using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.UI;

public enum ListState
{
    None,
    Selecting,
    Filtering,
}

public class ListDisplay<T> : GameObject
    where T : class
{

    public StateMachine<ListState, KeyPress> statemachine { get; }

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

    private bool showTitle = true;
    private bool showCount = true;
    private bool background = false;

    private Text countDisplay = new();

    private const string baseFilterDisplayText = "select";
    private readonly TextInputBox filterDisplay = new() { TextFilterRegex = new System.Text.RegularExpressions.Regex(@"^[^\n\[\]\t]{0,10}$") };

    public string Filter => filterDisplay.Text;

    public ListDisplay(
        IEnumerable<T> items,
        int maxDisplay,
        Func<T, string> display,
        Func<T, string>? filterBy = null,
        T? currentValue = null,
        bool showTitle = true,
        bool showCount = true,
        bool background = false)
    {
        this.display = display;
        this.items = items.ToList();
        this.filterBy = filterBy;
        filteredChildren = items.ToList();
        this.maxDisplay = maxDisplay;
        filterDisplay.SetOptions(new() { background = Color.Black });

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
        SetDisplayTypeName("");
        SetFilter("");

        SetShowTitle(showTitle);
        SetShowCount(showCount);
        SetShowBackground(background);

        statemachine = new StateMachine<ListState, KeyPress>("filter modal", ListState.None)
            .State(
                ListState.Selecting,
                c => c.KeyPressed switch
                {
                    Keys.F => filterBy == null ? ListState.Selecting : ListState.Filtering,
                    Keys.Up => Up(),
                    Keys.Down => Down(),
                    Keys.Enter => Pick(),
                    Keys.E => Edit(),
                    Keys.A => Add(),
                    Keys.D => Remove(),
                    _ => 0,
                }
            ).State(
                ListState.Filtering,
                c => c switch
                {
                    KeyPress { KeyPressed: Keys.Escape } => ListState.Selecting,
                    KeyPress { KeyPressed: Keys.Enter } => Pick(),
                    KeyPress k => addCharToFilter(k),
                }
            );
        statemachine.Initialize(ListState.Selecting);

        _setSelected(Math.Max(0, filteredChildren.IndexOf(currentValue!)));
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

    private int? getSelectedItemIndex()
    {
        if (Selected == null) return null;
        return filteredChildren.IndexOf(Selected);
    }

    private ListState Up()
    {
        var sid = getSelectedItemIndex() ?? 0;
        _setSelected(Math.Max(sid - 1, 0));
        return ListState.Selecting;
    }

    private ListState Down()
    {
        var sid = getSelectedItemIndex() ?? 0;
        _setSelected(Math.Min(sid + 1, filteredChildren.Count - 1));
        return ListState.Selecting;
    }

    private ListState Pick()
    {
        if (Selected != null && OnSelect != null)
        {
            OnSelect(Selected);
            _drawChildren();
        }
        return ListState.None;
    }

    private ListState Edit()
    {
        if (Selected != null && OnEdit != null)
        {
            OnEdit(Selected);
        }
        return ListState.None;
    }

    private ListState Add()
    {
        if (OnAdd != null)
        {
            OnAdd();
            _drawChildren();
        }
        return ListState.None;
    }

    private ListState Remove()
    {
        if (Selected != null && OnRemove != null)
        {
            OnRemove(Selected);
            _drawChildren();
        }
        return ListState.None;
    }

    public ListState Handle(KeyPress ev) => statemachine.SendAction(ev);

    private ListState addCharToFilter(KeyPress c)
    {
        filterDisplay.HandleInput(c);
        _afterSetFilter();
        return ListState.Filtering;
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

    public void SetDisplayTypeName(string typeName)
    {
        var pre = $"{baseFilterDisplayText} {typeName}: {{0}}\n".Trim();
        filterDisplay.SetFormat(pre);
        _drawChildren();
    }

    public void SetShowTitle(bool showTitle)
    {
        this.showTitle = showTitle;
        filterDisplay.Graphics.alpha = showTitle ? 1 : 0;
        _drawStaticParts();
    }

    public void SetShowCount(bool showCount)
    {
        this.showCount = showCount;
        countDisplay.Graphics.alpha = showCount ? 1f : 0f;
        _drawStaticParts();
    }

    public void SetShowBackground(bool showBackground)
    {
        background = showBackground;
        _drawStaticParts();
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

        if (showCount)
        {
            countDisplay.SetText($"{sid + 1}/{filteredChildren.Count}");
            countDisplay.Graphics.y = 24 * (maxDisplay + 1);
        }
    }

    private void _drawStaticParts()
    {
        var titleBg = showTitle ? 2 : 0;
        var countBg = showCount ? 1 : 0;

        if (background)
        {
            var name = "textboxoutline";
            var c = ChildByName(name);
            if (c is not TextWithBoxOutline outline)
            {
                RemoveChild(c);
                outline = new TextWithBoxOutline() { Name = name };
                var longestDisplay = items.Count > 0 ? items.Select(x => display(x)).MaxBy(s => s.Length) ?? "" : "";

                var maxSize = Text.ParseText(longestDisplay).TextCharLength() + 2;
                outline.SetText(" ".Repeat(maxSize) + "\n".Repeat(maxDisplay + countBg + titleBg), new() { backgroundColor = Color.Black });
                outline.Graphics.x = -Text.DEFAULT_LINE_HEIGHT;
                AddChild(outline, index: 0);
                itemDisplay.y = Text.DEFAULT_LINE_HEIGHT * (titleBg + 1);
                filterDisplay.Graphics.y = Text.DEFAULT_LINE_HEIGHT;
            }
        }
        else
        {
            itemDisplay.y = Text.DEFAULT_LINE_HEIGHT * titleBg;
            filterDisplay.Graphics.y = 0;
        }
    }
}
