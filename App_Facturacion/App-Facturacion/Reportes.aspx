<%@ Page Title="Reportes de Ventas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="App_Facturacion.Reportes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4">Reportes de Ventas</h2>

        <!-- Información de Empresa y Sucursal -->
        <div class="card mb-4">
            <div class="card-body">
                <h5 class="card-title">Información de la Empresa</h5>
                <asp:Label ID="lblEmpresa" runat="server" CssClass="d-block"></asp:Label>
                <asp:Label ID="lblSucursal" runat="server" CssClass="d-block"></asp:Label>
            </div>
        </div>

        <!-- Formulario de selección de fechas -->
        <div class="card mb-4">
            <div class="card-body">
                <h5 class="card-title">Selección de Fechas</h5>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="txtFechaInicial">Fecha Inicial:</label>
                            <asp:TextBox ID="txtFechaInicial" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="txtFechaFinal">Fecha Final:</label>
                            <asp:TextBox ID="txtFechaFinal" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>&nbsp;</label>
                            <asp:Button ID="btnGenerarReporte" runat="server" Text="Generar Reporte" CssClass="btn btn-primary d-block w-100" OnClick="btnGenerarReporte_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Resumen de Ventas -->
        <asp:Panel ID="pnlResumenVentas" runat="server" Visible="false">
            <div class="card mb-4">
                <div class="card-body">
                    <h5 class="card-title">Resumen de Ventas</h5>
                    <div class="row">
                        <div class="col-md-3">
                            <strong>Subtotal:</strong> <asp:Label ID="lblSubtotal" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <strong>Impuestos:</strong> <asp:Label ID="lblImpuestos" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <strong>Descuentos:</strong> <asp:Label ID="lblDescuentos" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <strong>Gran Total:</strong> <asp:Label ID="lblGranTotal" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- Lista de Productos Vendidos -->
        <asp:Panel ID="pnlProductosVendidos" runat="server" Visible="false">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Productos Vendidos</h5>
                    <asp:GridView ID="gvProductosVendidos" runat="server" CssClass="table table-striped" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                            <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                            <asp:BoundField DataField="Subcategoria" HeaderText="Subcategoría" />
                            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="CantidadVendida" HeaderText="Cantidad Vendida" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
