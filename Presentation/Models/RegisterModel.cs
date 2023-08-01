using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Nombre de usuario es requerido.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Formato de número de teléfono inválido.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contraseña es requerida.")]
        public string Password { get; set; } = string.Empty;
    }
}