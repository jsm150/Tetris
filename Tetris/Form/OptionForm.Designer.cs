using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public sealed partial class OptionForm
    {

        private void InitializeComponent()
        {
            this.tBar_Volume = new MetroFramework.Controls.MetroTrackBar();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.tog_UiRendering = new MetroFramework.Controls.MetroToggle();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_HardDropP2 = new MetroFramework.Controls.MetroButton();
            this.btn_MoveLeftP2 = new MetroFramework.Controls.MetroButton();
            this.btn_MoveRightP2 = new MetroFramework.Controls.MetroButton();
            this.btn_MoveDownP2 = new MetroFramework.Controls.MetroButton();
            this.btn_RotationRightP2 = new MetroFramework.Controls.MetroButton();
            this.btn_HardDropP1 = new MetroFramework.Controls.MetroButton();
            this.btn_MoveRightP1 = new MetroFramework.Controls.MetroButton();
            this.btn_MoveLeftP1 = new MetroFramework.Controls.MetroButton();
            this.btn_MoveDownP1 = new MetroFramework.Controls.MetroButton();
            this.btn_RotationRightP1 = new MetroFramework.Controls.MetroButton();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tBar_Volume
            // 
            this.tBar_Volume.BackColor = System.Drawing.Color.Transparent;
            this.tBar_Volume.Location = new System.Drawing.Point(8, 46);
            this.tBar_Volume.Name = "tBar_Volume";
            this.tBar_Volume.Size = new System.Drawing.Size(180, 29);
            this.tBar_Volume.TabIndex = 17;
            this.tBar_Volume.TabStop = false;
            this.tBar_Volume.Text = "metroTrackBar1";
            this.tBar_Volume.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tBar_Volume_Scroll);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(8, 24);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(44, 19);
            this.metroLabel1.TabIndex = 18;
            this.metroLabel1.Text = "sound";
            // 
            // tog_UiRendering
            // 
            this.tog_UiRendering.AutoSize = true;
            this.tog_UiRendering.Location = new System.Drawing.Point(194, 52);
            this.tog_UiRendering.Name = "tog_UiRendering";
            this.tog_UiRendering.Size = new System.Drawing.Size(80, 16);
            this.tog_UiRendering.TabIndex = 19;
            this.tog_UiRendering.Text = "Off";
            this.tog_UiRendering.UseSelectable = true;
            this.tog_UiRendering.CheckedChanged += new System.EventHandler(this.tog_UiRendering_CheckedChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(194, 24);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(85, 19);
            this.metroLabel2.TabIndex = 18;
            this.metroLabel2.Text = "UI Rendering";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_HardDropP2);
            this.groupBox1.Controls.Add(this.btn_MoveLeftP2);
            this.groupBox1.Controls.Add(this.btn_MoveRightP2);
            this.groupBox1.Controls.Add(this.btn_MoveDownP2);
            this.groupBox1.Controls.Add(this.btn_RotationRightP2);
            this.groupBox1.Controls.Add(this.btn_HardDropP1);
            this.groupBox1.Controls.Add(this.btn_MoveRightP1);
            this.groupBox1.Controls.Add(this.btn_MoveLeftP1);
            this.groupBox1.Controls.Add(this.btn_MoveDownP1);
            this.groupBox1.Controls.Add(this.btn_RotationRightP1);
            this.groupBox1.Controls.Add(this.metroLabel7);
            this.groupBox1.Controls.Add(this.metroLabel6);
            this.groupBox1.Controls.Add(this.metroLabel5);
            this.groupBox1.Controls.Add(this.metroLabel4);
            this.groupBox1.Controls.Add(this.metroLabel3);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox1.Location = new System.Drawing.Point(9, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 185);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Key Setting";
            // 
            // btn_HardDropP2
            // 
            this.btn_HardDropP2.Location = new System.Drawing.Point(196, 147);
            this.btn_HardDropP2.Name = "btn_HardDropP2";
            this.btn_HardDropP2.Size = new System.Drawing.Size(58, 18);
            this.btn_HardDropP2.TabIndex = 19;
            this.btn_HardDropP2.UseSelectable = true;
            this.btn_HardDropP2.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_HardDropP2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_MoveLeftP2
            // 
            this.btn_MoveLeftP2.Location = new System.Drawing.Point(196, 87);
            this.btn_MoveLeftP2.Name = "btn_MoveLeftP2";
            this.btn_MoveLeftP2.Size = new System.Drawing.Size(58, 18);
            this.btn_MoveLeftP2.TabIndex = 19;
            this.btn_MoveLeftP2.UseSelectable = true;
            this.btn_MoveLeftP2.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_MoveLeftP2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_MoveRightP2
            // 
            this.btn_MoveRightP2.Location = new System.Drawing.Point(196, 117);
            this.btn_MoveRightP2.Name = "btn_MoveRightP2";
            this.btn_MoveRightP2.Size = new System.Drawing.Size(58, 18);
            this.btn_MoveRightP2.TabIndex = 19;
            this.btn_MoveRightP2.UseSelectable = true;
            this.btn_MoveRightP2.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_MoveRightP2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_MoveDownP2
            // 
            this.btn_MoveDownP2.Location = new System.Drawing.Point(196, 57);
            this.btn_MoveDownP2.Name = "btn_MoveDownP2";
            this.btn_MoveDownP2.Size = new System.Drawing.Size(58, 18);
            this.btn_MoveDownP2.TabIndex = 19;
            this.btn_MoveDownP2.UseSelectable = true;
            this.btn_MoveDownP2.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_MoveDownP2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_RotationRightP2
            // 
            this.btn_RotationRightP2.Location = new System.Drawing.Point(196, 27);
            this.btn_RotationRightP2.Name = "btn_RotationRightP2";
            this.btn_RotationRightP2.Size = new System.Drawing.Size(58, 18);
            this.btn_RotationRightP2.TabIndex = 19;
            this.btn_RotationRightP2.UseSelectable = true;
            this.btn_RotationRightP2.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_RotationRightP2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_HardDropP1
            // 
            this.btn_HardDropP1.Location = new System.Drawing.Point(121, 147);
            this.btn_HardDropP1.Name = "btn_HardDropP1";
            this.btn_HardDropP1.Size = new System.Drawing.Size(58, 18);
            this.btn_HardDropP1.TabIndex = 19;
            this.btn_HardDropP1.UseSelectable = true;
            this.btn_HardDropP1.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_HardDropP1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_MoveRightP1
            // 
            this.btn_MoveRightP1.Location = new System.Drawing.Point(121, 117);
            this.btn_MoveRightP1.Name = "btn_MoveRightP1";
            this.btn_MoveRightP1.Size = new System.Drawing.Size(58, 18);
            this.btn_MoveRightP1.TabIndex = 19;
            this.btn_MoveRightP1.UseSelectable = true;
            this.btn_MoveRightP1.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_MoveRightP1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_MoveLeftP1
            // 
            this.btn_MoveLeftP1.Location = new System.Drawing.Point(121, 87);
            this.btn_MoveLeftP1.Name = "btn_MoveLeftP1";
            this.btn_MoveLeftP1.Size = new System.Drawing.Size(58, 18);
            this.btn_MoveLeftP1.TabIndex = 19;
            this.btn_MoveLeftP1.UseSelectable = true;
            this.btn_MoveLeftP1.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_MoveLeftP1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_MoveDownP1
            // 
            this.btn_MoveDownP1.Location = new System.Drawing.Point(121, 57);
            this.btn_MoveDownP1.Name = "btn_MoveDownP1";
            this.btn_MoveDownP1.Size = new System.Drawing.Size(58, 18);
            this.btn_MoveDownP1.TabIndex = 19;
            this.btn_MoveDownP1.UseSelectable = true;
            this.btn_MoveDownP1.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_MoveDownP1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // btn_RotationRightP1
            // 
            this.btn_RotationRightP1.Location = new System.Drawing.Point(121, 27);
            this.btn_RotationRightP1.Name = "btn_RotationRightP1";
            this.btn_RotationRightP1.Size = new System.Drawing.Size(58, 18);
            this.btn_RotationRightP1.TabIndex = 19;
            this.btn_RotationRightP1.UseMnemonic = false;
            this.btn_RotationRightP1.UseSelectable = true;
            this.btn_RotationRightP1.Click += new System.EventHandler(this.btn_SetKeyboard_Click);
            this.btn_RotationRightP1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btn_SetKeyboard_PreviewKeyDown);
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(14, 146);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(72, 19);
            this.metroLabel7.TabIndex = 18;
            this.metroLabel7.Text = "Hard Drop";
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(14, 116);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(76, 19);
            this.metroLabel6.TabIndex = 18;
            this.metroLabel6.Text = "Move Right";
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(14, 86);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(67, 19);
            this.metroLabel5.TabIndex = 18;
            this.metroLabel5.Text = "Move Left";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(14, 56);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(79, 19);
            this.metroLabel4.TabIndex = 18;
            this.metroLabel4.Text = "Move Down";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(14, 26);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(92, 19);
            this.metroLabel3.TabIndex = 18;
            this.metroLabel3.Text = "Rotation Right";
            // 
            // OptionForm
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(286, 273);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tog_UiRendering);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.tBar_Volume);
            this.MaximizeBox = false;
            this.Name = "OptionForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MetroFramework.Controls.MetroTrackBar tBar_Volume;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroToggle tog_UiRendering;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroButton btn_RotationRightP1;
        private MetroFramework.Controls.MetroButton btn_HardDropP2;
        private MetroFramework.Controls.MetroButton btn_MoveLeftP2;
        private MetroFramework.Controls.MetroButton btn_MoveRightP2;
        private MetroFramework.Controls.MetroButton btn_MoveDownP2;
        private MetroFramework.Controls.MetroButton btn_RotationRightP2;
        private MetroFramework.Controls.MetroButton btn_HardDropP1;
        private MetroFramework.Controls.MetroButton btn_MoveRightP1;
        private MetroFramework.Controls.MetroButton btn_MoveLeftP1;
        private MetroFramework.Controls.MetroButton btn_MoveDownP1;
    }
}
