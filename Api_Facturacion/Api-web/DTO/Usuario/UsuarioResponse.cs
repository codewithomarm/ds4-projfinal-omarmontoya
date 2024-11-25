using Api_web.DTO;
using Api_web.DTO.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Usuario
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public RolResponse Rol { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public string Estado { get; set; }
    }
}