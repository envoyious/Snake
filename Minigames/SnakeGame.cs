using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minigames
{
    class SnakeGame : Engine
    {

        // References
        public static Snake Snake { get; } = new Snake();
        public static Food Food { get; } = new Food();
        private readonly static Score score = new Score();

        // Utils
        private static bool fullscreen = false;

        private static int windowWidth = 640;
        private static int windowHeight = 360;

        // Textures
        public static Texture2D SpriteBackground { get; private set; }
        public static Texture2D Tileset { get; private set; }
        public static Texture2D DebugOverlay { get; private set; }

        public SnakeGame() : base(80, 45, windowWidth, windowHeight, "Snake", fullscreen)
        {
            ClearColor = new Color(70, 135, 143);
        }

        protected override void Initialize()
        {
            Console.WriteLine($"Height: {Window.ClientBounds.Width}\nWidth: {Window.ClientBounds.Height}");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBackground = Content.Load<Texture2D>("SnakeBackground");
            Tileset = Content.Load<Texture2D>("SnakeTileset");
            DebugOverlay = Content.Load<Texture2D>("DebugOverlay");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            SpriteBackground.Dispose();
            Tileset.Dispose();
            DebugOverlay.Dispose();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Input.IsKeyPressed(Keys.F11))
            {
                fullscreen = !fullscreen;
                if (fullscreen)
                {
                    windowWidth = Window.ClientBounds.Width;
                    windowHeight = Window.ClientBounds.Height;
                    Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    Graphics.IsFullScreen = true;
                }
                else
                {
                    Graphics.PreferredBackBufferWidth = windowWidth;
                    Graphics.PreferredBackBufferHeight = windowHeight;
                    Graphics.IsFullScreen = false;
                }
                Graphics.ApplyChanges();
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.Space))
            //    Food.NewFoodLocation();

            if (Snake.EatenFood(Food.Position))
                Food.NewFoodLocation();

            Snake.Update();
            score.Refresh();
        }

        public override void Render()
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(SpriteBackground, Vector2.Zero, Color.White);

            Snake.Draw();
            Food.Draw();
            score.Draw();

            //SpriteBatch.Draw(DebugOverlay, Vector2.Zero, Color.White);

            SpriteBatch.End();

            base.Render();
        }
    }
}