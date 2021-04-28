using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris
{
    public static class GeneticAlgorithm
    {
        private static readonly Random Random = new Random();
        private static readonly List<Tetris> Players = new List<Tetris>();
        private static Form1 Form1;

        private static void PlayerAdd(GeneticTetris player)
        {
            Players.Add(player);
        }

        private static async Task GameStart()
        {
            await Task.WhenAll(Players.Select(t => t.GameStart()));
        }

        public static async Task AlgorithmStart(Form1 form1 = null)
        {
            Form1 = form1 ?? Form1;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var player = new GeneticTetris(Form1, 1 + (i % 4 * 11), 15 + (i / 4 * 21), i + 1, new Weight());
                    PlayerAdd(player);
                }

                await GameStart();
            }

            MixParents();
        }

        public static void MixParents()
        {
            Players.Sort((i, j) => i.Score > j.Score ? -1 : 1);

        }


    }
}