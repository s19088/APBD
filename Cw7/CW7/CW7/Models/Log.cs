﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW7.Models
{
    public class Log
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string QueryString { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
