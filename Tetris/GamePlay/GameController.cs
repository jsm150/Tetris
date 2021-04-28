using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public static class GameController
    {
        private static readonly List<Tetris> Players = new List<Tetris>();

        public static Action<string> GameEndAction { get; set; }

        public static ReadOnlyCollection<Tetris> GetPlayers()
        {
            return Players.AsReadOnly();
        }

        public static void PlayerAdd(Tetris player)
        {
            Players.Add(player);
        }

        public static void KeyBoardAction(KeyEventArgs e)
        {
            Players.ForEach(t => t.KeyBoardAction(e));
        }

        public static async Task GameStart()
        {
            MultiPlay.Access(Players);
            await Task.WhenAny(Players.Select(t => t.GameStart()));
            GameEnd();
        }

        private static void GameEnd()
        {
            string mag = GameEndAlertMsg();
            Players.ForEach(t => t.GamePlaying = false);
            Players.Clear();
            GameEndAction.Invoke(mag);
        }

        private static string GameEndAlertMsg()
        {
            if (Players.Count <= 1)
                return "Game Over!";
            int id = Players.FirstOrDefault(t => t.GamePlaying)?.PlayerId ?? 0;

            return id != 0 ? $"플레이어{id} 승리!" : "비겼습니다!";
        }
    }
}