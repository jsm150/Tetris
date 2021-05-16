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
        private static int _generation;
        private static long _bestScore;

        public static void Initialization()
        {
            if (File.Exists(@".\WeightList.json"))
            {
                _weightArr = WeightFileReader<Weight[]>(@".\WeightList.json");
            }
            else
            {
                for (var i = 0; i < _weightArr.Length; i++)
                    _weightArr[i] = GetRandomWeight();
                if (File.Exists(@".\Weight.json"))
                    _weightArr[0] = WeightFileReader<Weight>(@".\Weight.json");
            }
        }

        public static async Task AlgorithmStart(Form1 form1, Label lbl_Score, Label lbl_BestScore, Label lbl_Generation,
            Label lblBestNum)
        {
            while (true)
            {
                _generation++;
                Players.Clear();

                lbl_Generation.Text = $@"{_generation} 세대";
                for (var i = 0; i < _weightArr.Length; i++)
                {
                    int offsetX = 1 + i % 6 * 11;
                    int offsetY = 16 + i / 6 * 21;
                    int id = i + 1;

                    TetrisAI player = TetrisAI.GeneticMode(form1, offsetX, offsetY, lbl_Score, lblBestNum, id,
                        _weightArr[i].Clone());
                    Players.Add(player);
                }

                await GameStart();
                _bestScore = Math.Max(_bestScore, Players.Max(t => t.Score));
                lbl_BestScore.Text = _bestScore.ToString();
                lbl_Score.Text = "0";

                MixParents();
            }
        }

        private static async Task GameStart()
        {
            await Task.WhenAll(Players.Select(t => Task.Run(t.GameStart)));
        }

        private static void MixParents()
        {
            Players.Sort((i, j) => i.Score > j.Score ? -1 : 1);
            WeightFileWriter(Players[0].Weight, @".\Weight.json");

            // 상위 개체 6개를 뽑아서 교배
            var cnt = 0;
            for (var i = 0; i < 3; i++)
            for (var j = 3; j < 6; j++)
            {
                if (Random.Next(0, 10) == 1)
                {
                    _weightArr[cnt] = GetRandomWeight();
                    _weightArr[cnt + 1] = GetRandomWeight();
                    cnt += 2;
                    continue;
                }

                for (var k = 0; k < 7; k++)
                    if (Random.NextDouble() < 0.2)
                    {
                        _weightArr[cnt][k] = GetRandom(k);
                        _weightArr[cnt + 1][k] = GetRandom(k);
                    }
                    else if (Random.NextDouble() > 0.5)
                    {
                        _weightArr[cnt][k] = Players[i].Weight[k];
                        _weightArr[cnt + 1][k] = Players[j].Weight[k];
                    }
                    else
                    {
                        _weightArr[cnt][k] = Players[j].Weight[k];
                        _weightArr[cnt + 1][k] = Players[i].Weight[k];
                    }

                cnt += 2;
            }

            for (var i = 0; i < 6; i++)
            for (var j = 0; j < 7; j++)
                _weightArr[cnt + i][j] = Players[i].Weight[j];

            WeightFileWriter(_weightArr, @".\WeightList.json");

            float GetRandom(int k)
            {
                float value = k <= 2 ? Random.Next(-100, 0) : Random.Next(0, 100);
                value += (float) Random.NextDouble();
                return value;
            }
        }

        private static Weight GetRandomWeight()
        {
            var weight = new Weight();
            for (var i = 0; i < 7; i++)
            {
                weight[i] = i <= 2 ? Random.Next(-100, 0) : Random.Next(0, 100);
                weight[i] += (float) Random.NextDouble();
            }

            return weight;
        }

        private static void WeightFileWriter<T>(T weight, string path)
        {
            using (var stream = new StreamWriter(path))
            using (var writer = new JsonTextWriter(stream))
            {
                new JsonSerializer().Serialize(writer, weight);
            }
        }

        public static T WeightFileReader<T>(string path)
        {
            using (var stream = new StreamReader(path))
            using (var reader = new JsonTextReader(stream))
            {
                return new JsonSerializer().Deserialize<T>(reader);
            }
        }

        private static T Clone<T>(this T obj)
        {
            string temp = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(temp);
        }
    }
}