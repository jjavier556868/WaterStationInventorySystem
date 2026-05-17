using InvSys.Domain.Models.Enums;
using System;

namespace InvSys.Services.DTOs
{
    public class AccountDisplayDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}