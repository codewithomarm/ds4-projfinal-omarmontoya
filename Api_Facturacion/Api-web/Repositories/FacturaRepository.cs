using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Api_web.Repositories
{
    public class FacturaRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Factura> GetAll()
        {
            List<Factura> facturas = new List<Factura>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT Id, EmpresaId, SucursalId, Fecha, Hora, NumeroFactura, Subtotal, Impuesto, Descuento, Total 
                             FROM Facturas";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Factura factura = new Factura
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            EmpresaId = Convert.ToInt32(reader["EmpresaId"]),
                            SucursalId = Convert.ToInt32(reader["SucursalId"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Hora = (TimeSpan)reader["Hora"],
                            NumeroFactura = reader["NumeroFactura"].ToString(),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                            Impuesto = Convert.ToDecimal(reader["Impuesto"]),
                            Descuento = Convert.ToDecimal(reader["Descuento"]),
                            Total = Convert.ToDecimal(reader["Total"])
                        };
                        factura.Productos = GetFacturaProductos(factura.Id);
                        facturas.Add(factura);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception("Error al obtener todas las facturas", ex);
                }
            }
            return facturas;
        }

        public Factura GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT Id, EmpresaId, SucursalId, Fecha, Hora, NumeroFactura, Subtotal, Impuesto, Descuento, Total 
                             FROM Facturas WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Factura factura = new Factura
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            EmpresaId = Convert.ToInt32(reader["EmpresaId"]),
                            SucursalId = Convert.ToInt32(reader["SucursalId"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Hora = (TimeSpan)reader["Hora"],
                            NumeroFactura = reader["NumeroFactura"].ToString(),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                            Impuesto = Convert.ToDecimal(reader["Impuesto"]),
                            Descuento = Convert.ToDecimal(reader["Descuento"]),
                            Total = Convert.ToDecimal(reader["Total"])
                        };
                        reader.Close();
                        factura.Productos = GetFacturaProductos(factura.Id);
                        return factura;
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al obtener la factura con Id {id}", ex);
                }
            }
            return null;
        }

        public int Add(Factura factura)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string query = @"INSERT INTO Facturas (EmpresaId, SucursalId, Fecha, Hora, NumeroFactura, Subtotal, Impuesto, Descuento, Total) 
                                 VALUES (@EmpresaId, @SucursalId, @Fecha, @Hora, @NumeroFactura, @Subtotal, @Impuesto, @Descuento, @Total);
                                 SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new SqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@EmpresaId", factura.EmpresaId);
                    command.Parameters.AddWithValue("@SucursalId", factura.SucursalId);
                    command.Parameters.AddWithValue("@Fecha", factura.Fecha);
                    command.Parameters.AddWithValue("@Hora", factura.Hora);
                    command.Parameters.AddWithValue("@NumeroFactura", factura.NumeroFactura);
                    command.Parameters.AddWithValue("@Subtotal", factura.Subtotal);
                    command.Parameters.AddWithValue("@Impuesto", factura.Impuesto);
                    command.Parameters.AddWithValue("@Descuento", factura.Descuento);
                    command.Parameters.AddWithValue("@Total", factura.Total);

                    int facturaId = Convert.ToInt32(command.ExecuteScalar());
                    factura.Id = facturaId;

                    AddFacturaProductos(factura.Productos, facturaId, connection, transaction);

                    transaction.Commit();
                    return facturaId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Log the exception
                    throw new Exception("Error al agregar la factura", ex);
                }
            }
        }

        public void Update(Factura factura)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string query = @"UPDATE Facturas 
                                 SET EmpresaId = @EmpresaId, SucursalId = @SucursalId, Fecha = @Fecha, 
                                     Hora = @Hora, NumeroFactura = @NumeroFactura, Subtotal = @Subtotal, 
                                     Impuesto = @Impuesto, Descuento = @Descuento, Total = @Total 
                                 WHERE Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@Id", factura.Id);
                    command.Parameters.AddWithValue("@EmpresaId", factura.EmpresaId);
                    command.Parameters.AddWithValue("@SucursalId", factura.SucursalId);
                    command.Parameters.AddWithValue("@Fecha", factura.Fecha);
                    command.Parameters.AddWithValue("@Hora", factura.Hora);
                    command.Parameters.AddWithValue("@NumeroFactura", factura.NumeroFactura);
                    command.Parameters.AddWithValue("@Subtotal", factura.Subtotal);
                    command.Parameters.AddWithValue("@Impuesto", factura.Impuesto);
                    command.Parameters.AddWithValue("@Descuento", factura.Descuento);
                    command.Parameters.AddWithValue("@Total", factura.Total);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No se encontró la factura con Id {factura.Id}");
                    }

                    // Delete existing FacturaProductos and add new ones
                    DeleteFacturaProductos(factura.Id, connection, transaction);
                    AddFacturaProductos(factura.Productos, factura.Id, connection, transaction);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Log the exception
                    throw new Exception($"Error al actualizar la factura con Id {factura.Id}", ex);
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Delete associated FacturaProductos first
                    DeleteFacturaProductos(id, connection, transaction);

                    // Then delete the Factura
                    string query = "DELETE FROM Facturas WHERE Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No se encontró la factura con Id {id}");
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Log the exception
                    throw new Exception($"Error al eliminar la factura con Id {id}", ex);
                }
            }
        }

        private List<FacturaProducto> GetFacturaProductos(int facturaId)
        {
            List<FacturaProducto> facturaProductos = new List<FacturaProducto>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT ProductoId, Cantidad, PrecioUnitario, Subtotal 
                             FROM FacturaProductos WHERE FacturaId = @FacturaId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FacturaId", facturaId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        facturaProductos.Add(new FacturaProducto
                        {
                            ProductoId = Convert.ToInt32(reader["ProductoId"]),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"])
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception($"Error al obtener los productos de la factura con Id {facturaId}", ex);
                }
            }
            return facturaProductos;
        }

        private void AddFacturaProductos(List<FacturaProducto> productos, int facturaId, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"INSERT INTO FacturaProductos (FacturaId, ProductoId, Cantidad, PrecioUnitario, Subtotal) 
                         VALUES (@FacturaId, @ProductoId, @Cantidad, @PrecioUnitario, @Subtotal)";
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var producto in productos)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@FacturaId", facturaId);
                command.Parameters.AddWithValue("@ProductoId", producto.ProductoId);
                command.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                command.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);
                command.Parameters.AddWithValue("@Subtotal", producto.Subtotal);
                command.ExecuteNonQuery();
            }
        }

        private void DeleteFacturaProductos(int facturaId, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "DELETE FROM FacturaProductos WHERE FacturaId = @FacturaId";
            SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@FacturaId", facturaId);
            command.ExecuteNonQuery();
        }
    }
}