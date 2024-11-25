using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Subcategoria
{
    public class UpdateSubcategoriaRequest
    {
        [Required(ErrorMessage = "El nombre de la subcategoria es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de la subcategoria debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El nombre de la categoria es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de la categoria debe tener entre 3 y 100 caracteres")]
        public string Categoria { get; set; }
    }
}