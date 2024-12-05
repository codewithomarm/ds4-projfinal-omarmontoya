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
    public partial class EliminarProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            int productoId;
            if (int.TryParse(txtProductoId.Text, out productoId))
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    try
                    {
                        string response = client.DownloadString($"https://localhost:44327/api/productos/{productoId}");
                        var producto = new JavaScriptSerializer().Deserialize<ProductoResponseAdminEPP>(response);

                        lblNombre.Text = producto.Nombre;
                        lblCategoria.Text = producto.Categoria?.Nombre ?? "N/A";
                        lblSubcategoria.Text = producto.Subcategoria?.Nombre ?? "N/A";
                        lblMarca.Text = producto.Marca?.Nombre ?? "N/A";
                        lblPrecio.Text = producto.Precio.ToString("C2");
                        lblStock.Text = producto.Stock.ToString();
                        lblCodigoBarras.Text = producto.CodigoBarras;

                        pnlProductoInfo.Visible = true;
                    }
                    catch (WebException ex)
                    {
                        if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Producto no encontrado.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Error al buscar el producto: {ex.Message}');", true);
                        }
                        pnlProductoInfo.Visible = false;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Por favor, ingrese un ID de producto válido.');", true);
            }
        }

        protected void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            int productoId;
            if (int.TryParse(txtProductoId.Text, out productoId))
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    try
                    {
                        client.UploadString($"https://localhost:44327/api/productos/{productoId}", "DELETE", "");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Producto eliminado con éxito.');", true);
                        pnlProductoInfo.Visible = false;
                        txtProductoId.Text = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Error al eliminar el producto: {ex.Message}');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Por favor, ingrese un ID de producto válido.');", true);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdministrarProductos.aspx");
        }
    }

    public class ProductoResponseAdminEPP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public CategoriaResponseAdminEPP Categoria { get; set; }
        public SubcategoriaResponseAdminEPP Subcategoria { get; set; }
        public MarcaResponseAdminEPP Marca { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string CodigoBarras { get; set; }
    }

    public class CategoriaResponseAdminEPP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class SubcategoriaResponseAdminEPP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class MarcaResponseAdminEPP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}