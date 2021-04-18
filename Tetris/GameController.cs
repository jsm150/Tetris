using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class GameController
    {
        public readonly List<Tetris> Player = new List<Tetris>();

        private GameController()
        {
        }

        public static GameController GetInstance { get; } = new GameController();

        public Action<string> GameEndAction { get; set; }

        public void PlayerAdd(Tetris player)
        {
            Player.Add(player);
        }

        public void KeyBoardAction(KeyEventArgs e)
        {
            Player.ForEach(t => t.KeyBoardAction(e));
        }

        public async Task GameStart()
        {
            await Task.WhenAny(Player.Select(t => t.GameStart()));
            GameEnd();
        }

        private void GameEnd()
        {
            string mag = GameEndAlertMsg();
            Player.ForEach(t => t.CanGameRun = false);
            Player.Clear();
            GameEndAction.Invoke(mag);
        }

        private string GameEndAlertMsg()
        {
            if (Player.Count <= 1)
                return "Game Over!";
            int id = Player.First(t => t.CanGameRun).PlayerId;
            return $"플레이어{id} 승리!";
        }
    }
}