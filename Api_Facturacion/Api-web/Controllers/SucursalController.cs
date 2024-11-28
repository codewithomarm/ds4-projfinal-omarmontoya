using Api_web.DTO.Sucursal;
using Api_web.Servicio.Sucursal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Api_web.Controllers
{
    [RoutePrefix("api/sucursales")]
    public class SucursalController : ApiController
    {
        private readonly SucursalService _sucursalService;

        public SucursalController()
        {
            _sucursalService = new SucursalService();
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllSucursales()
        {
            try
            {
                var sucursales = _sucursalService.GetAllSucursales();
                return Ok(sucursales);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetSucursal")]
        public IHttpActionResult GetSucursal(int id)
        {
            try
            {
                var sucursal = _sucursalService.GetSucursalById(id);
                if (sucursal == null)
                {
                    return NotFound();
                }
                return Ok(sucursal);
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
        [Route("empresa/{empresaId:int}")]
        public IHttpActionResult GetSucursalesPorEmpresa(int empresaId)
        {
            try
            {
                var sucursales = _sucursalService.GetSucursalesByEmpresaId(empresaId);
                return Ok(sucursales);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateSucursal([FromBody] CreateSucursalRequest sucursalRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdSucursal = _sucursalService.CreateSucursal(sucursalRequest);
                return CreatedAtRoute("GetSucursal", new { id = createdSucursal.Id }, createdSucursal);
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
        public IHttpActionResult UpdateSucursal(int id, [FromBody] UpdateSucursalRequest sucursalRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sucursalRequest.Id)
            {
                return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la solicitud.");
            }

            try
            {
                var updatedSucursal = _sucursalService.UpdateSucursal(sucursalRequest);
                return Ok(updatedSucursal);
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
        public IHttpActionResult DeleteSucursal(int id)
        {
            try
            {
                _sucursalService.DeleteSucursal(id);
                return StatusCode(HttpStatusCode.NoContent);
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
    }
}