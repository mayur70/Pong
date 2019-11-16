using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Paddle
    {
        public float x;
        public float y;
        private float width;
        private float height;
        public float dy;

        public Rectangle transform
        {
            get
            {
                return new Rectangle((int)x, (int)y, (int)width, (int)height);
            }
        }

        public Paddle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            dy = 0f;
        }

        public void Update(float delta)
        {
            if (dy < 0)
                y = MathHelper.Max(0, y + (dy * delta));
            else
                y = MathHelper.Min(GameMain.VIRTUAL_HEIGHT - height, y + (dy * delta));
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameMain.pixel, transform, Color.White);
        }
    }
}
