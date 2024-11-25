using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Producto
{
    public class UpdateStockRequest
    {
        [Required(ErrorMessage = "El nuevo stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int NewStock { get; set; }
    }
}