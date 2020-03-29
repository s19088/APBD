using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw4.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {

            using (var client=new SqlConnection("Data Source=db-mssql;Initial Catalog=s19088;Integrated Security=True"))
            using(var com=new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = "Select IndexNumber,FirstName,LastName,BirthDate,Semester,Name from Studies inner join enrollment on studies.idStudy=enrollment.idStudy" +
                    " inner join Student on enrollment.idEnrollment=student.idEnrollment";
                client.Open();
                var dr = com.ExecuteReader();
                String students = "";
                while(dr.Read())
               {
                    Student student = new Student();
                    student.IndexNumber = dr["IndexNumber"].ToString();
                    student.FirstName = dr["FirstName"].ToString();
                    student.LastName = dr["LastName"].ToString();
                    student.BirthDate = dr["BirthDate"].ToString();
                    student.Semester = dr["Semester"].ToString() + " Semestr";
                    student.Studies = dr["Name"].ToString();

                    students =students+ student.ToString() + "\n";

               }
                return Ok(students);
            }
            

            
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            using(var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19088;Integrated Security=True"))
                using(var com=new SqlCommand())
            {
                com.Connection = client;
                string SqlId = "'" + id + "'";//SQLINjection https://localhost:5001/api/students/s101';Drop%20Table%20Student--  takim zapytaniem usunalem sobie tabele w sumie mozna by cala baze
                com.CommandText = "Select * from Studies inner join enrollment on studies.idStudy=enrollment.idStudy" +
                    " inner join Student on enrollment.idEnrollment=student.idEnrollment where IndexNumber=" + SqlId;
               

                client.Open();
                var dr = com.ExecuteReader();
                dr.Read();
                Student student = new Student();
                student.IndexNumber = dr["IndexNumber"].ToString();
                student.FirstName = dr["FirstName"].ToString();
                student.LastName = dr["LastName"].ToString();
                student.BirthDate = dr["BirthDate"].ToString();
                student.Semester = dr["Semester"].ToString() + " Semestr";
                student.Studies = dr["Name"].ToString();

                return Ok(student.ToString());
            }

        }
    }
}