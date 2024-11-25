using Api_web.DTO.Marca;
using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Api_web.Repositories
{
    public class MarcaRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Marca> GetAll()
        {
            List<Marca> marcas = new List<Marca>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Marcas";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    marcas.Add(MapToMarca(reader));
                }
            }

            return marcas;
        }

        public Marca GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Marcas WHERE id_marca = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapToMarca(reader);
                }
            }

            return null;
        }

        public List<Marca> GetByName(string name)
        {
            List<Marca> marcas = new List<Marca>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Marcas WHERE nombre LIKE @Name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    marcas.Add(MapToMarca(reader));
                }
            }

            return marcas;
        }

        public Marca Add(CreateMarcaRequest request)
        {
            Marca nuevaMarca = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO Marcas (nombre) 
                                     VALUES (@Nombre);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);

                        int newId = Convert.ToInt32(command.ExecuteScalar());

                        nuevaMarca = new Marca
                        {
                            Id = newId,
                            Nombre = request.Nombre
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar la marca: {ex.Message}");
                }
            }
            return nuevaMarca;
        }

        public void Update(int id, UpdateMarcaRequest request)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Marcas 
                                SET nombre = @Nombre
                                WHERE id_marca = @Id";
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
                string query = "DELETE FROM Marcas WHERE id_marca = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private Marca MapToMarca(SqlDataReader reader)
        {
            return new Marca
            {
                Id = Convert.ToInt32(reader["id_marca"]),
                Nombre = reader["nombre"].ToString()
            };
        }
    }
}