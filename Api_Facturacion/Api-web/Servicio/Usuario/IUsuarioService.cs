using Api_web.DTO.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_web.Servicio.Usuario
{
    internal interface IUsuarioService
    {
        List<UsuarioResponse> GetAll();
        UsuarioResponse GetById(int id);
        UsuarioResponse GetByNombreUsuario(string name);
        UsuarioResponse Create(CreateUsuarioRequest request);
        void Update(int id, UpdateUsuarioRequest request);
        void Delete(int id);
        void UpdateLastAccess(int id);
        bool VerifyPassword(string username, string password);
    }
}
