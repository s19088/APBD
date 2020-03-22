using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CW3.DAL;
using CW3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CW3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDbService _dbService;
       public StudentController(IDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpGet]
        public /*string*/ IActionResult GetStudent(string orderBy)
        {
            //return $"Bartek, Kuba Marta Olgi sort={orderBy}";
            return Ok(_dbService.GetStudents());
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {

            /*if (id==1 || id==3)
            {
                return Ok("Kuba");
            }else if (id > 3)
            {
                return Unauthorized("bartek");
            }
            else
            {
                return NotFound("WRRR");
            }*/
            foreach(Student student in _dbService.GetStudents()){
                if (student.IdStudent ==student.IdStudent)
                {
                    return Ok(student);
                }
                else
                {
                    return NotFound("nie znaleziono studenta o podanym id");
                }
            }
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"S{new Random().Next(1, 2000)}";
            _dbService.AddStudent(student);
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult CreateStudent(int id)
        {
            /*
            if (student.IdStudent == id)
            {
                student.IndexNumber = $"S{new Random().Next(1, 2000)}";
                return Ok(" Ąktualizacja dokończona");// zakładam ,ze w przypadku tej apki put ma aktualizowac studentow wiec szuka istniejacych jesli sa OK jesli nie notfound
            }
            else
                return NotFound("nima");*/
            foreach (Student student in _dbService.GetStudents())
            {
                if (student.IdStudent == student.IdStudent)
                {
                    student.IndexNumber = $"S{new Random().Next(1, 2000)}";// aktuali
                    return Ok(" Ąktualizacja dokończona");// zakładam ,ze w przypadku tej apki put ma aktualizowac studentow wiec szuka istniejacych jesli sa OK jesli nie
                }
                else
                {
                    return NotFound("nie znaleziono studenta o podanym id");
                }
            }
        }
        [HttpDelete("{id}")]

        public IActionResult DeleteStudent(int id) //Student student)
        {
            /*
            if (student.IdStudent == id)
            {
                student = null;
                return Ok(" Usuwanie ukończone");
            }
            else
                return NotFound("nima");
        }*/
            foreach (Student student in _dbService.GetStudents())
            {
                if (student.IdStudent == id)
                {
                    student = null;
                    return Ok(" Usuwanie ukończone");
                }
                else
                {
                    return NotFound("nie znaleziono studenta o podanym id");
                }
            }
        }
}