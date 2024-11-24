using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Api_web.DTO.Categoria;

namespace Api_web.Repositories
{
    public class CategoriaRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Categoria> GetAll()
        {
            List<Categoria> categorias = new List<Categoria>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Categorias";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categorias.Add(MapToCategoria(reader));
                }
            }

            return categorias;
        }

        public Categoria GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Categorias WHERE id_categoria = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapToCategoria(reader);
                }
            }

            return null;
        }

        public List<Categoria> GetByName(string name)
        {
            List<Categoria> categorias = new List<Categoria>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Categorias WHERE nombre LIKE @Name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categorias.Add(MapToCategoria(reader));
                }
            }

            return categorias;
        }

        public Categoria Add(CreateCategoriaRequest request)
        {
            Categoria nuevaCategoria = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO Categorias (nombre) 
                                 VALUES (@Nombre);
                                 SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);

                        int newId = Convert.ToInt32(command.ExecuteScalar());

                        nuevaCategoria = new Categoria
                        {
                            Id = newId,
                            Nombre = request.Nombre
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar la categoría: {ex.Message}");
                }
            }
            return nuevaCategoria;
        }

        public void Update(int id, UpdateCategoriaRequest request)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Categorias 
                                 SET nombre = @Nombre
                                 WHERE id_categoria = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Nombre", request.Nombre);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Categorias WHERE id_categoria = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private Categoria MapToCategoria(SqlDataReader reader)
        {
            return new Categoria
            {
                Id = Convert.ToInt32(reader["id_categoria"]),
                Nombre = reader["nombre"].ToString()
            };
        }
    }
}