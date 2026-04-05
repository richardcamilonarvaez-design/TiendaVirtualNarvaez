using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TiendaVirtualNarvaez.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a 0")]
        public double Precio { get; set; }

<<<<<<< HEAD
        [Range(0, 1000, ErrorMessage = "El stock no puede ser un valor negativo")]
=======
        [Range(0, 1000, ErrorMessage = "El stock no puede ser negativo")]
>>>>>>> eb6aa5aef1d4c1a475c61204ce7e404f9c460ba8
        public int Stock { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida")]
        public int CategoriaId { get; set; }

        [ValidateNever] 
        public Categoria Categoria { get; set; }

        public double CalcularValorInventario()
        {
            return Precio * Stock;
        }

        public bool TieneStock()
        {
            return Stock > 0;
        }
    }
}