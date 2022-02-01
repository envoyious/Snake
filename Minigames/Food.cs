using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Minigames
{
    class Food
    {
        // Utils
        public Vector2 Position { get; private set; } = new Vector2(60, 24);

        private const int columns = 18;
        private const int rows = 7;
        private readonly Random random = new Random();

        // References
        private readonly Rectangle spriteFood = new Rectangle(4, 0, 4, 4);

        public void NewFoodLocation()
        {
            int x = (random.Next(1, columns + 1) * 4);
            int y = (random.Next(3, rows + 3) * 4);

            for (int i = 0; i < SnakeGame.Snake.Body.Count; i++)
            {
                if (SnakeGame.Snake.Body.Count >= columns * rows)
                    break;

                Vector2 part = SnakeGame.Snake.Body[i].Position;
                if (part.X == x && part.Y == y)
                {
                    x = (random.Next(1, columns + 1) * 4);
                    y = (random.Next(3, rows + 3) * 4);
                    i = 0;
                }
            }

            Position = new Vector2(x, y);
        }

        public void ResetFoodLocation()
        {
            Position = new Vector2(60, 24);
        }

        public void Draw()
        {
            Engine.SpriteBatch.Draw(SnakeGame.Tileset, Position, spriteFood, Color.White);
        }
    }
}
