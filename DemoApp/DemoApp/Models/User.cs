﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Address { get; set; }
        public string ProfilePic { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Brush Color { get; set; }
    }

}
