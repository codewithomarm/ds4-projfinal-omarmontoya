using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Facturacion.Models;

namespace App_Facturacion
{
    public partial class VerFactura : System.Web.UI.Page
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

                if (Session["FacturaActual"] == null)
                {
                    Response.Redirect("Facturacion.aspx");
                    return;
                }

                CargarDatosFactura();
            }
        }

        private void CargarDatosFactura()
        {
            var factura = Session["FacturaActual"] as FacturaActual;
            if (factura != null)
            {
                // Información de la Empresa
                lblEmpresaNombre.Text = factura.EmpresaNombre;
                lblEmpresaRUC.Text = $"R.U.C.: {factura.EmpresaRUC}";
                lblSucursalDireccion1.Text = $"{factura.SucursalProvincia}, {factura.SucursalDistrito}";
                lblSucursalDireccion2.Text = factura.SucursalCorregimiento;
                lblSucursalDireccion3.Text = $"{factura.SucursalUrbanizacion}\n{factura.SucursalCalle}, {factura.SucursalLocal}";

                // Información de la Factura
                lblSucursalNombre.Text = $"Sucursal: {factura.SucursalNombre}";
                lblNumeroFactura.Text = $"Factura #{factura.NumeroFactura}";

                // Fecha y Hora
                lblFecha.Text = $"Fecha: {factura.Fecha:dd/MM/yyyy}";
                lblHora.Text = $"Hora: {factura.Fecha:HH:mm:ss}";

                // Productos
                rptProductos.DataSource = factura.Productos;
                rptProductos.DataBind();

                // Totales
                lblSubtotal.Text = factura.Subtotal.ToString("C2");
                lblDescuento.Text = factura.Descuento.ToString("C2");
                lblImpuesto.Text = factura.Impuesto.ToString("C2");
                lblTotal.Text = factura.Total.ToString("C2");

                // Pie de Factura
                lblTotalArticulos.Text = $"TOT. ARTICULOS VENDIDOS = {factura.Productos.Count}";
                lblFechaHora.Text = $"{factura.Fecha:dd/MM/yyyy} {factura.Fecha:HH:mm:ss}";
            }
        }

        protected void btnVolverMenu_Click(object sender, EventArgs e)
        {
            Session["FacturaActual"] = null;
            Response.Redirect("MenuPrincipal.aspx");
        }

        protected void btnVolverFacturacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("Facturacion.aspx");
        }
    }
}