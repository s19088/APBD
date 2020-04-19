using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW6.Models
{
    public class Log
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string QueryString { get; set; }
        public string Body { get; set; }
    }
}
