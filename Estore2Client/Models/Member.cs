﻿using System;
using System.Collections.Generic;

namespace Estore2Client.Models
{
    public partial class Member
    {
        public int MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Password { get; set; } = null!;
    }
}
