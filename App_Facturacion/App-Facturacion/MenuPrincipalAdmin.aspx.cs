using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App_Facturacion
{
    public partial class MenuPrincipalAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void btnVolverSeleccionEmpresa_Click(object sender, EventArgs e)
        {
            Response.Redirect("SeleccionEmpresaAdmin.aspx");
        }

        protected void btnAdministrarProductos_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdministrarProductos.aspx");
        }

        protected void btnReportes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reportes.aspx");
        }
    }
}