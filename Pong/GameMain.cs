using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{


    enum GameState
    {
        Start,
        Serve,
        Play,
        Done
    }

    public class GameMain : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        const int WINDOW_WIDTH = 1280;
        const int WINDOW_HEIGHT = 720;
        public const int VIRTUAL_WIDTH = 432;
        public const int VIRTUAL_HEIGHT = 243;

        Color backgroundColor;
        public static Texture2D pixel;
        public static Random random;

        private Paddle player1;
        private Paddle player2;
        private Ball ball;

        private int player1Score;
        private int player2Score;

        private int winningPlayer;
        private int servingPlayer;

        private SpriteFont smallFont;
        private SpriteFont largeFont;
        private SpriteFont scoreFont;

        private SoundEffect paddleHitSound;
        private SoundEffect scoreSound;
        private SoundEffect wallHitSound;

        const float PADDLE_SPEED = 300f;

        private IInputHandler input;

        private GameState gameState;

        private readonly FPS fps;
        


        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
#if DEBUG
            fps = new FPS(this);
#else
            fps = new FPS(this, true, true, this.TargetElapsedTime); 
#endif
            Components.Add(fps);
            input = new InputHandler(this);
            Components.Add((IGameComponent)input);
        }

        protected override void Initialize()
        {
            random = new Random();

            backgroundColor = new Color(40, 45, 52);

            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Window.AllowUserResizing = true;
            graphics.ApplyChanges();

            ScalingClever.ResolutionScaling.Initialize(this, new Point(VIRTUAL_WIDTH, VIRTUAL_HEIGHT));

            Window.Title = "Pong";

            player1 = new Paddle(10, 30, 5, 20);
            player2 = new Paddle(VIRTUAL_WIDTH - 10, VIRTUAL_HEIGHT - 30, 5, 20);

            ball = new Ball(VIRTUAL_WIDTH / 2 - 2, VIRTUAL_HEIGHT / 2 - 2, 4, 4);

            servingPlayer = 1;
            winningPlayer = 0;

            gameState = GameState.Start;

            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            ScalingClever.ResolutionScaling.LoadContent(this, new Point(VIRTUAL_WIDTH, VIRTUAL_HEIGHT));
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] color = new Color[1];
            color[0] = Color.White;
            pixel.SetData(color);

            smallFont = Content.Load<SpriteFont>("SmallFont");
            largeFont = Content.Load<SpriteFont>("LargeFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            paddleHitSound = Content.Load<SoundEffect>("paddle_hit");
            scoreSound = Content.Load<SoundEffect>("score");
            wallHitSound = Content.Load<SoundEffect>("wall_hit");
            
        }

        protected override bool BeginDraw()
        {
            ScalingClever.ResolutionScaling.BeginDraw(this);
            return base.BeginDraw();
        }

        protected override void EndDraw()
        {
            ScalingClever.ResolutionScaling.EndDraw(this, spriteBatch);
            base.EndDraw();
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (input.GamePadState.Buttons.Back == ButtonState.Pressed || input.KeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // start management
            if (input.KeyboardState.IsKeyDown(Keys.Enter))
            {
                if (gameState == GameState.Start)
                    gameState = GameState.Serve;
                else if (gameState == GameState.Serve)
                    gameState = GameState.Play;
                else if (gameState == GameState.Done)
                {
                    gameState = GameState.Serve;

                    ball.Reset();

                    player1Score = 0;
                    player2Score = 0;

                    if (winningPlayer == 1)
                        servingPlayer = 2;
                    else
                        servingPlayer = 1;
                }
            }


            if (gameState == GameState.Serve)
            {
                ball.dy = random.Next(-50, 50);
                if (servingPlayer == 1)
                    ball.dx = random.Next(140, 200);
                else
                    ball.dx = -random.Next(140, 200);
            }
            else if(gameState == GameState.Play)
            {
                if (ball.Collides(player1))
                {
                    ball.dx = -ball.dx * 1.03f;
                    ball.x = player1.x + player1.transform.Width;

                    paddleHitSound.Play();

                    if (ball.dy < 0)
                        ball.dy = -random.Next(10, 150);
                    else
                        ball.dy = random.Next(10, 150);
                }

                if (ball.Collides(player2))
                {
                    ball.dx = -ball.dx * 1.03f;
                    ball.x = player2.x - ball.transform.Width;

                    paddleHitSound.Play();

                    if (ball.dy < 0)
                        ball.dy = -random.Next(10, 150);
                    else
                        ball.dy = random.Next(10, 150);
                }

                if(ball.y < 0)
                {
                    ball.y = 0;
                    ball.dy = -ball.dy;

                    wallHitSound.Play();
                }
                if(ball.y > VIRTUAL_HEIGHT - ball.transform.Height)
                {
                    ball.y = VIRTUAL_HEIGHT - ball.transform.Height;
                    ball.dy = -ball.dy;

                    wallHitSound.Play();
                }

                if(ball.x < 0)
                {
                    servingPlayer = 1;
                    player2Score++;

                    scoreSound.Play();

                    if(player2Score == 10)
                    {
                        winningPlayer = 2;
                        gameState = GameState.Done;
                    }
                    else
                    {
                        ball.Reset();
                        gameState = GameState.Serve;
                    }
                }
                if(ball.x > VIRTUAL_WIDTH)
                {
                    servingPlayer = 2;
                    player1Score++;

                    scoreSound.Play();

                    if (player1Score == 10)
                    {
                        winningPlayer = 1;
                        gameState = GameState.Done;
                    }
                    else
                    {
                        ball.Reset();
                        gameState = GameState.Serve;
                    }
                }
            }

            if (input.KeyboardState.IsKeyDown(Keys.W))
                player1.dy = -PADDLE_SPEED;
            else if (input.KeyboardState.IsKeyDown(Keys.S))
                player1.dy = PADDLE_SPEED;
            else
                player1.dy = 0;

            if (input.KeyboardState.IsKeyDown(Keys.Up))
                player2.dy = -PADDLE_SPEED;
            else if (input.KeyboardState.IsKeyDown(Keys.Down))
                player2.dy = PADDLE_SPEED;
            else
                player2.dy = 0;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (gameState == GameState.Play)
            {
                ball.Update(delta);
            }


            player1.Update(delta);
            player2.Update(delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            DisplayScore();

            if(gameState == GameState.Start)
            {
                string welcomeMsg = "Welcome to Pong!";
                string beginMsg = "Press Enter to begin!";
                Vector2 msgSize = smallFont.MeasureString(welcomeMsg);
                spriteBatch.DrawString(smallFont, welcomeMsg, new Vector2((VIRTUAL_WIDTH / 2) - (msgSize.X / 2), 10), Color.White);
                msgSize = smallFont.MeasureString(beginMsg);
                spriteBatch.DrawString(smallFont, beginMsg, new Vector2((VIRTUAL_WIDTH / 2) - (msgSize.X / 2), 20), Color.White);
            }
            else if(gameState == GameState.Serve)
            {
                string servingPlayerMsg = "Player " + servingPlayer + "'s serve!";
                string serveMsg = "Press Enter to serve!";
                Vector2 msgSize = smallFont.MeasureString(servingPlayerMsg);
                spriteBatch.DrawString(smallFont, servingPlayerMsg, new Vector2((VIRTUAL_WIDTH / 2) - (msgSize.X / 2), 10), Color.White);
                msgSize = smallFont.MeasureString(serveMsg);
                spriteBatch.DrawString(smallFont, serveMsg, new Vector2((VIRTUAL_WIDTH / 2) - (msgSize.X / 2), 20), Color.White);
            }
            else if(gameState == GameState.Done)
            {
                string winingPlayerMsg = "Player " + winningPlayer + " Wins!";
                string restartMsg = "Press Enter to Restart!";
                Vector2 msgSize = largeFont.MeasureString(winingPlayerMsg);
                spriteBatch.DrawString(largeFont, winingPlayerMsg, new Vector2((VIRTUAL_WIDTH / 2) - (msgSize.X / 2), 10), Color.White);
                msgSize = smallFont.MeasureString(restartMsg);
                spriteBatch.DrawString(smallFont, restartMsg, new Vector2((VIRTUAL_WIDTH / 2) - (msgSize.X / 2), 30), Color.White);
            }

            player1.Render(spriteBatch);
            player2.Render(spriteBatch);
            ball.Render(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }


        private void DisplayScore()
        {
            spriteBatch.DrawString(scoreFont, 
                player1Score.ToString(), 
                new Vector2(VIRTUAL_WIDTH / 2 - 50,
                VIRTUAL_HEIGHT / 3),
                Color.White);

            spriteBatch.DrawString(scoreFont,
                player2Score.ToString(),
                new Vector2(VIRTUAL_WIDTH / 2 + 30,
                VIRTUAL_HEIGHT / 3),
                Color.White);
        }
        
    }
}
