using Api_web.DTO.Empresa;
using Api_web.DTO.Sucursal;
using Api_web.Models;
using Api_web.Repositories;
using Api_web.Servicio.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio.Sucursal
{
    public class SucursalService
    {
        private readonly SucursalRepository _sucursalRepository = new SucursalRepository();
        private readonly EmpresaRepository _empresaRepository = new EmpresaRepository();

        public List<SucursalResponse> GetAllSucursales()
        {
            try
            {
                var sucursales = _sucursalRepository.GetAll();
                return sucursales.Select(MapToSucursalResponse).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error al obtener todas las sucursales", ex);
            }
        }

        public SucursalResponse GetSucursalById(int id)
        {
            try
            {
                var sucursal = _sucursalRepository.GetById(id);
                if (sucursal == null)
                {
                    throw new KeyNotFoundException($"No se encontró la sucursal con Id {id}");
                }
                return MapToSucursalResponse(sucursal);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al obtener la sucursal con Id {id}", ex);
            }
        }

        public SucursalResponse CreateSucursal(CreateSucursalRequest request)
        {
            try
            {
                var empresa = _empresaRepository.GetByNombre(request.Empresa);
                if (empresa == null)
                {
                    throw new KeyNotFoundException($"No se encontró la empresa con nombre {request.Empresa}");
                }

                var newSucursal = _sucursalRepository.Add(request, empresa.Id);
                return MapToSucursalResponse(newSucursal);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error al crear la sucursal", ex);
            }
        }

        public SucursalResponse UpdateSucursal(UpdateSucursalRequest request)
        {
            try
            {
                var empresa = _empresaRepository.GetByNombre(request.Empresa);
                if (empresa == null)
                {
                    throw new KeyNotFoundException($"No se encontró la empresa con nombre {request.Empresa}");
                }

                _sucursalRepository.Update(request, empresa.Id);
                var updatedSucursal = _sucursalRepository.GetById(request.Id);
                if (updatedSucursal == null)
                {
                    throw new KeyNotFoundException($"No se encontró la sucursal con Id {request.Id} después de la actualización");
                }
                return MapToSucursalResponse(updatedSucursal);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al actualizar la sucursal con Id {request.Id}", ex);
            }
        }

        public void DeleteSucursal(int id)
        {
            try
            {
                _sucursalRepository.Delete(id);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al eliminar la sucursal con Id {id}", ex);
            }
        }

        private SucursalResponse MapToSucursalResponse(Models.Sucursal sucursal)
        {
            var empresa = _empresaRepository.GetById(sucursal.EmpresaId);
            return new SucursalResponse
            {
                Id = sucursal.Id,
                Nombre = sucursal.Nombre,
                Provincia = sucursal.Provincia,
                Distrito = sucursal.Distrito,
                Corregimiento = sucursal.Corregimiento,
                Urbanizacion = sucursal.Urbanizacion,
                Calle = sucursal.Calle,
                Local = sucursal.Local,
                Empresa = new EmpresaResponse
                {
                    Id = empresa.Id,
                    RUC = empresa.RUC,
                    Nombre = empresa.Nombre
                }
            };
        }
    }
}