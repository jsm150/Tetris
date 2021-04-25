using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tetris
{
    public static class MultiPlay
    {
        public static void Access(List<Tetris> players)
        {
            players.ForEach(t => t.LineClearEvent += SendBlockToPlayer);
        }

        private static void SendBlockToPlayer(object sender, TetrisEventArgs e)
        {
            ReadOnlyCollection<Tetris> players = GameController.GetPlayers();
            if (players.Count <= 1) return;

            var tetris = sender as Tetris;
            int garbageLine = GetGarbageLine(e.LineClearCount);

            foreach (Tetris player in players)
                if (player.PlayerId != tetris?.PlayerId)
                    player.SetGarbageLine(garbageLine);
        }

        private static int GetGarbageLine(int count)
        {
            if (count <= 1)
                return 0;
            if (count <= 2)
                return 1;
            if (count <= 3)
                return 2;
            return 4;
        }
    }
}