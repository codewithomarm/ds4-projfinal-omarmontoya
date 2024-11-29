using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Facturacion.Models
{
    public class FacturaTotales
    {
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }

    public class ProductoFactura
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string CodigoBarras { get; set; }
        public decimal Subtotal { get { return Precio * Cantidad; } }
    }

    public class ProductoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Barcode { get; set; }
        public string ImagenUrl { get; set; }
        public int Stock { get; set; }
        public int MarcaId { get; set; }
        public int SubcategoriaId { get; set; }
    }

    public class CategoriaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class SubcategoriaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CategoriaId { get; set; }
    }
}