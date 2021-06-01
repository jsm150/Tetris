using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using Newtonsoft.Json;

namespace Tetris
{
    public static class GeneticAlgorithm
    {
        private static readonly Random Random = new Random();
        private static readonly List<TetrisAI> Players = new List<TetrisAI>();
        private static Weight[] _weights = new Weight[24];
        private static readonly TetrisPanel[] _panels = new TetrisPanel[24];
        private static int _generation;
        private static long _bestScore;

        private static void Initialization(Control form)
        {
            SetWeights();
            SetPanels(form);
        }

        private static void SetWeights()
        {
            if (File.Exists(FilePath.WeightList))
            {
                _weights = MainForm.FileLoad<Weight[]>(FilePath.WeightList);
            }
            else
            {
                for (int i = 0; i < _weights.Length; i++)
                    _weights[i] = GetRandomWeight();
                if (File.Exists(FilePath.Weight))
                    _weights[0] = MainForm.FileLoad<Weight>(FilePath.Weight);
            }
        }

        private static void SetPanels(Control form)
        {
            for (int i = 0; i < _weights.Length; i++)
                _panels[i] = new TetrisPanel(10)
                {
                    Location = new Point((1 + i % 6 * 11) * 10, (16 + i / 6 * 21) * 10),
                    Size = new Size(105, 205),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.Black
                };

            form.Controls.AddRange(_panels);
        }

        public static void SetRendering()
        {
            Players.ForEach(t => t.SetRenderingAtGenetic());
        }

        public static async Task AlgorithmStart(MetroForm form, Label lbl_Score, Label lbl_BestScore,
            Label lbl_Generation,
            Label lblBestNum)
        {
            Initialization(form);
            while (true)
            {
                _generation++;
                Players.Clear();

                lbl_Generation.Text = $@"{_generation} 세대";
                for (int i = 0; i < _weights.Length; i++)
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
            MainForm.FileSave(Players[0].Weight, FilePath.Weight);

            // 상위 개체 6개를 뽑아서 교배
            int cnt = 0;
            for (int i = 0; i < 3; i++)
            for (int j = 3; j < 6; j++)
            {
                if (Random.Next(0, 10) == 1)
                {
                    _weights[cnt] = GetRandomWeight();
                    _weights[cnt + 1] = GetRandomWeight();
                    cnt += 2;
                    continue;
                }

                foreach (Weight.Property property in Enum.GetValues(typeof(Weight.Property)))
                    if (Random.NextDouble() < 0.2)
                    {
                        _weights[cnt][property] = GetRandom(property);
                        _weights[cnt + 1][property] = GetRandom(property);
                    }
                    else if (Random.NextDouble() > 0.5)
                    {
                        _weights[cnt][property] = Players[i].Weight[property];
                        _weights[cnt + 1][property] = Players[j].Weight[property];
                    }
                    else
                    {
                        _weights[cnt][property] = Players[j].Weight[property];
                        _weights[cnt + 1][property] = Players[i].Weight[property];
                    }

                cnt += 2;
            }

            // 19 ~ 24번은 상위 개체 6개를 등록
            for (int i = 0; i < 6; i++)
                foreach (Weight.Property property in Enum.GetValues(typeof(Weight.Property)))
                    _weights[cnt + i][property] = Players[i].Weight[property];

            MainForm.FileSave(_weights, FilePath.WeightList);

            float GetRandom(Weight.Property p)
            {
                float value = (int) p <= 2 ? Random.Next(-100, 0) : Random.Next(0, 100);
                value += (float) Random.NextDouble();
                return value;
            }
        }

        private static Weight GetRandomWeight()
        {
            Weight weight = new Weight();
            foreach (Weight.Property property in Enum.GetValues(typeof(Weight.Property)))
            {
                weight[property] = (int) property <= 2 ? Random.Next(-100, 0) : Random.Next(0, 100);
                weight[property] += (float) Random.NextDouble();
            }

            return weight;
        }

        private static T Clone<T>(this T obj)
        {
            string temp = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(temp);
        }
    }
}