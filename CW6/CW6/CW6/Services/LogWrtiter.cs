using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CW6.Services
{
    public class LogWrtiter : IOService
    {
        private readonly static string path = "Log.txt";
        public void Write(string log)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
          using(var writer = new StreamWriter(path, true))
            {

                writer.WriteLine(JsonConvert.SerializeObject(DateTime.Now));
                writer.WriteLine(log);
            }
        }
    }
}
