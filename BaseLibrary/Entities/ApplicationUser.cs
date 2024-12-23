﻿
using BaseLibrary.DTOs;

namespace BaseLibrary.Entities
{
    public class ApplicationUser : AuditDTO
    {
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
