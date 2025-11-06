using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using FNAScreenSaver.Classes;

namespace FNAScreenSaver
{
    /// <summary>
    /// Основной класс игры
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D snowTexture;
        private Texture2D backgroundTexture;

        private List<Snowflake> snowflakes;
        private Random random;

        private int screenWidth;
        private int screenHeight;

        const int SnowflakesCount = 1500;
        const int MinSize = 10;
        const int MaxSize = 40;
        const int MinSpeed = 2;
        const int MaxSpeed = 8;

        /// <summary>
        /// Конструктор для класса <see cref="Game"/>
        /// </summary>
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            graphics.IsFullScreen = true;

            snowflakes = new List<Snowflake>();
            random = new Random();

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.HardwareModeSwitch = false;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();

            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;

            CreateSnowflakes();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            snowTexture = Content.Load<Texture2D>("snow");
            backgroundTexture = Content.Load<Texture2D>("background");
        }

        private void CreateSnowflakes()
        {
            for (var i = 0; i < SnowflakesCount; i++)
            {
                snowflakes.Add(new Snowflake
                {
                    X = random.Next(0, screenWidth),
                    Y = random.Next(-screenHeight, 0),
                    Size = random.Next(MinSize, MaxSize + 1),
                    Speed = random.Next(MinSpeed, MaxSpeed + 1)
                });
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().GetPressedKeys().Length > 0)
            {
                Exit();
            }

            foreach (var snowflake in snowflakes)
            {
                snowflake.Y += snowflake.Speed;

                if (snowflake.Y > screenHeight)
                {
                    snowflake.Y = -snowflake.Size;
                    snowflake.X = random.Next(0, screenWidth);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(
                backgroundTexture,
                new Rectangle(0,0, screenWidth, screenHeight),
                Color.White
            );

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (var snowflake in snowflakes)
            {
                spriteBatch.Draw(
                    snowTexture,
                    new Rectangle(snowflake.X, snowflake.Y, snowflake.Size, snowflake.Size),
                    Color.White
                );
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
