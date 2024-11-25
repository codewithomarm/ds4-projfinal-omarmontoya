using Api_web.DTO.Subcategoria;
using Api_web.Servicio.Subcategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Api_web.Controllers
{
    [RoutePrefix("api/subcategorias")]
    public class SubcategoriaController : ApiController
    {
        private readonly ISubcategoriaService _subcategoriaService;

        public SubcategoriaController()
        {
            _subcategoriaService = new SubcategoriaService();
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllSubcategorias()
        {
            try
            {
                var subcategorias = _subcategoriaService.GetAllSubcategorias();
                return Ok(subcategorias);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetSubcategoriaById")]
        public IHttpActionResult GetSubcategoriaById(int id)
        {
            try
            {
                var subcategoria = _subcategoriaService.GetSubcategoriaById(id);
                return Ok(subcategoria);
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
        public IHttpActionResult GetSubcategoriasByName(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El nombre de búsqueda no puede estar vacío.");
            }

            try
            {
                var subcategorias = _subcategoriaService.GetSubcategoriasByName(nombre);
                if (subcategorias == null || !subcategorias.Any())
                {
                    return NotFound();
                }
                return Ok(subcategorias);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("categoria/{categoriaName}")]
        public IHttpActionResult GetSubcategoriasByCategoria(string categoriaName)
        {
            if (string.IsNullOrWhiteSpace(categoriaName))
            {
                return BadRequest("El nombre de la categoría no puede estar vacío.");
            }

            try
            {
                var subcategorias = _subcategoriaService.GetSubcategoriasByCategoria(categoriaName);
                if (subcategorias == null || !subcategorias.Any())
                {
                    return NotFound();
                }
                return Ok(subcategorias);
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

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateSubcategoria([FromBody] CreateSubcategoriaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nuevaSubcategoria = _subcategoriaService.CreateSubcategoria(request);
                return CreatedAtRoute("GetSubcategoriaById", new { id = nuevaSubcategoria.Id }, nuevaSubcategoria);
            }
            catch (KeyNotFoundException ex)
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

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateSubcategoria(int id, [FromBody] UpdateSubcategoriaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _subcategoriaService.UpdateSubcategoria(id, request);
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
        public IHttpActionResult DeleteSubcategoria(int id)
        {
            try
            {
                _subcategoriaService.DeleteSubcategoria(id);
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