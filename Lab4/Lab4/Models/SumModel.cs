using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class SumModel
    {
        [Required]
        public int X { get; set; }

        [Required]
        public int Y { get; set; }
    }
}
