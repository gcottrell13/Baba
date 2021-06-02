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

        protected GameObject parent { get; private set; }
        protected List<GameObject> children = new List<GameObject>();

        public SpriteContainer Graphics { get; protected set; } = new SpriteContainer();

        protected virtual void OnBeforeUpdate(GameTime gameTime) { }
        protected virtual void OnAfterUpdate(GameTime gameTime) { }
        protected virtual void OnAfterInitialize() { }

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
            OnBeforeUpdate(gameTime);

            foreach (var child in children)
            {
                child.Tick(gameTime);
            }

            OnAfterUpdate(gameTime);
        }

        public void AddChild(GameObject gameObject, bool addGraphics = false)
        {
            children.Add(gameObject);
            gameObject.SetParent(this, addGraphics);
        }

        public void SetParent(GameObject gameObject, bool addGraphics = false)
        {
            if (parent == null)
            {
                parent = gameObject;

                if (addGraphics)
                {
                    parent.Graphics.AddChild(gameObject.Graphics);
                }
            }
            else
            {
                throw new Exception("GameObject already has a parent");
            }
        }
    }
}
