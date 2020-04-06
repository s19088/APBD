using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CW5.DTOs.Requests
{
    public class PromotionRequest
    {
        [Required]
        public string Studies { get; set; }
        [Required]
        [Range(1, 9)]
        public int Semester { get; set; }
    }
}
