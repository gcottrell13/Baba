using Core.Bootstrap;
using Core.Events;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
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

        private string? _name = null;
        public string Name { get
            {
                if (_name == null) 
                    _name = GetType().Name;
                return _name;
            }
            set { _name = value; } }

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnAfterChildrenUpdate(GameTime gameTime) { }
        protected virtual void OnAfterInitialize() { }

        public GameObject? ChildByName(string name)
        {
            return Children.First(x => x.Name == name);
        }

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
            foreach (var child in Children.ToList())
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


        public struct KeyPress
        {
            public Keys KeyPressed;
            public ModifierKeys ModifierKeys;
            public char Text;
        }

        public enum ModifierKeys
        {
            Shift = 0b1,
            Ctrl = 0b10,
            Alt = 0b100,
        }

        public delegate void OnGameKeyPressEvent(KeyPress ev);
        public delegate void Unsubscribe();
        public Unsubscribe onKeyPress(OnGameKeyPressEvent cb)
        {
            void _processKeyEvent(KeyEvent ev)
            {
                if (ev.Up) 
                    return;
                var pressed = KeyboardState.GetPressedKeys();
                var mod = (pressed.Contains(Keys.LeftShift) || pressed.Contains(Keys.RightShift) ? ModifierKeys.Shift : 0) |
                    (pressed.Contains(Keys.LeftControl) || pressed.Contains(Keys.RightControl) ? ModifierKeys.Ctrl : 0) |
                    (pressed.Contains(Keys.LeftAlt) || pressed.Contains(Keys.RightAlt) ? ModifierKeys.Alt : 0);
                cb(new() { ModifierKeys = mod, KeyPressed = ev.ChangedKey, Text = ev.ChangedKey.ToChar((mod & ModifierKeys.Shift) != 0) });
            }
            CoreEventChannels.KeyEvent.Subscribe(_processKeyEvent);
            return () => CoreEventChannels.KeyEvent.Unsubscribe(_processKeyEvent);
        }
    }
}
