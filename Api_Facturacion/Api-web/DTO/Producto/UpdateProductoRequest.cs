using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Producto
{
    public class UpdateProductoRequest
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del producto debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de la categoria debe tener entre 3 y 100 caracteres")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "La subcategoría es requerida")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de la subcategoria debe tener entre 3 y 100 caracteres")]
        public string Subcategoria { get; set; }

        [Required(ErrorMessage = "La marca es requerida")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de la marca debe tener entre 3 y 100 caracteres")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "La unidad de medida es requerida")]
        [StringLength(20, ErrorMessage = "La unidad de medida no puede exceder los 20 caracteres")]
        public string UnidadMedida { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El código de barras es requerido")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El código de barras debe tener 13 caracteres")]
        public string CodigoBarras { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public string Estado { get; set; }

        public string Foto { get; set; }
    }
}