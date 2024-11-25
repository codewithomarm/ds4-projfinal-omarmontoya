using Api_web.DTO.Usuario;
using Api_web.Models;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Api_web.Repositories
{
    public class UsuarioRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuarios";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(MapToUsuario(reader));
                }
            }

            return usuarios;
        }

        public Usuario GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE id_usuario = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapToUsuario(reader);
                }
            }

            return null;
        }

        public Usuario Add(CreateUsuarioRequest request, int rolId)
        {
            Usuario nuevoUsuario = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO Usuarios (nombre_usuario, contrasena, nombre_completo, id_rol, fecha_creacion, estado) 
                                     VALUES (@NombreUsuario, @Contrasena, @NombreCompleto, @RolId, @FechaCreacion, @Estado);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NombreUsuario", request.NombreUsuario);
                        command.Parameters.AddWithValue("@Contrasena", HashPassword(request.Contrasena));
                        command.Parameters.AddWithValue("@NombreCompleto", request.NombreCompleto);
                        command.Parameters.AddWithValue("@RolId", rolId);
                        command.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                        command.Parameters.AddWithValue("@Estado", "activo");

                        int newId = Convert.ToInt32(command.ExecuteScalar());

                        nuevoUsuario = new Usuario
                        {
                            Id = newId,
                            NombreUsuario = request.NombreUsuario,
                            Contrasena = request.Contrasena, // Note: We're not storing the hashed password in the object
                            NombreCompleto = request.NombreCompleto,
                            RolId = rolId,
                            FechaCreacion = DateTime.Now,
                            Estado = "activo"
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar el usuario: {ex.Message}");
                }
            }
            return nuevoUsuario;
        }

        public void Update(int id, UpdateUsuarioRequest request, int rolId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"UPDATE Usuarios 
                                     SET nombre_usuario = @NombreUsuario, 
                                         nombre_completo = @NombreCompleto, 
                                         id_rol = @RolId, 
                                         estado = @Estado";

                    if (!string.IsNullOrEmpty(request.Contrasena))
                    {
                        query += ", contrasena = @Contrasena";
                    }

                    query += " WHERE id_usuario = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NombreUsuario", request.NombreUsuario);
                        command.Parameters.AddWithValue("@NombreCompleto", request.NombreCompleto);
                        command.Parameters.AddWithValue("@RolId", rolId);
                        command.Parameters.AddWithValue("@Estado", request.Estado);
                        command.Parameters.AddWithValue("@Id", id);

                        if (!string.IsNullOrEmpty(request.Contrasena))
                        {
                            command.Parameters.AddWithValue("@Contrasena", HashPassword(request.Contrasena));
                        }

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
                    throw;
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM Usuarios WHERE id_usuario = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                    throw;
                }
            }
        }

        public void UpdateLastAccess(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Usuarios SET ultimo_acceso = @UltimoAcceso WHERE id_usuario = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UltimoAcceso", DateTime.Now);
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar el último acceso del usuario: {ex.Message}");
                    throw;
                }
            }
        }

        private Usuario MapToUsuario(SqlDataReader reader)
        {
            return new Usuario
            {
                Id = Convert.ToInt32(reader["id_usuario"]),
                NombreUsuario = reader["nombre_usuario"].ToString(),
                Contrasena = reader["contrasena"].ToString(),
                NombreCompleto = reader["nombre_completo"].ToString(),
                RolId = Convert.ToInt32(reader["id_rol"]),
                FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
                UltimoAcceso = reader["ultimo_acceso"] != DBNull.Value ? Convert.ToDateTime(reader["ultimo_acceso"]) : (DateTime?)null,
                Estado = reader["estado"].ToString()
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}