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

        public string Nombre { get; set; }

        public string Provincia { get; set; }

        public string Distrito { get; set; }

        public string Corregimiento { get; set; }

        public string Urbanizacion { get; set; }

        public string Calle { get; set; }

        public string Local { get; set; }

        public EmpresaResponse Empresa { get; set; }
    }
}