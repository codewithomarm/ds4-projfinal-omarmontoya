using Api_web.DTO.Producto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Factura
{
    public class FacturaProductoResponse
    {
        [Required]
        public ProductoResponse Producto { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal Subtotal { get; set; }
    }
}