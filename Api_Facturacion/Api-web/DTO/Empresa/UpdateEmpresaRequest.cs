using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Empresa
{
    public class UpdateEmpresaRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13)]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El RUC debe ser un número de 13 dígitos.")]
        public string RUC { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
}