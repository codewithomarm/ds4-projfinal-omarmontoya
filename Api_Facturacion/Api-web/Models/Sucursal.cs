using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Models
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Corregimiento { get; set; }
        public string Urbanizacion { get; set; }
        public string Calle { get; set; }
        public string Local { get; set; }
        public int EmpresaId { get; set; }
    }
}