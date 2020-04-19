using CW6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW6.Services
{
   public interface IStudentDbService
    {
        public Student GetStudent(string Index);
    }
}
