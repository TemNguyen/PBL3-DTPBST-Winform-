﻿using PBL3_DanTaPhaiBietSuTa.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBL3_DanTaPhaiBietSuTa
{
    public partial class GenData : Form
    {
        public GenData()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DB db = new DB();
            
            dataGridView1.DataSource = db.Questions.ToList(); 
            
        }
    }
}