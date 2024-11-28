using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App_Facturacion
{
    public partial class SeleccionEmpresa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    Response.Redirect("Login.aspx");
                }

                CargarEmpresas();
            }

            // Asegurarse de que el evento esté conectado
            ddlEmpresa.SelectedIndexChanged += ddlEmpresa_SelectedIndexChanged;
        }

        private void CargarEmpresas()
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                try
                {
                    // Primero limpiamos ambos dropdowns
                    ddlEmpresa.Items.Clear();
                    ddlSucursal.Items.Clear();

                    // Agregamos el item "Seleccione una empresa" primero
                    ddlEmpresa.Items.Add(new ListItem("Seleccione una empresa", ""));

                    string response = client.DownloadString("https://localhost:44327/api/empresas");
                    var empresas = new JavaScriptSerializer().Deserialize<List<EmpresaResponse>>(response);

                    // Luego agregamos las empresas
                    foreach (var empresa in empresas)
                    {
                        ddlEmpresa.Items.Add(new ListItem(empresa.Nombre, empresa.Id.ToString()));
                    }

                    // Asegurarnos de que se seleccione el item vacío
                    ddlEmpresa.SelectedValue = "";
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error al cargar las empresas: " + ex.Message;
                }
            }
        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpiar el dropdown de sucursales
            ddlSucursal.Items.Clear();
            ddlSucursal.Items.Add(new ListItem("Seleccione una sucursal", ""));

            if (!string.IsNullOrEmpty(ddlEmpresa.SelectedValue))
            {
                CargarSucursales(int.Parse(ddlEmpresa.SelectedValue));
            }
        }

        private void CargarSucursales(int empresaId)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                try
                {
                    string url = $"https://localhost:44327/api/sucursales/empresa/{empresaId}";
                    System.Diagnostics.Debug.WriteLine($"Cargando sucursales para empresa ID: {empresaId}");

                    string response = client.DownloadString(url);
                    var sucursales = new JavaScriptSerializer().Deserialize<List<SucursalResponse>>(response);

                    // Limpiar y agregar el item por defecto
                    ddlSucursal.Items.Clear();
                    ddlSucursal.Items.Add(new ListItem("Seleccione una sucursal", ""));

                    // Agregar las sucursales
                    foreach (var sucursal in sucursales)
                    {
                        ddlSucursal.Items.Add(new ListItem(sucursal.Nombre, sucursal.Id.ToString()));
                    }

                    // Asegurarnos de que se seleccione el item vacío
                    ddlSucursal.SelectedValue = "";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al cargar sucursales: {ex.Message}");
                    lblError.Text = $"Error al cargar las sucursales: {ex.Message}";
                }
            }
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int empresaId = int.Parse(ddlEmpresa.SelectedValue);
                int sucursalId = int.Parse(ddlSucursal.SelectedValue);
                Session["EmpresaId"] = empresaId;
                Session["SucursalId"] = sucursalId;
                Response.Redirect("Facturacion.aspx");
            }
        }
    }

    public class EmpresaResponse
    {
        public int Id { get; set; }
        public string RUC { get; set; }
        public string Nombre { get; set; }
    }

    public class SucursalResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Corregimiento { get; set; }
        public string Urbanizacion { get; set; }
        public string Calle { get; set; }
        public string Local { get; set; }
        public EmpresaResponse Empresa { get; set; }
    }

}