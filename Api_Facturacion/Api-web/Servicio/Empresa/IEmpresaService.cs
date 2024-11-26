using Api_web.DTO.Empresa;
using Api_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_web.Servicio.Empresa
{
    internal interface IEmpresaService
    {
        List<EmpresaResponse> GetAllEmpresas();
        EmpresaResponse GetEmpresaById(int id);
        EmpresaResponse CreateEmpresa(CreateEmpresaRequest request);
        EmpresaResponse UpdateEmpresa(UpdateEmpresaRequest request);
        void DeleteEmpresa(int id);
    }
}
