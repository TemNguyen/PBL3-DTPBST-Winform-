﻿using PBL3_DanTaPhaiBietSuTa.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3_DanTaPhaiBietSuTa
{
    public class DAL
    {
        public static DAL Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DAL();
                }
                return _Instance;
            }
            private set
            {

            }
        }
        private static DAL _Instance;
        private DAL() { }
        public bool AddUser(UserInfo user)
        {
            if (IsExistUser(user.Username)) return false;
            else
            {
                using (DB db = new DB())
                {
                    db.UserInfos.Add(user);
                    db.SaveChanges();
                    return true;
                }
            }
        }
        public bool CheckLogin(string userName, string passWord)
        {
            using (var db = new DB())
            {
                var User = db.UserInfos.Where(s => s.Username == userName && s.Password == passWord).FirstOrDefault<UserInfo>();
                if (User != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsExistUser(string userName)
        {
            using (var db = new DB())
            {
                var User = db.UserInfos.Where(s => s.Username == userName).FirstOrDefault<UserInfo>();
                if (User != null)
                {

                    return true; //Đã tồn tại
                }
                else
                {
                    return false;
                }
            }

        }
        public List<UserInfo> GetListUserInfo()
        {
            List<UserInfo> list = new List<UserInfo>();
            using (DB db = new DB())
            {
                list = db.UserInfos.ToList();
            }
            return list;
        }
        public bool UpdateUser(UserInfo usr)
        {
            using(DB db = new DB())
            {
                var user = db.UserInfos.Find(usr.UserID);
                if (user == null) return false;
                else
                {
                    user.Email = usr.Email;
                    user.Name = usr.Name;
                    user.Password = usr.Password;
                    db.SaveChanges();
                    return true;
                }
            }    
        }
        public UserInfo GetUserInfoByUsername(string username)
        {
            using (DB db = new DB())
            {
                UserInfo user = db.UserInfos.Where(s => s.Username == username).FirstOrDefault();
                return user ;
            }
        }
        public Video GetVideo(int stageID)
        {
            using (DB db = new DB())
            {
                Stage stage = db.Stages.Where(s => s.StageID == stageID).FirstOrDefault();
                Video video = stage.Video;
                return video;
            }
        }
        public List<Question> GetListQuestion(int stageID)
        {
            using (DB db = new DB())
            {
                List<Question> list = db.Stages.Find(stageID).Questions.ToList();
                
                return list;
            }
        }
        public List<Question> GetListQuestionByTimeStop(int stageID,int TimeStop)
        {
            using (DB db = new DB())
            {
                List<Question> list = new List<Question>();
                List<Question> list1 = db.Stages.Find(stageID).Questions.ToList();
                foreach (var question in list1)
                {
                    if(question.TimeStop == TimeStop)
                    list.Add(question);
                }    
                return list;
            }
        }
        public void UpdatePointTable(GameProcess gameProcess)
        {
            using (DB db = new DB())
            {
                bool kt = true;
                foreach(var point in db.Points)
                {
                    if(point.StageID == gameProcess.StageID && point.UserID == gameProcess.UserID)
                    {
                        if (point.point < gameProcess.Point) point.point = gameProcess.Point;
                        
                        kt = false;
                        break;
                    }    
                }
                if (kt) db.Points.Add(new Point
                {
                    StageID = gameProcess.StageID,
                    UserID = gameProcess.UserID,
                    point = gameProcess.Point
                });
                db.SaveChanges();
                
            }    
        }
        public Standing GetStandingByUserID(int UserID)
        {
            using(DB db = new DB())
            {
                Standing standing = new Standing();
                standing.UserID = UserID;
                standing.Point = 0;
                standing.StageID = 0;
                foreach (var point in db.Points)
                {
                    if(point.UserID == UserID)
                    {
                        standing.Point += point.point;
                        if (point.StageID > standing.StageID) standing.StageID = point.StageID;
                    }    
                }    
                return standing;
            }    
        }
        public List<Standing> GetListStanding()
        {
            List<Standing> list = new List<Standing>();
            //123
            using(DB db = new DB())
            {
                foreach(var user in db.UserInfos)
                {
                    var standing = GetStandingByUserID(user.UserID);
                    if(standing.StageID != 0) list.Add(standing);
                }    
            }    
            return list;
        }





    }
}