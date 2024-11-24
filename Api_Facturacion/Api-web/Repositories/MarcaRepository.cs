using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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

        public void Add(Marca marca)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Marcas (nombre) 
                                 VALUES (@Nombre);
                                 SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Nombre", marca.Nombre);

                connection.Open();
                marca.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Marca marca)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Marcas 
                                 SET nombre = @Nombre
                                 WHERE id_marca = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Nombre", marca.Nombre);
                command.Parameters.AddWithValue("@Id", marca.Id);

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