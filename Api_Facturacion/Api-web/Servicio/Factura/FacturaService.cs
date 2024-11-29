using Api_web.DTO.Factura;
using Api_web.Models;
using Api_web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_web.Servicio.Factura
{
    public class FacturaService
    {
        private readonly FacturaRepository _facturaRepository;
        private readonly EmpresaRepository _empresaRepository;
        private readonly SucursalRepository _sucursalRepository;
        private readonly ProductoRepository _productoRepository;
        private readonly CategoriaRepository _categoriaRepository;
        private readonly SubcategoriaRepository _subcategoriaRepository;
        private readonly MarcaRepository _marcaRepository;

        public FacturaService()
        {
            _facturaRepository = new FacturaRepository();
            _empresaRepository = new EmpresaRepository();
            _sucursalRepository = new SucursalRepository();
            _productoRepository = new ProductoRepository();
            _categoriaRepository = new CategoriaRepository();
            _subcategoriaRepository = new SubcategoriaRepository();
            _marcaRepository = new MarcaRepository();
        }

        public List<FacturaResponse> GetAllFacturas()
        {
            try
            {
                var facturas = _facturaRepository.GetAll();
                return facturas.Select(MapToFacturaResponse).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error al obtener todas las facturas", ex);
            }
        }

        public FacturaResponse GetFacturaById(int id)
        {
            try
            {
                var factura = _facturaRepository.GetById(id);
                if (factura == null)
                {
                    throw new KeyNotFoundException($"No se encontró la factura con Id {id}");
                }
                return MapToFacturaResponse(factura);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al obtener la factura con Id {id}", ex);
            }
        }

        public string GetSiguienteNumeroFactura()
        {
            var ultimoId = _facturaRepository.GetUltimoId();
            return (ultimoId + 1).ToString("D5");
        }

        public FacturaResponse CreateFactura(CreateFacturaRequest request)
        {
            try
            {
                var empresa = _empresaRepository.GetByNombre(request.Empresa);
                if (empresa == null)
                {
                    throw new KeyNotFoundException($"No se encontró la empresa con nombre {request.Empresa}");
                }

                var sucursal = _sucursalRepository.GetByNombre(request.Sucursal);
                if (sucursal == null)
                {
                    throw new KeyNotFoundException($"No se encontró la sucursal con nombre {request.Sucursal}");
                }

                var facturaProductos = MapToFacturaProductos(request.Productos);

                int facturaId = _facturaRepository.Add(request, empresa.Id, sucursal.Id, facturaProductos);
                var newFactura = _facturaRepository.GetById(facturaId);
                return MapToFacturaResponse(newFactura);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error al crear la factura", ex);
            }
        }

        public FacturaResponse UpdateFactura(UpdateFacturaRequest request)
        {
            try
            {
                var empresa = _empresaRepository.GetByNombre(request.Empresa);
                if (empresa == null)
                {
                    throw new KeyNotFoundException($"No se encontró la empresa con nombre {request.Empresa}");
                }

                var sucursal = _sucursalRepository.GetByNombre(request.Sucursal);
                if (sucursal == null)
                {
                    throw new KeyNotFoundException($"No se encontró la sucursal con nombre {request.Sucursal}");
                }

                var facturaProductos = MapToFacturaProductos(request.Productos);

                _facturaRepository.Update(request, empresa.Id, sucursal.Id, facturaProductos);
                var updatedFactura = _facturaRepository.GetById(request.Id);
                if (updatedFactura == null)
                {
                    throw new KeyNotFoundException($"No se encontró la factura con Id {request.Id} después de la actualización");
                }
                return MapToFacturaResponse(updatedFactura);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al actualizar la factura con Id {request.Id}", ex);
            }
        }

        public void DeleteFactura(int id)
        {
            try
            {
                _facturaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error al eliminar la factura con Id {id}", ex);
            }
        }

        private FacturaResponse MapToFacturaResponse(Models.Factura factura)
        {
            var empresa = _empresaRepository.GetById(factura.EmpresaId);
            var sucursal = _sucursalRepository.GetById(factura.SucursalId);

            return new FacturaResponse
            {
                Id = factura.Id,
                Empresa = new DTO.Empresa.EmpresaResponse
                {
                    Id = empresa.Id,
                    RUC = empresa.RUC,
                    Nombre = empresa.Nombre
                },
                Sucursal = new DTO.Sucursal.SucursalResponse
                {
                    Id = sucursal.Id,
                    Nombre = sucursal.Nombre,
                    Provincia = sucursal.Provincia,
                    Distrito = sucursal.Distrito,
                    Corregimiento = sucursal.Corregimiento,
                    Urbanizacion = sucursal.Urbanizacion,
                    Calle = sucursal.Calle,
                    Local = sucursal.Local,
                    Empresa = new DTO.Empresa.EmpresaResponse
                    {
                        Id = empresa.Id,
                        RUC = empresa.RUC,
                        Nombre = empresa.Nombre
                    }
                },
                Fecha = factura.Fecha,
                Hora = factura.Hora,
                NumeroFactura = factura.NumeroFactura,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Descuento = factura.Descuento,
                Total = factura.Total,
                Productos = factura.Productos.Select(fp =>
                {
                    var producto = _productoRepository.GetById(fp.ProductoId);
                    var categoria = _categoriaRepository.GetById(producto.CategoriaId);
                    var subcategoria = _subcategoriaRepository.GetById(producto.SubcategoriaId);
                    var marca = _marcaRepository.GetById(producto.MarcaId);

                    return new FacturaProductoResponse
                    {
                        Producto = new DTO.Producto.ProductoResponse
                        {
                            Id = producto.Id,
                            Nombre = producto.Nombre,
                            Descripcion = producto.Descripcion,
                            Categoria = new DTO.Categoria.CategoriaResponse
                            {
                                Id = categoria.Id,
                                Nombre = categoria.Nombre
                            },
                            Subcategoria = new DTO.Subcategoria.SubcategoriaResponse
                            {
                                Id = subcategoria.Id,
                                Nombre = subcategoria.Nombre,
                                Categoria = new DTO.Categoria.CategoriaResponse
                                {
                                    Id = categoria.Id,
                                    Nombre = categoria.Nombre
                                },
                            },
                            Marca = new DTO.Marca.MarcaResponse
                            {
                                Id = marca.Id,
                                Nombre = marca.Nombre
                            },
                            UnidadMedida = producto.UnidadMedida,
                            Cantidad = producto.Cantidad,
                            Precio = producto.Precio,
                            Stock = producto.Stock,
                            CodigoBarras = producto.CodigoBarras,
                            FechaCreacion = producto.FechaCreacion,
                            Estado = producto.Estado,
                            Foto = producto.Foto,
                            FechaModificacion = producto.FechaModificacion
                        },
                        Cantidad = fp.Cantidad,
                        PrecioUnitario = fp.PrecioUnitario,
                        Subtotal = fp.Subtotal
                    };
                }).ToList()
            };
        }

        private List<FacturaProducto> MapToFacturaProductos(List<FacturaProductoRequest> productosRequest)
        {
            return productosRequest.Select(pr =>
            {
                var productos = _productoRepository.SearchByName(pr.Producto);
                if (!productos.Any())
                {
                    throw new KeyNotFoundException($"No se encontró el producto con nombre {pr.Producto}");
                }

                var producto = productos.First();

                return new FacturaProducto
                {
                    ProductoId = producto.Id,
                    Cantidad = pr.Cantidad,
                    PrecioUnitario = pr.PrecioUnitario,
                    Subtotal = pr.Subtotal
                };
            }).ToList();
        }
    }
}