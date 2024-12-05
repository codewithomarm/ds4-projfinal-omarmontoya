<%@ Page Title="Facturación" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Facturacion.aspx.cs" Inherits="App_Facturacion.Facturacion" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <!-- Botón Volver -->
        <div class="row mb-4">
            <div class="col">
                <asp:Button ID="btnVolverMenuPrincipal" runat="server" Text="← Volver al Menú Principal" 
                    CssClass="btn btn-secondary" OnClick="btnVolverMenuPrincipal_Click" />
            </div>
        </div>

        <h2>Facturación</h2>
        
        <!-- Información de Empresa y Sucursal -->
        <div class="row mb-4">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Información de la Empresa</h5>
                        <asp:Label ID="lblEmpresa" runat="server" CssClass="form-control-plaintext"></asp:Label>
                        <asp:Label ID="lblSucursal" runat="server" CssClass="form-control-plaintext"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Descuentos</h5>
                        <asp:DropDownList ID="ddlDescuento" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDescuento_SelectedIndexChanged">
                            <asp:ListItem Text="Sin descuento" Value="0" />
                            <asp:ListItem Text="Descuento Jubilados (15%)" Value="0.15" />
                            <asp:ListItem Text="Descuento Gobierno (10%)" Value="0.10" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

        <!-- Búsqueda por Código de Barras -->
        <div class="card mb-4">
            <div class="card-header">
                <h5>Buscar por Código de Barras</h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-8">
                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" placeholder="Ingrese el código de barras"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnBuscarBarcode" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscarBarcode_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Búsqueda por Categorías -->
        <div class="card mb-4">
            <div class="card-header">
                <h5>Buscar por Categorías</h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-4">
                        <label>Categoría:</label>
                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label>Subcategoría:</label>
                        <asp:DropDownList ID="ddlSubcategoria" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSubcategoria_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label>Producto:</label>
                        <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:Button ID="btnAgregarProducto" runat="server" Text="Agregar" CssClass="btn btn-success mt-2" OnClick="btnAgregarProducto_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Productos Agregados -->
        <div class="card mb-4">
            <div class="card-header">
                <h5>Productos en la Factura</h5>
            </div>
            <div class="card-body">
                <asp:GridView ID="gvProductos" runat="server" CssClass="table table-striped" AutoGenerateColumns="false" 
                    OnRowCommand="gvProductos_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                        <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C2}" />
                        <asp:TemplateField HeaderText="Cantidad">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" Text='<%# Eval("Cantidad") %>' 
                                    Width="70px" AutoPostBack="true" OnTextChanged="txtCantidad_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C2}" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger btn-sm" 
                                    CommandName="Eliminar" CommandArgument='<%# Container.DataItemIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Totales -->
        <div class="card mb-4">
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6 offset-md-6">
                        <table class="table">
                            <tr>
                                <td>Subtotal:</td>
                                <td class="text-right">
                                    <asp:Label ID="lblSubtotal" runat="server" Text="$0.00"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Descuento:</td>
                                <td class="text-right">
                                    <asp:Label ID="lblDescuento" runat="server" Text="$0.00"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>ITBMS (7%):</td>
                                <td class="text-right">
                                    <asp:Label ID="lblImpuesto" runat="server" Text="$0.00"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><strong>Total:</strong></td>
                                <td class="text-right">
                                    <asp:Label ID="lblTotal" runat="server" Text="$0.00" CssClass="font-weight-bold"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnProcesarFactura" runat="server" Text="Procesar Factura" 
                            CssClass="btn btn-primary btn-lg btn-block" OnClick="btnProcesarFactura_Click" />
                    </div>
                </div>
            </div>
        </div>

        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
    </div>
</asp:Content>
