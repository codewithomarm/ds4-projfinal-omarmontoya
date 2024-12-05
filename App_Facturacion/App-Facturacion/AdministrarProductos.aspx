<%@ Page Title="Administrar Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdministrarProductos.aspx.cs" Inherits="App_Facturacion.AdministrarProductos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <!-- Botón Volver -->
        <div class="row mb-4">
            <div class="col">
                <asp:Button ID="btnVolverMenuPrincipalAdmin" runat="server" Text="← Volver a Menu Principal" 
                    CssClass="btn btn-secondary" OnClick="btnVolverMenuPrincipalAdmin_Click" />
            </div>
        </div>
        <h2 class="mb-4">Administrar Productos</h2>
        <div class="row">
            <div class="col-md-3 mb-3">
                <asp:Button ID="btnListarProductos" runat="server" Text="Listar Productos" CssClass="btn btn-primary btn-block" OnClick="btnListarProductos_Click" />
            </div>
            <div class="col-md-3 mb-3">
                <asp:Button ID="btnEditarProductos" runat="server" Text="Editar Productos" CssClass="btn btn-secondary btn-block" OnClick="btnEditarProductos_Click" />
            </div>
            <div class="col-md-3 mb-3">
                <asp:Button ID="btnCrearProductos" runat="server" Text="Crear Productos" CssClass="btn btn-success btn-block" OnClick="btnCrearProductos_Click" />
            </div>
            <div class="col-md-3 mb-3">
                <asp:Button ID="btnEliminarProductos" runat="server" Text="Eliminar Productos" CssClass="btn btn-danger btn-block" OnClick="btnEliminarProductos_Click" />
            </div>
        </div>
    </div>
</asp:Content>