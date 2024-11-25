using Api_web.DTO.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_web.Servicio.Categoria
{
    internal interface ICategoriaService
    {
        List<CategoriaResponse> GetAllCategorias();
        CategoriaResponse GetCategoriaById(int id);
        List<CategoriaResponse> GetCategoriasByName(string name);
        CategoriaResponse CreateCategoria(CreateCategoriaRequest request);
        void UpdateCategoria(int id, UpdateCategoriaRequest request);
        void DeleteCategoria(int id);
    }
}
