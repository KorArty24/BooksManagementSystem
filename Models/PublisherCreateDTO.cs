﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCorePublisherWebAPI.Models
{
    public class PublisherCreateDTO
    {
        [Required(ErrorMessage = "You must enter a name.")]
        [MaxLength(50)]
        public string Name { get; set; }
        public int Established { get; set; }
        

        
    }
}
