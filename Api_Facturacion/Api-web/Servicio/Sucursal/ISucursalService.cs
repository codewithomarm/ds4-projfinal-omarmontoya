using Api_web.DTO.Sucursal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_web.Servicio.Sucursal
{
    internal interface ISucursalService
    {
        List<SucursalResponse> GetAllSucursales();
        SucursalResponse GetSucursalById(int id);
        SucursalResponse CreateSucursal(CreateSucursalRequest request);
        SucursalResponse UpdateSucursal(UpdateSucursalRequest request);
        void DeleteSucursal(int id);
    }
}
