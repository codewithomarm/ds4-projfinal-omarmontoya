using Api_web.DTO.Empresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Sucursal
{
    public class SucursalResponse
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Provincia { get; set; }

        [Required]
        [StringLength(50)]
        public string Distrito { get; set; }

        [Required]
        [StringLength(50)]
        public string Corregimiento { get; set; }

        [Required]
        [StringLength(100)]
        public string Urbanizacion { get; set; }

        [Required]
        [StringLength(100)]
        public string Calle { get; set; }

        [StringLength(50)]
        public string Local { get; set; }

        [Required]
        public EmpresaResponse Empresa { get; set; }
    }
}