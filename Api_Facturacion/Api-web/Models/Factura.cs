using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public int SucursalId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string NumeroFactura { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public List<FacturaProducto> Productos { get; set; }
    }
}