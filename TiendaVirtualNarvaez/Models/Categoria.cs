namespace TiendaVirtualNarvaez.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Descripcion { get; set; }

        // El signo ? es la clave para que no sea obligatorio en el formulario
        public string? Estado { get; set; }
    }
}
