using BabaGame.Engine;
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
    private const float BIG_FACTOR = 2f;

    private ObjectTypeId previousName = 0;
    private int previousX = 0;
    private int previousY = 0;
    private Direction previousFacing = Direction.None;
    private short previousColor = 0;
    private ObjectKind previousKind;

    private SpriteValues? spriteValue;
    private ObjectSpriteContainer? currentSprite;
    private int moveStep = -1;

    private double wobbleTimer = 0;
    private double maxWobbleTimer = 0;
    private readonly string theme;

    public ObjectSprite(string theme)
	{
        maxWobbleTimer = CollectionExtension.rng.NextDouble() * 300 + 200;
        this.theme = theme;
    }

    private void setSprite(ObjectTypeId name, ObjectKind kind, Direction d)
    {
        var strname = ObjectInfo.IdToName[name];
        if (kind == ObjectKind.Text && !strname.StartsWith("text_")) strname = $"text_{strname}";

        spriteValue = ContentLoader.LoadedContent!.SpriteValues[strname];

        Graphics.zindex = ObjectInfo.Info[strname].layer;
        setWobbler(spriteValue.GetInitial(d));
        moveStep = -1;
    }

    private void setWobbler(Wobbler wobbler)
    {
        if (currentSprite?.wobbler != wobbler)
        {
            Graphics.RemoveChild(currentSprite);
            currentSprite = new ObjectSpriteContainer(wobbler);
            currentSprite.SetColor(ThemeInfo.GetColor(theme, previousColor));
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
    public void OnMove(BabaObject objectData, bool animate)
    {
        Name = $"{objectData.Kind}-{objectData.Name}";

        if (objectData.Name != previousName || objectData.Kind != previousKind)
        {
            // TODO: animate a changing sprite
            setSprite(objectData.Name, objectData.Kind, objectData.Facing);
            previousName = objectData.Name;
            previousKind = objectData.Kind;
        }

        if (previousX != objectData.x || previousY != objectData.y || previousFacing != objectData.Facing)
        {
            // TODO: animate a move
            Graphics.x = objectData.x;
            Graphics.y = objectData.y;

            previousX = objectData.x;
            previousY = objectData.y;
            previousFacing = objectData.Facing;

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
                        if (objectData.state.HasFlag(ObjectStatesToDisplay.Sleep))
                            setWobbler(f.Sleep(objectData.Facing, ref moveStep));
                        else
                            setWobbler(f.Move(objectData.Facing, ref moveStep));
                        break;
                    }
                case Joinable j:
                    {
                        setWobbler(j.Join(objectData.Facing));
                        break;
                    }
            }
        }

        if (currentSprite != null)
        {
            if (objectData.state.HasFlag(ObjectStatesToDisplay.Big))
            {
                currentSprite.SetRelativeScale(BIG_FACTOR);
                Graphics.x = objectData.x - (BIG_FACTOR / 4);
                Graphics.y = objectData.y - (BIG_FACTOR / 4);
            }
            else
            {
                currentSprite.SetRelativeScale(1);
                Graphics.x = objectData.x;
                Graphics.y = objectData.y;
            }
        }

        _afterOnMoveAnimation(objectData);
    }

    private void _afterOnMoveAnimation(BabaObject objectData)
    {
        if (objectData.Deleted)
        {
            Parent?.RemoveChild(this);
        }

        if (!objectData.Present)
        {
            Graphics.alpha = 0;
        }

        var (colorInactive, colorActive) = ThemeInfo.GetColorsByKind(objectData.Name, objectData.Kind);
        var color = objectData.Color == colorInactive || objectData.Color == colorActive
            ? (objectData.Active ? colorActive : colorInactive)
            : objectData.Color;

        if (color != previousColor && currentSprite != null)
        {
            currentSprite.SetColor(ThemeInfo.GetColor(theme, color));
            previousColor = color;
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
        xscale = 1f / wobbler.Size.X;
        yscale = 1f / wobbler.Size.Y;
    }

    public void SetRelativeScale(float scale)
    {
        xscale = scale / wobbler.Size.X;
        yscale = scale / wobbler.Size.Y;
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

