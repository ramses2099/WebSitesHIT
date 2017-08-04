<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageV2.master" AutoEventWireup="true" CodeFile="Consignee.aspx.cs" Inherits="Consignee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style3 {
            width: 74px;
        }

        .auto-style5 {
            width: 63px;
        }

        .auto-style6 {
        }

        .auto-style8 {
            width: 8px;
        }

        .auto-style10 {
            width: 128px;
        }

        .auto-style11 {
            width: 21px;
        }

        .auto-style15 {
            height: 44px;
        }

        .auto-style16 {
            width: 60px;
        }

        .auto-style18 {
        }

        .auto-style19 {
            width: 13px;
        }

        .auto-style21 {
            width: 561px;
        }

        .auto-style22 {
            width: 34px;
        }

        .GridViewStyle {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Consignee</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>

    <div class="row">
        <div class="col-lg-6">
            <div class="form-group">
                <label for="TextBox1" class="col-sm-2 control-label">DGACode:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="10" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="TextBox1_AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                        Enabled="True" ServiceMethod="GetListofDGACode" MinimumPrefixLength="1" EnableCaching="true"
                        ServicePath="" TargetControlID="TextBox1">
                    </asp:AutoCompleteExtender>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtVessel" class="col-sm-2 control-label">Paquete:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox3" runat="server" MaxLength="4" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-6">
            <div class="form-group">
                <label for="TextBox2" class="col-sm-2 control-label">Mercancía:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox2" runat="server" MaxLength="30" Width="350px" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="TextBox2_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                        Enabled="True" ServiceMethod="GetListofMercancia" MinimumPrefixLength="1" EnableCaching="true"
                        ServicePath="" TargetControlID="TextBox2">
                    </asp:AutoCompleteExtender>
                </div>
            </div>


        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <label for="TextBox4" class="col-sm-2 control-label">PaqueteMedida:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox4" runat="server" MaxLength="20" Width="350px" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="TextBox4_AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                        Enabled="True" ServiceMethod="GetListofPaqMedida" MinimumPrefixLength="1" EnableCaching="true"
                        ServicePath="" TargetControlID="TextBox4">
                    </asp:AutoCompleteExtender>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtVessel" class="col-sm-2 control-label">Medida:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox5" runat="server" MaxLength="4" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtVessel" class="col-sm-2 control-label">UnidadMedida:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox6" runat="server" MaxLength="20" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="TextBox6_AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                        Enabled="True" ServiceMethod="GetListofUnidMedida" MinimumPrefixLength="1" EnableCaching="true"
                        ServicePath="" TargetControlID="TextBox6">
                    </asp:AutoCompleteExtender>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-6">
            <div class="form-group">
                <label for="TextBox8" class="col-sm-2 control-label">Consignatario:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox8" runat="server" MaxLength="50" Width="350px" CssClass="form-control" Style="text-transform: uppercase"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtVessel" class="col-sm-2 control-label">RNC:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox7" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="TextBox7_AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                        Enabled="True" ServiceMethod="GetListofUnidMedida" MinimumPrefixLength="1" EnableCaching="true"
                        ServicePath="" TargetControlID="TextBox6">
                    </asp:AutoCompleteExtender>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-1"></div>
        <div class="col-lg-6">
            <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="New" Value="New"></asp:MenuItem>
                    <asp:MenuItem Text="|" Value="|"></asp:MenuItem>
                    <asp:MenuItem Text="Save" Value="Save"></asp:MenuItem>
                    <asp:MenuItem Text="|" Value="|"></asp:MenuItem>
                    <asp:MenuItem Text="Cancel" Value="Cancel"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            &nbsp;
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            <asp:Label ID="lblIDUpdate" runat="server" Text="lblIDUpdate" Visible="False"></asp:Label>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            <asp:Label ID="lblTitulo" runat="server" Text="lblTitulo" Visible="False"></asp:Label>
        </div>
        <div class="col-lg-4">
            <asp:Label ID="lblMessage" runat="server" Text="lblMessage" Visible="False"></asp:Label>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Buscar                       
                </div>
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-1"></div>
                        <div class="col-lg-3">
                            <div class="form-group">
                                <asp:TextBox ID="txtFind" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <div class="form-group">
                                <asp:DropDownList ID="DropDownListFields" runat="server" CssClass="form-control">
                                    <asp:ListItem>--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-1">
                            <div class="form-group">
                                <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" Width="77px" CssClass="btn btn-default" />
                            </div>
                        </div>
                        <div class="col-lg-1">
                            <div class="form-group">
                                <asp:Menu ID="Menu2" runat="server" OnMenuItemClick="Menu2_MenuItemClick" Orientation="Horizontal">
                                    <Items>
                                        <asp:MenuItem Text="Refresh" Value="Refresh"></asp:MenuItem>
                                    </Items>
                                </asp:Menu>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">

                            <div id="DataDiv" style="overflow: auto; width: 100%; height: 259px;"
                                onscroll="Onscrollfnction();">
                                <asp:GridView ID="GridView1" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover"
                                    EmptyDataText="Do not found!" OnPageIndexChanging="GridView1_PageIndexChanging"
                                    OnRowCommand="GridView1_RowCommand" PageSize="20" Width="100%" Height="165px" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                                   <%-- <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />--%>
                                    <Columns>
                                        <%--                        <asp:TemplateField>
                            <ItemTemplate>
                             <asp:LinkButton ID="LkB1" runat="server" CommandName="Edit">Edit</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                             <asp:LinkButton ID="LB2" runat="server" CommandName="Update">Update</asp:LinkButton>
                             <asp:LinkButton ID="LB3" runat="server" CommandName="Cancel">Cancel</asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="ID">
                                            <ControlStyle Height="20px" Width="30px" />
                                            <ItemTemplate>
                                                &nbsp;
                                <asp:LinkButton ID="LinkButton2" runat="server" BorderStyle="None"
                                    CommandArgument='<%# Bind("ID") %>' CommandName="Update"
                                    Font-Names="Arial" Font-Size="7pt" ForeColor="Navy" Text='<%# Eval("ID") %>'
                                    Width="20px" OnClientClick="return confirm('¿Desea editar el registro?');">Update</asp:LinkButton>
                                                <asp:Label ID="Label" runat="server" Text='<%# Eval("ID") %>' Visible="False"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DGA">
                                            <ControlStyle Height="20px" Width="50px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("DGA")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MERCANCIA">
                                            <ControlStyle Height="20px" Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("MERCANCIA")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PAQUETE">
                                            <ControlStyle Height="20px" Width="20px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("PAQUETE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PAQMEDIDA">
                                            <ControlStyle Height="20px" Width="20px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("PAQMEDIDA")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MEDIDA">
                                            <ControlStyle Height="20px" Width="20px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("MEDIDA")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UNIDMEDIDA">
                                            <ControlStyle Height="20px" Width="20px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("UNIDMEDIDA")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RNC">
                                            <ControlStyle Height="20px" Width="50px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("RNC")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CONSIGNATARIO">
                                            <ControlStyle Height="20px" Width="200px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("CONSIGNATARIO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Image/Icon/DeleteHS.png"
                                            HeaderText="Delete" ShowDeleteButton="True" ShowHeader="True" Visible="False" />
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server"
                                                    CommandArgument='<%# Eval("ID")%>' CommandName="Delete"
                                                    ImageUrl="~/Image/Icon/DeleteHS.png"
                                                    OnClientClick="return confirm('¿Desea eliminar el registro?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
                                </asp:ScriptManager>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

