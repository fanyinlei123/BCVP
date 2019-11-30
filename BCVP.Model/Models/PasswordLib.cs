﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models
{
    /// <summary>
    /// 密码库表
    /// </summary>
    public class PasswordLib
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int PLID { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string plURL { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string plPWD { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string plAccountName { get; set; }

        [SugarColumn( IsNullable = true)]
        public int? plStatus { get; set; }

        [SugarColumn( IsNullable = true)]
        public int? plErrorCount { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string plHintPwd { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string plHintquestion { get; set; }

        [SugarColumn( IsNullable = true)]
        public DateTime? plCreateTime { get; set; }

        [SugarColumn( IsNullable = true)]
        public DateTime? plUpdateTime { get; set; }

        [SugarColumn( IsNullable = true)]
        public DateTime? plLastErrTime { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string test { get; set; }


    }
}
