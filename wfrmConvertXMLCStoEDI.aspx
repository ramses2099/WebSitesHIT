<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageV2.master" AutoEventWireup="true" CodeFile="wfrmConvertXMLCStoEDI.aspx.cs" Inherits="wfrmConvertXMLCStoEDI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style3 {
            width: 100%;
        }

        .auto-style1 {
            height: 21px;
        }

        .auto-style2 {
            height: 91px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">General Cargo</h1>
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
                        <asp:Button ID="btnLoadXML" type="submit" runat="server" CssClass="btn btn-default" Text="Load XMLCS" OnClick="btnLoadXML_Click" />
                        <asp:Button ID="btnCreateEDI" runat="server" Text="Create EDI" CssClass="btn btn-default" OnClick="btnCreateEDI_Click" />
                        <asp:Button ID="btnCargaOCEANIS" runat="server" Text="OCEANIS" CssClass="btn btn-default" OnClick="btnCargaOCEANIS_Click" />
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
        <div class="col-lg-4">
            <div class="form-group">
                <label for="txtVessel" class="col-sm-2 control-label">Vessel:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtVessel" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="form-group">
                <label for="txtLine" class="col-sm-2 control-label">Line:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtLine" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="form-group">
                <label for="txtVoyage" class="col-sm-2 control-label">Voyage:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtVoyage" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-4">
            <div class="form-group">
                <label for="txtCallsign" class="col-sm-2 control-label">LloydsId:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtCallsign" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="form-group">
                <label for="txtManifestNo" class="col-sm-2 control-label">ManifestNo:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtManifestNo" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="form-group">
                <label for="txtNaviera" class="col-sm-2 control-label">Naviera:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtNaviera" runat="server" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
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
                <li class="list-group-item active">General Cargo Details</li>
                <li class="list-group-item">
                    <asp:Label ID="Label1" runat="server" Text="ManifestBL"></asp:Label></li>
                <li class="list-group-item">
                    <asp:Label ID="Label6" runat="server" Text="ShippersConsignees"></asp:Label></li>
                <li class="list-group-item">
                    <asp:Label ID="Label2" runat="server" Text="RoutingPoint"></asp:Label></li>
                <li class="list-group-item">
                    <asp:Label ID="Label7" runat="server" Text="FamiliaMercancía"></asp:Label></li>
                <li class="list-group-item">
                    <asp:Label ID="Label4" runat="server" Text="ContainerBL" Visible="False"></asp:Label></li>
                <li class="list-group-item">
                    <asp:Label ID="Label3" runat="server" Text="ManifestVehicle" Visible="False"></asp:Label></li>
            </ul>
        </div>
    </div>


    <asp:ScriptManager ID="ScriptManager1" runat="server" OnAsyncPostBackError="ScriptManager1_AsyncPostBackError">
    </asp:ScriptManager>
    <script type="text/javascript" language="javascript">
        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_endRequest(endRequest);
        }

        function endRequest(sender, args) {
            if (args.get_error() != null) {
                alert(args.get_error().message);
                args.set_errorHandled(true);
            }
        }
    </script>
</asp:Content>

