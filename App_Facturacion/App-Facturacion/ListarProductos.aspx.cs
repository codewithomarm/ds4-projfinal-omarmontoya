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
    public partial class ListarProductos : System.Web.UI.Page
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
                    var productos = new JavaScriptSerializer().Deserialize<List<ProductoResponseAdminLP>>(response);
                    var productosFormateados = productos.Select(p => new
                    {
                        p.Id,
                        p.Nombre,
                        Categoria = p.Categoria?.Nombre ?? "Sin Categoría",
                        Subcategoria = p.Subcategoria?.Nombre ?? "Sin Subcategoría",
                        p.Precio,
                        p.Stock
                    }).ToList();
                    gvProductos.DataSource = productosFormateados;
                    gvProductos.DataBind();
                }
                catch (Exception ex)
                {
                    // Manejar el error apropiadamente
                    Console.WriteLine($"Error al cargar productos: {ex.Message}");
                }
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdministrarProductos.aspx");
        }
    }

    public class ProductoResponseAdminLP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public CategoriaResponseAdminLP Categoria { get; set; }
        public SubcategoriaResponseAdminLP Subcategoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class CategoriaResponseAdminLP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class SubcategoriaResponseAdminLP
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}