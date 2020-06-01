using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CW7.Services
{
    public class LogWriter : IOService
    {
        private readonly static string path = "Log.txt";
        public void Write(string log)
        {
           
            using (var writer = new StreamWriter(path, true))
            {
                writer.WriteLine(log);
            }
        }
    }
}
