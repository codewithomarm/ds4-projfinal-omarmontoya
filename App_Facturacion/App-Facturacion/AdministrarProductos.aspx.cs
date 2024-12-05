using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
using App_Facturacion.Models;

namespace App_Facturacion
{
    public partial class AdministrarProductos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnListarProductos_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarProductos.aspx");
        }

        protected void btnEditarProductos_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarProducto.aspx");
        }

        protected void btnCrearProductos_Click(object sender, EventArgs e)
        {
            Response.Redirect("CrearProducto.aspx");
        }

        protected void btnEliminarProductos_Click(object sender, EventArgs e)
        {
            Response.Redirect("EliminarProducto.aspx");
        }

        protected void btnVolverMenuPrincipalAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("MenuPrincipalAdmin.aspx");
        }

    }
}