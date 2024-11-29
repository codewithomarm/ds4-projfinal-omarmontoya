using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net;
using System.IO;

namespace App_Facturacion
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/MenuPrincipal.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Text;

            if (ValidarUsuario(usuario, contrasena))
            {
                FormsAuthentication.SetAuthCookie(usuario, false);
                Response.Redirect("~/MenuPrincipal.aspx");
            }
            else
            {
                lblError.Text = "Usuario o contraseña incorrectos.";
            }
        }

        private bool ValidarUsuario(string usuario, string contrasena)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string url = "https://localhost:44327/api/usuarios/verificar-contrasena";
                string data = new JavaScriptSerializer().Serialize(new
                {
                    NombreUsuario = usuario,
                    Contrasena = contrasena
                });

                try
                {
                    string response = client.UploadString(url, "POST", data);
                    // Imprimir la respuesta para debug
                    System.Diagnostics.Debug.WriteLine("API Response: " + response);

                    // Deserializar la respuesta directamente a un objeto anónimo
                    var result = new JavaScriptSerializer().Deserialize<dynamic>(response);

                    // Verificar si la propiedad existe antes de acceder a ella
                    if (result != null && result.ContainsKey("isValid"))
                    {
                        return (bool)result["isValid"];
                    }
                    else if (result != null && result.ContainsKey("IsValid"))
                    {
                        return (bool)result["IsValid"];
                    }

                    // Si no encontramos la propiedad esperada, retornamos false
                    return false;
                }
                catch (WebException ex)
                {
                    // Capturar y mostrar el error específico
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine("API Error: " + errorResponse);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error general: " + ex.Message);
                    return false;
                }
            }
        }
    }
}