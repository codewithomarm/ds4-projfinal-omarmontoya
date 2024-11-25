using Api_web.DTO.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.DTO.Subcategoria
{
    public class SubcategoriaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public CategoriaResponse Categoria { get; set; }
    }
}