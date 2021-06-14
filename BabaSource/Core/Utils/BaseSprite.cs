using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public abstract class BaseSprite
    {
        private bool _shouldRecalculateTransform = false;

        private float _x;
        private float _y;
        private float _xscale = 1;
        private float _yscale = 1;
        private float _rotation;
        private float _xanchor;
        private float _yanchor;

        public float alpha { get; set; } = 1;

        public Color color { get; set; } = Color.White;

        public float x
        {
            get => _x; set {
                _x = value;
                _shouldRecalculateTransform = true;
            }
        }
        public float y
        {
            get => _y; set {
                _y = value;
                _shouldRecalculateTransform = true;
            }
        }
        public float xscale
        {
            get => _xscale; set {
                _xscale = value;
                _shouldRecalculateTransform = true;
            }
        }
        public float yscale
        {
            get => _yscale; set
            {
                _yscale = value;
                _shouldRecalculateTransform = true;
            }
        }
        public float rotation
        {
            get => _rotation; set
            {
                _rotation = value;
                _shouldRecalculateTransform = true;
            }
        }
        public float xanchor
        {
            get => _xanchor; set
            {
                _xanchor = value;
                _shouldRecalculateTransform = true;
            }
        }
        public float yanchor
        {
            get => _yanchor; set
            {
                _yanchor = value;
                _shouldRecalculateTransform = true;
            }
        }


        public SpriteContainer? parent;

        private Matrix transformMatrix = Matrix.Identity;
        public Matrix TransformMatrix => transformMatrix;

        public int _z;
        public int zindex
        {
            get => _z; set
            {
                _z = value;
                if (parent != null)
                {
                    parent.ReorderChildren();
                }
            }
        }

        public void Destroy() { OnDestroy();  }

        protected virtual void OnDestroy() { }

        public abstract void Draw();

        public void RecalculateTransform()
        {
            if (_shouldRecalculateTransform)
            {
                transformMatrix =
                    Matrix.CreateScale(xscale, yscale, 1)
                    * Matrix.CreateRotationZ(rotation) 
                    * Matrix.CreateTranslation(x - xanchor, y - yanchor, 0);
                _shouldRecalculateTransform = false;
            }
        }
    }
}
