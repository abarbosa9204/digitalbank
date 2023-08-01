using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string Nombre { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [Required, StringLength(1)]
        public string Sexo { get; set; } = "";

        [Required]
        public bool Estado { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? FechaModificacion { get; set; }
    }
}
