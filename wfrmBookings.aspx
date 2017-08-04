<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageV2.master" AutoEventWireup="true" CodeFile="wfrmBookings.aspx.cs" Inherits="wfrmBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Bookings</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <!-- /.row -->
    <div class="row">
        <div class="col-lg-12">
            <div class="row">
                <div class="col-lg-2">
                    <input id="MyFile" type="file" size="81" name="MyFile" runat="server" />
                </div>
                <div class="col-lg-4">
                    <div class="btn-group" role="group" aria-label="...">
                        <asp:Button ID="btnLoadXML" type="submit" runat="server" Text="Load XLS" OnClick="btnLoadXML_Click" CssClass="btn btn-default" />
                        <asp:Button ID="btnCreateEDI" runat="server" Text="Create EDI" OnClick="btnCreateEDI_Click" CssClass="btn btn-default" />
                    </div>
                    
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            &nbsp;
        </div>
    </div>

    <div class="row">
        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtVessel" class="col-sm-2 control-label">Vessel:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtVessel" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtVessel" class="col-sm-2 control-label">Line:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtLine" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>

    </div>

    <div class="row">
        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtVoyage" class="col-sm-2 control-label">Voyage:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtVoyage" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="col-lg-6">
            <div class="form-group">
                <label for="txtVoyage" class="col-sm-2 control-label">LloydsId:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtCallsign" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            &nbsp;
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            <asp:Panel ID="PanelMessage" Visible="False" runat="server" CssClass="alert alert-danger">
                <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                <span class="sr-only">Error:</span>
                <asp:Label ID="lblMessage" runat="server" Text="Message"></asp:Label>
            </asp:Panel>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            <asp:Panel ID="PanelPath" Visible="False" runat="server" CssClass="alert alert-success">
                <span class="glyphicon glyphicon glyphicon-ok" aria-hidden="true"></span>
                <span class="sr-only">Success:</span>
                <asp:Label ID="lblPath" runat="server" Text="Path"></asp:Label>                
            </asp:Panel>
            <asp:HiddenField ID="hfPath" runat="server" />
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            &nbsp;
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <ul class="list-group">
                <li class="list-group-item active">Bookings Details</li>
                <li class="list-group-item"><asp:Label ID="Label1" runat="server" Text="Bookings"></asp:Label></li>
            </ul>
        </div>
    </div>
    
</asp:Content>

