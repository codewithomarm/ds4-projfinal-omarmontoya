<%@ Page Title="Eliminar Producto" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EliminarProducto.aspx.cs" Inherits="App_Facturacion.EliminarProducto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4">Eliminar Producto</h2>
        <div class="row">
            <div class="col-md-6">
                <div class="mb-3">
                    <label for="<%= txtProductoId.ClientID %>" class="form-label">ID del Producto</label>
                    <asp:TextBox ID="txtProductoId" runat="server" CssClass="form-control" TextMode="Number" required></asp:TextBox>
                </div>
                <asp:Button ID="btnBuscarProducto" runat="server" Text="Buscar Producto" CssClass="btn btn-primary" OnClick="btnBuscarProducto_Click" />
            </div>
        </div>
        <asp:Panel ID="pnlProductoInfo" runat="server" Visible="false" CssClass="mt-4">
            <h3>Información del Producto</h3>
            <div class="row">
                <div class="col-md-6">
                    <p><strong>Nombre:</strong> <asp:Label ID="lblNombre" runat="server"></asp:Label></p>
                    <p><strong>Categoría:</strong> <asp:Label ID="lblCategoria" runat="server"></asp:Label></p>
                    <p><strong>Subcategoría:</strong> <asp:Label ID="lblSubcategoria" runat="server"></asp:Label></p>
                    <p><strong>Precio:</strong> <asp:Label ID="lblPrecio" runat="server"></asp:Label></p>
                </div>
                <div class="col-md-6">
                    <p><strong>Marca:</strong> <asp:Label ID="lblMarca" runat="server"></asp:Label></p>
                    <p><strong>Stock:</strong> <asp:Label ID="lblStock" runat="server"></asp:Label></p>
                    <p><strong>Código de Barras:</strong> <asp:Label ID="lblCodigoBarras" runat="server"></asp:Label></p>
                </div>
            </div>
            <div class="mt-3">
                <asp:Button ID="btnEliminarProducto" runat="server" Text="Eliminar Producto" CssClass="btn btn-danger" OnClick="btnEliminarProducto_Click" OnClientClick="return confirm('¿Está seguro de que desea eliminar este producto?');" />
            </div>
        </asp:Panel>
        <div class="mt-3">
            <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-secondary" OnClick="btnVolver_Click" CausesValidation="false" />
        </div>
    </div>
</asp:Content>
