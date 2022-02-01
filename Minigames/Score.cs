using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Minigames
{
    class Score
    {
        #region Sprites

        private static readonly Rectangle spriteZero = new Rectangle(16, 0, 4, 8);
        private static readonly Rectangle spriteOne = new Rectangle(20, 0, 4, 8);
        private static readonly Rectangle spriteTwo = new Rectangle(24, 0, 4, 8);
        private static readonly Rectangle spriteThree = new Rectangle(28, 0, 4, 8);
        private static readonly Rectangle spriteFour = new Rectangle(16, 8, 4, 8);
        private static readonly Rectangle spriteFive = new Rectangle(20, 8, 4, 8);
        private static readonly Rectangle spriteSix = new Rectangle(24, 8, 4, 8);
        private static readonly Rectangle spriteSeven = new Rectangle(28, 8, 4, 8);
        private static readonly Rectangle spriteEight = new Rectangle(16, 16, 4, 8);
        private static readonly Rectangle spriteNine = new Rectangle(20, 16, 4, 8);

        #endregion

        private Rectangle[] sprite = new Rectangle[4] { spriteZero, spriteZero, spriteZero, spriteZero };

        private string text;
        private Vector2[] position = new Vector2[4]
        {
            new Vector2(4,0),
            new Vector2(8,0),
            new Vector2(12,0),
            new Vector2(16,0)
        };

        private const int points = 7;

        public void Refresh()
        {
            text = ((SnakeGame.Snake.Body.Count - 3) * points).ToString();

            if ((SnakeGame.Snake.Body.Count - 3) * points < 10000)
            {
                switch (text.Length)
                {
                    case 1:
                        text = "000" + text;
                        break;
                    case 2:
                        text = "00" + text;
                        break;
                    case 3:
                        text = "0" + text;
                        break;
                }
            }
            else
                text = "9999";


            for (int i = 0; i < 4; i++)
            {
                switch (text[i])
                {
                    case '0':
                        sprite[i] = spriteZero;
                        break;
                    case '1':
                        sprite[i] = spriteOne;
                        break;
                    case '2':
                        sprite[i] = spriteTwo;
                        break;
                    case '3':
                        sprite[i] = spriteThree;
                        break;
                    case '4':
                        sprite[i] = spriteFour;
                        break;
                    case '5':
                        sprite[i] = spriteFive;
                        break;
                    case '6':
                        sprite[i] = spriteSix;
                        break;
                    case '7':
                        sprite[i] = spriteSeven;
                        break;
                    case '8':
                        sprite[i] = spriteEight;
                        break;
                    case '9':
                        sprite[i] = spriteNine;
                        break;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < 4; i++)
            {
                Engine.SpriteBatch.Draw(SnakeGame.Tileset, position[i], sprite[i], Color.White);
            }
        }
    }
}
