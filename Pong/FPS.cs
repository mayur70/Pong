using Microsoft.Xna.Framework;
using System;

namespace Pong
{
    public sealed partial class FPS : DrawableGameComponent
    {
        private float fps;
        private float updateInterval = 1.0f;
        private float timeSinceLastUpdate = 0.0f;
        private float frameCount = 0;

        public FPS(Game game) : this(game, false, false, game.TargetElapsedTime)
        {
        }

        public FPS(Game game, bool synchWithVericalRetrace, bool isFixedTimeStep, TimeSpan targetElapsedTime) : base(game)
        {
            GraphicsDeviceManager graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));
            graphics.SynchronizeWithVerticalRetrace = synchWithVericalRetrace;
            Game.IsFixedTimeStep = isFixedTimeStep;
            Game.TargetElapsedTime = targetElapsedTime;
        }


        public sealed override void Initialize()
        {
            base.Initialize();
        }

        public sealed override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public sealed override void Draw(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;
            timeSinceLastUpdate += elapsed;
            if(timeSinceLastUpdate > updateInterval)
            {
                fps = frameCount / timeSinceLastUpdate;
                Game.Window.Title = "FPS: "+ fps.ToString() ;

                frameCount = 0;
                timeSinceLastUpdate -= updateInterval;
            }
            base.Draw(gameTime);
        }

    }
}
