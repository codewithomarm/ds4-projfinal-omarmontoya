<%@ Page Title="Crear Producto" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CrearProducto.aspx.cs" Inherits="App_Facturacion.CrearProducto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4">Crear Nuevo Producto</h2>
        <div class="row">
            <div class="col-md-6 mb-3">
                <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="col-md-6 mb-3">
                <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 mb-3">
                <label for="<%= txtCategoria.ClientID %>" class="form-label">Categoría</label>
                <asp:TextBox ID="txtCategoria" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="col-md-4 mb-3">
                <label for="<%= txtSubcategoria.ClientID %>" class="form-label">Subcategoría</label>
                <asp:TextBox ID="txtSubcategoria" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="col-md-4 mb-3">
                <label for="<%= txtMarca.ClientID %>" class="form-label">Marca</label>
                <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 mb-3">
                <label for="<%= txtUnidadMedida.ClientID %>" class="form-label">Unidad de Medida</label>
                <asp:TextBox ID="txtUnidadMedida" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="col-md-3 mb-3">
                <label for="<%= txtCantidad.ClientID %>" class="form-label">Cantidad</label>
                <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" TextMode="Number" step="0.01" min="0.01" required></asp:TextBox>
            </div>
            <div class="col-md-3 mb-3">
                <label for="<%= txtPrecio.ClientID %>" class="form-label">Precio</label>
                <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" TextMode="Number" step="0.01" min="0.01" required></asp:TextBox>
            </div>
            <div class="col-md-3 mb-3">
                <label for="<%= txtStock.ClientID %>" class="form-label">Stock</label>
                <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" min="0" required></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 mb-3">
                <label for="<%= txtCodigoBarras.ClientID %>" class="form-label">Código de Barras</label>
                <asp:TextBox ID="txtCodigoBarras" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="col-md-6 mb-3">
                <label for="<%= txtFoto.ClientID %>" class="form-label">URL de la Foto</label>
                <asp:TextBox ID="txtFoto" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <asp:Button ID="btnCrearProducto" runat="server" Text="Crear Producto" CssClass="btn btn-primary" OnClick="btnCrearProducto_Click" />
                <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-secondary ml-2" OnClick="btnVolver_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</asp:Content>

