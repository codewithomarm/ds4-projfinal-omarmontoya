using Api_web.DTO.Categoria;
using Api_web.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Api_web.Controllers
{
    [RoutePrefix("api/categorias")]
    public class CategoriaController : ApiController
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController()
        {
            _categoriaService = new CategoriaService();
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllCategorias()
        {
            try
            {
                var categorias = _categoriaService.GetAllCategorias();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetCategoriaById(int id)
        {
            try
            {
                var categoria = _categoriaService.GetCategoriaById(id);
                return Ok(categoria);
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
        public IHttpActionResult GetCategoriasByName(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El nombre de búsqueda no puede estar vacío.");
            }

            try
            {
                var categorias = _categoriaService.GetCategoriasByName(nombre);
                if (categorias == null || !categorias.Any())
                {
                    return NotFound();
                }
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateCategoria([FromBody] CreateCategoriaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nuevaCategoria = _categoriaService.CreateCategoria(request);
                return CreatedAtRoute("DefaultApi", new { id = nuevaCategoria.Id }, nuevaCategoria);
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
        public IHttpActionResult UpdateCategoria(int id, [FromBody] UpdateCategoriaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _categoriaService.UpdateCategoria(id, request);
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
        public IHttpActionResult DeleteCategoria(int id)
        {
            try
            {
                _categoriaService.DeleteCategoria(id);
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