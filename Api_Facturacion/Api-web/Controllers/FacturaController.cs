using Api_web.DTO.Factura;
using Api_web.Servicio.Factura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Api_web.Controllers
{
    [RoutePrefix("api/facturas")]
    public class FacturaController : ApiController
    {
        private readonly FacturaService _facturaService;

        public FacturaController()
        {
            _facturaService = new FacturaService();
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllFacturas()
        {
            try
            {
                var facturas = _facturaService.GetAllFacturas();
                return Ok(facturas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetFactura")]
        public IHttpActionResult GetFactura(int id)
        {
            try
            {
                var factura = _facturaService.GetFacturaById(id);
                if (factura == null)
                {
                    return NotFound();
                }
                return Ok(factura);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("siguiente-numero")]
        public IHttpActionResult GetSiguienteNumeroFactura()
        {
            try
            {
                var siguienteNumero = _facturaService.GetSiguienteNumeroFactura();
                return Ok(new { NumeroFactura = siguienteNumero });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateFactura([FromBody] CreateFacturaRequest facturaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdFactura = _facturaService.CreateFactura(facturaRequest);
                return CreatedAtRoute("GetFactura", new { id = createdFactura.Id }, createdFactura);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateFactura(int id, [FromBody] UpdateFacturaRequest facturaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != facturaRequest.Id)
            {
                return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la solicitud.");
            }

            try
            {
                var updatedFactura = _facturaService.UpdateFactura(facturaRequest);
                return Ok(updatedFactura);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteFactura(int id)
        {
            try
            {
                _facturaService.DeleteFactura(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}