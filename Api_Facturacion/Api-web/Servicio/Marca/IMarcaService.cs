using Api_web.DTO.Marca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_web.Servicio.Marca
{
    internal interface IMarcaService
    {
        List<MarcaResponse> GetAllMarcas();
        MarcaResponse GetMarcaById(int id);
        List<MarcaResponse> GetMarcasByName(string name);
        MarcaResponse CreateMarca(CreateMarcaRequest request);
        void UpdateMarca(int id, UpdateMarcaRequest request);
        void DeleteMarca(int id);
    }
}
