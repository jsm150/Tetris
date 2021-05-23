using System;

namespace Tetris
{
    sealed partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose(); 
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Score = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_BestScore = new System.Windows.Forms.Label();
            this.lbl_2pScore = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_Generation = new System.Windows.Forms.Label();
            this.lbl_bestNum = new System.Windows.Forms.Label();
            this.btn_GameStart = new MetroFramework.Controls.MetroButton();
            this.btn_1vs1 = new MetroFramework.Controls.MetroButton();
            this.btn_AI = new MetroFramework.Controls.MetroButton();
            this.btn_GeneticAlgorithm = new MetroFramework.Controls.MetroButton();
            this.btn_Setting = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(33, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Score:";
            // 
            // lbl_Score
            // 
            this.lbl_Score.AutoSize = true;
            this.lbl_Score.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Score.Location = new System.Drawing.Point(87, 43);
            this.lbl_Score.Name = "lbl_Score";
            this.lbl_Score.Size = new System.Drawing.Size(12, 12);
            this.lbl_Score.TabIndex = 2;
            this.lbl_Score.Text = "0";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(33, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "BestScore:";
            // 
            // lbl_BestScore
            // 
            this.lbl_BestScore.AutoSize = true;
            this.lbl_BestScore.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_BestScore.Location = new System.Drawing.Point(116, 65);
            this.lbl_BestScore.Name = "lbl_BestScore";
            this.lbl_BestScore.Size = new System.Drawing.Size(12, 12);
            this.lbl_BestScore.TabIndex = 4;
            this.lbl_BestScore.Text = "0";
            // 
            // lbl_2pScore
            // 
            this.lbl_2pScore.AutoSize = true;
            this.lbl_2pScore.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_2pScore.Location = new System.Drawing.Point(422, 43);
            this.lbl_2pScore.Name = "lbl_2pScore";
            this.lbl_2pScore.Size = new System.Drawing.Size(12, 12);
            this.lbl_2pScore.TabIndex = 7;
            this.lbl_2pScore.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(368, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "Score:";
            // 
            // lbl_Generation
            // 
            this.lbl_Generation.AutoSize = true;
            this.lbl_Generation.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Generation.Location = new System.Drawing.Point(33, 132);
            this.lbl_Generation.Name = "lbl_Generation";
            this.lbl_Generation.Size = new System.Drawing.Size(0, 12);
            this.lbl_Generation.TabIndex = 3;
            // 
            // lbl_bestNum
            // 
            this.lbl_bestNum.AutoSize = true;
            this.lbl_bestNum.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_bestNum.Location = new System.Drawing.Point(110, 132);
            this.lbl_bestNum.Name = "lbl_bestNum";
            this.lbl_bestNum.Size = new System.Drawing.Size(0, 12);
            this.lbl_bestNum.TabIndex = 3;
            // 
            // btn_GameStart
            // 
            this.btn_GameStart.Location = new System.Drawing.Point(253, 32);
            this.btn_GameStart.Name = "btn_GameStart";
            this.btn_GameStart.Size = new System.Drawing.Size(99, 35);
            this.btn_GameStart.TabIndex = 12;
            this.btn_GameStart.TabStop = false;
            this.btn_GameStart.Text = "Game Start!";
            this.btn_GameStart.UseSelectable = true;
            this.btn_GameStart.Click += new System.EventHandler(this.btn_GameStart_Click);
            // 
            // btn_1vs1
            // 
            this.btn_1vs1.Location = new System.Drawing.Point(196, 32);
            this.btn_1vs1.Name = "btn_1vs1";
            this.btn_1vs1.Size = new System.Drawing.Size(51, 35);
            this.btn_1vs1.TabIndex = 13;
            this.btn_1vs1.TabStop = false;
            this.btn_1vs1.Text = "1 vs 1";
            this.btn_1vs1.UseSelectable = true;
            this.btn_1vs1.Click += new System.EventHandler(this.btn_1vs1_Click);
            // 
            // btn_AI
            // 
            this.btn_AI.Location = new System.Drawing.Point(157, 92);
            this.btn_AI.Name = "btn_AI";
            this.btn_AI.Size = new System.Drawing.Size(33, 23);
            this.btn_AI.TabIndex = 14;
            this.btn_AI.TabStop = false;
            this.btn_AI.Text = "AI";
            this.btn_AI.UseSelectable = true;
            this.btn_AI.Click += new System.EventHandler(this.btn_AI_Click);
            // 
            // btn_GeneticAlgorithm
            // 
            this.btn_GeneticAlgorithm.Location = new System.Drawing.Point(14, 92);
            this.btn_GeneticAlgorithm.Name = "btn_GeneticAlgorithm";
            this.btn_GeneticAlgorithm.Size = new System.Drawing.Size(123, 23);
            this.btn_GeneticAlgorithm.TabIndex = 15;
            this.btn_GeneticAlgorithm.TabStop = false;
            this.btn_GeneticAlgorithm.Text = "GeneticAlgorithm";
            this.btn_GeneticAlgorithm.UseSelectable = true;
            this.btn_GeneticAlgorithm.Click += new System.EventHandler(this.btn_GeneticAlgorithm_Click);
            // 
            // btn_Setting
            // 
            this.btn_Setting.Location = new System.Drawing.Point(212, 92);
            this.btn_Setting.Name = "btn_Setting";
            this.btn_Setting.Size = new System.Drawing.Size(127, 22);
            this.btn_Setting.TabIndex = 17;
            this.btn_Setting.TabStop = false;
            this.btn_Setting.Text = "Setting";
            this.btn_Setting.UseSelectable = true;
            this.btn_Setting.Click += new System.EventHandler(this.btn_Setting_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(362, 125);
            this.Controls.Add(this.btn_Setting);
            this.Controls.Add(this.btn_GeneticAlgorithm);
            this.Controls.Add(this.btn_AI);
            this.Controls.Add(this.btn_1vs1);
            this.Controls.Add(this.btn_GameStart);
            this.Controls.Add(this.lbl_2pScore);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbl_BestScore);
            this.Controls.Add(this.lbl_bestNum);
            this.Controls.Add(this.lbl_Generation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_Score);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Score;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_BestScore;
        private System.Windows.Forms.Label lbl_2pScore;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_Generation;
        private System.Windows.Forms.Label lbl_bestNum;
        private MetroFramework.Controls.MetroButton btn_GameStart;
        private MetroFramework.Controls.MetroButton btn_1vs1;
        private MetroFramework.Controls.MetroButton btn_AI;
        private MetroFramework.Controls.MetroButton btn_GeneticAlgorithm;
        private MetroFramework.Controls.MetroButton btn_Setting;
    }
}

