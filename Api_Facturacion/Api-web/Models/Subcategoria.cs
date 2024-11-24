using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Models
{
    public class Subcategoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}