using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public static class Scene
    {
        public static RenderTarget2D scene { get; private set; }
        public static SpriteBatch spriteBatch { get; private set; }

        private static Stack<TransformationItem> transformStack;

        public static int X { get; set; }
        public static int Y { get; set; }

        public static void Initialize(SpriteBatch spriteBatch, int width, int height)
        {
            scene = new RenderTarget2D(spriteBatch.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            Scene.spriteBatch = spriteBatch;
        }

        public static void Begin()
        {
            spriteBatch.GraphicsDevice.SetRenderTarget(scene);
            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(X, Y, 0), samplerState: SamplerState.PointClamp);
            transformStack = new Stack<TransformationItem>();
            transformStack.Push(new TransformationItem
            {
                Transform = Matrix.Identity,
                Rotation = 0,
                Alpha = 1.0f,
            });
        }

        public static void EnterSpriteDrawing(BaseSprite sprite)
        {
            transformStack.Push(new TransformationItem
            {
                Transform = sprite.TransformMatrix,
                Rotation = sprite.rotation,
                Alpha = sprite.alpha,
            } * transformStack.Peek());
        }

        public static void ExitSpriteDrawing()
        {
            transformStack.Pop();
        }

        public static TransformationItem GetCurrentTransformation()
        {
            return transformStack.Peek();
        }

        public static void End()
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
        }
    }

    public class SceneContextManager : IDisposable
    {
        public SceneContextManager(BaseSprite sprite)
        {
            Sprite = sprite;
            Scene.EnterSpriteDrawing(sprite);
        }

        public BaseSprite Sprite { get; }

        public void Dispose()
        {
            Scene.ExitSpriteDrawing();
        }
    }

    public struct TransformationItem
    {
        public Matrix Transform;
        public double Rotation;
        public float Alpha;

        public static TransformationItem operator *(TransformationItem t1, TransformationItem t2)
        {
            return new TransformationItem
            {
                Transform = t1.Transform * t2.Transform,
                Rotation = t1.Rotation + t2.Rotation,
                Alpha = t1.Alpha * t2.Alpha,
            };
        }
    }
}
