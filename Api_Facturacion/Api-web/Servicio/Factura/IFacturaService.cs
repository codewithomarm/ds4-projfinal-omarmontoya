using Api_web.DTO.Factura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_web.Servicio.Factura
{
    internal interface IFacturaService
    {
        List<FacturaResponse> GetAllFacturas();
        FacturaResponse GetFacturaById(int id);
        string GetSiguienteNumeroFactura();
        FacturaResponse CreateFactura(CreateFacturaRequest request);
        FacturaResponse UpdateFactura(UpdateFacturaRequest request);
        void DeleteFactura(int id);
    }
}
