<%@ Page Title="Menú Principal Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuPrincipalAdmin.aspx.cs" Inherits="App_Facturacion.MenuPrincipalAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .center-content {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            height: 100%;
        }
    </style>
    <div class="container mt-5" style="margin-top: 1rem !important;">
        <!-- Botón Volver -->
        <div class="row mb-4">
            <div class="col">
                <asp:Button ID="btnVolverSeleccionEmpresa" runat="server" Text="← Volver a Selección de Empresa" 
                    CssClass="btn btn-secondary" OnClick="btnVolverSeleccionEmpresa_Click" />
            </div>
        </div>
        <!-- Menú Principal Admin -->
        <h2 class="text-center mb-4">Menú Principal Administrador</h2>
        
        <div class="row justify-content-center d-grid gap-3 center-content">
            <asp:Button ID="btnAdministrarUsuarios" runat="server" Text="Administrar Usuarios" 
                CssClass="btn btn-primary btn-lg" OnClick="btnAdministrarUsuarios_Click" />

            <asp:Button ID="btnAdministrarProductos" runat="server" Text="Administrar Productos" 
                CssClass="btn btn-success btn-lg" OnClick="btnAdministrarProductos_Click" />

            <asp:Button ID="btnReportes" runat="server" Text="Reportes" 
                CssClass="btn btn-info btn-lg" OnClick="btnReportes_Click" />
        </div>
    </div>
</asp:Content>
