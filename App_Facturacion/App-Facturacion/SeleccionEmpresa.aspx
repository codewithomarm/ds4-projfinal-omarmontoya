<%@ Page Title="Selección de Empresa y Sucursal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeleccionEmpresa.aspx.cs" Inherits="App_Facturacion.SeleccionEmpresa" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header bg-success text-white">
                        <h2 class="text-center mb-0">Selección de Empresa y Sucursal</h2>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="ddlEmpresa">Empresa:</label>
                            <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEmpresa" runat="server" 
                                ControlToValidate="ddlEmpresa" 
                                ErrorMessage="Debe seleccionar una empresa." 
                                Display="Dynamic" CssClass="text-danger" InitialValue="">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="ddlSucursal">Sucursal:</label>
                            <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                                ControlToValidate="ddlSucursal" 
                                ErrorMessage="Debe seleccionar una sucursal." 
                                Display="Dynamic" CssClass="text-danger" InitialValue="">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btn btn-success btn-block" OnClick="btnContinuar_Click" />
                        </div>
                        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>