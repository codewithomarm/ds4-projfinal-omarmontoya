<%@ Page Title="Ver Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerProductos.aspx.cs" Inherits="App_Facturacion.VerProductos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <!-- Botón Volver -->
        <div class="row mb-4">
            <div class="col">
                <asp:Button ID="btnVolver" runat="server" Text="← Volver al Menú Principal" 
                    CssClass="btn btn-secondary" OnClick="btnVolver_Click" />
            </div>
        </div>

        <!-- Filtros -->
        <div class="row mb-4">
            <div class="col-md-4">
                <div class="form-group">
                    <label for="ddlCategoria">Categoría:</label>
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-control" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label for="ddlSubcategoria">Subcategoría:</label>
                    <asp:DropDownList ID="ddlSubcategoria" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label>&nbsp;</label>
                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" 
                        CssClass="btn btn-primary d-block" OnClick="btnFiltrar_Click" />
                </div>
            </div>
        </div>

        <!-- Productos -->
        <div class="row">
            <asp:Repeater ID="rptProductos" runat="server">
                <ItemTemplate>
                    <div class="col-md-4 mb-4">
                        <div class="card h-100">
                            <img src='<%# $"https://localhost:44327/api/imageproxy?url={HttpUtility.UrlEncode(Eval("foto").ToString())}" %>'
                                class="card-img-top" 
                                alt='<%# Eval("nombre") %>'
                                style="height: 200px; object-fit: contain; padding: 10px;"
                                onerror="this.onerror=null; this.src='/Content/images/placeholder.png';">
                            <div class="card-body">
                                <h5 class="card-title"><%# Eval("nombre") %></h5>
                                <p class="card-text">
                                    <strong>Categoría:</strong> <%# Eval("categoria.nombre") %><br>
                                    <strong>Subcategoría:</strong> <%# Eval("subcategoria.nombre") %><br>
                                    <strong>Marca:</strong> <%# Eval("marca.nombre") %><br>
                                    <strong>Unidad:</strong> <%# Eval("unidadMedida") %><br>
                                    <strong>Cantidad:</strong> <%# Eval("cantidad") %><br>
                                    <strong>Precio:</strong> $<%# Eval("precio", "{0:N2}") %><br>
                                    <strong>Código de Barras:</strong> <%# Eval("codigoBarras") %>
                                </p>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Mensaje de error -->
        <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    </div>
</asp:Content>
