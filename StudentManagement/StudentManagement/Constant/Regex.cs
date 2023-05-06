﻿
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Constant {
    internal class Regex {
        public static string EMAIL = "^[\\w-\\.]+@st\\.phenikaa-uni\\.edu\\.vn$";
        public static string PHONE = "^[0-9]{10}$";
        public static string CREDITS = "^\\d+$";
    }
}
