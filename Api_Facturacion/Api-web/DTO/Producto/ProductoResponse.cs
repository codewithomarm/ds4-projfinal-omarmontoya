using Api_web.DTO.Categoria;
using Api_web.DTO.Marca;
using Api_web.DTO.Subcategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Producto
{
    public class ProductoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public CategoriaResponse Categoria { get; set; }
        public SubcategoriaResponse Subcategoria { get; set; }
        public MarcaResponse Marca { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string CodigoBarras { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public string Foto { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}