﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3_DanTaPhaiBietSuTa.DTO
{
    [Table("Stage")]
    public class Stage
    {
        [Key]
        public int StageID { get; set; }
        public string StageName { get; set; }
        public int VideoID { get; set; }
        [ForeignKey("VideoID")]
        public virtual Video Video { get; set; }
        public override string ToString()
        {
            return StageName;
        }
    }
}