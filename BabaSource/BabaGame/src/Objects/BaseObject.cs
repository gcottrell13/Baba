using BabaGame.src.Resources;
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

        public char Facing { get; private set; }

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
            Facing = (phase ?? "u")[0];

            var palettePointer = JsonValues.Colors[name].colour;
            sprite.color = JsonValues.TryGetPaletteColor(World.Palette, palettePointer);
        }

        public void Move(char direction)
        {
            if (direction == 'u')
            {
                MoveY(-1);
            }
            else if (direction == 'd')
            {
                MoveY(1);
            }
            else if (direction == 'l')
            {
                MoveX(-1);
            }
            else if (direction == 'r')
            {
                MoveX(1);
            }
        }

        public void MoveX(int direction)
        {
            SetX(X + direction);
        }

        public void MoveY(int direction)
        {
            SetY(Y + direction);
        }

        public void SetX(int x)
        {
            if (x != X)
            {
                var (sx, _) = World.GameCoordToScreenCoord(x, Y);
                animateX = new AnimateValue(Graphics.x, sx, 0.1f);
                sprite.StepIndex();
                if (x < X) FaceDirection('l'); else FaceDirection('r');
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
                if (y < Y) FaceDirection('u'); else FaceDirection('d');
                Y = y;
            }
        }

        public void FaceDirection(char direction)
        {
            string phase = "";
            switch (direction)
            {
                case 'd': phase = "down"; break;
                case 'u': phase = "up"; break;
                case 'l': phase = "left"; break;
                case 'r': phase = "right"; break;
            }
            if (sprite.HasPhase(phase))
            {
                sprite.SetPhase(phase); 
                Facing = direction;
            }
        }

        public void AboutFace()
        {
            string phase = "";
            switch (Facing)
            {
                case 'd': phase = "up"; break;
                case 'u': phase = "down"; break;
                case 'l': phase = "right"; break;
                case 'r': phase = "left"; break;
            }
            if (sprite.HasPhase(phase))
            {
                sprite.SetPhase(phase);
                Facing = phase[0];
            }
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
