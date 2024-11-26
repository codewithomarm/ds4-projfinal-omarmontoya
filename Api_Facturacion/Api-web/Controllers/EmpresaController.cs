using Api_web.DTO.Empresa;
using Api_web.Servicio.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Api_web.Controllers
{
    [RoutePrefix("api/empresas")]
    public class EmpresaController : ApiController
    {
        private readonly IEmpresaService _empresaService = new EmpresaService();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllEmpresas()
        {
            try
            {
                var empresas = _empresaService.GetAllEmpresas();
                return Ok(empresas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name ="GetEmpresa")]
        public IHttpActionResult GetEmpresa(int id)
        {
            try
            {
                var empresa = _empresaService.GetEmpresaById(id);
                if (empresa == null)
                {
                    return NotFound();
                }
                return Ok(empresa);
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

        // POST: api/empresas
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateEmpresa([FromBody] CreateEmpresaRequest empresaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdEmpresa = _empresaService.CreateEmpresa(empresaRequest);
                return CreatedAtRoute("GetEmpresa", new { id = createdEmpresa.Id }, createdEmpresa);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateEmpresa(int id, [FromBody] UpdateEmpresaRequest empresaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empresaRequest.Id)
            {
                return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la solicitud.");
            }

            try
            {
                var updatedEmpresa = _empresaService.UpdateEmpresa(empresaRequest);
                return Ok(updatedEmpresa);
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

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteEmpresa(int id)
        {
            try
            {
                _empresaService.DeleteEmpresa(id);
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