using Api_web.DTO.Subcategoria;
using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Api_web.Repositories
{
    public class SubcategoriaRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Subcategoria> GetAll()
        {
            List<Subcategoria> subcategorias = new List<Subcategoria>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Subcategorias";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    subcategorias.Add(MapToSubcategoria(reader));
                }
            }

            return subcategorias;
        }

        public Subcategoria GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Subcategorias WHERE id_subcategoria = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapToSubcategoria(reader);
                }
            }

            return null;
        }

        public Subcategoria Add(CreateSubcategoriaRequest request, int categoriaId)
        {
            Subcategoria nuevaSubcategoria = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO Subcategorias (nombre, id_categoria) 
                                     VALUES (@Nombre, @CategoriaId);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@CategoriaId", categoriaId);

                        int newId = Convert.ToInt32(command.ExecuteScalar());

                        nuevaSubcategoria = new Subcategoria
                        {
                            Id = newId,
                            Nombre = request.Nombre,
                            CategoriaId = categoriaId
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar la subcategoría: {ex.Message}");
                }
            }
            return nuevaSubcategoria;
        }

        public void Update(int id, UpdateSubcategoriaRequest request, int categoriaId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"UPDATE Subcategorias 
                                     SET nombre = @Nombre, id_categoria = @CategoriaId
                                     WHERE id_subcategoria = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@CategoriaId", categoriaId);
                        command.Parameters.AddWithValue("@Id", id);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar la subcategoría: {ex.Message}");
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

                    string query = "DELETE FROM Subcategorias WHERE id_subcategoria = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar la subcategoría: {ex.Message}");
                    throw;
                }
            }
        }

        public List<Subcategoria> GetByCategoriaId(int categoriaId)
        {
            List<Subcategoria> subcategorias = new List<Subcategoria>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Subcategorias WHERE id_categoria = @CategoriaId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoriaId", categoriaId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    subcategorias.Add(MapToSubcategoria(reader));
                }
            }

            return subcategorias;
        }

        private Subcategoria MapToSubcategoria(SqlDataReader reader)
        {
            return new Subcategoria
            {
                Id = Convert.ToInt32(reader["id_subcategoria"]),
                Nombre = reader["nombre"].ToString(),
                CategoriaId = Convert.ToInt32(reader["id_categoria"])
            };
        }
    }
}