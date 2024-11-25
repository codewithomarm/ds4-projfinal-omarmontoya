using Api_web.DTO.Categoria;
using Api_web.Repositories;
using Api_web.Servicio.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio
{
    public class CategoriaService : ICategoriaService
    {
        private readonly CategoriaRepository _categoriaRepository;

        public CategoriaService()
        {
            _categoriaRepository = new CategoriaRepository();
        }

        public List<CategoriaResponse> GetAllCategorias()
        {
            try
            {
                var categorias = _categoriaRepository.GetAll();
                return categorias.Select(MapToCategoriaResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todas las categorías: {ex.Message}");
                throw new Exception("Error al obtener las categorías", ex);
            }
        }

        public CategoriaResponse GetCategoriaById(int id)
        {
            try
            {
                var categoria = _categoriaRepository.GetById(id);
                if (categoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con ID {id}");
                }
                return MapToCategoriaResponse(categoria);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la categoría con ID {id}: {ex.Message}");
                throw new Exception($"Error al obtener la categoría con ID {id}", ex);
            }
        }

        public List<CategoriaResponse> GetCategoriasByName(string name)
        {
            try
            {
                var categorias = _categoriaRepository.GetByName(name);
                return categorias.Select(MapToCategoriaResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar categorías por nombre '{name}': {ex.Message}");
                throw new Exception($"Error al buscar categorías por nombre '{name}'", ex);
            }
        }

        public CategoriaResponse CreateCategoria(CreateCategoriaRequest request)
        {
            try
            {
                var existingCategorias = _categoriaRepository.GetByName(request.Nombre);
                if (existingCategorias.Any(c => c.Nombre.Equals(request.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Ya existe una categoría con el nombre '{request.Nombre}'");
                }

                var nuevaCategoria = _categoriaRepository.Add(request);
                return MapToCategoriaResponse(nuevaCategoria);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la categoría: {ex.Message}");
                throw new Exception("Error al crear la categoría", ex);
            }
        }

        public void UpdateCategoria(int id, UpdateCategoriaRequest request)
        {
            try
            {
                var existingCategoria = _categoriaRepository.GetById(id);
                if (existingCategoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con ID {id}");
                }

                var categoriasWithSameName = _categoriaRepository.GetByName(request.Nombre);
                if (categoriasWithSameName.Any(c => c.Id != id && c.Nombre.Equals(request.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Ya existe otra categoría con el nombre '{request.Nombre}'");
                }

                _categoriaRepository.Update(id, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la categoría con ID {id}: {ex.Message}");
                throw new Exception($"Error al actualizar la categoría con ID {id}", ex);
            }
        }

        public void DeleteCategoria(int id)
        {
            try
            {
                var existingCategoria = _categoriaRepository.GetById(id);
                if (existingCategoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con ID {id}");
                }

                _categoriaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la categoría con ID {id}: {ex.Message}");
                throw new Exception($"Error al eliminar la categoría con ID {id}", ex);
            }
        }

        private CategoriaResponse MapToCategoriaResponse(Models.Categoria categoria)
        {
            return new CategoriaResponse
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }
    }
}