<%@ Page Title="About Us" Language="C#" MasterPageFile="~/MasterPageV2.master" AutoEventWireup="true"
    CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table.tablamultimedia {
            width: 230px; /*No Cambiar*/
            float: left; /*No Cambiar*/
        }

        .pirobox_content table, tbody, tr, th, td {
            margin: 0;
            padding: 0;
            border: none;
        }

        table.tablamultimedia td {
            padding: 5px; /*No Cambiar*/
        }

        span.piefoto {
            color: Red;
        }

        span.contenido {
            display: block; /*No Cambiar*/
            text-align: justify;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">About</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>

    <span class="contenido"><strong>Haina International Terminals, S. A</strong>., 
    es una empresa creada el 10 de Octubre del año 2000 por un grupo de empresarios 
    del área naviera, con el propósito de realizar y modernizar todas las 
    operaciones del Puerto de Río Haina, de esta manera colocarlo a la altura de los 
    puertos del área del Caribe, por ser este él más importante del país, realizando 
    una inversión millonaria para su modernización, según los estándares de comercio 
    internacional.<br />
        <br />
        En la actualidad estamos certificados BASC (La Alianza Empresarial para un 
    Comercio Seguro) y del Código PBIP (Protección de los Buques y de las 
    Instalaciones Portuarias) que son requerimientos establecidos por los organismos 
    internacionales que contribuyen a la facilitación y agilización del comercio, y 
    convertir este puerto en uno de los más seguros del área del Caribe.<br />
        <br />
        <strong>NUESTRA MISION<br />
        </strong>Ofrecer un servicio completo, como la mejor terminal marítima y 
    mantener la condición de mejor opción como destino de barcos de la República 
    Dominicana.<br />
        <br />
        <strong>NUESTRA VISION<br />
        </strong>Ser la operadora de Terminal líder en la República Dominicana en todos 
    los servicios concernientes a exportación e importación de carga y seguridad de 
    los buques, durante su permanencia en el Puerto de Haina.<br />
    </span>
    </p>

    <div class="row">
        <div class="col-lg-6">
            <span class="glyphicon glyphicon-user" aria-hidden="true"></span>
            Esta aplicacion fue desarrollada por Ing. Gerson Anthony Tejeda            
        </div>
    </div>
    <div class="row">
        <div class="col-lg-6">
            <span class="glyphicon glyphicon-user" aria-hidden="true"></span>
            Esta aplicacion fue Actualizada y Mejorada por Ing. Jose Emmanuel Encarnacion
        </div>
    </div>
    <div class="row">
        <div class="col-lg-4">
            &nbsp;
        </div>
    </div>
</asp:Content>
