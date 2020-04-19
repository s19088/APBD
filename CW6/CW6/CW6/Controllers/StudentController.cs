using CW6.Models;
using CW6.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW6.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController:ControllerBase
    {
        private IStudentDbService _service;
        public StudentController(IStudentDbService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            List<Student> list = _service.GetStudents();

            if (!list.Any()) return NotFound("NOT FOUND!!!"); //jesli lista pusta

            return Ok(list);
        }
    }
}
