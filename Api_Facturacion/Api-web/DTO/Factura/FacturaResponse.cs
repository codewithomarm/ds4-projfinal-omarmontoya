using Api_web.DTO.Empresa;
using Api_web.DTO.Sucursal;
using Api_web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Factura
{
    public class FacturaResponse
    {
        public int Id { get; set; }

        [Required]
        public EmpresaResponse Empresa { get; set; }

        [Required]
        public SucursalResponse Sucursal { get; set; }

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
        public List<FacturaProductoResponse> Productos { get; set; }
    }
}