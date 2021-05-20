using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Newtonsoft.Json;

namespace Tetris
{
    public static class GeneticAlgorithm
    {
        private static readonly Random Random = new Random();
        private static readonly List<TetrisAI> Players = new List<TetrisAI>();
        private static Weight[] _weights = new Weight[24];
        private static TetrisPanel[] _panels = new TetrisPanel[24];
        private static int _generation;
        private static long _bestScore;

        private static void Initialization(Control form)
        {
            SetWeights();
            SetPanels(form);
        }

        private static void SetWeights()
        {
            if (File.Exists(@".\WeightList.json"))
            {
                _weights = WeightFileReader<Weight[]>(@".\WeightList.json");
            }
            else
            {
                for (var i = 0; i < _weights.Length; i++)
                    _weights[i] = GetRandomWeight();
                if (File.Exists(@".\Weight.json"))
                    _weights[0] = WeightFileReader<Weight>(@".\Weight.json");
            }
        }

        private static void SetPanels(Control form)
        {
            for (var i = 0; i < _weights.Length; i++)
            {
                _panels[i] = new TetrisPanel(10)
                {
                    Location = new Point((1 + i % 6 * 11) * 10, (16 + i / 6 * 21) * 10),
                    Size = new Size(105, 205),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.Black
                };
            }
            
            form.Controls.AddRange(_panels);
        }

        public static async Task AlgorithmStart(MetroForm form, Label lbl_Score, Label lbl_BestScore, Label lbl_Generation,
            Label lblBestNum)
        {
            Initialization(form);
            while (true)
            {
                _generation++;
                Players.Clear();

                lbl_Generation.Text = $@"{_generation} 세대";
                for (var i = 0; i < _weights.Length; i++)
                {
                    TetrisAI player = TetrisAI.GeneticMode(_panels[i], lbl_Score, lblBestNum, i + 1,
                        _weights[i].Clone());
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
                    _weights[cnt] = GetRandomWeight();
                    _weights[cnt + 1] = GetRandomWeight();
                    cnt += 2;
                    continue;
                }

                for (var k = 0; k < 7; k++)
                    if (Random.NextDouble() < 0.2)
                    {
                        _weights[cnt][k] = GetRandom(k);
                        _weights[cnt + 1][k] = GetRandom(k);
                    }
                    else if (Random.NextDouble() > 0.5)
                    {
                        _weights[cnt][k] = Players[i].Weight[k];
                        _weights[cnt + 1][k] = Players[j].Weight[k];
                    }
                    else
                    {
                        _weights[cnt][k] = Players[j].Weight[k];
                        _weights[cnt + 1][k] = Players[i].Weight[k];
                    }

                cnt += 2;
            }

            for (var i = 0; i < 6; i++)
            for (var j = 0; j < 7; j++)
                _weights[cnt + i][j] = Players[i].Weight[j];

            WeightFileWriter(_weights, @".\WeightList.json");

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