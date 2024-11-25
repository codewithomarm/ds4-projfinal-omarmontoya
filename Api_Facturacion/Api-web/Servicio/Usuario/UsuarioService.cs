using Api_web.DTO.Usuario;
using Api_web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly RolRepository _rolRepository;

        public UsuarioService()
        {
            _usuarioRepository = new UsuarioRepository();
            _rolRepository = new RolRepository();
        }

        public List<UsuarioResponse> GetAll()
        {
            var usuarios = _usuarioRepository.GetAll();
            return usuarios.Select(MapToUsuarioResponse).ToList();
        }

        public UsuarioResponse GetById(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            return usuario != null ? MapToUsuarioResponse(usuario) : null;
        }

        public UsuarioResponse GetByNombreUsuario(string name)
        {
            var usuarios = _usuarioRepository.GetAll();
            var usuario = usuarios.FirstOrDefault(u => u.NombreUsuario.Equals(name, StringComparison.OrdinalIgnoreCase));
            return usuario != null ? MapToUsuarioResponse(usuario) : null;
        }

        public UsuarioResponse Create(CreateUsuarioRequest request)
        {
            var rol = _rolRepository.GetByNombre(request.Rol);
            if (rol == null)
            {
                throw new ArgumentException("El rol especificado no existe.");
            }

            var nuevoUsuario = _usuarioRepository.Add(request, rol.Id);
            return MapToUsuarioResponse(nuevoUsuario);
        }

        public void Update(int id, UpdateUsuarioRequest request)
        {
            var rol = _rolRepository.GetByNombre(request.Rol);
            if (rol == null)
            {
                throw new ArgumentException("El rol especificado no existe.");
            }

            _usuarioRepository.Update(id, request, rol.Id);
        }

        public void Delete(int id)
        {
            _usuarioRepository.Delete(id);
        }

        public void UpdateLastAccess(int id)
        {
            _usuarioRepository.UpdateLastAccess(id);
        }

        public bool VerifyPassword(string username, string password)
        {
            var usuarios = _usuarioRepository.GetAll();
            var usuario = usuarios.FirstOrDefault(u => u.NombreUsuario == username);

            if (usuario == null)
            {
                return false;
            }

            return _usuarioRepository.VerifyPassword(password, usuario.Contrasena);
        }

        private UsuarioResponse MapToUsuarioResponse(Models.Usuario usuario)
        {
            var rol = _rolRepository.GetById(usuario.RolId);
            return new UsuarioResponse
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Rol = new DTO.Rol.RolResponse
                { 
                    Id = rol.Id,
                    Nombre = rol.Nombre
                },
                FechaCreacion = usuario.FechaCreacion,
                UltimoAcceso = usuario.UltimoAcceso,
                Estado = usuario.Estado
            };
        }
    }
}