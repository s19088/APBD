using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CW7.DTOs.Requests;
using CW7.DTOs.Responses;
using CW7.Models;
using CW7.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CW7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EnrollmentsController : ControllerBase
    {
        IStudentDbService _service;
        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;

        }
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.Studies = request.Studies;
            st.Semester = "1";
            st.LastName = request.LastName;
            st.Studies = request.Studies;

            EnrollStudentResponse response = _service.EnrollStudent(st);
            if (response != null)
                return Ok(response);//nie ma 201 spośród zwracanych IAResult. tylko ok, bad request, not found
            else
                return BadRequest("bad request!!");
        }

        [HttpPost("{promotions}")]
        public IActionResult EnrollPromote(EnrollPromoteRequest request)
        {
            EnrollPromoteResponse output = _service.Promote(request);

                if (output != null)
                return Ok(output);
            else
                return BadRequest("Bad request!!!");
        }
    } }
    