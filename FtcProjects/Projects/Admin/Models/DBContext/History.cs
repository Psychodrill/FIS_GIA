﻿using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class History
    {
        public string Participant { get; set; }
        public string Hash { get; set; }
        public int? SubjectCode { get; set; }
    }
}