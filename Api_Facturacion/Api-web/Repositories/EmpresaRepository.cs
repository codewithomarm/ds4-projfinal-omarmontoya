﻿using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Api_web.Repositories
{
    public class EmpresaRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Empresa> GetAll()
        {
            List<Empresa> empresas = new List<Empresa>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, RUC, Nombre FROM Empresas";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        empresas.Add(new Empresa
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            RUC = reader["RUC"].ToString(),
                            Nombre = reader["Nombre"].ToString()
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception("Error al obtener todas las empresas", ex);
                }
            }
            return empresas;
        }

        public Empresa GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, RUC, Nombre FROM Empresas WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Empresa
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            RUC = reader["RUC"].ToString(),
                            Nombre = reader["Nombre"].ToString()
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al obtener la empresa con Id {id}", ex);
                }
            }
            return null;
        }

        public void Add(Empresa empresa)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Empresas (RUC, Nombre) VALUES (@RUC, @Nombre); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RUC", empresa.RUC);
                command.Parameters.AddWithValue("@Nombre", empresa.Nombre);

                try
                {
                    connection.Open();
                    empresa.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception("Error al agregar la empresa", ex);
                }
            }
        }

        public void Update(Empresa empresa)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Empresas SET RUC = @RUC, Nombre = @Nombre WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", empresa.Id);
                command.Parameters.AddWithValue("@RUC", empresa.RUC);
                command.Parameters.AddWithValue("@Nombre", empresa.Nombre);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No se encontró la empresa con Id {empresa.Id}");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al actualizar la empresa con Id {empresa.Id}", ex);
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Empresas WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No se encontró la empresa con Id {id}");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al eliminar la empresa con Id {id}", ex);
                }
            }
        }
    }
}