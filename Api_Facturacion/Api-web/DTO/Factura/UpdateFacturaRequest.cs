using Api_web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Factura
{
    public class UpdateFacturaRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Empresa { get; set; }

        [Required]
        [StringLength(100)]
        public string Sucursal { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string NumeroFactura { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal Subtotal { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal Impuesto { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal Descuento { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal Total { get; set; }

        [Required]
        public List<FacturaProductoRequest> Productos { get; set; }
    }
}