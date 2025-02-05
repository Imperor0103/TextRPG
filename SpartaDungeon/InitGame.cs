using SpartaDungeon.Managers;

namespace SpartaDungeon
{
    internal class InitGame
    {
        static void Main(string[] args)
        {
            GameProcess gameProcess = new GameProcess();
            gameProcess.StartGame();
            gameProcess.Loop();
        }
    }
}
