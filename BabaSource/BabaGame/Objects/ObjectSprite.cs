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
    private int previousX;
    private int previousY;

    private SpriteValues? spriteValue;
    private ObjectSpriteContainer? currentSprite;
    private int moveStep;

    private double wobbleTimer;

    public ObjectSprite(ObjectData objectData)
	{
        this.objectData = objectData;
        previousName = objectData.Name;
        previousX = objectData.X;
        previousY = objectData.Y;
        setSprite(objectData.Name, objectData.Facing);
        wobbleTimer = CollectionExtension.rng.NextDouble() * 500;
        MoveSpriteNoAnimate();
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

        Name = $"{objectData.Kind}-{objectData.Name}";
    }

    private void setSprite(ObjectTypeId name, Direction d)
    {
        spriteValue = ContentLoader.LoadedContent!.SpriteValues[ObjectInfo.IdToName[name]];
        setWobbler(spriteValue.GetInitial(d));
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

    public void Check()
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

        if (objectData.Deleted)
        {
            Parent?.RemoveChild(this);
        }

        if (!objectData.Present)
        {
            Graphics.alpha = 0;
        }
    }

    protected override void OnUpdate(GameTime gameTime)
    {
        wobbleTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (wobbleTimer > 500)
        {
            wobbleTimer = 0;
            currentSprite?.Step();
        }

        Check();
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

