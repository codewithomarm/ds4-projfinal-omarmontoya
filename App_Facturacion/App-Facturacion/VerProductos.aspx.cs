using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Net;

namespace App_Facturacion
{
    public partial class VerProductos : System.Web.UI.Page
    {
        private List<Producto> Productos
        {
            get
            {
                if (ViewState["Productos"] == null)
                {
                    ViewState["Productos"] = new List<Producto>();
                }
                return (List<Producto>)ViewState["Productos"];
            }
            set
            {
                ViewState["Productos"] = value;
            }
        }

        private const string API_BASE_URL = "https://localhost:44327/api";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos();
                CargarCategorias();
            }
            else if (Productos.Count == 0)
            {
                CargarProductos();
            }

            if (!IsPostBack)
            {
                FiltrarProductos();
            }
        }

        private void CargarProductos()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string response = client.DownloadString($"{API_BASE_URL}/productos");
                    Productos = new JavaScriptSerializer().Deserialize<List<Producto>>(response);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar los productos: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private void CargarCategorias()
        {
            try
            {
                // Agregar opción por defecto
                ddlCategoria.Items.Clear();
                ddlCategoria.Items.Add(new ListItem("Todas las categorías", ""));

                if (Productos != null && Productos.Any())
                {
                    // Cargar categorías únicas
                    var categorias = Productos
                        .GroupBy(p => p.categoria.id)
                        .Select(g => g.First().categoria)
                        .OrderBy(c => c.nombre);

                    foreach (var categoria in categorias)
                    {
                        ddlCategoria.Items.Add(new ListItem(categoria.nombre, categoria.id.ToString()));
                    }
                }

                // Limpiar subcategorías
                ddlSubcategoria.Items.Clear();
                ddlSubcategoria.Items.Add(new ListItem("Todas las subcategorías", ""));
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar las categorías: " + ex.Message;
                lblError.Visible = true;
            }
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpiar y recargar subcategorías
            ddlSubcategoria.Items.Clear();
            ddlSubcategoria.Items.Add(new ListItem("Todas las subcategorías", ""));

            // Asegurar que la opción "Seleccione una categoría" permanezca
            if (!ddlCategoria.Items.Cast<ListItem>().Any(i => i.Value == ""))
            {
                ddlCategoria.Items.Insert(0, new ListItem("Todas las categorías", ""));
            }

            if (!string.IsNullOrEmpty(ddlCategoria.SelectedValue))
            {
                var subcategorias = Productos
                    .Where(p => p.categoria.id.ToString() == ddlCategoria.SelectedValue)
                    .GroupBy(p => p.subcategoria.id)
                    .Select(g => g.First().subcategoria)
                    .OrderBy(s => s.nombre);

                foreach (var subcategoria in subcategorias)
                {
                    ddlSubcategoria.Items.Add(new ListItem(subcategoria.nombre, subcategoria.id.ToString()));
                }
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            FiltrarProductos();
        }

        private void FiltrarProductos()
        {
            if (Productos == null || !Productos.Any()) return;

            var productosFiltrados = Productos.AsEnumerable();

            // Aplicar filtro de categoría
            if (!string.IsNullOrEmpty(ddlCategoria.SelectedValue))
            {
                productosFiltrados = productosFiltrados.Where(p =>
                    p.categoria.id.ToString() == ddlCategoria.SelectedValue);
            }

            // Aplicar filtro de subcategoría
            if (!string.IsNullOrEmpty(ddlSubcategoria.SelectedValue))
            {
                productosFiltrados = productosFiltrados.Where(p =>
                    p.subcategoria.id.ToString() == ddlSubcategoria.SelectedValue);
            }

            // Asignar al repeater
            rptProductos.DataSource = productosFiltrados.ToList();
            rptProductos.DataBind();
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MenuPrincipal.aspx");
        }
    }

    // Clases para deserialización
    [Serializable]
    public class Categoria
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    [Serializable]
    public class Subcategoria
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public Categoria categoria { get; set; }
    }

    [Serializable]
    public class Marca
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    [Serializable]
    public class Producto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Categoria categoria { get; set; }
        public Subcategoria subcategoria { get; set; }
        public Marca marca { get; set; }
        public string unidadMedida { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public string codigoBarras { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string estado { get; set; }
        public string foto { get; set; }
        public DateTime? fechaModificacion { get; set; }
    }
}