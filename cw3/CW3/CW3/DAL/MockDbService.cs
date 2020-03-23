using CW3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW3.DAL
{
    public class MockDbService : IDbService
    {
        private static /*IEnumerable*/ List<Student> _students;

        MockDbService()
        {
            _students= new List<Student>
            {
                new Student{IdStudent=1,FirstName="Vuko",LastName="Drakkainen",IndexNumber="s11111"},
                new Student{IdStudent=2,FirstName="Kris",LastName="Kelvin",IndexNumber="s11151"},
                new Student{IdStudent=3,FirstName="Zoltan",LastName="Chivay",IndexNumber="s1113"},
            };
        }

       

        public /*IEnumerable*/List<Student> GetStudents()
        {
           return _students;
        }
        
    }
}
