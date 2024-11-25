using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Api_web.Repositories
{
    public class RolRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Rol> GetAll()
        {
            List<Rol> roles = new List<Rol>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Roles";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                roles.Add(MapToRol(reader));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener todos los roles: {ex.Message}");
                    throw;
                }
            }

            return roles;
        }

        public Rol GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Roles WHERE id_rol = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapToRol(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener el rol por ID: {ex.Message}");
                    throw;
                }
            }

            return null;
        }

        public Rol GetByNombre(string nombre)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Roles WHERE nombre = @Nombre";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapToRol(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener el rol por nombre: {ex.Message}");
                    throw;
                }
            }

            return null;
        }

        private Rol MapToRol(SqlDataReader reader)
        {
            return new Rol
            {
                Id = Convert.ToInt32(reader["id_rol"]),
                Nombre = reader["nombre"].ToString()
            };
        }
    }
}