﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CW7.DTOs.Responses
{
    public class EnrollPromoteResponse
    {
        [Required]
        public string StudiesName { get; set; }
        [Required]
        public int Semester { get; set; }
    }
}
