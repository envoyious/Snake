using System;

namespace Minigames
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (SnakeGame game = new SnakeGame())
            {
                game.Run();
            }
        }
    }
}