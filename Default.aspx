<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/MasterPageV2.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Menu Principal</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <!-- /.row -->
    <div class="row">
        <div class="col-lg-4 col-md-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-home fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">1</div>
                            <div>Home</div>
                        </div>
                    </div>
                </div>
                <a href="Default.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">Inicio</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-4 col-md-6">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-gear fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">2</div>
                            <div>Containers</div>
                        </div>
                    </div>
                </div>
                <a href="wfrmConvertXMLtoEDI.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">Containers</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-4 col-md-6">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-book fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">3</div>
                            <div>Bookings</div>
                        </div>
                    </div>
                </div>
                <a href="wfrmBookings.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">Bookings</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
    </div>
    <!-- /.row -->
    <!-- /.row -->
    <div class="row">
        <div class="col-lg-4 col-md-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-user fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">4</div>
                            <div>Consignee</div>
                        </div>
                    </div>
                </div>
                <a href="Consignee.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">Consignee</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-4 col-md-6">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-cubes fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">5</div>
                            <div>OEA</div>
                        </div>
                    </div>
                </div>
                <a href="OEACustomers.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">OEA</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-4 col-md-6">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-photo fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">6</div>
                            <div>About</div>
                        </div>
                    </div>
                </div>
                <a href="About.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">About</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
    </div>
    <!-- /.row -->
</asp:Content>
