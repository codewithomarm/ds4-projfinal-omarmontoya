using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Categoria
{
    public class UpdateCategoriaRequest
    {
        [Required(ErrorMessage = "El nombre de la categoría es requerido")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "El nombre de la categoría debe tener entre 3 y 50 caracteres")]
        public string Nombre { get; set; }
    }
}