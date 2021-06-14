using BabaGame.src.Engine;
using BabaGame.src.Events;
using BabaGame.src.Resources;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public MapData MapData;

        public bool Dead { get; set; }

        public Direction Facing { get; private set; }

        public ObjectType Type { get; private set; }

        public bool Joinable { get; private set; }

        public string Color { get; private set; }

        public List<(int x, int y)> PreviousCoordinates { get; }

        public BaseObject(string name, int x, int y, MapData map, string? phase=null)
        {
            Name = name;
            MapData = map;
            PreviousCoordinates = new List<(int x, int y)>();

            X = x;
            Y = y;

            sprite = new BaseObjectSprite(name, phase);
            Graphics.AddChild(sprite);

            AnimateGraphicsToPoint(X, Y);

            Facing = DirectionExtensions.FromString(phase);

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

        protected void OnMove(int oldX, int oldY, Direction direction)
        {
            if (MapData != null && Joinable)
            {
                EventChannels.ScheduledCallback.SendAsyncMessage(new Core.Events.ScheduledCallback(0.001f)
                {
                    Callback = () =>
                    {
                        MapData.JoinableObjectUpdate(this);
                        foreach (var obj in MapData.GetObjectsNear(oldX, oldY).SelectMany(kvp => kvp.Value))
                        {
                            MapData.JoinableObjectUpdate(obj);
                        }
                        foreach (var obj in MapData.GetObjectsNear(X, Y).SelectMany(kvp => kvp.Value))
                        {
                            MapData.JoinableObjectUpdate(obj);
                        }
                    },
                });
            }

            PreviousCoordinates.Add((oldX, oldY));
            if (PreviousCoordinates.Count > 10)
                PreviousCoordinates.RemoveAt(0);
        }

        private void AnimateGraphicsToPoint(int x, int y)
        {
            var (sx, sy) = WorldVariables.GameCoordToScreenCoord(x, y);
            sx -= MapData.MapX * WorldVariables.MapWidthPixels;
            sy -= MapData.MapY * WorldVariables.MapHeightPixels;

            animateX = new AnimateValue(Graphics.x, sx, WorldVariables.MoveAnimationSeconds, f => (float)Math.Sqrt(f));
            animateY = new AnimateValue(Graphics.y, sy, WorldVariables.MoveAnimationSeconds, f => (float)Math.Sqrt(f));
        }

        public void SetX(int x)
        {
            if (x != X)
            {
                AnimateGraphicsToPoint(x, Y);

                sprite.StepIndex();
                if (x < X) FaceDirection(Direction.Left); else FaceDirection(Direction.Right);

                OnMove(X, Y, x < X ? Direction.Left : Direction.Right);
                X = x;
            }
        }

        public void SetY(int y)
        {
            if (y != Y)
            {
                AnimateGraphicsToPoint(X, y);
                sprite.StepIndex();
                if (y < Y) FaceDirection(Direction.Up); else FaceDirection(Direction.Down);

                OnMove(X, Y, y < Y ? Direction.Up : Direction.Down);
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

        public void FaceDirection(Direction? direction)
        {
            string phase = direction?.ToString().ToLower() ?? "up";

            if (sprite.HasPhase(phase) && Facing != direction && direction != null)
            {
                sprite.SetPhase(phase); 
                Facing = direction ?? Direction.None;
            }
        }

        public void AboutFace()
        {
            FaceDirection(Facing switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => Facing,
            });
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
