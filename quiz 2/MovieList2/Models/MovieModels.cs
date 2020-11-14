using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieList2.Models
{
    public class MovieModels
    {
        public int id { get; set; }
        [DisplayName("Movie Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string Director { get; set; }
    }
}