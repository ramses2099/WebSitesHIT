<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageV2.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="OEACustomers.aspx.cs" Inherits="OEACustomers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">


    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">OEA Customers</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <!-- /.row -->
    <div class="row">
       <%-- <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    OEA                       
                </div>
                <div class="panel-body">
                    <div class="row">--%>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label for="TextBox1" class="col-sm-2 control-label">RNC</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" Width="350px" MaxLength="11"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-2 control-label">Customer</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server" MaxLength="50" Style="text-transform: uppercase"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label for="TextBox1" class="col-sm-2 control-label">Status:</label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DropDownList1" runat="server" Height="30px" Width="110px">
                                        <asp:ListItem>ACTIVE</asp:ListItem>
                                        <asp:ListItem>OBSOLETE</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-1"></div>
                        <div class="col-lg-6">
                            <div class="form-group">
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
                    </div>
                    <div class="row">
                        <div class="col-lg-1"></div>
                        <div class="col-lg-6">
                            <asp:Label ID="lblTitulo" runat="server" Text="lblTitulo" Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-1"></div>
                        <div class="col-lg-6">
                            <asp:Label ID="lblMessage" runat="server" Text="lblMessage" Visible="False"></asp:Label>
                        </div>
                    </div>
                    <!-- /.row (nested) -->
              <%--  </div>
                <!-- /.panel-body -->
            </div>
            <!-- /.panel -->
        </div>--%>
        <!-- /.col-lg-12 -->
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
                                <asp:Button ID="btnExportExcel" runat="server" OnClick="btnExportExcel_Click" Text="Exp To Excel" Width="100px" CssClass="btn btn-default" />
                            </div>
                        </div>


                        <div class="col-lg-3">
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
                            <div id="DataDiv" onscroll="Onscrollfnction();">
                                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover"
                                    AutoGenerateColumns="False" 
                                    EmptyDataText="Do not found!" OnPageIndexChanging="GridView1_PageIndexChanging"
                                    OnRowCommand="GridView1_RowCommand" PageSize="20" Height="165px">                                   
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID">
                                            <ControlStyle Height="20px" Width="30px" />
                                            <ItemTemplate>
                                                &nbsp;
                                    <asp:LinkButton ID="LinkButton2" runat="server" BorderStyle="None"
                                        CommandArgument='<%# Bind("ID") %>' CommandName="seleccionar"
                                        Font-Names="Arial" Font-Size="7pt" ForeColor="Navy" Text='<%# Eval("ID") %>'
                                        Width="20px"></asp:LinkButton>
                                                <asp:Label ID="Label" runat="server" Text='<%# Eval("ID") %>' Visible="False"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RNC">
                                            <ControlStyle Height="20px" Width="50px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("RNC")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CUSTOMER">
                                            <ControlStyle Height="20px" Width="220px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("CUSTOMER")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                            <ItemStyle HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="STATUS">
                                            <ControlStyle Height="20px" Width="70px" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="7pt"
                                                    Style="text-transform: uppercase;font-weight:bold;" Text='<%# Eval("STATUS")%>'></asp:Label>
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
                            </div>
                        </div>
                    </div>
                    <!-- /.row (nested) -->
                </div>
                <!-- /.panel-body -->
            </div>
            <!-- /.panel -->
        </div>
        <!-- /.col-lg-12 -->
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
</asp:Content>

