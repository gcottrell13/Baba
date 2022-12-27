using System.Collections.Generic;

namespace Core.Screens
{
    public class ScreenStack
    {
        private readonly Stack<BaseScreen> stack = new();
        private readonly GameObject container;

        public ScreenStack(GameObject container)
        {
            this.container = container;
        }

        public void Add(BaseScreen screen)
        {
            container.RemoveAllChildren();
            stack.Push(screen);
            var visibleScreens = new List<BaseScreen>();
            foreach (var item in stack)
            {
                visibleScreens.Add(item);
                if (!item.Transparent) break;
            }
            foreach (var item in visibleScreens)
            {
                container.AddChild(item);
            }
        }

        public void Pop()
        {
            var popped = stack.TryPop(out var poppedItem);
            if (popped)
                container.RemoveChild(poppedItem);
        }

        public void PopTo(BaseScreen? screen)
        {
            while (stack.TryPeek(out var top) && top != screen) { 
                Pop(); 
            }
        }
    }
}
