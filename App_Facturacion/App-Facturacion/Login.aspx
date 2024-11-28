<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="App_Facturacion.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h2 class="text-center mb-0">Iniciar Sesión</h2>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtUsuario">Usuario:</label>
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" 
                                ControlToValidate="txtUsuario" 
                                ErrorMessage="El usuario es requerido." 
                                Display="Dynamic" CssClass="text-danger">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="txtContrasena">Contraseña:</label>
                            <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvContrasena" runat="server" 
                                ControlToValidate="txtContrasena" 
                                ErrorMessage="La contraseña es requerida." 
                                Display="Dynamic" CssClass="text-danger">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
                        </div>
                        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
