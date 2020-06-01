using CW7.DTOs.Requests;
using CW7.DTOs.Responses;
using CW7.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CW7.Services
{
    public class SqlServeStudentrDbService : IStudentDbService
    {
        public EnrollStudentResponse EnrollStudent(Student student)
        {
            EnrollStudentResponse enrollStudentResponse = null;
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19088;Integrated Security=True"))
            using (var command = new SqlCommand())
            {

                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;//naprawia blad z 38 linii
                try
                {


                    command.CommandText = "select IdStudies from Studies where name=@name";
                    command.Parameters.AddWithValue("name", student.Studies);

                    var dr = command.ExecuteReader();// z powodu transaction w linijce 30 ExecuteReader wyrzuca Exception, nie znalazłem sposobu by obejść 
                                                     // a na zajeciach czy wykladzie nie był wspomniany co z tym zrobić
                                                     //kod pisany zgodnie z tym co na wykładzie

                    if (!dr.Read())
                    {
                        transaction.Rollback();
                        return enrollStudentResponse;
                    }
                    int idStudies = (int)dr["IdStudies"];

                    command.CommandText = "select * from Enrollment " +
                        "where StartDate = (select MAX(StartDate) from Enrollment where idStudy=" + idStudies + ") " +
                        "AND Semester=1";

                    var execread = command.ExecuteReader();


                    if (!execread.Read())
                    {
                        command.CommandText = "Insert into Enrollment values (" +
                            "(SELECT MAX(idEnrollment)+1 from Enrollment),1," + idStudies + ",GetDate()";
                    }

                    command.CommandText = "Select * from Student WHERE IndexNumber=@indexNumber";
                    command.Parameters.AddWithValue("indexNumber", student.IndexNumber);

                    var studentread = command.ExecuteReader();

                    if (!studentread.Read())
                    {
                        transaction.Rollback();
                        return enrollStudentResponse;
                    }
                    command.CommandText = "Insert into Student values(" + student.IndexNumber + "," + student.FirstName + "," + student.LastName + "," + student.BirthDate + "," + idStudies + ")";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                }
                enrollStudentResponse = new EnrollStudentResponse();
                enrollStudentResponse.Semester = int.Parse(student.Semester);
                enrollStudentResponse.LastName = student.LastName;
            }
            return enrollStudentResponse;
        }

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
        public EnrollPromoteResponse Promote(EnrollPromoteRequest request)
        {
            EnrollPromoteResponse response = null;

            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19088;Integrated Security=True"))
            using (var command = new SqlCommand())
            {


                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();

                command.Transaction = transaction;

                try
                {
                    command.CommandText = "Execute PromoteStudents @nazwa, @semestr";
                    command.Parameters.AddWithValue("nazwa", request.StudiesName);
                    command.Parameters.AddWithValue("semestr", request.Semester);

                    var dr = command.ExecuteReader();//tu tak samo jak w poprzedniej metodzie

                    if (!dr.Read())
                    {
                        transaction.Rollback();
                        return response;
                    }
                    int semestr = (int)dr["Semester"];
                    int idStudy = (int)dr["IdStudy"];
                    int IdEnrollment = (int)dr["IdEnrollment"];
                    System.DateTime date = (System.DateTime)dr["StartDate"];

                    transaction.Commit();
                    response = new EnrollPromoteResponse();
                    response.Semester = semestr;
                    response.StudiesName = request.StudiesName;
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                }
                return response;
            }
        }


    }
}

