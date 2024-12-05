<%@ Page Title="Menú Principal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuPrincipal.aspx.cs" Inherits="App_Facturacion.MenuPrincipal" %>

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
        <!-- Menú Principal -->
        <h2 class="text-center mb-4">Menú Principal</h2>
        
        <div class="row justify-content-center d-grid gap-3 center-content">
            <asp:Button ID="btnVerProductos" runat="server" Text="Ver Productos" 
                CssClass="btn btn-primary btn-lg" OnClick="btnVerProductos_Click" />

            <asp:Button ID="btnFacturar" runat="server" Text="Facturar Productos" 
                CssClass="btn btn-success btn-lg" OnClick="btnFacturar_Click" />
        </div>
    </div>
</asp:Content>