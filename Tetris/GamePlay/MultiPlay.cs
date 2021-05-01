using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Tetris
{
    public static class MultiPlay
    {
        public static void Access(List<Tetris> players)
        {
            players.ForEach(t => t.LineClearEvent += SendBlockToPlayer);
        }

        private static async void SendBlockToPlayer(object sender, TetrisEventArgs e)
        {
            ReadOnlyCollection<Tetris> players = GameController.GetPlayers();
            if (players.Count <= 1) return;

            var tetris = sender as Tetris;
            int garbageLine = GetGarbageLine(e);

            foreach (Tetris player in players)
                if (player.PlayerId != tetris?.PlayerId)
                    await Task.Run(() => player.SetGarbageLine(garbageLine));
        }

        private static int GetGarbageLine(TetrisEventArgs e)
        {
            var cnt = 0;

            if (e.LineClearCount >= 4)
                cnt += 4;
            else if (e.LineClearCount >= 3)
                cnt += 2;
            else if (e.LineClearCount >= 2)
                cnt += 1;

            if (e.Combo >= 6)
                cnt += 3;
            else if (e.Combo >= 4)
                cnt += 2;
            else if (e.Combo >= 2)
                cnt += 1;

            return cnt;
        }
    }
}