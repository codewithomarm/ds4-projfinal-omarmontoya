using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Empresa
{
    public class EmpresaResponse
    {
        public int Id { get; set; }
        public string RUC { get; set; }
        public string Nombre { get; set; }
    }
}