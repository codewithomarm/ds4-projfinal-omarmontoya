using Api_web.DTO.Categoria;
using Api_web.DTO.Subcategoria;
using Api_web.Models;
using Api_web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio.Subcategoria
{
    public class SubcategoriaService : ISubcategoriaService
    {
        private readonly SubcategoriaRepository _subcategoriaRepository;
        private readonly CategoriaRepository _categoriaRepository;

        public SubcategoriaService()
        {
            _subcategoriaRepository = new SubcategoriaRepository();
            _categoriaRepository = new CategoriaRepository();
        }

        public List<SubcategoriaResponse> GetAllSubcategorias()
        {
            try
            {
                var subcategorias = _subcategoriaRepository.GetAll();
                return subcategorias.Select(MapToSubcategoriaResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todas las subcategorías: {ex.Message}");
                throw new Exception("Error al obtener las subcategorías", ex);
            }
        }

        public SubcategoriaResponse GetSubcategoriaById(int id)
        {
            try
            {
                var subcategoria = _subcategoriaRepository.GetById(id);
                if (subcategoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la subcategoría con ID {id}");
                }
                return MapToSubcategoriaResponse(subcategoria);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la subcategoría con ID {id}: {ex.Message}");
                throw new Exception($"Error al obtener la subcategoría con ID {id}", ex);
            }
        }

        public List<SubcategoriaResponse> GetSubcategoriasByName(string name)
        {
            try
            {
                var subcategorias = _subcategoriaRepository.GetAll().Where(s => s.Nombre.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                return subcategorias.Select(MapToSubcategoriaResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar subcategorías por nombre '{name}': {ex.Message}");
                throw new Exception($"Error al buscar subcategorías por nombre '{name}'", ex);
            }
        }

        public List<SubcategoriaResponse> GetSubcategoriasByCategoria(string categoriaName)
        {
            try
            {
                var categoria = _categoriaRepository.GetByName(categoriaName).FirstOrDefault();
                if (categoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con nombre '{categoriaName}'");
                }
                var subcategorias = _subcategoriaRepository.GetByCategoriaId(categoria.Id);
                return subcategorias.Select(MapToSubcategoriaResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener subcategorías para la categoría '{categoriaName}': {ex.Message}");
                throw new Exception($"Error al obtener subcategorías para la categoría '{categoriaName}'", ex);
            }
        }

        public SubcategoriaResponse CreateSubcategoria(CreateSubcategoriaRequest request)
        {
            try
            {
                var categoria = _categoriaRepository.GetByName(request.Categoria).FirstOrDefault();
                if (categoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con nombre '{request.Categoria}'");
                }

                var existingSubcategorias = _subcategoriaRepository.GetByCategoriaId(categoria.Id);
                if (existingSubcategorias.Any(s => s.Nombre.Equals(request.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Ya existe una subcategoría con el nombre '{request.Nombre}' en la categoría '{request.Categoria}'");
                }

                var nuevaSubcategoria = _subcategoriaRepository.Add(request, categoria.Id);
                return MapToSubcategoriaResponse(nuevaSubcategoria);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la subcategoría: {ex.Message}");
                throw new Exception("Error al crear la subcategoría", ex);
            }
        }

        public void UpdateSubcategoria(int id, UpdateSubcategoriaRequest request)
        {
            try
            {
                var existingSubcategoria = _subcategoriaRepository.GetById(id);
                if (existingSubcategoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la subcategoría con ID {id}");
                }

                var categoria = _categoriaRepository.GetByName(request.Categoria).FirstOrDefault();
                if (categoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la categoría con nombre '{request.Categoria}'");
                }

                var subcategoriasInSameCategory = _subcategoriaRepository.GetByCategoriaId(categoria.Id);
                if (subcategoriasInSameCategory.Any(s => s.Id != id && s.Nombre.Equals(request.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Ya existe otra subcategoría con el nombre '{request.Nombre}' en la categoría '{request.Categoria}'");
                }

                _subcategoriaRepository.Update(id, request, categoria.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la subcategoría con ID {id}: {ex.Message}");
                throw new Exception($"Error al actualizar la subcategoría con ID {id}", ex);
            }
        }

        public void DeleteSubcategoria(int id)
        {
            try
            {
                var existingSubcategoria = _subcategoriaRepository.GetById(id);
                if (existingSubcategoria == null)
                {
                    throw new KeyNotFoundException($"No se encontró la subcategoría con ID {id}");
                }

                _subcategoriaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la subcategoría con ID {id}: {ex.Message}");
                throw new Exception($"Error al eliminar la subcategoría con ID {id}", ex);
            }
        }

        private SubcategoriaResponse MapToSubcategoriaResponse(Models.Subcategoria subcategoria)
        {
            var categoria = _categoriaRepository.GetById(subcategoria.CategoriaId);
            return new SubcategoriaResponse
            {
                Id = subcategoria.Id,
                Nombre = subcategoria.Nombre,
                Categoria = new CategoriaResponse
                {
                    Id = categoria.Id,
                    Nombre = categoria.Nombre
                }
            };
        }
    }
}