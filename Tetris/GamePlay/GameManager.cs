using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class GameManager
    {
        private readonly List<Tetris> Players = new List<Tetris>();
        public static GameManager GetInstance { get; } = new GameManager();

        public Action<string> GameEndAction { get; set; }

        private GameManager()
        {
        }

        public IReadOnlyList<Tetris> GetPlayers()
        {
            return Players;
        }

        public void PlayerAdd(Tetris player)
        {
            Players.Add(player);
        }

        public void KeyBoardAction(KeyEventArgs e)
        {
            Players.ForEach(t => t.KeyBoardAction(e));
        }

        public async Task GameStart()
        {
            Players.ForEach(t => t.LineClearEvent += SendBlockToPlayer);
            await Task.WhenAny(Players.Select(t => Task.Run(t.GameStart)));
            GameEnd();
        }

        public void GameEnd()
        {
            if (Players.Count == 0) return;

            string mag = GameEndAlertMsg();
            Players.ForEach(t => t.GamePlaying = false);
            Players.Clear();
            GameEndAction.Invoke(mag);
        }

        private string GameEndAlertMsg()
        {
            if (Players.Count <= 1)
                return "Game Over!";
            int id = Players.FirstOrDefault(t => t.GamePlaying)?.PlayerId ?? 0;

            return id != 0 ? $"플레이어{id} 승리!" : "비겼습니다!";
        }

        private async void SendBlockToPlayer(object sender, TetrisEventArgs e)
        {
            if (Players.Count <= 1) return;

            Tetris tetris = sender as Tetris;
            int garbageLine = GetGarbageLine(e);

            foreach (Tetris player in Players)
                if (player.PlayerId != tetris?.PlayerId)
                    await Task.Run(() => player.SetGarbageLine(garbageLine));
        }

        private static int GetGarbageLine(TetrisEventArgs e)
        {
            int cnt = 0;

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