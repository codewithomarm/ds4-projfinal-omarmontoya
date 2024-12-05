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
                RedirectBasedOnRole();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Text;

            var (isValid, role) = ValidarUsuario(usuario, contrasena);

            if (isValid)
            {
                FormsAuthentication.SetAuthCookie(usuario, false);
                Session["UserRole"] = role;
                RedirectBasedOnRole();
            }
            else
            {
                lblError.Text = "Usuario o contraseña incorrectos.";
            }
        }

        private void RedirectBasedOnRole()
        {
            string role = Session["UserRole"] as string;
            if (role == "ADMIN")
            {
                Response.Redirect("~/SeleccionEmpresaAdmin.aspx");
            }
            else
            {
                Response.Redirect("~/SeleccionEmpresa.aspx");
            }
        }

        private (bool isValid, string role) ValidarUsuario(string usuario, string contrasena)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                // Step 1: Verify password
                string verifyUrl = "https://localhost:44327/api/usuarios/verificar-contrasena";
                string verifyData = new JavaScriptSerializer().Serialize(new
                {
                    NombreUsuario = usuario,
                    Contrasena = contrasena
                });

                try
                {
                    System.Diagnostics.Debug.WriteLine($"Sending verify password request to: {verifyUrl}");
                    System.Diagnostics.Debug.WriteLine($"Verify password request data: {verifyData}");

                    string verifyResponse = client.UploadString(verifyUrl, "POST", verifyData);
                    System.Diagnostics.Debug.WriteLine("Verify Password API Response: " + verifyResponse);

                    var verifyResult = new JavaScriptSerializer().Deserialize<dynamic>(verifyResponse);

                    if (verifyResult != null && verifyResult.ContainsKey("isValid") && (bool)verifyResult["isValid"])
                    {
                        // Step 2: Get user role
                        string getRoleUrl = "https://localhost:44327/api/usuarios/buscar";
                        string getRoleData = new JavaScriptSerializer().Serialize(new
                        {
                            nombreUsuario = usuario
                        });

                        System.Diagnostics.Debug.WriteLine($"Sending get role request to: {getRoleUrl}");
                        System.Diagnostics.Debug.WriteLine($"Get role request data: {getRoleData}");

                        // Asegurarse de que el Content-Type esté configurado para cada solicitud
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        string getRoleResponse = client.UploadString(getRoleUrl, "POST", getRoleData);
                        System.Diagnostics.Debug.WriteLine("Get Role API Response: " + getRoleResponse);

                        var userInfo = new JavaScriptSerializer().Deserialize<dynamic>(getRoleResponse);

                        if (userInfo != null && userInfo.ContainsKey("rol") && userInfo["rol"].ContainsKey("nombre"))
                        {
                            string role = userInfo["rol"]["nombre"];
                            System.Diagnostics.Debug.WriteLine($"User role: {role}");
                            return (true, role);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("User role information not found in the response");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid or unexpected verify password response format");
                    }

                    return (false, "USER");
                }
                catch (WebException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"WebException: {ex.Message}");
                    if (ex.Response != null)
                    {
                        using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            string errorResponse = reader.ReadToEnd();
                            System.Diagnostics.Debug.WriteLine("API Error Response: " + errorResponse);
                        }
                    }
                    return (false, "USER");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"General Exception: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return (false, "USER");
                }
            }
        }


    }
}