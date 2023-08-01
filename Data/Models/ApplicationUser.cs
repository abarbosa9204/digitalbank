using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        [Required]
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Telefono { get; set; }
    }
}