using System;
using System.Collections.Generic;

namespace Demo_PRN231.Models
{
    public partial class User
    {
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Gender { get; set; }
        public string? Address { get; set; }
    }
}
