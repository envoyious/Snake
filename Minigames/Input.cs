using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Minigames
{
    public static class Input
    {
        static Input()
        {
            PreviousKeyboardState = new KeyboardState();
            CurrentKeyboardState = Keyboard.GetState();
        }

        internal static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        #region Keyboard

        public static KeyboardState PreviousKeyboardState { get; private set; }
        public static KeyboardState CurrentKeyboardState { get; private set; }

        public static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && !PreviousKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return !CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyDown(key);
        }

        #endregion

    }
}
