<%@ Page Title="Menú Principal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuPrincipal.aspx.cs" Inherits="App_Facturacion.MenuPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="text-center mb-4">Menú Principal</h2>
        
        <div class="row justify-content-center">
            <div class="col-md-6 text-center">
                <div class="d-grid gap-3">
                    <asp:Button ID="btnVerProductos" runat="server" Text="Ver Productos" 
                        CssClass="btn btn-primary btn-lg" OnClick="btnVerProductos_Click" />
                    
                    <asp:Button ID="btnFacturar" runat="server" Text="Facturar Productos" 
                        CssClass="btn btn-success btn-lg" OnClick="btnFacturar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>