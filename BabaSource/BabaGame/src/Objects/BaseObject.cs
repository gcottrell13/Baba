using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.Objects
{
    public class BaseObject : GameObject
    {
        public string Name { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private readonly BaseObjectSprite sprite;

        private AnimateValue? animateX;
        private AnimateValue? animateY;

        public BaseObject(string name, int x, int y, string? phase=null)
        {
            Name = name;
            X = x;
            Y = y;
            sprite = new BaseObjectSprite(name, phase);
            Graphics.AddChild(sprite);
            var (px, py) = World.GameCoordToScreenCoord(x, y);
            Graphics.x = px;
            Graphics.y = py;
        }

        public void SetX(int x)
        {
            if (x != X)
            {
                var (sx, _) = World.GameCoordToScreenCoord(x, Y);
                animateX = new AnimateValue(Graphics.x, sx, 0.1f);
                sprite.StepIndex();
                if (x < X) turn('l'); else turn('r');
                X = x;
            }
        }

        public void SetY(int y)
        {
            if (y != Y)
            {
                var (_, sy) = World.GameCoordToScreenCoord(X, y);
                animateY = new AnimateValue(Graphics.y, sy, 0.1f);
                sprite.StepIndex();
                if (y < Y) turn('u'); else turn('d');
                Y = y;
            }
        }

        private void turn(char direction)
        {
            string phase = "";
            switch (direction)
            {
                case 'd': phase = "down"; break;
                case 'u': phase = "up"; break;
                case 'l': phase = "left"; break;
                case 'r': phase = "right"; break;
            }
            if (sprite.HasPhase(phase)) sprite.SetPhase(phase);
        }

        protected override void OnBeforeUpdate(GameTime gameTime)
        {
            if (animateX != null)
            {
                if (!animateX.ValueStillAlive(gameTime, out var x)) animateX = null;
                Graphics.x = x;
            }

            if (animateY != null)
            {
                if (!animateY.ValueStillAlive(gameTime, out var y)) animateY = null;
                Graphics.y = y;
            }

            base.OnBeforeUpdate(gameTime);
        }
    }
}
