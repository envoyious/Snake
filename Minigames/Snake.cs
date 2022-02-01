using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minigames
{
    class Snake
    {

        #region Sprites

        // Head
        private static readonly Rectangle spriteRightHead = new Rectangle(0, 8, 4, 4);
        private static readonly Rectangle spriteLeftHead = new Rectangle(4, 8, 4, 4);
        private static readonly Rectangle spriteUpHead = new Rectangle(8, 8, 4, 4);
        private static readonly Rectangle spriteDownHead = new Rectangle(12, 8, 4, 4);
        private static readonly Rectangle spriteRightOpenHead = new Rectangle(0, 12, 4, 4);
        private static readonly Rectangle spriteLeftOpenHead = new Rectangle(4, 12, 4, 4);
        private static readonly Rectangle spriteUpOpenHead = new Rectangle(8, 12, 4, 4);
        private static readonly Rectangle spriteDownOpenHead = new Rectangle(12, 12, 4, 4);

        // Body
        private static readonly Rectangle spriteVerticalBody = new Rectangle(0, 0, 4, 4);
        private static readonly Rectangle spriteHorizontalBody = new Rectangle(0, 4, 4, 4);
        private static readonly Rectangle spriteEatenBody = new Rectangle(4, 4, 4, 4);

        // Tail
        private static readonly Rectangle spriteRightTail = new Rectangle(8, 0, 4, 4);
        private static readonly Rectangle spriteLeftTail = new Rectangle(12, 0, 4, 4);
        private static readonly Rectangle spriteUpTail = new Rectangle(12, 4, 4, 4);
        private static readonly Rectangle spriteDownTail = new Rectangle(8, 4, 4, 4);

        // Corners
        private static readonly Rectangle spriteRightUpCorner = new Rectangle(8, 20, 4, 4);
        private static readonly Rectangle spriteLeftUpCorner = new Rectangle(12, 20, 4, 4);
        private static readonly Rectangle spriteRightDownCorner = new Rectangle(0, 20, 4, 4);
        private static readonly Rectangle spriteLeftDownCorner = new Rectangle(4, 20, 4, 4);
        private static readonly Rectangle spriteRightUpEatenCorner = new Rectangle(8, 16, 4, 4);
        private static readonly Rectangle spriteLeftUpEatenCorner = new Rectangle(12, 16, 4, 4);
        private static readonly Rectangle spriteRightDownEatenCorner = new Rectangle(0, 16, 4, 4);
        private static readonly Rectangle spriteLeftDownEatenCorner = new Rectangle(4, 16, 4, 4);

        // Debug
        private static readonly Rectangle spriteBody = new Rectangle(20, 24, 4, 4);
        private static readonly Rectangle spriteHead = new Rectangle(16, 28, 4, 4);
        private static readonly Rectangle spriteTail = new Rectangle(16, 24, 4, 4);

        #endregion

        public struct SnakeBody
        {
            public Vector2 Position;
            public Rectangle Sprite;
            public bool Eaten;

            public SnakeBody(float x, float y, Rectangle sprite, bool eaten)
            {
                Position = new Vector2(x, y);
                Sprite = sprite;
                Eaten = eaten;
            }

            public SnakeBody(Vector2 position, Rectangle sprite, bool eaten)
            {
                Position = position;
                Sprite = sprite;
                Eaten = eaten;
            }

        }

        public List<SnakeBody> Body { get; private set; } = new List<SnakeBody>()
        {
            new SnakeBody(28, 24, spriteRightTail, false),
            new SnakeBody(32, 24, spriteHorizontalBody, false),
            new SnakeBody(36, 24, spriteRightHead, false)
        };
        private List<SnakeBody> deadBody;

        // Utils
        private int xDirection = 0;
        private int yDirection = 0;
        private bool alive = true;
        private Color blinkColor = Color.White;

        private const int speed = 4;
        private int total = 3;

        // Time
        private const float delay = 0.18f;
        private float remainingDelay = delay;

        private const float gameOverDelay = 1.2f;
        private float remainingGameOverDelay = gameOverDelay;

        private const float blinkAmount = 0.18f;
        private float remainingBlinkAmount = blinkAmount;

        public void Update()
        {
            float timer = (float)Engine.DeltaTime;
            remainingDelay -= timer;

            SetDirection();

            if (alive)
            {
                if (remainingDelay <= 0)
                {
                    Vector2 head = Body[Body.Count - 1].Position;

                    if (!(xDirection + yDirection == 0))
                    {
                        deadBody = Body.ToList();

                        if (total == Body.Count)
                            Body.RemoveAt(0);
                        else
                            Body[Body.Count - 1] = new SnakeBody(Body[Body.Count - 1].Position, Body[Body.Count - 1].Sprite, true);

                        head += new Vector2(xDirection, yDirection);
                        Body.Add(new SnakeBody(head, spriteRightHead, false));

                        PushInbounds();
                        UpdateAnimations();
                    }
                    remainingDelay = delay;
                }
            }
            else
                IsDead();

            GameOver();
        }

        public void Draw()
        {
            for (int i = 0; i < Body.Count; i++)
            {
                Engine.SpriteBatch.Draw(SnakeGame.Tileset, Body[i].Position, Body[i].Sprite, blinkColor);
            }
        }

        public void GameOver()
        {
            Vector2 head = Body[Body.Count - 1].Position;

            for (int i = 0; i < Body.Count - 1; i++)
            {
                if (head == Body[i].Position)
                {
                    Body = deadBody.ToList();
                    alive = false;
                }
            }
        }

        public bool EatenFood(Vector2 position)
        {
            Vector2 head = Body[Body.Count - 1].Position;
            if (head == position)
            {
                total++;
                return true;
            }
            return false;
        }

        private void PushInbounds()
        {
            // Keep the snake within the boundaries
            for (int i = 0; i < Body.Count; i++)
                Body[i] = new SnakeBody(OutOfBoundsCheck(Body[i].Position), Body[i].Sprite, Body[i].Eaten);
        }

        private void IsDead()
        {
            float timer = (float)Engine.DeltaTime;

            remainingGameOverDelay -= timer;
            remainingBlinkAmount -= timer;

            if (remainingBlinkAmount <= 0)
            {
                if (blinkColor == Color.White)
                {
                    blinkColor = Color.Transparent;
                }
                else
                {
                    blinkColor = Color.White;
                }

                remainingBlinkAmount = blinkAmount;
            }

            if (remainingGameOverDelay <= 0)
            {
                remainingGameOverDelay = gameOverDelay;
                blinkColor = Color.White;

                total = 3;
                alive = true;
                xDirection = 0;
                yDirection = 0;

                Body = new List<SnakeBody>()
                    {
                        new SnakeBody(28, 24, spriteRightTail, false),
                        new SnakeBody(32, 24, spriteHorizontalBody, false),
                        new SnakeBody(36, 24, spriteRightHead, false)
                    };

                SnakeGame.Food.ResetFoodLocation();
            }
        }

        private void UpdateAnimations()
        {
            #region Tail

            int tail = 0;

            if (OutOfBoundsCheck(Body[tail].Position + new Vector2(speed, 0)) == Body[1].Position)
                Body[tail] = new SnakeBody(Body[tail].Position, spriteRightTail, Body[tail].Eaten);
            else if (OutOfBoundsCheck(Body[tail].Position + new Vector2(-speed, 0)) == Body[1].Position)
                Body[tail] = new SnakeBody(Body[tail].Position, spriteLeftTail, Body[tail].Eaten);
            else if (OutOfBoundsCheck(Body[tail].Position + new Vector2(0, speed)) == Body[1].Position)
                Body[tail] = new SnakeBody(Body[tail].Position, spriteDownTail, Body[tail].Eaten);
            else if (OutOfBoundsCheck(Body[tail].Position + new Vector2(0, -speed)) == Body[1].Position)
                Body[tail] = new SnakeBody(Body[tail].Position, spriteUpTail, Body[tail].Eaten);

            #endregion

            #region Body

            for (int i = 1; i < Body.Count - 1; i++)
            {
                // Horizontal
                if (OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i + 1].Position
                    && OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i - 1].Position
                    ||
                    OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i + 1].Position
                    && OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i - 1].Position)
                {
                    Body[i] = new SnakeBody(Body[i].Position, spriteHorizontalBody, Body[i].Eaten);
                }

                // Vertical
                if (OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i + 1].Position
                    && OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i - 1].Position
                    ||
                    OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i + 1].Position
                    && OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i - 1].Position)
                {
                    Body[i] = new SnakeBody(Body[i].Position, spriteVerticalBody, Body[i].Eaten);
                }

                // Eaten
                if (Body[i].Eaten)
                {
                    Body[i] = new SnakeBody(Body[i].Position, spriteEatenBody, Body[i].Eaten);
                }
            }

            #endregion

            #region Corners

            for (int i = 1; i < Body.Count - 1; i++)
            {
                if (!Body[i].Eaten)
                {
                    // Right-Up
                    if (OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i - 1].Position
                        ||
                        OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i - 1].Position)
                    {
                        Body[i] = new SnakeBody(Body[i].Position, spriteRightUpCorner, Body[i].Eaten);
                    }

                    // Left-Up
                    if (OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i - 1].Position
                        ||
                        OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i - 1].Position)
                    {
                        Body[i] = new SnakeBody(Body[i].Position, spriteLeftUpCorner, Body[i].Eaten);
                    }

                    // Right-Down
                    if (OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i - 1].Position
                        ||
                        OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i - 1].Position)
                    {
                        Body[i] = new SnakeBody(Body[i].Position, spriteRightDownCorner, Body[i].Eaten);
                    }

                    // Left-Down
                    if (OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i - 1].Position
                        ||
                        OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i - 1].Position)
                    {
                        Body[i] = new SnakeBody(Body[i].Position, spriteLeftDownCorner, Body[i].Eaten);
                    }
                }
                else
                {
                    // Right-Up Eaten
                    if (OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i - 1].Position
                        ||
                        OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i - 1].Position)
                    {
                        Body[i] = new SnakeBody(Body[i].Position, spriteRightUpEatenCorner, Body[i].Eaten);
                    }

                    // Left-Up Eaten
                    if (OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i - 1].Position
                        ||
                        OutOfBoundsCheck(Body[i].Position + new Vector2(0, -speed)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i - 1].Position)
                    {
                        Body[i] = new SnakeBody(Body[i].Position, spriteLeftUpEatenCorner, Body[i].Eaten);
                    }

                    // Right-Down Eaten
                    if (OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i - 1].Position
                        ||
                        OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(speed, 0)) == Body[i - 1].Position)
                    {
                        Body[i] = new SnakeBody(Body[i].Position, spriteRightDownEatenCorner, Body[i].Eaten);
                    }

                    // Left-Down Eaten
                    if (OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i - 1].Position
                        ||
                        OutOfBoundsCheck(Body[i].Position + new Vector2(0, speed)) == Body[i + 1].Position
                        && OutOfBoundsCheck(Body[i].Position + new Vector2(-speed, 0)) == Body[i - 1].Position)
                    {
                        Body[i] = new SnakeBody(Body[i].Position, spriteLeftDownEatenCorner, Body[i].Eaten);
                    }
                }
            }

            #endregion

            #region Head

            int head = Body.Count - 1;

            // Closed Mouth
            if (xDirection == speed)
                Body[head] = new SnakeBody(Body[head].Position, spriteRightHead, Body[head].Eaten);
            else if (xDirection == -speed)
                Body[head] = new SnakeBody(Body[head].Position, spriteLeftHead, Body[head].Eaten);
            else if (yDirection == speed)
                Body[head] = new SnakeBody(Body[head].Position, spriteDownHead, Body[head].Eaten);
            else if (yDirection == -speed)
                Body[head] = new SnakeBody(Body[head].Position, spriteUpHead, Body[head].Eaten);

            // Open Mouth
            if (OutOfBoundsCheck(Body[head].Position + new Vector2(speed, 0)) == SnakeGame.Food.Position && xDirection == speed)
                Body[head] = new SnakeBody(Body[head].Position, spriteRightOpenHead, Body[head].Eaten);
            else if (OutOfBoundsCheck(Body[head].Position + new Vector2(-speed, 0)) == SnakeGame.Food.Position && xDirection == -speed)
                Body[head] = new SnakeBody(Body[head].Position, spriteLeftOpenHead, Body[head].Eaten);
            else if (OutOfBoundsCheck(Body[head].Position + new Vector2(0, speed)) == SnakeGame.Food.Position && yDirection == speed)
                Body[head] = new SnakeBody(Body[head].Position, spriteDownOpenHead, Body[head].Eaten);
            else if (OutOfBoundsCheck(Body[head].Position + new Vector2(0, -speed)) == SnakeGame.Food.Position && yDirection == -speed)
                Body[head] = new SnakeBody(Body[head].Position, spriteUpOpenHead, Body[head].Eaten);

            #endregion
        }

        private Vector2 OutOfBoundsCheck(Vector2 position)
        {
            if (position.X < 4)
                position = new Vector2(72, position.Y);
            else if (position.X > 72)
                position = new Vector2(4, position.Y);

            if (position.Y < 12)
                position = new Vector2(position.X, 36);
            else if (position.Y > 36)
                position = new Vector2(position.X, 12);

            return position;
        }

        private void SetDirection()
        {
            Vector2 head = Body[Body.Count - 1].Position;
            Vector2 secondPart = Body[Body.Count - 2].Position;

            if (Input.IsKeyDown(Keys.W) || Input.IsKeyDown(Keys.Up))
            {
                if (!(OutOfBoundsCheck(head - new Vector2(0, speed)).Y == secondPart.Y))
                {
                    xDirection = 0;
                    yDirection = -speed;
                }
            }
            else if (Input.IsKeyDown(Keys.S) || Input.IsKeyDown(Keys.Down))
            {
                if (!(OutOfBoundsCheck(head + new Vector2(0, speed)).Y == secondPart.Y))
                {
                    xDirection = 0;
                    yDirection = speed;
                }
            }

            if (Input.IsKeyDown(Keys.D) || Input.IsKeyDown(Keys.Right))
            {
                if (!(OutOfBoundsCheck(head + new Vector2(speed, 0)).X == secondPart.X))
                {
                    xDirection = speed;
                    yDirection = 0;
                }
            }
            else if (Input.IsKeyDown(Keys.A) || Input.IsKeyDown(Keys.Left))
            {
                if (!(OutOfBoundsCheck(head - new Vector2(speed, 0)).X == secondPart.X))
                {
                    xDirection = -speed;
                    yDirection = 0;
                }
            }
        }
    }
}
