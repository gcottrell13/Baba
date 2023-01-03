using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Core.Screens
{
    public class ScreenStack : GameObject
    {
        private readonly Stack<BaseScreen> stack = new();

        public ScreenStack()
        {
        }

        public void Add(BaseScreen? screen)
        {
            if (screen == null) return;
            add(screen);
            ensureVisibility();
        }

        /// <summary>
        /// Makes sure that this screen is on the top of the stack
        /// </summary>
        /// <param name="screen"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void EnsureTop(BaseScreen? screen)
        {
            if (screen == null)
            {
                throw new ArgumentNullException("Screen cannot be null");
            }
            popTo(screen);
            pop();
            add(screen);
            ensureVisibility();
        }

        public BaseScreen Pop()
        {
            var p = pop();
            ensureVisibility();
            return p;
        }

        public void PopTo(BaseScreen? screen)
        {
            popTo(screen);
            ensureVisibility();
        }

        private void add(BaseScreen screen)
        {
            RemoveAllChildren();
            stack.Push(screen);
        }

        private BaseScreen pop()
        {
            var popped = stack.TryPop(out var poppedItem);
            if (popped && poppedItem != null)
            {
                RemoveChild(poppedItem);
                return poppedItem;
            }
            throw new Exception("stack is empty");
        }

        private void popTo(BaseScreen? screen)
        {
            while (stack.TryPeek(out var top) && top != screen)
            {
                pop();
            }
        }

        private void ensureVisibility()
        {
            var visibleScreens = new List<BaseScreen>();
            foreach (var item in stack)
            {
                visibleScreens.Insert(0, item);
                if (!item.Transparent) break;
            }
            foreach (var item in visibleScreens)
            {
                item.HideCommands();
                AddChild(item);
            }
            if (stack.TryPeek(out var top)) top.ShowCommands();
        }
    }
}
