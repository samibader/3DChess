using System;

namespace YATest
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (GameEngine.Game1 game = new GameEngine.Game1())
            {
                game.Run();
            }
        }
    }
}

