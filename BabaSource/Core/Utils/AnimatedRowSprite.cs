using Core.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Utils
{
    public class AnimatedRowSprite : Sprite
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        private int _currentFrame;
        public int CurrentFrame { get
            {
                if (RestrictedRow == null) return _currentFrame;
                return _currentFrame % Columns;
            }
            private set
            {
                _currentFrame = value;
            }
        }
        private int totalFrames;

        public int? RestrictedRow { get; private set; }

        public int FrameWidth => graphicsResource.Value.Width / Columns;
        public int FrameHeight => graphicsResource.Value.Height / Rows;

        public AnimatedRowSprite(ResourceHandle<Texture2D> graphicsResource, int rows, int columns) : base(graphicsResource)
        {
            Rows = rows;
            Columns = columns;
            CurrentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void SetRow(int? row)
        {
            RestrictedRow = row;
            if (row == null)
            {
                _currentFrame = 0;
            }
            else
            {
                _currentFrame = (int)RestrictedRow * Columns;
            }
        }

        public void Reset()
        {
            SetRow(RestrictedRow);
        }

        public void Update()
        {
            _currentFrame++;

            if (RestrictedRow != null)
            {
                if (_currentFrame >= (RestrictedRow + 1) * Columns)
                {
                    _currentFrame = (int)RestrictedRow * Columns;
                }
            }
            else if (_currentFrame == totalFrames)
                _currentFrame = 0;

        }

        public override void Draw()
        {
            int width = graphicsResource.Value.Width / Columns;
            int height = graphicsResource.Value.Height / Rows;
            int row = (int)((float)_currentFrame / (float)Columns);
            int column = _currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);

            draw(sourceRectangle);
        }
    }
}
