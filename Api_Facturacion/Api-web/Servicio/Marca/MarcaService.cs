using Api_web.DTO.Marca;
using Api_web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio.Marca
{
    public class MarcaService : IMarcaService
    {
        private readonly MarcaRepository _marcaRepository;

        public MarcaService()
        {
            _marcaRepository = new MarcaRepository();
        }

        public List<MarcaResponse> GetAllMarcas()
        {
            try
            {
                var marcas = _marcaRepository.GetAll();
                return marcas.Select(MapToMarcaResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todas las marcas: {ex.Message}");
                throw new Exception("Error al obtener las marcas", ex);
            }
        }

        public MarcaResponse GetMarcaById(int id)
        {
            try
            {
                var marca = _marcaRepository.GetById(id);
                if (marca == null)
                {
                    throw new KeyNotFoundException($"No se encontró la marca con ID {id}");
                }
                return MapToMarcaResponse(marca);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la marca con ID {id}: {ex.Message}");
                throw new Exception($"Error al obtener la marca con ID {id}", ex);
            }
        }

        public List<MarcaResponse> GetMarcasByName(string name)
        {
            try
            {
                var marcas = _marcaRepository.GetByName(name);
                return marcas.Select(MapToMarcaResponse).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar marcas por nombre '{name}': {ex.Message}");
                throw new Exception($"Error al buscar marcas por nombre '{name}'", ex);
            }
        }

        public MarcaResponse CreateMarca(CreateMarcaRequest request)
        {
            try
            {
                var existingMarcas = _marcaRepository.GetByName(request.Nombre);
                if (existingMarcas.Any(m => m.Nombre.Equals(request.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Ya existe una marca con el nombre '{request.Nombre}'");
                }

                var nuevaMarca = _marcaRepository.Add(request);
                return MapToMarcaResponse(nuevaMarca);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la marca: {ex.Message}");
                throw new Exception("Error al crear la marca", ex);
            }
        }

        public void UpdateMarca(int id, UpdateMarcaRequest request)
        {
            try
            {
                var existingMarca = _marcaRepository.GetById(id);
                if (existingMarca == null)
                {
                    throw new KeyNotFoundException($"No se encontró la marca con ID {id}");
                }

                var marcasWithSameName = _marcaRepository.GetByName(request.Nombre);
                if (marcasWithSameName.Any(m => m.Id != id && m.Nombre.Equals(request.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Ya existe otra marca con el nombre '{request.Nombre}'");
                }

                _marcaRepository.Update(id, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la marca con ID {id}: {ex.Message}");
                throw new Exception($"Error al actualizar la marca con ID {id}", ex);
            }
        }

        public void DeleteMarca(int id)
        {
            try
            {
                var existingMarca = _marcaRepository.GetById(id);
                if (existingMarca == null)
                {
                    throw new KeyNotFoundException($"No se encontró la marca con ID {id}");
                }

                _marcaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la marca con ID {id}: {ex.Message}");
                throw new Exception($"Error al eliminar la marca con ID {id}", ex);
            }
        }

        private MarcaResponse MapToMarcaResponse(Models.Marca marca)
        {
            return new MarcaResponse
            {
                Id = marca.Id,
                Nombre = marca.Nombre
            };
        }
    }
}