using Api_web.DTO.Subcategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_web.Servicio.Subcategoria
{
    internal interface ISubcategoriaService
    {
        List<SubcategoriaResponse> GetAllSubcategorias();
        SubcategoriaResponse GetSubcategoriaById(int id);
        List<SubcategoriaResponse> GetSubcategoriasByName(string name);
        List<SubcategoriaResponse> GetSubcategoriasByCategoria(string categoriaName);
        SubcategoriaResponse CreateSubcategoria(CreateSubcategoriaRequest request);
        void UpdateSubcategoria(int id, UpdateSubcategoriaRequest request);
        void DeleteSubcategoria(int id);
    }
}
