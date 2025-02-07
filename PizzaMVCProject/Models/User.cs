﻿using Microsoft.AspNetCore.Identity;

namespace PizzaMVCProject.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
