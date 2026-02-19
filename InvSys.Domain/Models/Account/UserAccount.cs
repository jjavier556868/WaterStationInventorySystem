using InvSys.Domain.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace InvSys.Domain.Models.Account
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(256)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}