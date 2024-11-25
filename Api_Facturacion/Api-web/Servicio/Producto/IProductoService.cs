using Api_web.DTO.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio.Producto
{
    public interface IProductoService
    {
        List<ProductoResponse> GetAllProductos();
        ProductoResponse GetProductoById(int id);
        List<ProductoResponse> GetProductosByCategory(string categoryName);
        ProductoResponse GetProductoByBarcode(string barcode);
        List<ProductoResponse> SearchProductosByName(string name);
        ProductoResponse CreateProducto(CreateProductoRequest request);
        void UpdateProducto(int id, UpdateProductoRequest request);
        void DeleteProducto(int id);
        void UpdateProductoStock(int id, int newStock);
    }
}