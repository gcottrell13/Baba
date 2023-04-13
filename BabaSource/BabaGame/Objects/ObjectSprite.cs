using Core;
using Core.Configuration;
using Core.Content;
using Core.Engine;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Objects;


/// <summary>
/// Handles displaying and animating the sprites.
/// </summary>
internal class ObjectSprite : GameObject
{
    private readonly ObjectData objectData;

    private ObjectTypeId previousName;
    private int previousX = 0;
    private int previousY = 0;
    private short previousColor = 0;

    private SpriteValues? spriteValue;
    private ObjectSpriteContainer? currentSprite;
    private int moveStep = 0;

    private double wobbleTimer = 0;
    private double maxWobbleTimer = 0;

    public ObjectSprite(ObjectData objectData)
	{
        this.objectData = objectData;
        maxWobbleTimer = CollectionExtension.rng.NextDouble() * 300 + 200;
    }

    /// <summary>
    /// used for initialization, and when reloading a map
    /// </summary>
    public void MoveSpriteNoAnimate()
    {
        previousName = objectData.Name;
        previousX = objectData.X;
        previousY = objectData.Y;
        Graphics.x = objectData.x;
        Graphics.y = objectData.y;
        previousColor = objectData.Color;

        Name = $"{objectData.Kind}-{objectData.Name}";
        setSprite(previousName, objectData.Facing);
    }

    private void setSprite(ObjectTypeId name, Direction d)
    {
        spriteValue = ContentLoader.LoadedContent!.SpriteValues[ObjectInfo.IdToName[name]];
        setWobbler(spriteValue.GetInitial(d));
        Graphics.SetColor(ThemeInfo.GetObjectColor("default", ObjectInfo.IdToName[name]));
    }

    private void setWobbler(Wobbler wobbler)
    {
        if (currentSprite?.wobbler != wobbler)
        {
            Graphics.RemoveChild(currentSprite);
            currentSprite = new(wobbler);
            Graphics.AddChild(currentSprite);
        }
    }

    /// <summary>
    /// constant game tick
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void OnUpdate(GameTime gameTime)
    {
        wobbleTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (wobbleTimer > maxWobbleTimer)
        {
            wobbleTimer = 0;
            currentSprite?.Step();
        }
    }


    /// <summary>
    /// for when something happened to the game state
    /// </summary>
    public void OnMove(bool isSleeping)
    {
        if (objectData.Name != previousName)
        {
            // TODO: animate a changing sprite
            setSprite(objectData.Name, objectData.Facing);
            previousName = objectData.Name;
        }

        if (previousX != objectData.x || previousY != objectData.y)
        {
            // TODO: animate a move
            Graphics.x = objectData.x;
            Graphics.y = objectData.y;

            // if the sprite allows, animate it
            switch (spriteValue)
            {
                case AnimateOnMove e:
                    {
                        setWobbler(e.Move(ref moveStep));
                        break;
                    }
                case FacingOnMove f:
                    {
                        if (isSleeping)
                            setWobbler(f.Sleep(objectData.Facing, ref moveStep));
                        else
                            setWobbler(f.Move(objectData.Facing, ref moveStep));
                        break;
                    }
                case Joinable j:
                    {
                        setWobbler(j.Join(Direction.None)); // TODO: get direction
                        break;
                    }
            }

            previousX = objectData.x;
            previousY = objectData.y;
        }

        if (objectData.Color != previousColor)
        {
            Graphics.SetColor(ThemeInfo.GetColor("default", objectData.Color));
            previousColor = objectData.Color;
        }
    }

    private void _afterOnMoveAnimation()
    {

        if (objectData.Deleted)
        {
            Parent?.RemoveChild(this);
        }

        if (!objectData.Present)
        {
            Graphics.alpha = 0;
        }
    }
}


internal class ObjectSpriteContainer : Sprite
{
    public ResourceHandle<Texture2D> Sprite { get => graphicsResource; set => graphicsResource = value; }

    public Wobbler wobbler { get; private set; }

    private int step = 0;

    public ObjectSpriteContainer(Wobbler wobbler) : base(wobbler.GetInitial(Direction.None).Texture)
    {
        this.wobbler = wobbler;
    }

    public void SetWobbler(Wobbler w)
    {
        wobbler = w;

    }

    public void Step()
    {
        step ++;
    }

    public override void Draw()
    {
        var rect = wobbler.GetPosition(ref step);
        draw(rect);
    }
}

