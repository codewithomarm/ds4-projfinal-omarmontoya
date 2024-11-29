using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Facturacion.Models
{
    public class FacturaActual
    {
        public string EmpresaNombre { get; set; }
        public string EmpresaRUC { get; set; }
        public string SucursalProvincia { get; set; }
        public string SucursalDistrito { get; set; }
        public string SucursalCorregimiento { get; set; }
        public string SucursalUrbanizacion { get; set; }
        public string SucursalCalle {  get; set; }
        public string SucursalLocal {  get; set; }
        public string SucursalNombre { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string NumeroFactura { get; set; }
        public List<ProductoFactura> Productos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }
}