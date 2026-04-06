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

<<<<<<< HEAD
       // El signo ? es la clave para que no sea obligatorio en el formulario
        public string? Estado { get; set; }

=======
<<<<<<< HEAD
        // El signo ? es la clave para que no sea obligatorio en el formulario
        public string? Estado { get; set; }
=======
        [Required(ErrorMessage = "Debe seleccionar un estado")]
        public string Estado { get; set; }
>>>>>>> eb6aa5aef1d4c1a475c61204ce7e404f9c460ba8
>>>>>>> 0fd856ef78af15b144221a17dfa67ec19f637d14
    }
}