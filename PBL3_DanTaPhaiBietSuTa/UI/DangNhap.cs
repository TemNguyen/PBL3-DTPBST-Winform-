﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PBL3_DanTaPhaiBietSuTa.DTO;
using PBL3_DanTaPhaiBietSuTa.UI;

namespace PBL3_DanTaPhaiBietSuTa
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
            IsRememberUser();
        }

        private void Setting_Click(object sender, EventArgs e)
        {
            SettingForm s = new SettingForm();
            s.ShowDialog();
        }

        private void txtLoginR_Click(object sender, EventArgs e)
        {
            txtAccountR.Text = "";
            txtEmailR.Text = "";
            txtPassR.Text = "";
            txtRepassR.Text = "";
            gbRegister.Visible = false;
            gbLogin.Visible = true;
        }

        private void txtRegister_Click(object sender, EventArgs e)
        {
            txtAccount.Text = "";
            txtPass.Text = "";
            gbLogin.Visible = false;
            gbRegister.Visible = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtAccount.Text;
            string passWord = txtPass.Text;
            if(BLL.Instance.CheckLogin(userName, passWord))
            {
                if(cbRemember.Checked) //lưu userName và passWord vào file.
                {
                    string rememberUserPath = @Application.StartupPath + @"\Assets\SavedUser\rememberUser.txt";
                    using (StreamWriter sw = File.CreateText(rememberUserPath))
                    {
                        sw.WriteLine(txtAccount.Text);
                        sw.WriteLine(txtPass.Text);
                    }
                }
                else
                {
                    string rememberUserPath = @Application.StartupPath + @"\Assets\SavedUser\rememberUser.txt";
                    File.Delete(rememberUserPath);
                }    
                MessageBox.Show("Đăng nhập thành công!");
                //Hiện Thông báo đăng nhập thành công
                //Hiện MainForm
            }
            else
            {
                //Hiện Thông báo đăng nhập thất bại
                MessageBox.Show("Có lỗi xảy ra, vui lòng kiểm tra lại thông tin tài khoản!");
                txtAccount.Text = "";
                txtPass.Text = "";
                return;
            }    
        }

        private void btnRegisterR_Click(object sender, EventArgs e)
        {
            if (txtPassR.Text != txtRepassR.Text)
            {
                //Hiện thông báo pass và repass không giống nhau!
                MessageBox.Show("Pass và Repass không giống nhau!");
                txtPassR.Text = "";
                txtRepassR.Text = "";
                return;
            }
            if (BLL.Instance.IsExistUser(txtAccountR.Text))
            {
                //Hiện thông báo user đã tồn tại.
                MessageBox.Show("Tên tài khoản đã tồn tại!");
                return;
            }    
            if(txtAccountR.Text == "" || txtEmailR.Text == "" || txtPassR.Text == "" || txtRepassR.Text == "")
            {
                //UX viền đỏ
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }
            UserInfo newUser = new UserInfo();
            //newUser.Name = txtNameR.Text;
            newUser.Username = txtAccountR.Text;
            newUser.Password = txtPassR.Text;
            newUser.Email = txtEmailR.Text;
            if(BLL.Instance.AddNewUser(newUser))
            {
                //Hiện thông báo đăng ký thành công
                MessageBox.Show("Đăng ký thành công!");
                txtAccountR.Text = "";
                txtEmailR.Text = "";
                txtPassR.Text = "";
                txtRepassR.Text = "";
                gbRegister.Visible = false;
                gbLogin.Visible = true;
            }    
        }
        private bool IsRememberUser()
        {
            string rememberUserPath = @Application.StartupPath + @"\Assets\SavedUser\rememberUser.txt";
            if (File.Exists(rememberUserPath))
            {
                cbRemember.Checked = true;
                List<string> rememberUser = new List<string>(File.ReadAllLines(rememberUserPath));
                txtAccount.Text = rememberUser[0];
                txtPass.Text = rememberUser[1];
            }
            return true;
        }
    }
}