using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Facturacion.Models;

namespace App_Facturacion
{
    public partial class Facturacion : System.Web.UI.Page
    {
        private const decimal IMPUESTO_RATE = 0.07M;
        private List<ProductoFactura> ProductosEnFactura
        {
            get
            {
                if (Session["ProductosFactura"] == null)
                    Session["ProductosFactura"] = new List<ProductoFactura>();
                return (List<ProductoFactura>)Session["ProductosFactura"];
            }
            set
            {
                Session["ProductosFactura"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                // Verificar si tenemos la información de empresa y sucursal
                if (Session["EmpresaId"] == null || Session["SucursalId"] == null)
                {
                    Response.Redirect("SeleccionEmpresa.aspx");
                    return;
                }

                CargarInformacionEmpresa();
                CargarCategorias();
                ActualizarGridProductos();
            }
        }

        private void CargarInformacionEmpresa()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    // Cargar información de la empresa
                    string empresaResponse = client.DownloadString($"https://localhost:44327/api/empresas/{Session["EmpresaId"]}");
                    var empresa = new JavaScriptSerializer().Deserialize<EmpresaResponse>(empresaResponse);

                    // Cargar información de la sucursal
                    string sucursalResponse = client.DownloadString($"https://localhost:44327/api/sucursales/{Session["SucursalId"]}");
                    var sucursal = new JavaScriptSerializer().Deserialize<SucursalResponse>(sucursalResponse);

                    lblEmpresa.Text = $"Empresa: {empresa.Nombre}";
                    lblSucursal.Text = $"Sucursal: {sucursal.Nombre}";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error completo: {ex.ToString()}");
                lblError.Text = "Error al cargar información de empresa y sucursal: " + ex.Message;
            }
        }

        private void CargarCategorias()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string response = client.DownloadString("https://localhost:44327/api/categorias");
                    var categorias = new JavaScriptSerializer().Deserialize<List<CategoriaResponse>>(response);

                    ddlCategoria.Items.Clear();
                    ddlCategoria.Items.Add(new ListItem("Seleccione una categoría", ""));
                    foreach (var categoria in categorias)
                    {
                        ddlCategoria.Items.Add(new ListItem(categoria.Nombre, categoria.Id.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar categorías: " + ex.Message;
            }
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlCategoria.SelectedValue))
            {
                ddlSubcategoria.Items.Clear();
                ddlProducto.Items.Clear();
                return;
            }

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string response = client.DownloadString($"https://localhost:44327/api/subcategorias/categoria/{ddlCategoria.SelectedItem.Text}");
                    var subcategorias = new JavaScriptSerializer().Deserialize<List<SubcategoriaResponse>>(response);

                    ddlSubcategoria.Items.Clear();
                    ddlSubcategoria.Items.Add(new ListItem("Seleccione una subcategoría", ""));
                    foreach (var subcategoria in subcategorias)
                    {
                        ddlSubcategoria.Items.Add(new ListItem(subcategoria.Nombre, subcategoria.Id.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar subcategorías: " + ex.Message;
            }
        }

        protected void ddlSubcategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlSubcategoria.SelectedValue))
            {
                ddlProducto.Items.Clear();
                return;
            }

            CargarProductosPorSubcategoria(ddlSubcategoria.SelectedItem.Text);
        }

        private void CargarProductosPorSubcategoria(string subcategoriaName)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string response = client.DownloadString($"https://localhost:44327/api/productos/subcategoria/{subcategoriaName}");
                    var productos = new JavaScriptSerializer().Deserialize<List<ProductoResponse>>(response);

                    ddlProducto.Items.Clear();
                    ddlProducto.Items.Add(new ListItem("Seleccione un producto", ""));
                    foreach (var producto in productos)
                    {
                        ddlProducto.Items.Add(new ListItem(producto.Nombre, producto.Id.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar productos: " + ex.Message;
            }
        }

        protected void btnBuscarBarcode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBarcode.Text))
            {
                lblError.Text = "Ingrese un código de barras";
                return;
            }

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string response = client.DownloadString($"https://localhost:44327/api/productos/barcode/{txtBarcode.Text}");
                    var producto = new JavaScriptSerializer().Deserialize<ProductoResponse>(response);

                    AgregarProductoAFactura(producto);
                    txtBarcode.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al buscar producto: " + ex.Message;
            }
        }

        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlProducto.SelectedValue))
            {
                lblError.Text = "Seleccione un producto";
                return;
            }

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string response = client.DownloadString($"https://localhost:44327/api/productos/{ddlProducto.SelectedValue}");
                    var producto = new JavaScriptSerializer().Deserialize<ProductoResponse>(response);

                    AgregarProductoAFactura(producto);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al agregar producto: " + ex.Message;
            }
        }

        private void AgregarProductoAFactura(ProductoResponse producto)
        {
            var productoExistente = ProductosEnFactura.FirstOrDefault(p => p.Id == producto.Id);
            if (productoExistente != null)
            {
                productoExistente.Cantidad++;
            }
            else
            {
                ProductosEnFactura.Add(new ProductoFactura
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1
                });
            }

            ActualizarGridProductos();
            CalcularTotales();
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                ProductosEnFactura.RemoveAt(index);
                ActualizarGridProductos();
                CalcularTotales();
            }
        }

        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            int index = row.RowIndex;

            if (int.TryParse(txt.Text, out int cantidad))
            {
                if (cantidad > 0)
                {
                    ProductosEnFactura[index].Cantidad = cantidad;
                }
                else
                {
                    ProductosEnFactura.RemoveAt(index);
                }
            }

            ActualizarGridProductos();
            CalcularTotales();
        }

        private void ActualizarGridProductos()
        {
            gvProductos.DataSource = ProductosEnFactura;
            gvProductos.DataBind();
        }

        protected void ddlDescuento_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcularTotales();
        }

        private void CalcularTotales()
        {
            decimal subtotal = ProductosEnFactura.Sum(p => p.Subtotal);
            decimal descuentoPorcentaje = decimal.Parse(ddlDescuento.SelectedValue);
            decimal descuento = subtotal * descuentoPorcentaje;
            decimal subtotalConDescuento = subtotal - descuento;
            decimal impuesto = subtotalConDescuento * IMPUESTO_RATE;
            decimal total = subtotalConDescuento + impuesto;

            lblSubtotal.Text = subtotal.ToString("C2");
            lblDescuento.Text = descuento.ToString("C2");
            lblImpuesto.Text = impuesto.ToString("C2");
            lblTotal.Text = total.ToString("C2");
        }

        protected void btnProcesarFactura_Click(object sender, EventArgs e)
        {
            if (!ProductosEnFactura.Any())
            {
                lblError.Text = "Agregue productos a la factura";
                return;
            }

            try
            {
                // Aquí implementaremos la lógica para guardar la factura
                // Por ahora solo limpiamos la factura
                ProductosEnFactura.Clear();
                ActualizarGridProductos();
                CalcularTotales();
                lblError.Text = "Factura procesada exitosamente";
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al procesar la factura: " + ex.Message;
            }
        }
    }
}