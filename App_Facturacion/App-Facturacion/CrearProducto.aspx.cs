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
    public partial class CrearProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCrearProducto_Click(object sender, EventArgs e)
        {
            var producto = new CreateProductoRequestAdminCP
            {
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                Categoria = txtCategoria.Text,
                Subcategoria = txtSubcategoria.Text,
                Marca = txtMarca.Text,
                UnidadMedida = txtUnidadMedida.Text,
                Cantidad = Convert.ToDecimal(txtCantidad.Text),
                Precio = Convert.ToDecimal(txtPrecio.Text),
                Stock = Convert.ToInt32(txtStock.Text),
                CodigoBarras = txtCodigoBarras.Text,
                Foto = txtFoto.Text
            };

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                try
                {
                    string data = new JavaScriptSerializer().Serialize(producto);
                    string response = client.UploadString("https://localhost:44327/api/productos", "POST", data);

                    // Mostrar mensaje de éxito
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Producto creado con éxito.');", true);
                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    // Manejar el error apropiadamente
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Error al crear el producto: {ex.Message}');", true);
                }
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtCategoria.Text = string.Empty;
            txtSubcategoria.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtUnidadMedida.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtStock.Text = string.Empty;
            txtCodigoBarras.Text = string.Empty;
            txtFoto.Text = string.Empty;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "window.location = 'AdministrarProductos.aspx';", true);
        }
    }

    public class CreateProductoRequestAdminCP
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public string Marca { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string CodigoBarras { get; set; }
        public string Foto { get; set; }
    }
}