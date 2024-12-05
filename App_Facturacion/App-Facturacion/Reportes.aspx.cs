using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App_Facturacion
{
    public partial class Reportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                CargarInformacionEmpresa();
            }
        }

        private void CargarInformacionEmpresa()
        {
            int empresaId = Convert.ToInt32(Session["EmpresaId"]);
            int sucursalId = Convert.ToInt32(Session["SucursalId"]);

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                try
                {
                    string empresaResponse = client.DownloadString($"https://localhost:44327/api/empresas/{empresaId}");
                    var empresa = new JavaScriptSerializer().Deserialize<EmpresaResponse>(empresaResponse);

                    string sucursalResponse = client.DownloadString($"https://localhost:44327/api/sucursales/{sucursalId}");
                    var sucursal = new JavaScriptSerializer().Deserialize<SucursalResponse>(sucursalResponse);

                    lblEmpresa.Text = $"Empresa: {empresa.Nombre}";
                    lblSucursal.Text = $"Sucursal: {sucursal.Nombre}";
                }
                catch (Exception ex)
                {
                    // Manejar el error apropiadamente
                    System.Diagnostics.Debug.WriteLine($"Error al cargar información: {ex.Message}");
                }
            }
        }

        protected void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            DateTime fechaInicial;
            DateTime fechaFinal;

            if (!DateTime.TryParse(txtFechaInicial.Text, out fechaInicial) ||
                !DateTime.TryParse(txtFechaFinal.Text, out fechaFinal))
            {
                // Mostrar mensaje de error si las fechas no son válidas
                return;
            }

            var facturas = ObtenerFacturas(fechaInicial, fechaFinal);
            if (facturas != null && facturas.Any())
            {
                MostrarReporteVentas(facturas);
            }
            else
            {
                // Mostrar mensaje de que no hay datos para el período seleccionado
            }
        }

        private List<FacturaAdmin> ObtenerFacturas(DateTime fechaInicial, DateTime fechaFinal)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                try
                {
                    string url = $"https://localhost:44327/api/facturas";
                    string response = client.DownloadString(url);
                    var todasLasFacturas = new JavaScriptSerializer().Deserialize<List<FacturaAdmin>>(response);

                    // Filtrar facturas por fecha y empresa/sucursal
                    return todasLasFacturas.Where(f =>
                        f.Fecha.Date >= fechaInicial.Date &&
                        f.Fecha.Date <= fechaFinal.Date &&
                        f.Empresa.Id == Convert.ToInt32(Session["EmpresaId"]) &&
                        f.Sucursal.Id == Convert.ToInt32(Session["SucursalId"]))
                        .ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al obtener facturas: {ex.Message}");
                    return null;
                }
            }
        }

        private void MostrarReporteVentas(List<FacturaAdmin> facturas)
        {
            decimal subtotal = facturas.Sum(f => f.Subtotal);
            decimal impuestos = facturas.Sum(f => f.Impuesto);
            decimal descuentos = facturas.Sum(f => f.Descuento);
            decimal granTotal = facturas.Sum(f => f.Total);

            lblSubtotal.Text = subtotal.ToString("C2");
            lblImpuestos.Text = impuestos.ToString("C2");
            lblDescuentos.Text = descuentos.ToString("C2");
            lblGranTotal.Text = granTotal.ToString("C2");

            var productosVendidos = facturas.SelectMany(f => f.Productos)
                .GroupBy(p => new
                    {
                    p.Producto.Id,
                    p.Producto.Nombre,
                    Categoria = p.Producto.Categoria?.Nombre ?? "Sin Categoría",
                    Subcategoria = p.Producto.Subcategoria?.Nombre ?? "Sin Subcategoría",
                    p.PrecioUnitario
                })
                .Select(g => new ProductoVendidoAdmin
                {
                    Nombre = g.Key.Nombre,
                    Categoria = g.Key.Nombre,
                    Subcategoria = g.Key.Nombre,
                    Precio = g.Key.PrecioUnitario,
                    CantidadVendida = g.Sum(p => p.Cantidad)
                })
                .OrderByDescending(p => p.CantidadVendida)
                .ToList();

            gvProductosVendidos.DataSource = productosVendidos;
            gvProductosVendidos.DataBind();

            pnlResumenVentas.Visible = true;
            pnlProductosVendidos.Visible = true;
        }
    }

    public class FacturaAdmin
    {
        public int Id { get; set; }
        public EmpresaResponseAdmin Empresa { get; set; }
        public SucursalResponseAdmin Sucursal { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string NumeroFactura { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public List<ProductoFacturaAdmin> Productos { get; set; }
    }

    public class ProductoFacturaAdmin
    {
        public ProductoResponseAdmin Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class ProductoResponseAdmin
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public CategoriaResponseAdmin Categoria { get; set; }
        public SubcategoriaResponseAdmin Subcategoria { get; set; }
        public decimal Precio { get; set; }
    }

    public class CategoriaResponseAdmin
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class SubcategoriaResponseAdmin
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class ProductoVendidoAdmin
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Precio { get; set; }
        public int CantidadVendida { get; set; }
    }
}