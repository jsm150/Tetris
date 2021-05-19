using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public sealed partial class SettingForm
    {

        private void InitializeComponent()
        {
            this.tBar_Volume = new MetroFramework.Controls.MetroTrackBar();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.tog_UiRendering = new MetroFramework.Controls.MetroToggle();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // tBar_Volume
            // 
            this.tBar_Volume.BackColor = System.Drawing.Color.Transparent;
            this.tBar_Volume.Location = new System.Drawing.Point(23, 47);
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
            this.metroLabel1.Location = new System.Drawing.Point(23, 25);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(44, 19);
            this.metroLabel1.TabIndex = 18;
            this.metroLabel1.Text = "sound";
            // 
            // tog_UiRendering
            // 
            this.tog_UiRendering.AutoSize = true;
            this.tog_UiRendering.Location = new System.Drawing.Point(123, 82);
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
            this.metroLabel2.Location = new System.Drawing.Point(23, 79);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(85, 19);
            this.metroLabel2.TabIndex = 18;
            this.metroLabel2.Text = "UI Rendering";
            // 
            // SettingForm
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(218, 119);
            this.Controls.Add(this.tog_UiRendering);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.tBar_Volume);
            this.MaximizeBox = false;
            this.Name = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MetroFramework.Controls.MetroTrackBar tBar_Volume;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroToggle tog_UiRendering;
        private MetroFramework.Controls.MetroLabel metroLabel2;
    }
}
