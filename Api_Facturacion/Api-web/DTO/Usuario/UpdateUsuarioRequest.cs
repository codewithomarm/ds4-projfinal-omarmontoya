using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Usuario
{
    public class UpdateUsuarioRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres")]
        public string NombreUsuario { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Contrasena { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder los 100 caracteres")]
        public string NombreCompleto { get; set; }

        [Required]
        public string Rol { get; set; }

        [Required]
        public string Estado { get; set; }
    }
}