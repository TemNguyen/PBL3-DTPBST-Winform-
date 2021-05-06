using PBL3_DanTaPhaiBietSuTa.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBL3_DanTaPhaiBietSuTa.UI
{
    public partial class Play : Form
    {
        Thread thUser;
        Color cBtn = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
        List<int> listTimeStop = new List<int>();
        static int stageID;
        Question selectedQuestion;
        int point = 0, questionID = 0, numCorrect = 0;
        int TVideo = 0, countDown = 300;
        int delay = 10, showAnswer = 10;
        public Play()
        {
            InitializeComponent();
            stageID = 1;
            lbPoint.Text = point.ToString();
            lbTime.Text = "Time: " + (countDown / 10).ToString();
            SetTimeStop();
            SetVideoStage();
            videoTime.Start();
            ResetQuestion();
        }
        private void SetVideoStage()
        {
            var video = BLL.Instance.GetVideo(stageID);
            Video.URL = @Application.StartupPath + @"\Assets\Video\" + video.VideoID + ".mp4";
            Video.Ctlenabled = false;
            Video.settings.volume = 100;
        }
        private void SetTimeStop()
        {
            listTimeStop.Clear();
            List<int> list = new List<int>();
            foreach (var i in BLL.Instance.GetQuestionsByStage(stageID))
            {
                list.Add(i.TimeStop);
            }
            HashSet<int> setListTimeStop = new HashSet<int>(list);
            listTimeStop.AddRange(setListTimeStop);
            listTimeStop.Sort();
        }
        private void btnSetting_Click(object sender, EventArgs e)
        {
            SettingForm s = new SettingForm();
            s.ShowDialog();
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            if (Video.playState != WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                //Hiện thông báo kh lưu proccess
                DialogResult d = MessageBox.Show("Tiến trình hiện tại sẽ không được lưu, Tiêp tục?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                switch(d)
                {
                    case DialogResult.Yes:
                        this.Dispose();
                        thUser = new Thread(OpenUserForm);
                        thUser.SetApartmentState(ApartmentState.STA);
                        thUser.Start();
                        break;
                    case DialogResult.No:
                        return;
                }    
            }
            else
            {
                new User().Show();
                this.Hide();
            }    
        }
        private void DisplayQuestion()
        {
            selectedQuestion = BLL.Instance.GetRandomQuestionByTimeStop(stageID, listTimeStop[questionID]);
            //Display question
            txtQuestion.Text = selectedQuestion.QuestionContent;
            List<string> listAnswer = new List<string>();
            listAnswer.Add(selectedQuestion.KeyA);
            listAnswer.Add(selectedQuestion.KeyB);
            listAnswer.Add(selectedQuestion.KeyC);
            listAnswer.Add(selectedQuestion.KeyD);
            Random rd = new Random();
            int questionrd;
            questionrd = rd.Next(0, listAnswer.Count - 1);
            btnA.Text = "A. " + listAnswer[questionrd];
            listAnswer.RemoveAt(questionrd);
            questionrd = rd.Next(0, listAnswer.Count - 1);
            btnB.Text += "B. " + listAnswer[questionrd];
            listAnswer.RemoveAt(questionrd);
            questionrd = rd.Next(0, listAnswer.Count - 1);
            btnC.Text = "C. " + listAnswer[questionrd];
            listAnswer.RemoveAt(questionrd);
            questionrd = rd.Next(0, listAnswer.Count - 1);
            btnD.Text = "D. " + listAnswer[questionrd];
            listAnswer.RemoveAt(questionrd);
        }
        private void SelectAnswer(object sender, EventArgs e)
        {
            string answer = ((Button)sender).Text.Remove(0, 3);
            //UX Show correct Answer
            ShowCorrectAnswer();
            if (CheckAnswer(answer))
            {
                CaculationPoint(countDown);
                numCorrect++;
            }
        }
        private bool CheckAnswer(string answer)
        {
            //answer == selectedQuestion.Answer => true
            //answer != selectedQuestion.Answer => true
            //countdown == 0 => false
            if (String.Compare(answer, selectedQuestion.Answer) != 0 || countDown == 0)
                return false;
            else
                return true;
        }
        private void ShowCorrectAnswer()
        {
            questionTime.Stop();
            btnA.Enabled = false;
            btnB.Enabled = false;
            btnC.Enabled = false;
            btnD.Enabled = false;
            changeColor.Start();
        }
        private void CaculationPoint(int timeUsed)
        {
            //Caculation Point
            //Display Point
            point += Convert.ToInt32((countDown*1.0/300) * 100);
            lbPoint.Text = point.ToString();
        }
        private void ResetQuestion()
        {
            txtQuestion.Text = "";
            btnA.Text = "";
            btnB.Text = "";
            btnC.Text = "";
            btnD.Text = "";
        }
        private void videoTime_Tick(object sender, EventArgs e)
        {
            btnA.Enabled = false;
            btnB.Enabled = false;
            btnC.Enabled = false;
            btnD.Enabled = false;
            TVideo = Convert.ToInt32(Video.Ctlcontrols.currentPosition * 10);
            try
            {
                if (TVideo >= listTimeStop[questionID])
                {
                    Video.Ctlcontrols.pause();
                    DisplayQuestion();
                    videoTime.Stop();
                    questionTime.Start();
                }
            }
            catch (Exception)
            {

            };
            IsFinish();
        }
        private void questionTime_Tick(object sender, EventArgs e)
        {
            btnA.Enabled = true;
            btnB.Enabled = true;
            btnC.Enabled = true;
            btnD.Enabled = true;
            try
            {
                if (TVideo >= listTimeStop[questionID])
                {
                    countDown--;
                }
            }
            catch (Exception) { };
            lbTime.Text = "Time: " + (countDown / 10).ToString();
            if (countDown == 0)
            {
                ShowCorrectAnswer();
            }
        }
        private void changeColor_Tick(object sender, EventArgs e)
        {
            showAnswer--;
            if (String.Compare(btnA.Text.Remove(0, 3), selectedQuestion.Answer) == 0)
            {
                if (showAnswer % 3 == 1)
                    changeBtnColor(btnA, Color.Green);
                if (showAnswer % 3 == 2)
                    changeBtnColor(btnA, Color.Pink);
                if (showAnswer % 3 == 0)
                    changeBtnColor(btnA, cBtn);
            }
            if (String.Compare(btnB.Text.Remove(0, 3), selectedQuestion.Answer) == 0)
            {
                if (showAnswer % 3 == 1)
                    changeBtnColor(btnB, Color.Green);
                if (showAnswer % 3 == 2)
                    changeBtnColor(btnB, Color.Pink);
                if (showAnswer % 3 == 0)
                    changeBtnColor(btnB, cBtn);
            }
            if (String.Compare(btnC.Text.Remove(0, 3), selectedQuestion.Answer) == 0)
            {
                if (showAnswer % 3 == 1)
                    changeBtnColor(btnC, Color.Green);
                if (showAnswer % 3 == 2)
                    changeBtnColor(btnC, Color.Pink);
                if (showAnswer % 3 == 0)
                    changeBtnColor(btnC, cBtn);
            }
            if (String.Compare(btnD.Text.Remove(0, 3), selectedQuestion.Answer) == 0)
            {
                if (showAnswer % 3 == 1)
                    changeBtnColor(btnD, Color.Green);
                if (showAnswer % 3 == 2)
                    changeBtnColor(btnD, Color.Pink);
                if (showAnswer % 3 == 0)
                    changeBtnColor(btnD, cBtn);
            }
            if (showAnswer < 0)
            {
                showAnswer = 10;
                changeColor.Stop();
                //Delay t(s)
                nextQuestionTime.Start();
            }
        }
        private void nextQuestionTime_Tick(object sender, EventArgs e)
        {
            delay--;
            if (delay < 0)
            {
                delay = 10;
                nextQuestionTime.Stop();
                //reset countDown
                countDown = 300;
                lbTime.Text = "Time: " + (countDown / 10).ToString();
                //Delete question
                ResetQuestion();
                //Increse questionID
                questionID++;
                videoTime.Start();
                Video.Ctlcontrols.play();
            }
        }

        private void lbPoint_TextChanged(object sender, EventArgs e)
        {
            lbPoint.Left = pictureBox2.Location.X + ((pictureBox2.Size.Width - lbPoint.Size.Width) / 2);
        }

        private void lbTime_TextChanged(object sender, EventArgs e)
        {
            lbTime.Left = (pictureBox1.Size.Width - lbTime.Size.Width) / 2;
        }

        private bool IsSavePoint()
        {
            string path = @Application.StartupPath + @"\Assets\SavedUser\Account.txt";
            GameProcess userProcess = new GameProcess();
            userProcess.StageID = stageID;
            userProcess.UserID = Convert.ToInt32(File.ReadLines(path).First());
            userProcess.Point = point;
            BLL.Instance.UpdatePoint(userProcess);
            return true;
        }
        private bool IsFinish()
        {
            if (Video.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                videoTime.Stop();
                IsSavePoint();
                //Hiện UI chơi lại
                ShowMessage("Chúc mừng bạn đã hoàn thành xong màn. \nSố điểm của bạn là: " + point);
                if (numCorrect >= listTimeStop.Count / 2)
                {
                    ShowMessage("Chúc mừng bạn đã qua được màn!");
                }
                else
                    ShowMessage("Cha mẹ thất vọng về em!");
                DialogResult d = MessageBox.Show("Bạn có muốn chơi lại không?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                switch(d)
                {
                    case DialogResult.Yes:
                        //reset form
                        List<int> listTimeStop = new List<int>();
                        point = 0; 
                        questionID = 0;
                        TVideo = 0;
                        countDown = 300;
                        lbPoint.Text = point.ToString();
                        lbTime.Text = "Time: " + (countDown / 10).ToString();
                        SetTimeStop();
                        SetVideoStage();
                        videoTime.Start();
                        ResetQuestion();
                        break;
                    case DialogResult.No:
                        this.Dispose();
                        thUser = new Thread(OpenUserForm);
                        thUser.SetApartmentState(ApartmentState.STA);
                        thUser.Start();
                        break;
                }
                return true;
            }
            return false;
        }
        private void changeBtnColor(CustomButton btn, Color c)
        {
            btn.ButtonColor = c;
            btn.BorderColor = c;
            // cách dùng (Color.Red): changeBtnColor(btnA, Color.Green); 
        }
        private void ShowMessage(string message)
        {
            Notification notification = new Notification();
            notification.Get(message);
            notification.ShowDialog();
        }
        private void OpenUserForm(object sender)
        {
            Application.Run(new User());
        }
    }
}
