using Api_web.DTO.Empresa;
using Api_web.Models;
using Api_web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio.Empresa
{
    public class EmpresaService : IEmpresaService
    {
        private readonly EmpresaRepository _empresaRepository = new EmpresaRepository();

        public List<EmpresaResponse> GetAllEmpresas()
        {
            try
            {
                List<Models.Empresa> empresas = _empresaRepository.GetAll();
                return empresas.Select(MapToEmpresaResponse).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error al obtener todas las empresas", ex);
            }
        }

        public EmpresaResponse GetEmpresaById(int id)
        {
            try
            {
                Models.Empresa empresa = _empresaRepository.GetById(id);
                if (empresa == null)
                {
                    throw new KeyNotFoundException($"No se encontró la empresa con Id {id}");
                }
                return MapToEmpresaResponse(empresa);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al obtener la empresa con Id {id}", ex);
            }
        }

        public EmpresaResponse CreateEmpresa(CreateEmpresaRequest request)
        {
            try
            {
                Models.Empresa newEmpresa = _empresaRepository.Add(request);
                return MapToEmpresaResponse(newEmpresa);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error al crear la empresa", ex);
            }
        }

        public EmpresaResponse UpdateEmpresa(UpdateEmpresaRequest request)
        {
            try
            {
                _empresaRepository.Update(request);
                Models.Empresa updatedEmpresa = _empresaRepository.GetById(request.Id);
                if (updatedEmpresa == null)
                {
                    throw new KeyNotFoundException($"No se encontró la empresa con Id {request.Id} después de la actualización");
                }
                return MapToEmpresaResponse(updatedEmpresa);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al actualizar la empresa con Id {request.Id}", ex);
            }
        }

        public void DeleteEmpresa(int id)
        {
            try
            {
                _empresaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al eliminar la empresa con Id {id}", ex);
            }
        }

        private EmpresaResponse MapToEmpresaResponse(Models.Empresa empresa)
        {
            return new EmpresaResponse
            {
                Id = empresa.Id,
                RUC = empresa.RUC,
                Nombre = empresa.Nombre
            };
        }
    }
}