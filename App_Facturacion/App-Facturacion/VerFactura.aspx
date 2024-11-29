<%@ Page Title="Ver Factura" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerFactura.aspx.cs" Inherits="App_Facturacion.VerFactura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="card">
            <div class="card-body">
                <!-- Información de la Empresa -->
                <div class="text-center mb-4">
                    <asp:Label ID="lblEmpresaNombre" runat="server" CssClass="d-block fw-bold"></asp:Label>
                    <asp:Label ID="lblEmpresaRUC" runat="server" CssClass="d-block"></asp:Label>
                    <asp:Label ID="lblSucursalDireccion1" runat="server" CssClass="d-block"></asp:Label>
                    <asp:Label ID="lblSucursalDireccion2" runat="server" CssClass="d-block"></asp:Label>
                    <asp:Label ID="lblSucursalDireccion3" runat="server" CssClass="d-block"></asp:Label>
                </div>

                <hr class="border-2 border-dark" />

                <!-- Información de la Factura -->
                <div class="mb-3">
                    <asp:Label ID="lblSucursalNombre" runat="server" CssClass="d-block"></asp:Label>
                    <asp:Label ID="lblNumeroFactura" runat="server" CssClass="d-block"></asp:Label>
                </div>

                <hr class="border-2 border-dark" />

                <div class="row mb-3">
                    <div class="col-6">
                        <asp:Label ID="lblFecha" runat="server"></asp:Label>
                    </div>
                    <div class="col-6 text-end">
                        <asp:Label ID="lblHora" runat="server"></asp:Label>
                    </div>
                </div>

                <hr class="border-2 border-dark" />

                <div class="text-center mb-3">
                    <h4>FACTURA</h4>
                </div>

                <hr class="border-2 border-dark" />

                <!-- Productos -->
                <div class="mb-4">
                    <asp:Repeater ID="rptProductos" runat="server">
                        <ItemTemplate>
                            <div class="mb-3">
                                <div><%# Eval("CodigoBarras") %></div>
                                <div><%# Eval("Nombre") %></div>
                                <div class="row">
                                    <div class="col-8">
                                        <%# Eval("Cantidad") %> x <%# Eval("Precio", "{0:C2}") %>
                                    </div>
                                    <div class="col-4 text-end">
                                        <%# Eval("Subtotal", "{0:C2}") %>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <hr class="border-2 border-dark" />

                <!-- Totales -->
                <div class="row">
                    <div class="col-8">
                        <div>SUBTOTAL</div>
                        <div>DESCUENTOS</div>
                        <div>ITBMS</div>
                        <div class="fw-bold">TOTAL</div>
                    </div>
                    <div class="col-4 text-end">
                        <asp:Label ID="lblSubtotal" runat="server" CssClass="d-block"></asp:Label>
                        <asp:Label ID="lblDescuento" runat="server" CssClass="d-block"></asp:Label>
                        <asp:Label ID="lblImpuesto" runat="server" CssClass="d-block"></asp:Label>
                        <asp:Label ID="lblTotal" runat="server" CssClass="d-block fw-bold"></asp:Label>
                    </div>
                </div>

                <hr class="border-2 border-dark" />

                <!-- Pie de Factura -->
                <div class="mb-3">
                    <asp:Label ID="lblTotalArticulos" runat="server" CssClass="d-block"></asp:Label>
                    <asp:Label ID="lblFechaHora" runat="server" CssClass="d-block"></asp:Label>
                </div>

                <hr class="border-2 border-dark" />
            </div>

            <!-- Botones -->
            <div class="card-footer">
                <div class="d-flex justify-content-between">
                    <asp:Button ID="btnVolverMenu" runat="server" Text="Volver a Menú Principal" 
                        CssClass="btn btn-secondary" OnClick="btnVolverMenu_Click" />
                    <asp:Button ID="btnVolverFacturacion" runat="server" Text="Volver a Facturación" 
                        CssClass="btn btn-primary" OnClick="btnVolverFacturacion_Click" />
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir Factura" 
                        CssClass="btn btn-success" OnClientClick="window.print(); return false;" />
                </div>
            </div>
        </div>
    </div>

    <!-- Estilos para impresión -->
    <style type="text/css" media="print">
        .card-footer {
            display: none !important;
        }
        .card {
            border: none !important;
        }
        @page {
            margin: 0.5cm;
        }
    </style>
</asp:Content>