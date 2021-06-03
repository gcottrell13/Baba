using System;

namespace Baba
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new BabaGame.BabaGame();
            game.Run();
        }
    }
}
