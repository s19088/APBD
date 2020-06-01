using CW7.DTOs.Requests;
using CW7.DTOs.Responses;
using CW7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW7.Services
{
    public interface IStudentDbService
    {
        public Student GetStudent(string Index);
        public List<Student> GetStudents();
        public EnrollStudentResponse EnrollStudent(Student student);
        public EnrollPromoteResponse Promote(EnrollPromoteRequest request);

    }
}