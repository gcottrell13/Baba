using BabaGame.src.Resources;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BabaGame.src.Objects
{
    [DebuggerDisplay("{Name} ({X} ,{Y}) {Facing}")]
    public class BaseObject : GameObject
    {
        public string Name { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private readonly BaseObjectSprite sprite;

        private AnimateValue? animateX;
        private AnimateValue? animateY;

        public bool Dead { get; set; }

        public char Facing { get; private set; }

        public ObjectType Type { get; private set; }

        public bool Joinable { get; private set; }

        public string Color { get; private set; }

        public BaseObject(string name, int x, int y, string? phase=null)
        {
            Name = name;
            X = x;
            Y = y;
            sprite = new BaseObjectSprite(name, phase);
            Graphics.AddChild(sprite);
            var (px, py) = WorldVariables.GameCoordToScreenCoord(x, y);
            Graphics.x = px;
            Graphics.y = py;
            Facing = (phase ?? "u")[0];

            var info = JsonValues.ObjectInfo[name];

            Type = info.unittype switch
            {
                "text" => ObjectType.Text,
                "object" => ObjectType.Object,
                _ => ObjectType.Unknown,
            };

            var palettePointer = info.color_inactive;
            sprite.color = JsonValues.TryGetPaletteColor(WorldVariables.Palette, palettePointer);
            Color = "";
            Joinable = JsonValues.Animations[name].ContainsKey("0");
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
                var (sx, _) = WorldVariables.GameCoordToScreenCoord(x, Y);
                animateX = new AnimateValue(Graphics.x, sx, WorldVariables.MoveAnimationSeconds);
                sprite.StepIndex();
                if (x < X) FaceDirection('l'); else FaceDirection('r');
                X = x;
            }
        }

        public void SetY(int y)
        {
            if (y != Y)
            {
                var (_, sy) = WorldVariables.GameCoordToScreenCoord(X, y);
                animateY = new AnimateValue(Graphics.y, sy, WorldVariables.MoveAnimationSeconds);
                sprite.StepIndex();
                if (y < Y) FaceDirection('u'); else FaceDirection('d');
                Y = y;
            }
        }

        public void SetColor(string color)
        {
            if (JsonValues.ObjectInfo.ContainsKey("text_" + color))
            {
                var c = JsonValues.ObjectInfo["text_" + color];
                var palettePointer = c.color ?? c.color_inactive;
                sprite.color = JsonValues.TryGetPaletteColor(WorldVariables.Palette, palettePointer);
                Color = color;
            }
            else
            {
                Color = "";
                var c = JsonValues.ObjectInfo[Name];
                var palettePointer = c.color ?? c.color_inactive;
                sprite.color = JsonValues.TryGetPaletteColor(WorldVariables.Palette, palettePointer);
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
            if (sprite.HasPhase(phase) && Facing != direction)
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

        public void SetJoinWithNeighbors(string phase)
        {
            if (sprite.Phase != phase)
                sprite.SetPhase(phase);
        }

        protected override void OnUpdate(GameTime gameTime)
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

            base.OnUpdate(gameTime);
        }
    }
}
