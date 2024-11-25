using Api_web.DTO.Categoria;
using Api_web.DTO.Marca;
using Api_web.DTO.Producto;
using Api_web.DTO.Subcategoria;
using Api_web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio.Producto
{
    public class ProductoService : IProductoService
    {
        private readonly ProductoRepository _productoRepository;
        private readonly CategoriaRepository _categoriaRepository;
        private readonly SubcategoriaRepository _subcategoriaRepository;
        private readonly MarcaRepository _marcaRepository;

        public ProductoService()
        {
            _productoRepository = new ProductoRepository();
            _categoriaRepository = new CategoriaRepository();
            _subcategoriaRepository = new SubcategoriaRepository();
            _marcaRepository = new MarcaRepository();
        }

        public List<ProductoResponse> GetAllProductos()
        {
            try
            {
                var productos = _productoRepository.GetAll();
                return productos.Select(MapToProductoResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los productos: {ex.Message}");
                throw new Exception("Error al obtener los productos", ex);
            }
        }

        public ProductoResponse GetProductoById(int id)
        {
            try
            {
                var producto = _productoRepository.GetById(id);
                if (producto == null)
                {
                    throw new KeyNotFoundException($"No se encontró el producto con ID {id}");
                }
                return MapToProductoResponse(producto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el producto con ID {id}: {ex.Message}");
                throw new Exception($"Error al obtener el producto con ID {id}", ex);
            }
        }

        public List<ProductoResponse> GetProductosByCategory(string categoryName)
        {
            try
            {
                var categoria = _categoriaRepository.GetByName(categoryName).FirstOrDefault();
                if (categoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con nombre '{categoryName}'");
                }
                var productos = _productoRepository.GetByCategory(categoria.Id);
                return productos.Select(MapToProductoResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos para la categoría '{categoryName}': {ex.Message}");
                throw new Exception($"Error al obtener productos para la categoría '{categoryName}'", ex);
            }
        }

        public ProductoResponse GetProductoByBarcode(string barcode)
        {
            try
            {
                var producto = _productoRepository.GetByBarcode(barcode);
                if (producto == null)
                {
                    throw new KeyNotFoundException($"No se encontró el producto con código de barras '{barcode}'");
                }
                return MapToProductoResponse(producto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el producto con código de barras '{barcode}': {ex.Message}");
                throw new Exception($"Error al obtener el producto con código de barras '{barcode}'", ex);
            }
        }

        public List<ProductoResponse> SearchProductosByName(string name)
        {
            try
            {
                var productos = _productoRepository.SearchByName(name);
                return productos.Select(MapToProductoResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar productos por nombre '{name}': {ex.Message}");
                throw new Exception($"Error al buscar productos por nombre '{name}'", ex);
            }
        }

        public ProductoResponse CreateProducto(CreateProductoRequest request)
        {
            try
            {
                var categoria = _categoriaRepository.GetByName(request.Categoria).FirstOrDefault();
                if (categoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con nombre '{request.Categoria}'");
                }

                var subcategoria = _subcategoriaRepository.GetByName(request.Subcategoria).FirstOrDefault();
                if (subcategoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la subcategoría con nombre '{request.Subcategoria}'");
                }

                var marca = _marcaRepository.GetByName(request.Marca).FirstOrDefault();
                if (marca == null)
                {
                    throw new KeyNotFoundException($"No se encontró la marca con nombre '{request.Marca}'");
                }

                var existingProducto = _productoRepository.GetByBarcode(request.CodigoBarras);
                if (existingProducto != null)
                {
                    throw new InvalidOperationException($"Ya existe un producto con el código de barras '{request.CodigoBarras}'");
                }

                var nuevoProducto = _productoRepository.Add(request, categoria.Id, subcategoria.Id, marca.Id);
                return MapToProductoResponse(nuevoProducto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el producto: {ex.Message}");
                throw new Exception("Error al crear el producto", ex);
            }
        }

        public void UpdateProducto(int id, UpdateProductoRequest request)
        {
            try
            {
                var existingProducto = _productoRepository.GetById(id);
                if (existingProducto == null)
                {
                    throw new KeyNotFoundException($"No se encontró el producto con ID {id}");
                }

                var categoria = _categoriaRepository.GetByName(request.Categoria).FirstOrDefault();
                if (categoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con nombre '{request.Categoria}'");
                }

                var subcategoria = _subcategoriaRepository.GetByName(request.Subcategoria).FirstOrDefault();
                if (subcategoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la subcategoría con nombre '{request.Subcategoria}'");
                }

                var marca = _marcaRepository.GetByName(request.Marca).FirstOrDefault();
                if (marca == null)
                {
                    throw new KeyNotFoundException($"No se encontró la marca con nombre '{request.Marca}'");
                }

                var productoWithSameBarcode = _productoRepository.GetByBarcode(request.CodigoBarras);
                if (productoWithSameBarcode != null && productoWithSameBarcode.Id != id)
                {
                    throw new InvalidOperationException($"Ya existe otro producto con el código de barras '{request.CodigoBarras}'");
                }

                _productoRepository.Update(id, request, categoria.Id, subcategoria.Id, marca.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el producto con ID {id}: {ex.Message}");
                throw new Exception($"Error al actualizar el producto con ID {id}", ex);
            }
        }

        public void DeleteProducto(int id)
        {
            try
            {
                var existingProducto = _productoRepository.GetById(id);
                if (existingProducto == null)
                {
                    throw new KeyNotFoundException($"No se encontró el producto con ID {id}");
                }

                _productoRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el producto con ID {id}: {ex.Message}");
                throw new Exception($"Error al eliminar el producto con ID {id}", ex);
            }
        }

        public void UpdateProductoStock(int id, int newStock)
        {
            try
            {
                var existingProducto = _productoRepository.GetById(id);
                if (existingProducto == null)
                {
                    throw new KeyNotFoundException($"No se encontró el producto con ID {id}");
                }

                _productoRepository.UpdateStock(id, newStock);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el stock del producto con ID {id}: {ex.Message}");
                throw new Exception($"Error al actualizar el stock del producto con ID {id}", ex);
            }
        }

        private ProductoResponse MapToProductoResponse(Models.Producto producto)
        {
            var categoria = _categoriaRepository.GetById(producto.CategoriaId);
            var subcategoria = _subcategoriaRepository.GetById(producto.SubcategoriaId);
            var marca = _marcaRepository.GetById(producto.MarcaId);

            return new ProductoResponse
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Categoria = new CategoriaResponse { Id = categoria.Id, Nombre = categoria.Nombre },
                Subcategoria = new SubcategoriaResponse { Id = subcategoria.Id, Nombre = subcategoria.Nombre },
                Marca = new MarcaResponse { Id = marca.Id, Nombre = marca.Nombre },
                UnidadMedida = producto.UnidadMedida,
                Cantidad = producto.Cantidad,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CodigoBarras = producto.CodigoBarras,
                FechaCreacion = producto.FechaCreacion,
                Estado = producto.Estado,
                Foto = producto.Foto,
                FechaModificacion = producto.FechaModificacion
            };
        }
    }
}