using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CW7.Models;
using CW7.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CW7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentDbService _service;
        public StudentsController(IStudentDbService service)
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