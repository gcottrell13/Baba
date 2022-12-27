using Core.Bootstrap;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core
{
    [DebuggerDisplay("GameObject Name={Name}")]
    public abstract class GameObject
    {
        protected MouseState MouseState => CoreMouse.mouseState;
        protected KeyboardState KeyboardState => CoreKeyboard.keyboardState;

        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        public static void SetScreenSize(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
        }

        public GameObject? Parent { get; private set; }
        public List<GameObject> Children { get; private set; } = new List<GameObject>();

        public SpriteContainer Graphics { get; protected set; } = new SpriteContainer();

        public string Name { get; set; }

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnAfterChildrenUpdate(GameTime gameTime) { }
        protected virtual void OnAfterInitialize() { }

        protected virtual void OnDestroy() { }

        public void Destroy()
        {
            foreach (var child in Children)
            {
                child.Destroy();
            }

            Graphics.Destroy();

            OnDestroy();

            Children.Clear();
        }

        public void AfterInitialize()
        {
            foreach (var child in Children)
            {
                child.OnAfterInitialize();
                child.AfterInitialize();
            }
        }


        public void Tick(GameTime gameTime)
        {
            OnUpdate(gameTime);

            foreach (var child in Children)
            {
                child.Tick(gameTime);
            }

            OnAfterChildrenUpdate(gameTime);
        }

        public void RemoveAllChildren(bool graphics = true)
        {
            foreach (var child in Children)
            {
                RemoveChild(child, graphics);
            }
        }

        public void RemoveChild(GameObject gameObject, bool graphics = true)
        {
            if (Children.Contains(gameObject))
            {
                Children.Remove(gameObject);
                if (graphics)
                {
                    Graphics.RemoveChild(gameObject.Graphics);
                }
                gameObject.Parent = null;
            }
        }

        public void AddChild(GameObject gameObject, bool addGraphics = true)
        {
            if (Children.Contains(gameObject)) return;
            Children.Add(gameObject);
            gameObject.SetParent(this, addGraphics);
        }

        public void SetParent(GameObject newParent, bool addGraphics = true)
        {
            if (Parent == null || Parent == newParent)
            {
                Parent = newParent;

                if (addGraphics)
                {
                    Parent.Graphics.AddChild(Graphics);
                }
            }
            else
            {
                throw new Exception("GameObject already has a parent");
            }
        }
    }
}
