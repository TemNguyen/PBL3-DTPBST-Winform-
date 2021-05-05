﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PBL3_DanTaPhaiBietSuTa.UI;

namespace PBL3_DanTaPhaiBietSuTa.UI
{
    public partial class HomePage : Form
    {
        Thread th;
        private static bool sttSetting = false;
        public HomePage()
        {
            InitializeComponent();
            PlaySound();
        }
        private void PlaySound()
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = @Application.StartupPath + @"\Assets\Sound\Sound.wav";
            player.PlayLooping();
        }

        private void OpenLoginForm(object sender)
        {
            Application.Run(new DangNhap());
        }

        private void btnPlayNow_Click(object sender, EventArgs e)
        {
            this.Dispose();
            th = new Thread(OpenLoginForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (sttSetting == false)
            {
                btnHelp.Visible = true;
                btnInfo.Visible = true;
                sttSetting = true;
            }
            else
            {
                btnHelp.Visible = false;
                btnInfo.Visible = false;
                sttSetting = false;
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            gbIntro.Visible = true;
            gbHelp.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gbIntro.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gbHelp.Visible = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            gbHelp.Visible = true;
            gbIntro.Visible = false;
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            label2.Location = new Point((gbIntro.Size.Width - label2.Size.Width) / 2, 20);
            label3.Location = new Point((gbIntro.Size.Width - label3.Size.Width) / 2, 63);
            label4.Location = new Point((gbIntro.Size.Width - label4.Size.Width) / 2, 100);
            label5.Location = new Point((gbIntro.Size.Width - label3.Size.Width) / 2, 128);
            label6.Location = new Point((gbIntro.Size.Width - label6.Size.Width) / 2, 162);
            label7.Location = new Point((gbIntro.Size.Width - label7.Size.Width) / 2, 197);
            label8.Location = new Point((gbIntro.Size.Width - label8.Size.Width) / 2, 233);
            gbIntro.Location = new Point((this.Size.Width - gbIntro.Size.Width) / 2,
                (this.Size.Height - gbIntro.Size.Height) / 2);
            gbHelp.Location = new Point((this.Size.Width - gbHelp.Size.Width) / 2,
                (this.Size.Height - gbHelp.Size.Height) / 2);
        }
    }
}
