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
    public partial class EditarProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos();
            }
        }

        private void CargarProductos()
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                try
                {
                    string response = client.DownloadString("https://localhost:44327/api/productos");
                    var productos = new JavaScriptSerializer().Deserialize<List<ProductoResponseAdminEP>>(response);
                    ddlProductos.DataSource = productos;
                    ddlProductos.DataTextField = "Nombre";
                    ddlProductos.DataValueField = "Id";
                    ddlProductos.DataBind();
                    ddlProductos.Items.Insert(0, new ListItem("Seleccione un producto", ""));
                }
                catch (Exception ex)
                {
                    // Manejar el error apropiadamente
                    Console.WriteLine($"Error al cargar productos: {ex.Message}");
                }
            }
        }

        protected void ddlProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductos.SelectedValue))
            {
                CargarDatosProducto(Convert.ToInt32(ddlProductos.SelectedValue));
            }
            else
            {
                pnlEditarProducto.Visible = false;
            }
        }

        protected void btnCargarProducto_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductos.SelectedValue))
            {
                CargarDatosProducto(Convert.ToInt32(ddlProductos.SelectedValue));
            }
        }

        private void CargarDatosProducto(int productId)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                try
                {
                    string response = client.DownloadString($"https://localhost:44327/api/productos/{productId}");
                    var producto = new JavaScriptSerializer().Deserialize<ProductoResponseAdminEP>(response);

                    txtNombre.Text = producto.Nombre;
                    txtDescripcion.Text = producto.Descripcion;
                    txtCategoria.Text = producto.Categoria.Nombre;
                    txtSubcategoria.Text = producto.Subcategoria.Nombre;
                    txtMarca.Text = producto.Marca.Nombre;
                    txtUnidadMedida.Text = producto.UnidadMedida;
                    txtCantidad.Text = producto.Cantidad.ToString();
                    txtPrecio.Text = producto.Precio.ToString();
                    txtStock.Text = producto.Stock.ToString();
                    txtCodigoBarras.Text = producto.CodigoBarras;
                    txtFoto.Text = producto.Foto;
                    ddlEstado.SelectedValue = producto.Estado;

                    pnlEditarProducto.Visible = true;
                }
                catch (Exception ex)
                {
                    // Manejar el error apropiadamente
                    Console.WriteLine($"Error al cargar datos del producto: {ex.Message}");
                }
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductos.SelectedValue))
            {
                int productId = Convert.ToInt32(ddlProductos.SelectedValue);
                var producto = new UpdateProductoRequestAdminEP
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
                    Estado = ddlEstado.SelectedValue,
                    Foto = txtFoto.Text
                };

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    try
                    {
                        string data = new JavaScriptSerializer().Serialize(producto);
                        client.UploadString($"https://localhost:44327/api/productos/{productId}", "PUT", data);
                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Producto actualizado con éxito.');", true);
                    }
                    catch (Exception ex)
                    {
                        // Manejar el error apropiadamente
                        Console.WriteLine($"Error al actualizar producto: {ex.Message}");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Error al actualizar producto: {ex.Message}');", true);
                    }
                }
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdministrarProductos.aspx");
        }
    }

    public class ProductoResponseAdminEP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public CategoriaResponseAdminEP Categoria { get; set; }
        public SubcategoriaResponseAdminEP Subcategoria { get; set; }
        public MarcaResponseAdminEP Marca { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string CodigoBarras { get; set; }
        public string Estado { get; set; }
        public string Foto { get; set; }
    }

    public class CategoriaResponseAdminEP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class SubcategoriaResponseAdminEP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class MarcaResponseAdminEP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class UpdateProductoRequestAdminEP
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
        public string Estado { get; set; }
        public string Foto { get; set; }
    }
}