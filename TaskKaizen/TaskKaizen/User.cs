﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskKaizen
{
    internal class User
    {
        [Key]
        public int ID { get; set; }

        public string? NAME { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; }
    }
}
