using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Tetris
{
    public static class GeneticAlgorithm
    {
        private static readonly Random Random = new Random();
        private static readonly List<TetrisAI> Players = new List<TetrisAI>();
        private static Weight[] _weightArr = new Weight[24];
        private static Form1 Form1;
        private static Label lbl_BestScore;
        private static Label lbl_Generation;
        private static int _generation;
        private static int _bestScore;

        public static void Initialization(Form1 form1, Label bs, Label ge)
        {
            Form1 = form1;
            lbl_BestScore = bs;
            lbl_Generation = ge;
            if (File.Exists(@".\WeightList.json"))
                _weightArr = WeightFileReader<Weight[]>(@".\WeightList.json");
            else
            {
                for (var i = 1; i < _weightArr.Length; i++)
                {
                    _weightArr[i] = new Weight()
                    {
                        LineClearValue = Random.Next(0, 100) + (float) Random.NextDouble(),
                        BlockHeightValue = Random.Next(-100, 0) + (float) Random.NextDouble(),
                        HoleValue = Random.Next(-100, 0) + (float) Random.NextDouble(),
                        BlockedValue = Random.Next(-100, 0) + (float) Random.NextDouble(),
                        BlockValue = Random.Next(0, 100) + (float) Random.NextDouble(),
                        FloorValue = Random.Next(0, 100) + (float) Random.NextDouble(),
                        SideValue = Random.Next(0, 100) + (float) Random.NextDouble(),
                    };
                }

                _weightArr[0] = WeightFileReader<Weight>(@".\Weight.json");
            }
        }

        private static async Task GameStart()
        {
            await Task.Delay(1);
            await Task.WhenAll(Players.Select(t => t.GameStart()));
        }

        public static async Task AlgorithmStart()
        {
            _generation++;
            Players.Clear();
            for (int j = 0; j < 3; j++)
            {
                lbl_Generation.Text = $@"{_generation} - {j + 1} 세대";
                for (int i = 0; i < 8; i++)
                {
                    var player = new GeneticTetris(Form1, 1 + (i % 4 * 11),
                        15 + (i / 4 * 21), j * 8 + i + 1, _weightArr[j * 8 + i]);
                    Players.Add(player);
                }

                await GameStart();
                _bestScore = Math.Max(_bestScore, Players.Skip(j * 8).Max(t => t.Score));
                lbl_BestScore.Text = _bestScore.ToString();
            }
            
            await MixParents();
        }

        private static async Task MixParents()
        {
            Players.Sort((i, j) => i.Score > j.Score ? -1 : 1);
            WeightFileWriter(Players[0].Weight, @".\Weight.json");
            WeightFileWriter(_weightArr, @".\WeightList.json");


            await AlgorithmStart();
        }

        private static void WeightFileWriter<T>(T weight, string path)
        {
            using (var stream = new StreamWriter(path))
            using (var writer = new JsonTextWriter(stream))
                new JsonSerializer().Serialize(writer, weight);
        }

        private static T WeightFileReader<T>(string path)
        {
            using (var stream = new StreamReader(path))
            using (var reader = new JsonTextReader(stream))
            {
                return new JsonSerializer().Deserialize<T>(reader);
            }
        }

    }
}