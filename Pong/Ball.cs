using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Ball
    {
        public float x;
        public float y;
        private float width;
        private float height;

        public float dx;
        public float dy;


        public Rectangle transform
        {
            get
            {
                return new Rectangle((int)x, (int)y, (int)width, (int)height);
            }
        }


        public Ball(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            dx = GameMain.random.Next(2) == 1 ? -100 : 100;
            dy = GameMain.random.Next(-50, 50);
        }

        public void Update(float delta)
        {
            x += (dx * delta);
            y += (dy * delta);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameMain.pixel, new Rectangle((int)x, (int)y, (int)width, (int)height), Color.White);
        }

        public void Reset()
        {
            x = GameMain.VIRTUAL_WIDTH / 2 - width / 2;
            y = GameMain.VIRTUAL_HEIGHT / 2 - height / 2;

            dx = GameMain.random.Next(2) == 1 ? -100 : 100;
            dy = GameMain.random.Next(-50, 50);
        }

        public bool Collides(Paddle paddle)
        {
            return transform.Intersects(paddle.transform);
        }
    }
}
