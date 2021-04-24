using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class GameController
    {
        private readonly List<Tetris> Player = new List<Tetris>();

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
            foreach (Tetris t in Player)
            {
                if (!t.AiPlaying)
                    t.KeyBoardAction(e);
            }
        }

        public async Task GameStart()
        {
            await Task.WhenAny(Player.Select(t => t.GameStart()));
            GameEnd();
        }

        private void GameEnd()
        {
            string mag = GameEndAlertMsg();
            Player.ForEach(t => t.GamePlaying = false);
            Player.Clear();
            GameEndAction.Invoke(mag);
        }

        private string GameEndAlertMsg()
        {
            if (Player.Count <= 1)
                return "Game Over!";
            int id = Player.FirstOrDefault(t => t.GamePlaying)?.PlayerId ?? 0;
            
            return id != 0? $"플레이어{id} 승리!" : "비겼습니다!";
        }
    }
}