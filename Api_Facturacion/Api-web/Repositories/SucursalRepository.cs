using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Api_web.Repositories
{
    public class SucursalRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Sucursal> GetAll()
        {
            List<Sucursal> sucursales = new List<Sucursal>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT Id, Nombre, Provincia, Distrito, Corregimiento, Urbanizacion, Calle, Local, EmpresaId 
                             FROM Sucursales";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        sucursales.Add(new Sucursal
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Provincia = reader["Provincia"].ToString(),
                            Distrito = reader["Distrito"].ToString(),
                            Corregimiento = reader["Corregimiento"].ToString(),
                            Urbanizacion = reader["Urbanizacion"].ToString(),
                            Calle = reader["Calle"].ToString(),
                            Local = reader["Local"] as string,
                            EmpresaId = Convert.ToInt32(reader["EmpresaId"])
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception("Error al obtener todas las sucursales", ex);
                }
            }
            return sucursales;
        }

        public Sucursal GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT Id, Nombre, Provincia, Distrito, Corregimiento, Urbanizacion, Calle, Local, EmpresaId 
                             FROM Sucursales WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Sucursal
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Provincia = reader["Provincia"].ToString(),
                            Distrito = reader["Distrito"].ToString(),
                            Corregimiento = reader["Corregimiento"].ToString(),
                            Urbanizacion = reader["Urbanizacion"].ToString(),
                            Calle = reader["Calle"].ToString(),
                            Local = reader["Local"] as string,
                            EmpresaId = Convert.ToInt32(reader["EmpresaId"])
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al obtener la sucursal con Id {id}", ex);
                }
            }
            return null;
        }

        public void Add(Sucursal sucursal)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Sucursales (Nombre, Provincia, Distrito, Corregimiento, Urbanizacion, Calle, Local, EmpresaId) 
                             VALUES (@Nombre, @Provincia, @Distrito, @Corregimiento, @Urbanizacion, @Calle, @Local, @EmpresaId);
                             SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                command.Parameters.AddWithValue("@Provincia", sucursal.Provincia);
                command.Parameters.AddWithValue("@Distrito", sucursal.Distrito);
                command.Parameters.AddWithValue("@Corregimiento", sucursal.Corregimiento);
                command.Parameters.AddWithValue("@Urbanizacion", sucursal.Urbanizacion);
                command.Parameters.AddWithValue("@Calle", sucursal.Calle);
                command.Parameters.AddWithValue("@Local", (object)sucursal.Local ?? DBNull.Value);
                command.Parameters.AddWithValue("@EmpresaId", sucursal.EmpresaId);

                try
                {
                    connection.Open();
                    sucursal.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception("Error al agregar la sucursal", ex);
                }
            }
        }

        public void Update(Sucursal sucursal)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Sucursales 
                             SET Nombre = @Nombre, Provincia = @Provincia, Distrito = @Distrito, 
                                 Corregimiento = @Corregimiento, Urbanizacion = @Urbanizacion, 
                                 Calle = @Calle, Local = @Local, EmpresaId = @EmpresaId 
                             WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", sucursal.Id);
                command.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                command.Parameters.AddWithValue("@Provincia", sucursal.Provincia);
                command.Parameters.AddWithValue("@Distrito", sucursal.Distrito);
                command.Parameters.AddWithValue("@Corregimiento", sucursal.Corregimiento);
                command.Parameters.AddWithValue("@Urbanizacion", sucursal.Urbanizacion);
                command.Parameters.AddWithValue("@Calle", sucursal.Calle);
                command.Parameters.AddWithValue("@Local", (object)sucursal.Local ?? DBNull.Value);
                command.Parameters.AddWithValue("@EmpresaId", sucursal.EmpresaId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No se encontró la sucursal con Id {sucursal.Id}");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al actualizar la sucursal con Id {sucursal.Id}", ex);
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Sucursales WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No se encontró la sucursal con Id {id}");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al eliminar la sucursal con Id {id}", ex);
                }
            }
        }
    }
}