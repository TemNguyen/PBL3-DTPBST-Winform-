﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3_DanTaPhaiBietSuTa.DTO
{
    [Table("GameProcess")]
    public class GameProcess
    {
        [Key]
        public int GameProcessID { get; set; }

        public int StageID { get; set; }
        
        public int Point { get; set; }
        public int UserID { get; set; }
        
        [ForeignKey("UserID")]
        public virtual UserInfo UserInfo { get; set; }
        [ForeignKey("StageID")]
        public virtual Stage Stage { get; set; }
    }
}