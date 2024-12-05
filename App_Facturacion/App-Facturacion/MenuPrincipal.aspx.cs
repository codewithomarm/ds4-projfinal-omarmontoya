using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App_Facturacion
{
    public partial class MenuPrincipal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void btnVerProductos_Click(object sender, EventArgs e)
        {
            Response.Redirect("VerProductos.aspx");
        }

        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Facturacion.aspx");
        }

        protected void btnVolverSeleccionEmpresa_Click(object sender, EventArgs e)
        {
            Response.Redirect("SeleccionEmpresa.aspx");
        }
    }
}