using CW6.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CW6.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        public Student GetStudent(string Index)
        {
            Student student = null;
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19088;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                command.CommandText = "Select IndexNumber,FirstName,LastName,BirthDate,Semester,Name from Studies inner join enrollment on studies.idStudy=enrollment.idStudy" +
                    " inner join Student on enrollment.idEnrollment=student.idEnrollment where indexnumber=@index";
                command.Parameters.AddWithValue("index", Index);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    student = new Student();
                    student.IndexNumber = dr["IndexNumber"].ToString();
                    student.FirstName = dr["FirstName"].ToString();
                    student.LastName = dr["LastName"].ToString();
                    student.BirthDate = dr["BirthDate"].ToString();
                    student.Semester = dr["Semester"].ToString();
                    student.Studies = dr["Name"].ToString();
                    Console.Out.WriteLine(student.ToString());
                }
                dr.Close();





            }


            return student;
        }

        public List<Student> GetStudents()
        {
            List<Student> list = new List<Student>();
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19088;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = "Select IndexNumber,FirstName,LastName,BirthDate,Semester,Name from Studies inner join enrollment on studies.idStudy=enrollment.idStudy" +
                    " inner join Student on enrollment.idEnrollment=student.idEnrollment";
                client.Open();
                var dr = com.ExecuteReader();

                while (dr.Read())
                {
                    Student student = new Student();
                    student.IndexNumber = dr["IndexNumber"].ToString();
                    student.FirstName = dr["FirstName"].ToString();
                    student.LastName = dr["LastName"].ToString();
                    student.BirthDate = dr["BirthDate"].ToString();
                    student.Semester = dr["Semester"].ToString() + " Semestr";
                    student.Studies = dr["Name"].ToString();
                    list.Add(student);


                }
            }
            return list;
        }
    }
}

