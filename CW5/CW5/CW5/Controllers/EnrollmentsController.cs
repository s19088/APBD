using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using CW5.DTOs.Requests;
using CW5.DTOs.Responses;
using CW5.Models;
using Microsoft.AspNetCore.Mvc;

namespace CW5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.Studies = request.Studies;
            st.Semester = "1";
            st.LastName = request.LastName;

            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19088;Integrated Security=True"))
            using (var command = new SqlCommand())
            {

                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {


                    command.CommandText = "select IdStudies from Studies where name=@name";
                    command.Parameters.AddWithValue("name", request.Studies);

                    var dr = command.ExecuteReader();// z powodu transaction w linijce 30 ExecuteReader wyrzuca Exception, nie znalazłem sposobu by obejść 
                    // a na zajeciach czy wykladzie nie był wspomniany co z tym zrobić
                    //kod pisany zgodnie z tym co na wykładzie

                    if (!dr.Read())
                    {
                        transaction.Rollback();
                        return BadRequest("Studia nie istnieja.");
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
                    command.Parameters.AddWithValue("indexNumber", request.IndexNumber);

                    var studentread = command.ExecuteReader();

                    if (!studentread.Read())
                    {
                        transaction.Rollback();
                        return BadRequest("Index jest nieunikalny.");
                    }
                    command.CommandText = "Insert into Student values(" + request.IndexNumber + "," +request.FirstName + "," +  request.LastName + "," + request.BirthDate + "," +   idStudies +")";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }catch(SqlException ex){
                    transaction.Rollback();
                }



                var response = new EnrollStudentResponse();

                response.Semester = int.Parse(st.Semester);
                response.LastName = st.LastName;
                return Ok(response);//nie ma 201 spośród zwracanych IAResult. tylko ok, bad request, not found
            }
        }
        [HttpPost("{promotions}")]
        public IActionResult Promotions(PromotionRequest request)
        {
            string output = "";

            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19088;Integrated Security=True"))
            using (var command = new SqlCommand())
            {


                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    command.CommandText = "Execute PromoteStudents @nazwa, @semestr";
                    command.Parameters.AddWithValue("nazwa", request.Studies);
                    command.Parameters.AddWithValue("semestr", request.Semester);

                    var dr = command.ExecuteReader();//tu tak samo jak w poprzedniej metodzie

                    if (!dr.Read())
                    {
                        transaction.Rollback();
                        return NotFound();
                    }
                    int semestr = (int)dr["Semester"];
                    int idStudy = (int)dr["IdStudy"];
                    int IdEnrollment = (int)dr["IdEnrollment"];
                    System.DateTime date = (System.DateTime)dr["StartDate"];

                    transaction.Commit();
                    output = "Semestr: " + semestr + ", idStudy: " + idStudy + ", IdEnrollment: " + IdEnrollment + ", Data: " + date;

                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                }
            }
            return Ok(output);
        }
    }
}
    


