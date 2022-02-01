using System;

namespace Minigames
{
    public static class Program
    {
        static void Main(string[] args)
        {
            using (SnakeGame game = new SnakeGame())
            {
                game.Run();
            }
        }
    }
}