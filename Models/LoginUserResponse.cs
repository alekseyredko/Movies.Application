﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public class LoginUserResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
    }
}
