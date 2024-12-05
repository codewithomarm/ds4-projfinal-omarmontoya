<%@ Page Title="Listar Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListarProductos.aspx.cs" Inherits="App_Facturacion.ListarProductos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4">Lista de Productos</h2>
        <asp:GridView ID="gvProductos" runat="server" CssClass="table table-striped" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                <asp:BoundField DataField="Subcategoria" HeaderText="Subcategoría" />
                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C2}" />
                <asp:BoundField DataField="Stock" HeaderText="Stock" />
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-secondary mt-3" OnClick="btnVolver_Click" />
    </div>
</asp:Content>
