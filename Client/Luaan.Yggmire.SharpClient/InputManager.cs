using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient
{
    public class InputManager
    {
        YggmireGame game;

        private KeyboardManager keyboard;

        private KeyboardState lastKeyboardState;
        private KeyboardState keyboardState;

        private MouseManager mouse;

        private MouseState lastMouseState;
        private MouseState mouseState;

        public InputManager(YggmireGame game)
        {
            this.game = game;

            // Initialize input keyboard system
            keyboard = new KeyboardManager(game);

            // Initialize input mouse system
            mouse = new MouseManager(game);
        }

        public KeyboardState KeyboardState { get { return this.keyboardState; } }
        public KeyboardState LastKeyboardState { get { return this.lastKeyboardState; } }

        public MouseState MouseState { get { return this.mouseState; } }
        public MouseState LastMouseState { get { return this.lastMouseState; } }

        public void Initialize()
        {
            keyboard.Initialize();
            mouse.Initialize();
        }

        public void Update()
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;

            // Get the current state of the keyboard
            keyboardState = keyboard.GetState();

            // Get the current state of the mouse
            mouseState = mouse.GetState();
        }
    }
}
