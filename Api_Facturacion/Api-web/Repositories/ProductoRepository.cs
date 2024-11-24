﻿using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Api_web.Repositories
{
    public class ProductoRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FacturacionDbContext"].ConnectionString;

        public List<Producto> GetAll()
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Productos";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(MapToProducto(reader));
                }
            }

            return productos;
        }

        public Producto GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Productos WHERE id_producto = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapToProducto(reader);
                }
            }

            return null;
        }

        public List<Producto> GetByCategory(int categoryId)
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Productos WHERE id_categoria = @CategoryId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(MapToProducto(reader));
                }
            }

            return productos;
        }

        public Producto GetByBarcode(string barcode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Productos WHERE codigo_barras = @Barcode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Barcode", barcode);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return MapToProducto(reader);
                }
            }

            return null;
        }

        public List<Producto> SearchByName(string name)
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Productos WHERE nombre LIKE @Name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(MapToProducto(reader));
                }
            }

            return productos;
        }

        public void Add(Producto producto)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Productos (nombre, descripcion, id_categoria, id_subcategoria, id_marca, unidad_medida, cantidad, precio, stock, codigo_barras, fecha_creacion, estado, foto) 
                                 VALUES (@Nombre, @Descripcion, @CategoriaId, @SubcategoriaId, @MarcaId, @UnidadMedida, @Cantidad, @Precio, @Stock, @CodigoBarras, @FechaCreacion, @Estado, @Foto);
                                 SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);

                AddParametersToCommand(command, producto);

                connection.Open();
                producto.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Producto producto)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Productos 
                                 SET nombre = @Nombre, descripcion = @Descripcion, id_categoria = @CategoriaId, 
                                     id_subcategoria = @SubcategoriaId, id_marca = @MarcaId, unidad_medida = @UnidadMedida, 
                                     cantidad = @Cantidad, precio = @Precio, stock = @Stock, codigo_barras = @CodigoBarras, 
                                     estado = @Estado, foto = @Foto, fecha_modificacion = @FechaModificacion
                                 WHERE id_producto = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                AddParametersToCommand(command, producto);
                command.Parameters.AddWithValue("@Id", producto.Id);
                command.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Productos WHERE id_producto = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStock(int id, int newStock)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Productos SET stock = @Stock WHERE id_producto = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Stock", newStock);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private Producto MapToProducto(SqlDataReader reader)
        {
            return new Producto
            {
                Id = Convert.ToInt32(reader["id_producto"]),
                Nombre = reader["nombre"].ToString(),
                Descripcion = reader["descripcion"].ToString(),
                CategoriaId = Convert.ToInt32(reader["id_categoria"]),
                SubcategoriaId = Convert.ToInt32(reader["id_subcategoria"]),
                MarcaId = Convert.ToInt32(reader["id_marca"]),
                UnidadMedida = reader["unidad_medida"].ToString(),
                Cantidad = Convert.ToDecimal(reader["cantidad"]),
                Precio = Convert.ToDecimal(reader["precio"]),
                Stock = Convert.ToInt32(reader["stock"]),
                CodigoBarras = reader["codigo_barras"].ToString(),
                FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
                Estado = reader["estado"].ToString(),
                Foto = reader["foto"].ToString(),
                FechaModificacion = reader["fecha_modificacion"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_modificacion"]) : (DateTime?)null
            };
        }

        private void AddParametersToCommand(SqlCommand command, Producto producto)
        {
            command.Parameters.AddWithValue("@Nombre", producto.Nombre);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);
            command.Parameters.AddWithValue("@SubcategoriaId", producto.SubcategoriaId);
            command.Parameters.AddWithValue("@MarcaId", producto.MarcaId);
            command.Parameters.AddWithValue("@UnidadMedida", producto.UnidadMedida);
            command.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@Stock", producto.Stock);
            command.Parameters.AddWithValue("@CodigoBarras", producto.CodigoBarras);
            command.Parameters.AddWithValue("@FechaCreacion", producto.FechaCreacion);
            command.Parameters.AddWithValue("@Estado", producto.Estado);
            command.Parameters.AddWithValue("@Foto", producto.Foto ?? (object)DBNull.Value);
        }
    }
}