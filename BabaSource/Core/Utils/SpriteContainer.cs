using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public sealed class SpriteContainer : BaseSprite
    {
        public readonly List<BaseSprite> children = new List<BaseSprite>();

        private bool _shouldReorder = false;

        public override void Draw()
        {
            _reorderChildren();
            RecalculateTransform();
            using var t = new SceneContextManager(this);
            foreach (var child in children)
            {
                if (child.alpha > 0)
                    child.Draw();
            }
        }

        public void AddChild(BaseSprite child)
        {
            children.Add(child);
            child.parent = this;
            ReorderChildren();
        }

        public void RemoveChild(BaseSprite child)
        {
            if (children.Contains(child))
            {
                children.Remove(child);
                child.parent = null;
            }
        }

        public void ReorderChildren()
        {
            // do the sorting after the current update to avoid performance issues
            _shouldReorder = true;
        }

        private void _reorderChildren()
        {
            if (_shouldReorder)
            {
                children.Sort((a, b) => (int)a.zindex - (int)b.zindex);
                _shouldReorder = false;
            }
        }

        protected override void OnDestroy()
        {
            foreach (var child in children)
                child.Destroy();
            children.Clear();
        }
    }
}
