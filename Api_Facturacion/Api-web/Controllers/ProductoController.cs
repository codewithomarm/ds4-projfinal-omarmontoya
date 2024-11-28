using Api_web.DTO.Producto;
using Api_web.Servicio.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Api_web.Controllers
{
    [RoutePrefix("api/productos")]
    public class ProductoController : ApiController
    {
        private readonly IProductoService _productoService;

        public ProductoController()
        {
            _productoService = new ProductoService();
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllProductos()
        {
            try
            {
                var productos = _productoService.GetAllProductos();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetProductoById")]
        public IHttpActionResult GetProductoById(int id)
        {
            try
            {
                var producto = _productoService.GetProductoById(id);
                return Ok(producto);
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
        [Route("categoria/{categoryName}")]
        public IHttpActionResult GetProductosByCategory(string categoryName)
        {
            try
            {
                var productos = _productoService.GetProductosByCategory(categoryName);
                if (productos == null || productos.Count == 0)
                {
                    return NotFound();
                }
                return Ok(productos);
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
        [Route("subcategoria/{subcategoriaNombre}")]
        public IHttpActionResult GetProductosBySubcategoria(string subcategoriaNombre)
        {
            if (string.IsNullOrWhiteSpace(subcategoriaNombre))
            {
                return BadRequest("El nombre de la subcategoría no puede estar vacío.");
            }

            try
            {
                var productos = _productoService.GetProductosBySubcategoria(subcategoriaNombre);
                if (productos == null || !productos.Any())
                {
                    return NotFound();
                }
                return Ok(productos);
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

        [HttpGet]
        [Route("barcode/{barcode}")]
        public IHttpActionResult GetProductoByBarcode(string barcode)
        {
            try
            {
                var producto = _productoService.GetProductoByBarcode(barcode);
                return Ok(producto);
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
        [Route("buscar/{name}")]
        public IHttpActionResult SearchProductosByName(string name)
        {
            try
            {
                var productos = _productoService.SearchProductosByName(name);
                if (productos == null || productos.Count == 0)
                {
                    return NotFound();
                }
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateProducto([FromBody] CreateProductoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nuevoProducto = _productoService.CreateProducto(request);
                return CreatedAtRoute("GetProductoById", new { id = nuevoProducto.Id }, nuevoProducto);
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
        public IHttpActionResult UpdateProducto(int id, [FromBody] UpdateProductoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _productoService.UpdateProducto(id, request);
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
        public IHttpActionResult DeleteProducto(int id)
        {
            try
            {
                _productoService.DeleteProducto(id);
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

        [HttpPut]
        [Route("{id:int}/stock")]
        public IHttpActionResult UpdateProductoStock(int id, [FromBody] UpdateStockRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _productoService.UpdateProductoStock(id, request.NewStock);
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