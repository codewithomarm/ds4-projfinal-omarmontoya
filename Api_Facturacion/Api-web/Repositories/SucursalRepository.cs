using Api_web.DTO.Sucursal;
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

        public Sucursal GetByNombre(string nombre)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre, Provincia, Distrito, Corregimiento, Urbanizacion, Calle, Local, EmpresaId FROM Sucursales WHERE Nombre = @Nombre";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", nombre);

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
                    throw new Exception($"Error al obtener la sucursal con nombre {nombre}", ex);
                }
            }
            return null;
        }

        public List<Sucursal> GetSucursalesByEmpresaId(int empresaId)
        {
            List<Sucursal> sucursales = new List<Sucursal>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT s.Id, s.Nombre, s.Provincia, s.Distrito, s.Corregimiento, s.Urbanizacion, s.Calle, s.Local, " +
                    "e.Id AS EmpresaId, e.RUC, e.Nombre AS EmpresaNombre " +
                    "FROM Sucursales s " +
                    "INNER JOIN Empresas e ON s.EmpresaId = e.Id " +
                    "WHERE s.EmpresaId = @EmpresaId", connection);

                command.Parameters.AddWithValue("@EmpresaId", empresaId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sucursales.Add(new Sucursal
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            Provincia = reader["Provincia"].ToString(),
                            Distrito = reader["Distrito"].ToString(),
                            Corregimiento = reader["Corregimiento"].ToString(),
                            Urbanizacion = reader["Urbanizacion"].ToString(),
                            Calle = reader["Calle"].ToString(),
                            Local = reader["Local"].ToString(),
                            EmpresaId = (int)reader["EmpresaId"]
                        });
                    }
                }
            }

            return sucursales;
        }

        public Sucursal Add(CreateSucursalRequest sucursalRequest, int empresaId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Sucursales (Nombre, Provincia, Distrito, Corregimiento, Urbanizacion, Calle, Local, EmpresaId) 
                         VALUES (@Nombre, @Provincia, @Distrito, @Corregimiento, @Urbanizacion, @Calle, @Local, @EmpresaId);
                         SELECT CAST(SCOPE_IDENTITY() as int)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", sucursalRequest.Nombre);
                command.Parameters.AddWithValue("@Provincia", sucursalRequest.Provincia);
                command.Parameters.AddWithValue("@Distrito", sucursalRequest.Distrito);
                command.Parameters.AddWithValue("@Corregimiento", sucursalRequest.Corregimiento);
                command.Parameters.AddWithValue("@Urbanizacion", sucursalRequest.Urbanizacion);
                command.Parameters.AddWithValue("@Calle", sucursalRequest.Calle);
                command.Parameters.AddWithValue("@Local", (object)sucursalRequest.Local ?? DBNull.Value);
                command.Parameters.AddWithValue("@EmpresaId", empresaId);

                try
                {
                    connection.Open();
                    int id = (int)command.ExecuteScalar();

                    return new Sucursal
                    {
                        Id = id,
                        Nombre = sucursalRequest.Nombre,
                        Provincia = sucursalRequest.Provincia,
                        Distrito = sucursalRequest.Distrito,
                        Corregimiento = sucursalRequest.Corregimiento,
                        Urbanizacion = sucursalRequest.Urbanizacion,
                        Calle = sucursalRequest.Calle,
                        Local = sucursalRequest.Local,
                        EmpresaId = empresaId
                    };
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception("Error al agregar la sucursal", ex);
                }
            }
        }

        public void Update(UpdateSucursalRequest sucursalRequest, int empresaId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Sucursales 
                         SET Nombre = @Nombre, Provincia = @Provincia, Distrito = @Distrito, 
                             Corregimiento = @Corregimiento, Urbanizacion = @Urbanizacion, 
                             Calle = @Calle, Local = @Local, EmpresaId = @EmpresaId 
                         WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", sucursalRequest.Id);
                command.Parameters.AddWithValue("@Nombre", sucursalRequest.Nombre);
                command.Parameters.AddWithValue("@Provincia", sucursalRequest.Provincia);
                command.Parameters.AddWithValue("@Distrito", sucursalRequest.Distrito);
                command.Parameters.AddWithValue("@Corregimiento", sucursalRequest.Corregimiento);
                command.Parameters.AddWithValue("@Urbanizacion", sucursalRequest.Urbanizacion);
                command.Parameters.AddWithValue("@Calle", sucursalRequest.Calle);
                command.Parameters.AddWithValue("@Local", (object)sucursalRequest.Local ?? DBNull.Value);
                command.Parameters.AddWithValue("@EmpresaId", empresaId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No se encontró la sucursal con Id {sucursalRequest.Id}");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al actualizar la sucursal con Id {sucursalRequest.Id}", ex);
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