using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Minigames
{
    public class Engine : Game
    {

        // References
        public static Engine Instance { get; private set; }
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }

        // Screen size
        public static int ViewWidth { get; private set; }
        public static int ViewHeight { get; private set; }
        private RenderTarget2D pixelPerfect;

        // Time
        public static float DeltaTime { get; private set; }

        //  Utils
        public static Color ClearColor { get; set; }

        public Engine(int viewWidth = 1280, int viewHeight = 720, int windowWidth = 1280, int windowHeight = 720, string windowTitle = "Minigames", bool fullscreen = false)
        {
            Instance = this;

            Window.Title = windowTitle;
            ViewWidth = viewWidth;
            ViewHeight = viewHeight;
            ClearColor = Color.Black;

            Graphics = new GraphicsDeviceManager(this);
            Graphics.SynchronizeWithVerticalRetrace = true;

            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            if (fullscreen)
            {
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

            Content.RootDirectory = @"Content";
        }

        protected override void Initialize()
        {
            pixelPerfect = new RenderTarget2D(GraphicsDevice, ViewWidth, ViewHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            SpriteBatch.Dispose();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Input.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(pixelPerfect);
            GraphicsDevice.Clear(ClearColor);

            Render();

            float scale = Math.Min(Window.ClientBounds.Width / (float)ViewWidth, Window.ClientBounds.Height / (float)ViewHeight);
            Vector2 screenCenter = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            Vector2 targetCenter = new Vector2(pixelPerfect.Width / 2, pixelPerfect.Height / 2);

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(ClearColor);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            SpriteBatch.Draw(
                pixelPerfect,
                new Rectangle((int)screenCenter.X, (int)screenCenter.Y, (int)(ViewWidth * scale), (int)(ViewHeight * scale)),
                null,
                Color.White,
                0f,
                targetCenter,
                SpriteEffects.None,
                0f);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public virtual void Render() { }

    }
}