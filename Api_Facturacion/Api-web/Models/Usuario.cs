using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public string Estado { get; set; }

        public virtual Rol Rol { get; set; }
    }
}