﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCH.Models.Students.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? SSN { get; set; }

        public string? Image { get; set; }

        public DateTime? StartDate { get; set; }

        public bool IsActive { get; set; }
    }
}
