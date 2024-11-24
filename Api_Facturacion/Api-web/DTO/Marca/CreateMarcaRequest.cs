using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Marca
{
    public class CreateMarcaRequest
    {
        [Required(ErrorMessage = "El nombre de la marca es requerido")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "El nombre de la marca debe tener entre 3 y 50 caracteres")]
        public string Nombre { get; set; }
    }
}