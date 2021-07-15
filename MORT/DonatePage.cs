﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    public partial class DonatePage : Form
    {
        public DonatePage()
        {
            InitializeComponent();
        }

        private void DonatePage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormManager.GetIsRemain())
            {
                FormManager.Instace.DestoryDonatePage();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenDonatePage();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenDonatePage();
        }

        private void OpenDonatePage()
        {
            try
            {
                System.Diagnostics.Process.Start("https://toon.at/donate/mort");
            }
            catch { }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenKakaoPay();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenKakaoPay();
        }

        private void OpenKakaoPay()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
        }

        private void DonatePage_Load(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
            panel4.Visible = false;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
            panel4.Visible = false;
        }
    }
}
