using Api_web.DTO.Marca;
using Api_web.Servicio.Marca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Api_web.Controllers
{
    [RoutePrefix("api/marcas")]
    public class MarcaController : ApiController
    {
        private readonly MarcaService _marcaService;

        public MarcaController()
        {
            _marcaService = new MarcaService();
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllMarcas()
        {
            try
            {
                var marcas = _marcaService.GetAllMarcas();
                return Ok(marcas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetMarcaById")]
        public IHttpActionResult GetMarcaById(int id)
        {
            try
            {
                var marca = _marcaService.GetMarcaById(id);
                return Ok(marca);
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
        [Route("buscar/{nombre}")]
        public IHttpActionResult GetMarcasByName(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El nombre de búsqueda no puede estar vacío.");
            }

            try
            {
                var marcas = _marcaService.GetMarcasByName(nombre);
                if (marcas == null || !marcas.Any())
                {
                    return NotFound();
                }
                return Ok(marcas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateMarca([FromBody] CreateMarcaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nuevaMarca = _marcaService.CreateMarca(request);
                return CreatedAtRoute("GetMarcaById", new { id = nuevaMarca.Id }, nuevaMarca);
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateMarca(int id, [FromBody] UpdateMarcaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _marcaService.UpdateMarca(id, request);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteMarca(int id)
        {
            try
            {
                _marcaService.DeleteMarca(id);
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