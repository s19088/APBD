using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CW7.DTOs.Requests
{
    public class EnrollPromoteRequest
    {
        [Required]
        public string StudiesName { get; set; }
        [Required]
        public int Semester { get; set; }
    }
}
