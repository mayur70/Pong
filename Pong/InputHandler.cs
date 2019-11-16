using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Pong
{
    public interface IInputHandler
    {

        KeyboardState KeyboardState { get; }
        GamePadState GamePadState { get; }
    }

    public class InputHandler : GameComponent, IInputHandler
    {

        private KeyboardState keyboardState;
        private GamePadState gamepadState;
        public KeyboardState KeyboardState => keyboardState;
        public GamePadState GamePadState => gamepadState;

        public InputHandler(Game game) : base(game)
        {
            game.Services.AddService(typeof(IInputHandler), this);
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);

            base.Update(gameTime);
        }
    }
}
