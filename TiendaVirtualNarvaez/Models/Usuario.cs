using System.ComponentModel.DataAnnotations;

namespace TiendaVirtualNarvaez.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]

        // Validar correo
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|co|edu\.co|net|org)$",
            ErrorMessage = "Correo inválido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; }

        [Required(ErrorMessage = "El celular es obligatorio")]
        [RegularExpression(@"^3\d{9}$",
            ErrorMessage = "El celular debe estar entre 3000000000 y 3999999999")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Clave { get; set; }
    }
}
