using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public abstract class GameObject
    {
        protected MouseState MouseState => CoreMouse.mouseState;
        protected KeyboardState KeyboardState => CoreKeyboard.keyboardState;

        protected int ScreenWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        protected int ScreenHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        protected GameObject? parent { get; private set; }
        protected List<GameObject> children = new List<GameObject>();

        public SpriteContainer Graphics { get; protected set; } = new SpriteContainer();

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnAfterChildrenUpdate(GameTime gameTime) { }
        protected virtual void OnAfterInitialize() { }

        protected virtual void OnDestroy() { }

        public void Destroy()
        {
            foreach (var child in children)
            {
                child.Destroy();
            }

            Graphics.Destroy();

            OnDestroy();

            children.Clear();
        }

        public void AfterInitialize()
        {
            foreach (var child in children)
            {
                child.OnAfterInitialize();
                child.AfterInitialize();
            }
        }


        public void Tick(GameTime gameTime)
        {
            OnUpdate(gameTime);

            foreach (var child in children)
            {
                child.Tick(gameTime);
            }

            OnAfterChildrenUpdate(gameTime);
        }

        public void RemoveChild(GameObject gameObject, bool graphics = false)
        {
            if (children.Contains(gameObject))
            {
                children.Remove(gameObject);
                gameObject.RemoveParent(graphics);
            }
        }

        public void AddChild(GameObject gameObject, bool addGraphics = false)
        {
            children.Add(gameObject);
            gameObject.SetParent(this, addGraphics);
        }

        public void SetParent(GameObject newParent, bool addGraphics = false)
        {
            if (parent == null || parent == newParent)
            {
                parent = newParent;

                if (addGraphics)
                {
                    parent.Graphics.AddChild(Graphics);
                }
            }
            else
            {
                throw new Exception("GameObject already has a parent");
            }
        }

        public void RemoveParent(bool graphics = false)
        {
            if (parent != null)
            {
                parent = null;

                if (graphics)
                {
                    Graphics.parent = null;
                }
            }
        }
    }
}
