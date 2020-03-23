using CW3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW3.DAL
{
    public interface IDbService
    {
        public /*IEnumerable*/ List<Student> GetStudents();

       ///////// public void AddStudent(Student student);
    }
}
