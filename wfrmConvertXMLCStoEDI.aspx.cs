//------------------------------------------------------------------------------
//'   Objetivo : Conversion de manifiestos Carga General Suelta XML a extension EDI     
//'      Fecha : 15 de Abril de 2011
//'    Alcance : Desarrollar un sistema de procesos automatizados
//'
//'      Autor : Gerson Tejeda 'Dom. Rep.'
//'      Creado: Gerson Tejeda
//' Modificado : 09/Abril/2014 10:03 PM
//' Modificado : 16/Mayo/2014 08:00 AM
//' Modificado : 10/Julio/2014 07:00 AM
//' Modificado : 21/Agosto/2014 18:41 PM
//' Modificado : 2016 March 03 08:41 AM
//' Terminado  : 24/Diciembre/2013 17:30 PM
//' Modificado : 2016 Octubre 26 08:41 AM
//'------------------------------------------------------------------------------

using System;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Net;
using System.Security.Principal;

public partial class wfrmConvertXMLCStoEDI : System.Web.UI.Page
{
    #region "PUBLIC"
    //Public Variable.-
    string result1, result2, line, voyageNo, vessel, BLNo, Container1, ContainerVehicle1, ContainerVehicle3;
    string callsign, ContainerNoLetter, ContainerNoNumber, ManifestNo, Container2, ContainerVehicle2, naviera, registroNo, terminal;
    int longitud, longitudContainer, longitudContainerVehicle, rowcountVehicle, ConsignorDocumentNoIndex, ConsigneeDocumentNoIndex, ConsignorNameIndex, ConsigneeNameIndex;

    string fecha = DateTime.Today.ToString("yyyyMMdd");
    string hora = DateTime.Now.ToString("HHmm");

    string dat = DateTime.Today.ToString("yy");
    string hour = DateTime.Now.ToString("HHmm");
    
    Guid secuencia;
    
    string columnnameconsignor = "ConsignorDocumentNo";
    string columnnameconsignee = "ConsigneeDocumentNo";
    string columnnameconsignorname = "ConsignorName";
    string columnnameconsigneename = "ConsigneeName";

    string strFileName;
    string strFilePath;
    string strFolder;
    string localComputerName;
    string IDPort;
    string Port;
    string NameConsignees;
    string NameConsignors;
    string IDConsignors;
    string IDConsignees;
    string IDFamilia;
    string CGSPaquete;
    string DGACode;
    string CGSMercancia;
    string CGSMedida;
    string CGSPaqMedida;
    string BL;
    string Action;
    string Msg;
    string Minor;
    int Calc;
    int Incremento = 1000;
    int IncQtyWgt = 2;
    int Mssg = 0;
    int CGSPaqueteFinal;
    int CGSResults;
    int CalcWgt;
    int CGSWeight;
    int Weight, WeightFirst, WeightLast;
    int CalcWgtFirst;
    double CalcWgtdbl;
    IPAddress[] localIPs;

    //DataSet
    DataSet ds = new DataSet("Books DataSet");

    //DataSet Excel.*
    DataSet dsMsExcelShippersConsignees = new DataSet("ExcelBooks DataSet ShippersConsignees");
    DataSet dsfm = new DataSet("Table DataSet Familia de Mercancía");
    DataSet dsCGS = new DataSet("Table DataSet Carga General Suelta");
    DataSet dsDistinctCGS = new DataSet("Table DataSet Carga General Suelta Distinct");
    DataSet dsShippersConsignees = new DataSet("Table DataSet Shippers & Consignees");
    DataSet dsRoutingPoint = new DataSet("Table DataSet RoutingPoint");
    DataSet dsOEACustomers = new DataSet("Table DataSet OEACustomers");

    //GridView
    GridView gvManifestBL = new GridView();
    GridView gvManifestVehicleBL = new GridView();
    GridView gvdsMsExcelShippersConsignees = new GridView();
    GridView gvfm = new GridView();
    GridView gvCGS = new GridView();
    GridView gvShippersConsignees = new GridView();
    GridView gvRoutingPoint = new GridView();
    GridView gvOEACustomers = new GridView();

    //private SqlConnection sqlCon = new SqlConnection("Server=TECN-07;Database=XMLOCEANIS;User ID=sa;Password=Jehova-07;");
    private SqlConnection sqlCon = new SqlConnection("Server=HIT-SQL01;Database=XML_DBF;User ID=ConsultHit;Password=Jehova-07;");
    private SqlCommand cmdsqlCon = new SqlCommand();

    //Familia de Mercancía
    private SqlConnection sqlConfm = new SqlConnection("Server=HIT-SQL01;Database=manifiestodb;User ID=Portal01;Password=Abcd.1234;");

    //Database apex Prod 
    //private SqlConnection sqlConCGS = new SqlConnection("Server=HIT-SQL01-1;Database=testn4;User ID=navistest;Password=navistest;");
    private SqlConnection sqlConCGS = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;");
    private SqlCommand SqlCmdCGS = new SqlCommand();
    private SqlCommand SqlCmdShippersConsignees = new SqlCommand();
    private SqlCommand SqlCmdRoutingPoint = new SqlCommand();
    private SqlCommand SqlCmdOEACustomers = new SqlCommand();

    //Carga General Suelta Disctinct
    //private SqlConnection sqlConDistinctCGS = new SqlConnection("Server=HIT-SQL01-1;Database=testn4;User ID=navistest;Password=navistest;");
    private SqlConnection sqlConDistinctCGS = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;");

    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.MyFile.Value.DefaultIfEmpty();
            this.lblMessage.Text = "";
            this.lblPath.Text = "";
        }
    }

    protected void btnLoadXML_Click(object sender, EventArgs e)
    {
        //Execute private function.....
        this.LoadingFile();
        this.LoadingXMLtoDataSet();
        //this.LoadingXLStoDataSetShippersConsignees();
        this.Familia();
        this.CargaGeneralSuelta();
        this.CargaGeneralSueltaDistinct();
        this.ShippersConsignees();
        this.RoutingPoint();
    }

    protected void btnCreateEDI_Click(object sender, EventArgs e)
    {
        if (this.lblPath.Text == "")
        {
            Jalert.MessageBoxError(this, "There not file");
            return;
        }
        else
        {
            //Execute function......

            this.LoadingXMLtoDataSet();
            //this.LoadingXLStoDataSetShippersConsignees();
            this.Familia();
            this.CargaGeneralSuelta();
            this.CargaGeneralSueltaDistinct();
            this.ShippersConsignees();
            this.RoutingPoint();

            //************************NAVIS************************//

            #region "Validate NAVIS"

            if (txtVessel.Text == "")
            {
                Jalert.MessageBoxError(this, "There are append field");
                return;
            }
            else
            {
                vessel = txtVessel.Text;
            }

            if (txtCallsign.Text == "")
            {
                Jalert.MessageBoxError(this, "There are append field");
                return;
            }
            else
            {
                callsign = txtCallsign.Text;
            }

            if (txtLine.Text == "")
            {
                Jalert.MessageBoxError(this, "There are append field");
                return;
            }
            else
            {
                line = txtLine.Text;
            }

            if (txtManifestNo.Text == "")
            {
                Jalert.MessageBoxError(this, "There are append field");
                return;
            }
            else
            {
                ManifestNo = txtManifestNo.Text;
            }

            if (txtVoyage.Text == "")
            {
                Jalert.MessageBoxError(this, "There are append field");
                return;
            }
            else
            {
                voyageNo = txtVoyage.Text;
            }

            # endregion

            ////************************OCEANIS**********************//

            #region "Validate OCEANIS"

            //if (txtNaviera.Text == "")
            //{
            //    this.lblMessage.Text = "There are append field";
            //    return;
            //}
            //else
            //{
            //    naviera = txtNaviera.Text;
            //}

            //if (txtRegistroNo.Text == "")
            //{
            //    this.lblMessage.Text = "There are append field";
            //    return;
            //}
            //else
            //{
            //    registroNo = txtRegistroNo.Text;
            //}

            //if (ddlTerminal.Text == "")
            //{
            //    this.lblMessage.Text = "There are append field";
            //    return;
            //}
            //else
            //{
            //    terminal = ddlTerminal.Text;
            //}

            # endregion

            //Creating File in directory.
            //string fileName = @"C:\HIT_EDI\IN\" + this.line + @"\310\" + ManifestNo + "_from_app.edi";

            string fileName = String.Format(PathHitEdi.Path310, this.line) + ManifestNo + "_from_app.edi";


            //>>>>>>>>>>>>>>>>>PROD ENVIROMENT<<<<<<<<<<<<<<<<<<<<<
            //string fileName = @"\\N4JOBS\HIT_EDI\IN\" + this.line + @"\310\" + ManifestNo + fecha + hora + "_carga_general_suelta.edi";

            //>>>>>>>>>>>>>>>>>TEST ENVIROMENT<<<<<<<<<<<<<<<<<<<<<
            //string fileName = @"\\TESTN41NODES\HIT_EDI\IN\" + this.line + @"\310\" + ManifestNo + fecha + hora + "_carga_general_suelta.edi";

            if (File.Exists(fileName))
            {
                //this.lblMessage.Text = "{0} already exists., " + fileName + "";
                Jalert.MessageBoxError(this, String.Format("{0} already exists., ", fileName));
                return;
            }
            using (StreamWriter sw = File.CreateText(fileName))
            {

                # region "Writing Container to Container"

                //Count to rows.-
                int rowcount = this.ds.Tables["ManifestBL"].Rows.Count;
                rowcount = rowcount + 1;
                for (int i = 0; i < rowcount - 1; i++)
                {
                    //Assign BLNo
                    //string s = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString();
                    this.BL = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString();
                    ////Search ContainerNo by BLNo.-
                    //DataRow foundRow = this.ds.Tables["ContainerBL"].Rows.Find(s);
                    //if (foundRow != null)
                    //{
                    //    BLNo = foundRow[0].ToString();
                    //    //if (BLNo == "SMLU2901586A")
                    //    //{
                    //    //    lblMessage.Text = "Found it";
                    //    //}
                    //this.ds.Tables["ManifestBL"].DefaultView.RowFilter = ("BLNo like '%" + BLNo + "%'");
                    this.gvManifestBL.DataSource = this.ds.Tables["ManifestBL"].DefaultView;
                    this.gvManifestBL.DataMember = "ManifestBL";
                    this.gvManifestBL.DataBind();
                    this.gvManifestBL.Rows.Count.ToString();

                    #region "Search Port ID by Port Code."

                    //Search Port ID by Port Code
                    if (this.dsRoutingPoint.Tables.Contains("Table"))
                    {
                        this.Port = this.gvManifestBL.Rows[0].Cells[4].Text.ToString();
                        this.dsRoutingPoint.Tables["Table"].DefaultView.RowFilter = ("UnLoc like '%" + this.Port + "%'");
                        this.gvRoutingPoint.DataSource = this.dsRoutingPoint.Tables["Table"].DefaultView;
                        this.gvRoutingPoint.DataBind();

                        if (this.gvRoutingPoint.Rows.Count == 0)
                        {
                            this.IDPort = "UNKNOWN";
                        }
                        else
                        {
                            if ((this.gvRoutingPoint.Rows[0].Cells[6].Text.ToString() == "") || (this.gvRoutingPoint.Rows[0].Cells[6].Text.ToString() == null) || (this.gvRoutingPoint.Rows[0].Cells[6].Text.ToString() == "&nbsp;"))
                            {
                                this.IDPort = "UNKNOWN";
                            }
                            else
                            {
                                this.IDPort = this.gvRoutingPoint.Rows[0].Cells[6].Text.ToString();
                            }

                        }
                    }

                    #endregion

                    #region "Search ShippersConsignees ID by Consignee Code."

                    //Search ShippersConsignees ID by Consignee Code
                    if (this.ds.Tables.Contains("ManifestBL"))
                    {
                        try
                        {
                            #region "CONSIGNEES & CONSIGNORS"
                            //Search index Column ConsigneeDocumentNo in table ManifestBL
                            foreach (DataColumn dc in ds.Tables["ManifestBL"].Columns)
                            {
                                //////////////////////////////////ConsigneeDocumentNo///////////////////////////////////
                                if (dc.ColumnName.ToLower().Trim() == columnnameconsignee.ToLower().Trim())
                                {
                                    this.ConsigneeDocumentNoIndex = this.ds.Tables["ManifestBL"].Columns.IndexOf(dc.ColumnName);
                                }
                                //////////////////////////////////ConsignorDocumentNo///////////////////////////////////
                                if (dc.ColumnName.ToLower().Trim() == columnnameconsignor.ToLower().Trim())
                                {
                                    this.ConsignorDocumentNoIndex = this.ds.Tables["ManifestBL"].Columns.IndexOf(dc.ColumnName);
                                }
                                //////////////////////////////////ConsigneeNAME///////////////////////////////////
                                if (dc.ColumnName.ToLower().Trim() == columnnameconsigneename.ToLower().Trim())
                                {
                                    this.ConsigneeNameIndex = this.ds.Tables["ManifestBL"].Columns.IndexOf(dc.ColumnName);
                                }
                                //////////////////////////////////ConsignorNAME///////////////////////////////////
                                if (dc.ColumnName.ToLower().Trim() == columnnameconsignorname.ToLower().Trim())
                                {
                                    this.ConsignorNameIndex = this.ds.Tables["ManifestBL"].Columns.IndexOf(dc.ColumnName);
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            this.Msg = "Can not load DataSet ds in ManifestBL for Search ShippersConsignees ID Index Fields in XML ManifestBL by Consignee Code Index : > " + this.ConsigneeDocumentNoIndex + " < or Consignor Code Index : > " + this.ConsignorDocumentNoIndex + " < of BL : > " + this.BL + "! " + ex.Message.ToString();
                            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
                            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
                            Jalert.MessageBoxError(this, this.Msg);
                        }
                        finally
                        {
                            /////////////////////Consignees////////////////////////
                            try
                            {
                                this.IDConsignees = this.gvManifestBL.Rows[i].Cells[ConsigneeDocumentNoIndex].Text.ToString();
                                /////////////////////UNKNOWN CONSIGNEES////////////////////
                                if ((this.IDConsignees == "") || (this.IDConsignees == null) || (this.IDConsignees == "&nbsp;"))
                                {
                                    this.IDConsignees = "UNKNOWN_CONSIGNEE_ID";
                                    this.NameConsignees = this.gvManifestBL.Rows[i].Cells[ConsigneeNameIndex].Text.ToString();
                                }
                                else
                                {
                                    /////////////////////KNOW CONSIGNEES////////////////////
                                    this.dsShippersConsignees.Tables["Table"].DefaultView.RowFilter = ("Id='" + this.IDConsignees + "'");
                                    {
                                        this.gvShippersConsignees.DataSource = this.dsShippersConsignees.Tables["Table"].DefaultView;
                                        this.gvShippersConsignees.DataBind();

                                        if (this.gvShippersConsignees.Rows.Count == 0)
                                        {
                                            this.NameConsignees = this.gvManifestBL.Rows[i].Cells[ConsigneeNameIndex].Text.ToString();
                                        }
                                        else
                                        {
                                            if ((this.gvShippersConsignees.Rows[0].Cells[2].Text.ToString() == "") || (this.gvShippersConsignees.Rows[0].Cells[2].Text.ToString() == null) || (this.gvShippersConsignees.Rows[0].Cells[2].Text.ToString() == "&nbsp;"))
                                            {
                                                this.NameConsignees = "UNKNOWN_CONSIGNEE_NAME";
                                            }
                                            else
                                            {
                                                this.NameConsignees = this.gvShippersConsignees.Rows[0].Cells[2].Text.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                this.Msg = "Can not load DataSet ds in ManifestBL by Search ShippersConsignees ID in CONSIGNEES by Consignee Code : > " + this.IDConsignees + " < ! " + ex.Message.ToString();
                                //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
                                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
                                this.NameConsignees = this.gvManifestBL.Rows[i].Cells[ConsigneeNameIndex].Text.ToString();
                                Jalert.MessageBoxError(this, this.Msg);
                            }
                            finally
                            {
                                ///////////////////////////Consignors/////////////////////////////
                                try
                                {
                                    this.IDConsignors = this.gvManifestBL.Rows[i].Cells[this.ConsignorDocumentNoIndex].Text.ToString();
                                    /////////////////////UNKNOWN CONSIGNORS////////////////////
                                    if ((this.IDConsignors == "") || (this.IDConsignors == null) || (this.IDConsignors == "&nbsp;"))
                                    {
                                        this.IDConsignors = "UNKNOWN_CONSIGNOR_ID";
                                        this.NameConsignors = this.gvManifestBL.Rows[i].Cells[ConsignorNameIndex].Text.ToString();
                                    }
                                    else
                                    {
                                        /////////////////////KNOW CONSIGNORS////////////////////
                                        this.dsShippersConsignees.Tables["Table"].DefaultView.RowFilter = ("Id='" + this.IDConsignors + "'");
                                        {
                                            this.gvShippersConsignees.DataSource = this.dsShippersConsignees.Tables["Table"].DefaultView;
                                            this.gvShippersConsignees.DataBind();

                                            if (this.gvShippersConsignees.Rows.Count == 0)
                                            {
                                                this.NameConsignors = this.gvManifestBL.Rows[i].Cells[ConsignorNameIndex].Text.ToString();
                                            }
                                            else
                                            {
                                                if ((this.gvShippersConsignees.Rows[0].Cells[2].Text.ToString() == "") || (this.gvShippersConsignees.Rows[0].Cells[2].Text.ToString() == null) || (this.gvShippersConsignees.Rows[0].Cells[2].Text.ToString() == "&nbsp;"))
                                                {
                                                    this.NameConsignors = "UNKNOWN_CONSIGNOR_NAME";
                                                }
                                                else
                                                {
                                                    this.NameConsignors = this.gvShippersConsignees.Rows[0].Cells[2].Text.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    this.Msg = "Can not load DataSet ds in ManifestBL by Search ShippersConsignees ID in CONSIGNOR by Consignor Code : > " + this.IDConsignors + " < ! " + ex.Message.ToString();
                                    //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
                                    //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
                                    this.NameConsignors = this.gvManifestBL.Rows[i].Cells[ConsignorNameIndex].Text.ToString();
                                    Jalert.MessageBoxError(this, this.Msg);
                                }
                                finally
                                {

                                }
                            }
                        }
                    }


                    #endregion

                    //////////////////////////Modificado : 29/Febrero/2016 16:00 PM///////////////////////////////////

                    #region "Search Familia by DGA Code."

                    //Search Familia by DGA Code
                    if (this.dsfm.Tables.Contains("Table"))
                    {
                        this.DGACode = this.gvManifestBL.Rows[i].Cells[6].Text.ToString();
                        this.dsfm.Tables["Table"].DefaultView.RowFilter = ("Codigos_DGA like '%" + this.DGACode + "%'");
                        this.gvfm.DataSource = this.dsfm.Tables["Table"].DefaultView;
                        this.gvfm.DataBind();

                        if (this.gvfm.Rows.Count == 0)
                        {
                            this.IDFamilia = "UNKNOWN";
                        }
                        else
                        {
                            if ((this.gvfm.Rows[0].Cells[0].Text.ToString() == "") || (this.gvfm.Rows[0].Cells[0].Text.ToString() == null) || (this.gvfm.Rows[0].Cells[0].Text.ToString() == "&nbsp;"))
                            {
                                this.IDFamilia = "UNKNOWN";
                            }
                            else
                            {
                                this.IDFamilia = this.gvfm.Rows[0].Cells[0].Text.ToString();
                            }

                        }
                    }

                    #endregion

                    //////////////////////////Modificado : 03/Marzo/2016 08:00 AM///////////////////////////////////

                    #region "Search Mercancía by DGACode + Mercancía + Medida + RNC."

                    //Search Familia by DGA Code
                    if (this.dsCGS.Tables.Contains("Table"))
                    {
                        try
                        {
                            if ((this.gvManifestBL.Rows[0].Cells[5].Text.ToString() != null) || (this.gvManifestBL.Rows[0].Cells[5].Text.ToString() != ""))
                            {
                                //Count to rows.-
                                int countCGSMercancia = this.dsCGS.Tables["Table"].Rows.Count;
                                countCGSMercancia = countCGSMercancia + 1;

                                for (int m = 0; m < countCGSMercancia - 1; m++)
                                {
                                    this.CGSPaqMedida = this.dsCGS.Tables["Table"].Rows[m].ItemArray[4].ToString().Trim();
                                    string WordIn = this.dsCGS.Tables["Table"].Rows[m].ItemArray[2].ToString().Trim();
                                    string GoodsName = this.gvManifestBL.Rows[i].Cells[5].Text.ToString();

                                    if (GoodsName.Contains(WordIn))
                                    {
                                        this.CGSMercancia = WordIn;

                                        if (this.dsCGS.Tables["Table"].Rows[m].ItemArray[5].ToString() != "1")
                                        {
                                            if (this.dsCGS.Tables["Table"].Rows[m].ItemArray[5].ToString() != "")
                                            {
                                                int firstMed = GoodsName.IndexOf(CGSMercancia + "-") + CGSMercancia.Length + 1;
                                                int lastMed = GoodsName.LastIndexOf(")");
                                                string MedOut = GoodsName.Substring(firstMed, lastMed - firstMed);
                                                this.CGSMedida = MedOut;

                                                break;
                                            }
                                        }
                                        else
                                        {
                                            this.CGSMedida = "1";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Msg = "Search Mercancía by DGACode : > " + this.DGACode + " < + Mercancía : > " + this.CGSMercancia + " < + Medida : > " + this.CGSMedida + " < + RNC : > " + this.IDConsignees.ToString() + " < ! " + ex.Message.ToString();
                            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
                            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
                            Jalert.MessageBoxError(this, this.Msg);
                        }
                        finally
                        {
                            try
                            {
                                string CGSRNC = this.IDConsignees.ToString();


                                if (this.dsCGS.Tables["Table"].Rows.Count >= 0)
                                {

                                    this.dsCGS.Tables["Table"].DefaultView.RowFilter = ("DGA = '" + this.DGACode + "' AND MERCANCIA = '" + this.CGSMercancia + "' AND MEDIDA = '" + this.CGSMedida + "' AND RNC = '" + CGSRNC + "'");
                                    this.gvCGS.DataSource = this.dsCGS.Tables["Table"].DefaultView;
                                    this.gvCGS.DataBind();
                                }

                                if (this.gvCGS.Rows.Count == 0)
                                {
                                    this.lblMessage.Visible = true;
                                    this.lblMessage.Text = "Consignatario RNC: " + this.IDConsignees.ToString() + ", NOMBRE: " + this.NameConsignees.ToString() + " presenta discrepancia en la tabla Carga General Suelta!";
                                }
                                else
                                {
                                    this.CGSPaquete = this.gvCGS.Rows[0].Cells[3].Text.ToString();
                                }
                            }
                            catch (Exception ex)
                            {
                                this.Msg = "Search Mercancía by DGACode : > " + this.DGACode + " < + Mercancía : > " + this.CGSMercancia + " < + Medida : > " + this.CGSMedida + " < + RNC : > " + this.IDConsignees.ToString() + " < ! " + ex.Message.ToString();
                                //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
                                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
                                Jalert.MessageBoxError(this, this.Msg);
                            }
                            finally
                            {
                                try
                                {
                                    //PackageQty de paquetes que esta en la base de datos
                                    int Qty = Int32.Parse(this.gvManifestBL.Rows[i].Cells[7].Text.ToString());
                                    int CGSQty = Int32.Parse(this.CGSPaquete);

                                    if (Qty <= CGSQty)
                                    {
                                        CGSQty = Qty;
                                        this.Minor = "Minor";
                                    }

                                    this.Calc = Qty / CGSQty;
                                    int Val = this.Calc * CGSQty;
                                    this.CGSPaqueteFinal = Qty - Val;
                                    this.IncQtyWgt = 2;

                                    //CGSPaqueteFinal equal 1
                                    if (this.CGSPaqueteFinal == 1)
                                    {
                                        this.Action = "Qty-Val=1";
                                        //this.IncQtyWgt = 1;
                                    }

                                    //GrossWeight
                                    int Wgt = Int32.Parse(this.gvManifestBL.Rows[i].Cells[8].Text.ToString());
                                    this.CalcWgt = Wgt / Qty;
                                    int Value = this.CalcWgt * Qty;
                                    this.CGSWeight = Wgt - Value;


                                    //PackageQty equal "int Val". It mean exactly!
                                    if ((Qty == Val) && (this.CGSPaqueteFinal == 0))
                                    {
                                        this.Calc = Qty / CGSQty;
                                        this.CGSPaqueteFinal = this.Calc;
                                        this.IncQtyWgt = 1;

                                        //To GrossWeight decimals!
                                        this.CalcWgtdbl = (double)Wgt / (double)Qty;
                                        double WeightFirstdbl = this.CalcWgtdbl * CGSQty;
                                        this.WeightFirst = (int)WeightFirstdbl;
                                    }
                                    else
                                    {
                                        //GrossWeight X Package
                                        this.WeightFirst = this.CalcWgt * CGSQty;
                                        this.WeightLast = this.CalcWgt * this.CGSPaqueteFinal + this.CGSWeight;
                                    }

                                    //GrossWeight equal "int Value". It mean exactly!
                                    if ((Wgt == Value) && (this.CGSWeight == 0) && (this.CGSPaqueteFinal == 0))
                                    {
                                        this.WeightLast = WeightFirst;
                                        this.IncQtyWgt = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    this.Msg = "Search Mercancía by DGACode : > " + this.DGACode + " < + Mercancía : > " + this.CGSMercancia + " < + Medida : > " + this.CGSMedida + " < + RNC : > " + this.IDConsignees.ToString() + " < ! " + ex.Message.ToString();
                                    //var exception = new Exception(this.lblMessage.Text);
                                    //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
                                    //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
                                    Jalert.MessageBoxError(this, this.Msg);
                                }
                                finally
                                {
                                    if (this.CGSPaqueteFinal == 0)
                                    {
                                        this.Calc = 0;
                                        this.CGSPaqueteFinal = 1;
                                        this.IncQtyWgt = 2;
                                    }

                                    if (this.CGSPaqMedida == "TUBERIA")
                                    {
                                        this.Calc = 0;
                                        this.CGSPaqueteFinal = 1;
                                        this.IncQtyWgt = 2;
                                    }

                                    if (this.Action == "Qty-Val=1")
                                    {
                                        this.CalcWgtFirst = this.WeightFirst / Int32.Parse(CGSPaquete);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    //ContainerNoLetter = this.ds.Tables["ManifestContainer"].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                    //ContainerNoNumber = this.ds.Tables["ManifestContainer"].Rows[i].ItemArray[0].ToString().Substring(4, 7);

                    #region "Height to Container"

                    if (this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() == null)
                    {
                        Container2 = "la variable esta en blanco";
                    }
                    else
                    {
                        longitudContainer = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Length;

                        //if (longitudContainer >= 12)
                        //{
                        //    Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                        //    Container2 = Container1.Substring(0, 11);
                        //    ContainerNoLetter = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                        //    ContainerNoNumber = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(4, 7);
                        //}
                        //if (longitudContainer == 11)
                        //{
                        //    Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                        //    Container2 = Container1.Substring(0, 11);
                        //    ContainerNoLetter = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                        //    ContainerNoNumber = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(4, 7);
                        //}
                        //if (longitudContainer == 10)
                        //{
                        //    Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                        //    Container2 = Container1.Substring(0, 10);
                        //    ContainerNoLetter = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                        //    ContainerNoNumber = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(4, 6);
                        //}
                        //if (longitudContainer == 9)
                        //{
                        //    Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                        //    Container2 = Container1.Substring(0, 9);
                        //    ContainerNoLetter = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                        //    ContainerNoNumber = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(4, 5);
                        //}
                        //if (longitudContainer >= 8)
                        //{
                        //    Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                        //    Container2 = Container1.Substring(longitudContainer - 8, 8);
                        //    ContainerNoLetter = this.Container2.Substring(0, 4);
                        //    ContainerNoNumber = this.Container2.Substring(4, 4);
                        //}

                        if (longitudContainer >= 7)
                        {
                            Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(longitudContainer - 6, 7);
                            ContainerNoLetter = this.Container2.Substring(0, 4);
                            ContainerNoNumber = this.Container2.Substring(4, 3);
                        }
                        if (longitudContainer == 6)
                        {
                            Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 6);
                            ContainerNoLetter = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(4, 2);
                        }
                        if (longitudContainer == 5)
                        {
                            Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 5);
                            ContainerNoLetter = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(4, 1);
                        }
                        if (longitudContainer == 4)
                        {
                            Container1 = " " + this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 4);
                            ContainerNoLetter = this.ds.Tables["ManifestBL"].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = "";
                        }
                    }

                    #endregion

                    //}

                    #region "Writing Container in EDI file"

                    if (this.Calc != null)
                    {
                        //Count to rows.-
                        int countCalc = this.Calc;
                        countCalc = countCalc + this.IncQtyWgt;

                        for (int p = 0; p < countCalc - 1; p++)
                        {
                            if (p == this.Calc)
                            {
                                this.CGSResults = this.CGSPaqueteFinal;

                                if (CGSPaqueteFinal == 1)
                                {
                                    if (this.Action == "Qty-Val=1")
                                    {
                                        this.Weight = this.CalcWgtFirst;
                                    }
                                    else
                                    {
                                        this.Weight = Int32.Parse(this.gvManifestBL.Rows[i].Cells[8].Text.ToString());
                                        this.CGSResults = Int32.Parse(this.gvManifestBL.Rows[i].Cells[7].Text.ToString());
                                    }
                                }
                                else
                                {
                                    this.Weight = WeightLast;
                                }
                            }
                            else
                            {
                                if (this.Minor == "Minor")
                                {
                                    this.CGSResults = Int32.Parse(this.gvManifestBL.Rows[i].Cells[7].Text.ToString());
                                    this.Minor = "";
                                }
                                else
                                {
                                    this.CGSResults = Int32.Parse(this.CGSPaquete);
                                }
                                this.Weight = WeightFirst;
                            }

                            this.Incremento = this.Incremento + 1;

                            if ((i == 0) && (p == 0))
                            {
                                //First line.-
                                sw.WriteLine("ISA*00*1000008   *00*          *ZZ*" + line + "            *ZZ*HIT            *060701*1911*U*00200*000012452*0*P*>~");
                                sw.WriteLine("GS*IO*" + line + "*HIT*" + fecha + "*" + hora + "*5*X*004020~");
                                sw.WriteLine("ST*310*" + this.Incremento + "~");
                                sw.WriteLine("B3*" + ManifestNo + "*" + this.gvManifestBL.Rows[i].Cells[0].Text.ToString() + "*" + this.gvManifestBL.Rows[i].Cells[6].Text.ToString() + "*PP**" + fecha + "*0000000****" + line + "****" + this.gvManifestBL.Rows[i].Cells[6].Text.ToString() + "*" + this.IDFamilia.ToString() + "*" + this.gvManifestBL.Rows[i].Cells[8].Text.ToString() + "~");
                                sw.WriteLine("B2A*00~");
                                sw.WriteLine("N9*BM*" + this.gvManifestBL.Rows[i].Cells[0].Text.ToString() + "~");
                                sw.WriteLine("V1*" + callsign + "*" + vessel + "*DE*" + voyageNo + "*" + line + "***L~");
                                sw.WriteLine("N1*CN*" + this.NameConsignees.ToString() + "**" + this.IDConsignees.ToString() + "~");
                                sw.WriteLine("N3*N/A~");
                                sw.WriteLine("N1*SH*" + this.NameConsignors.ToString() + "**" + this.IDConsignors.ToString() + "~");
                                sw.WriteLine("N3*N/A~");
                                sw.WriteLine("N1*N1*" + this.NameConsignees.ToString() + "**" + this.IDConsignees.ToString() + "~");
                                sw.WriteLine("N3*N/A~");
                                sw.WriteLine("R4*E*K*24741*HAINA TERMINAL~");
                                //sw.WriteLine("R4*R*K*22255*HKG TERMINAL~");
                                sw.WriteLine("R4*R*K*" + this.IDPort.ToString() + "*" + this.gvManifestBL.Rows[0].Cells[4].Text.ToString() + "~");
                                sw.WriteLine("R4*D*K*24741*HAINA TERMINAL~");
                                sw.WriteLine("R4*O*K*" + this.IDPort.ToString() + "*" + this.gvManifestBL.Rows[0].Cells[4].Text.ToString() + "~");
                                sw.WriteLine("LX*1~");

                                //secuencia
                                secuencia = Guid.NewGuid();
                                string sec = secuencia.ToString().Substring(0, 6);
                                
                                //sw.WriteLine("N7*CG" + this.dat + this.hour + "*" + this.Incremento + "*" + this.Weight + "*G****20*X********K*****CGS***" + this.CGSResults + "~");
                                sw.WriteLine("N7*CG" + sec.ToUpper() + "*" + this.Incremento + "*" + this.Weight + "*G****20*X********K*****CGS***" + this.CGSResults + "~");
                                
                                //sw.WriteLine("N7*CGS" + ContainerNoLetter + "*" + this.Incremento + "*" + this.Weight + "*G****20*X********K*****CG" + ContainerNoLetter + this.Incremento + "***" + this.CGSResults + "~");
                                //sw.WriteLine("N7*CGS" + ContainerNoLetter + "*" + ContainerNoNumber + "*" + this.gvManifestBL.Rows[0].Cells[8].Text.ToString() + "*G****20*X********K*****" + this.gvManifestBL.Rows[i].Cells[0].Text.ToString() + "~");
                                sw.WriteLine("M7*" + this.txtManifestNo.Text + "*" + this.txtNaviera.Text + "*" + this.txtVessel.Text + "~");
                                //sw.WriteLine("M7*" + this.txtManifestNo.Text + "*" + this.txtNaviera.Text + "*" + this.txtVessel.Text + "*" + this.txtRegistroNo.Text + "~");
                                //sw.WriteLine("M7*" + this.gvManifestBL.Rows[i].Cells[7].Text.ToString() + "~");
                                //sw.WriteLine("N7*" + ContainerNoLetter + "*" + ContainerNoNumber + "*" + this.ds.Tables["ManifestContainer"].Rows[i].ItemArray[5].ToString() + "*G****20*X********K*****" + this.ds.Tables["ManifestContainer"].Rows[i].ItemArray[0].ToString() + "~");
                                //sw.WriteLine("M7*" + this.ds.Tables["ManifestContainer"].Rows[i].ItemArray[7].ToString() + "~");
                                sw.WriteLine("L0*1*****20*X*609*PCS~");

                                if (this.ds.Tables["ManifestBL"].Rows[i].ItemArray[5].ToString() == null)
                                {
                                    result2 = "la variable en blanco";
                                }
                                else
                                {
                                    longitud = this.gvManifestBL.Rows[i].Cells[5].Text.ToString().Length;

                                    if (longitud >= 120)
                                    {
                                        result1 = " " + this.gvManifestBL.Rows[i].Cells[5].Text.ToString() + " ";
                                        result2 = result1.Substring(0, 120);
                                    }
                                    else
                                    {
                                        result2 = this.gvManifestBL.Rows[i].Cells[5].Text.ToString();
                                    }
                                }
                                sw.WriteLine("L5*1*" + result2 + "~");
                                sw.WriteLine("SE*19*3155~");
                            }
                            else
                            {
                                sw.WriteLine("ST*310*" + this.Incremento + "~");
                                sw.WriteLine("B3*" + ManifestNo + "*" + this.gvManifestBL.Rows[i].Cells[0].Text.ToString() + "*" + this.gvManifestBL.Rows[i].Cells[6].Text.ToString() + "*PP**" + fecha + "*0000000****" + line + "****" + this.gvManifestBL.Rows[i].Cells[6].Text.ToString() + "*" + this.IDFamilia.ToString() + "*" + this.gvManifestBL.Rows[i].Cells[8].Text.ToString() + "~");
                                sw.WriteLine("B2A*00~");
                                sw.WriteLine("N9*BM*" + this.gvManifestBL.Rows[i].Cells[0].Text.ToString() + "~");
                                sw.WriteLine("V1*" + callsign + "*" + vessel + "*DE*" + voyageNo + "*" + line + "***L~");
                                sw.WriteLine("N1*CN*" + this.NameConsignees.ToString() + "**" + this.IDConsignees.ToString() + "~");
                                sw.WriteLine("N3*N/A~");
                                sw.WriteLine("N1*SH*" + this.NameConsignors.ToString() + "**" + this.IDConsignors.ToString() + "~");
                                sw.WriteLine("N3*N/A~");
                                sw.WriteLine("N1*N1*" + this.NameConsignees.ToString() + "**" + this.IDConsignees.ToString() + "~");
                                sw.WriteLine("N3*N/A~");
                                sw.WriteLine("R4*E*K*24741*HAINA TERMINAL~");
                                //sw.WriteLine("R4*R*K*22255*HKG TERMINAL~");
                                sw.WriteLine("R4*R*K*" + this.IDPort.ToString() + "*" + this.gvManifestBL.Rows[0].Cells[4].Text.ToString() + "~");
                                sw.WriteLine("R4*D*K*24741*HAINA TERMINAL~");
                                sw.WriteLine("R4*O*K*" + this.IDPort.ToString() + "*" + this.gvManifestBL.Rows[0].Cells[4].Text.ToString() + "~");
                                sw.WriteLine("LX*1~");

                                //secuencia
                                secuencia = Guid.NewGuid();
                                string sec = secuencia.ToString().Substring(0, 6);

                                sw.WriteLine("N7*CG" + sec.ToUpper() + "*" + this.Incremento + "*" + this.Weight + "*G****20*X********K*****CGS***" + this.CGSResults + "~");

                                //sw.WriteLine("N7*CG" + this.dat + this.hour + "*" + this.Incremento + "*" + this.Weight + "*G****20*X********K*****CGS***" + this.CGSResults + "~");

                                //sw.WriteLine("N7*CGS" + ContainerNoLetter + "*" + this.Incremento + "*" + this.Weight + "*G****20*X********K*****CGS" + ContainerNoLetter + this.Incremento + "***" + this.CGSResults + "~");
                                //sw.WriteLine("N7*CGS" + ContainerNoLetter + "*" + ContainerNoNumber + "*" + this.gvManifestBL.Rows[i].Cells[8].Text.ToString() + "*G****20*X********K*****" + this.gvManifestBL.Rows[i].Cells[0].Text.ToString() + "~");
                                sw.WriteLine("M7*" + this.txtManifestNo.Text + "*" + this.txtNaviera.Text + "*" + this.txtVessel.Text + "~");
                                //sw.WriteLine("M7*" + this.txtManifestNo.Text + "*" + this.txtNaviera.Text + "*" + this.txtVessel.Text + "*" + this.txtRegistroNo.Text + "~");
                                //sw.WriteLine("M7*" + this.gvManifestBL.Rows[i].Cells[7].Text.ToString() + "~");
                                //sw.WriteLine("N7*" + ContainerNoLetter + "*" + ContainerNoNumber + "*" + this.ds.Tables["ManifestContainer"].Rows[i].ItemArray[5].ToString() + "*G****20*X********K*****" + this.ds.Tables["ManifestContainer"].Rows[i].ItemArray[0].ToString() + "~");
                                //sw.WriteLine("M7*" + this.ds.Tables["ManifestContainer"].Rows[i].ItemArray[7].ToString() + "~");
                                sw.WriteLine("L0*1*****20*X*609*PCS~");

                                if (this.gvManifestBL.Rows[i].Cells[5].Text.ToString() == null)
                                {
                                    result2 = "la variable en blanco";
                                }
                                else
                                {
                                    longitud = this.gvManifestBL.Rows[i].Cells[5].Text.ToString().Length;

                                    if (longitud >= 120)
                                    {
                                        result1 = " " + this.gvManifestBL.Rows[i].Cells[5].Text.ToString() + " ";
                                        result2 = result1.Substring(0, 120);
                                    }
                                    else
                                    {
                                        result2 = this.gvManifestBL.Rows[i].Cells[5].Text.ToString();
                                    }
                                }
                                sw.WriteLine("L5*1*" + result2 + "~");
                                sw.WriteLine("SE*19*3155~");
                            }
                        }
                    }
                }
                //sw.WriteLine("SE*19*3155~");
                sw.WriteLine("GE*1*2~");
                sw.WriteLine("IEA*1*000000002~");

                #endregion

                #endregion

                sw.Close(); //Don't Forget Close the TextWriter Object(sw)      
                //this.lblMessage.Text = "Data Successfully Exported";
                Jalert.MessageBoxSuccess(this, "Data Successfully Exported");

            }
        }
    }

    private void LoadingXMLtoDataSet()
    {
        try
        {
            //Create XmlDocument
            //<input id="MyFile"   type="file" size="81" name="File1" runat="server" />
            XmlDataDocument xmlDatadoc = new XmlDataDocument();
            {
                //Load File XML
                xmlDatadoc.DataSet.ReadXml(this.lblPath.Text);
                //xmlDatadoc.DataSet.ReadXml("D:\\ImportManifest_ST993_DOHAI.xml");
                //xmlDatadoc.DataSet.ReadXml("D:\\ImportManifest_LIN060_DOHAI.xml");
                //xmlDatadoc.DataSet.ReadXml("D:\\ImportManifest_SPC105_DOHAI.xml");
                //xmlDatadoc.DataSet.ReadXml("D:\\ImportManifest_KNO132_DOHAI.xml");

                ////****************CARGA DE MANIFIESTO XML A OCEANIS****************//
                #region "OCEANIS, SP"

                //try
                //{
                //    #region"Pasando parametros"

                //    SqlParameter[] param = new SqlParameter[7];
                //    param[0] = new SqlParameter("@xml", SqlDbType.Xml);
                //    param[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(xmlDatadoc.InnerXml, XmlNodeType.Document, null));

                //    param[1] = new SqlParameter("@manifiesto", SqlDbType.NVarChar);
                //    param[1].Value = this.txtManifestNo.Text;

                //    param[2] = new SqlParameter("@barco", SqlDbType.NVarChar);
                //    param[2].Value = this.txtVessel.Text;

                //    param[3] = new SqlParameter("@naviera", SqlDbType.NVarChar);
                //    param[3].Value = this.txtNaviera.Text;

                //    param[4] = new SqlParameter("@registro", SqlDbType.NVarChar);
                //    param[4].Value = this.txtRegistroNo.Text;

                //    param[5] = new SqlParameter("@terminal", SqlDbType.NVarChar);
                //    param[5].Value = this.ddlTerminal.Text;

                //    param[6] = new SqlParameter("@tipo_carga", SqlDbType.NVarChar);
                //    param[6].Value = "Contenedores";

                //    //param[7] = new sqlparameter("@accion", sqldbtype.nvarchar);
                //    //param[7].Value = "Cargadedatos";

                //    #endregion

                //    //Conectando con StoreProcedure
                //    sqlCon.Open();
                //    cmdsqlCon.Connection = sqlCon;
                //    cmdsqlCon.Parameters.AddRange(param);
                //    cmdsqlCon.CommandType = CommandType.StoredProcedure;
                //    cmdsqlCon.CommandText = "Cargar_datos";
                //    cmdsqlCon.ExecuteNonQuery();
                //    this.lblMessage.Visible = true;
                //    this.lblMessage.Text = "Este registro ha sido guardado satisfactoriamente";
                //    sqlCon.Close();
                //}

                //catch (Exception ex)
                //{
                //    this.lblMessage.Text = "Can not load Manifest XML OCEANIS! " + ex.Message.ToString();
                //    throw;
                //}
                //finally
                //{
                //    this.lblMessage.Text = "Ready XML OCEANIS!!!";
                //}

                #endregion

                //Binding DataSet
                # region"Binding DataSet"

                ds = xmlDatadoc.DataSet;
                if (this.ds.Tables.Contains("ManifestBL"))
                {
                    this.Label1.Text = "Total ManifestBL : " + ds.Tables["ManifestBL"].Rows.Count.ToString();
                }
                else
                {
                    this.Label1.Text = "Total ManifestBL : 0";
                }

                # region "DON'T USE"

                //if (ds.Tables.Contains("ManifestContainer"))
                //{
                //    this.Label2.Text = "Total Manifest Container : " + ds.Tables["ManifestContainer"].Rows.Count.ToString();
                //}
                //else
                //{
                //    this.Label2.Text = "Total Manifest Container : 0";
                //}
                //if (ds.Tables.Contains("ManifestVehicle"))
                //{
                //    this.Label3.Text = "Total Manifest Vehicle : " + ds.Tables["ManifestVehicle"].Rows.Count.ToString();
                //}
                //else
                //{
                //    this.Label3.Text = "Total Manifest Vehicle : 0";
                //}
                //if (this.ds.Tables.Contains("ContainerBL"))
                //{
                //    this.Label4.Text = "Total Container BL : " + ds.Tables["ContainerBL"].Rows.Count.ToString();
                //}
                //else
                //{
                //    this.Label4.Text = "Total Container BL : 0";
                //}

                # endregion

                # endregion

                //Assign primery key at DataSet.-
                //this.ds.Tables["ContainerBL"].Constraints.Add("pk_sid", this.ds.Tables["ContainerBL"].Columns[1], true);
            }
        }
        catch (Exception ex)
        {
            this.Msg = "Can not load DataSet! " + ex.Message.ToString();
            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
            Jalert.MessageBoxError(this, this.Msg);

        }
        finally
        { }
    }

    private void CreatingFolder()
    {
        //Specify the directory you want to manipulate.
        string path = @"C:\\EDI_file";

        try
        {
            //Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                this.lblMessage.Text = "That path exists already.";
            }
            else
            {
                //Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                this.lblMessage.Text = "The directory was created successfully at {0}." + Directory.GetCreationTime(path) + "";
            }
            //Delete the directory
            //di.Delete();
            //this.lblMessage.Text = "The directory was deleted successfully";
        }
        catch (Exception e)
        {
            this.Msg = "The process failed: {0}, " + e.ToString() + "";
            this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
        }
        finally { }
    }

    private void LoadingFile()
    {
        this.localComputerName = Dns.GetHostName();
        this.localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        strFolder = PathHitEdi.PathXMLFolder;


        // Retrieve the name of the file that is posted.
        try
        {
            strFileName = this.MyFile.PostedFile.FileName;
            strFileName = Path.GetFileName(strFileName);
            if (this.MyFile.Value != "")
            {
                // Create the folder if it does not exist.
                if (!Directory.Exists(strFolder))
                {
                    Directory.CreateDirectory(strFolder);
                }

                // Save the uploaded file to the server.
                strFilePath = strFolder + strFileName;

                if (File.Exists(strFilePath))
                {
                    Jalert.MessageBoxError(this, strFileName + " already exists on the server!");
                }
                else
                {
                    this.MyFile.PostedFile.SaveAs(strFilePath);
                    Jalert.MessageBoxSuccess(this, strFileName + " has been successfully uploaded.");

                }

            }
            else
            {
                Jalert.MessageBoxError(this, "Click 'Browse' to select the file to upload.");
            }
        }
        catch (Exception ex)
        {
            this.Msg = "ERROR: " + ex.Message.ToString();
            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
            //this.PanelMessage.Visible = true;
            Jalert.MessageBoxError(this, this.Msg);
        }
        finally
        {
            // Display the result of the upload.
            //this.PanelMessage.Visible = true;
            //this.PanelPath.Visible = true;
            this.lblPath.Text = this.strFilePath;
        }
    }

    private void LoadingXLStoDataSetShippersConsignees()
    {
        try
        {
            //DataTable Worksheets.*
            # region "Connect to file XLS"

            DataTable worksheetsShippersConsignees;

            string connectionStringShippersConsignees = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\N4JOBS\HIT_EDI\IN\SHIPPERSCONSIGNEES\ShippersConsignees.xls;Extended Properties=Excel 8.0;";
            //string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.lblPath.Text + ";Extended Properties=Excel 8.0;";

            //You must use the $ after the object and you reference in the spreadsheet
            OleDbDataAdapter da = new OleDbDataAdapter
            ("SELECT * FROM [ShippersConsignees$]", connectionStringShippersConsignees);

            # endregion

            //da.TableMappings.Add("Table", "ExcelTest");
            da.Fill(dsMsExcelShippersConsignees);

            //Delete rows null.*
            # region "Delete rows null"

            //Count to rows.-
            int rowcount = this.dsMsExcelShippersConsignees.Tables[0].Rows.Count;
            rowcount = rowcount + 1;
            for (int i = 0; i < rowcount - 1; i++)
            {
                if (this.dsMsExcelShippersConsignees.Tables[0].Rows[i].ItemArray[0].ToString() == "")
                {
                    this.dsMsExcelShippersConsignees.Tables[0].Rows[i].Delete();
                }
            }

            # endregion

            this.gvdsMsExcelShippersConsignees.DataSource = this.dsMsExcelShippersConsignees.Tables[0].DefaultView;
            this.gvdsMsExcelShippersConsignees.DataBind();
        }
        catch (Exception ex)
        {
            this.Msg = "Can not load DataSet ShippersConsignees! " + ex.Message.ToString();
            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
            Jalert.MessageBoxError(this, this.Msg);
        }
        finally
        {
            string dsMsExcelCountShippersConsignees = this.gvdsMsExcelShippersConsignees.Rows.Count.ToString();
            //Doing count the DataSet.*
            this.Label6.Text = "Total ShippersConsignees : " + dsMsExcelCountShippersConsignees;
        }
    }

    private int GetColumnIndexByName(GridView grid, string name)
    {
        grid.DataSource = this.dsMsExcelShippersConsignees.Tables.Contains("Table");
        grid.DataBind();
        {
            foreach (DataControlField col in grid.Columns)
            {
                if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim())
                {
                    return grid.Columns.IndexOf(col);
                }
            }

            return -1;
        }
    }

    protected void btnCargaOCEANIS_Click(object sender, EventArgs e)
    {
        //************************OCEANIS**********************//

        #region "Validate OCEANIS"

        if (txtNaviera.Text == "")
        {
            this.lblMessage.Text = "There are append field";
            return;
        }
        else
        {
            naviera = txtNaviera.Text;
        }

        //if (txtRegistroNo.Text == "")
        //{
        //    this.lblMessage.Text = "There are append field";
        //    return;
        //}
        //else
        //{
        //    registroNo = txtRegistroNo.Text;
        //}

        //if (ddlTerminal.Text == "")
        //{
        //    this.lblMessage.Text = "There are append field";
        //    return;
        //}
        //else
        //{
        //    terminal = ddlTerminal.Text;
        //}

        # endregion

        if (this.lblPath.Text != null || this.lblPath.Text != "")
        {
            //****************CARGA DE MANIFIESTO XML A OCEANIS****************//
            #region "OCEANIS, SP"
            //Create XmlDocument
            //<input id="MyFile"   type="file" size="81" name="File1" runat="server" />
            XmlDataDocument xmlDatadocOCEANIS = new XmlDataDocument();
            {
                //Load File XML
                xmlDatadocOCEANIS.DataSet.ReadXml(this.lblPath.Text);

                try
                {
                    #region"Pasando parametros"

                    SqlParameter[] param = new SqlParameter[7];
                    param[0] = new SqlParameter("@xml", SqlDbType.Xml);
                    param[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(xmlDatadocOCEANIS.InnerXml, XmlNodeType.Document, null));

                    param[1] = new SqlParameter("@manifiesto", SqlDbType.NVarChar);
                    param[1].Value = this.txtManifestNo.Text;

                    param[2] = new SqlParameter("@barco", SqlDbType.NVarChar);
                    param[2].Value = this.txtVessel.Text;

                    param[3] = new SqlParameter("@naviera", SqlDbType.NVarChar);
                    param[3].Value = this.txtNaviera.Text;

                    //param[4] = new SqlParameter("@registro", SqlDbType.NVarChar);
                    //param[4].Value = this.txtRegistroNo.Text;

                    //param[5] = new SqlParameter("@terminal", SqlDbType.NVarChar);
                    //param[5].Value = this.ddlTerminal.Text;

                    param[6] = new SqlParameter("@tipo_carga", SqlDbType.NVarChar);
                    param[6].Value = "Contenedores";

                    //param[7] = new sqlparameter("@accion", sqldbtype.nvarchar);
                    //param[7].Value = "Cargadedatos";

                    #endregion

                    //Conectando con StoreProcedure
                    sqlCon.Open();
                    cmdsqlCon.Connection = sqlCon;
                    cmdsqlCon.Parameters.AddRange(param);
                    cmdsqlCon.CommandType = CommandType.StoredProcedure;
                    cmdsqlCon.CommandText = "Cargar_datos";
                    cmdsqlCon.ExecuteNonQuery();
                    this.lblMessage.Visible = true;
                    this.lblMessage.Text = "Este registro ha sido guardado satisfactoriamente";
                    sqlCon.Close();
                }

                catch (Exception ex)
                {
                    this.lblMessage.Text = "Can not load Manifest XML OCEANIS! " + ex.Message.ToString();
                    throw;
                }
                finally
                {
                    this.lblMessage.Text = "Ready XML OCEANIS!!!";
                }
            }
            #endregion
        }
    }

    private void Familia()
    {
        sqlConfm.Open();
        //SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=EMS;User ID=sa;Password=Jehova-07");
        string Queryfm = "SELECT * FROM dbo.familia_mercancia";
        SqlCommand cmdsqlConfm = new SqlCommand(Queryfm, sqlConfm);
        SqlDataAdapter sqlDafm = new SqlDataAdapter(cmdsqlConfm);
        //DataSet dsfm = new DataSet();
        sqlDafm.Fill(dsfm);

        //this.gvfm.DataSource = dsfm;
        //this.gvfm.DataBind();

        sqlConfm.Close();

        this.Label7.Visible = true;
        this.Label7.Text = "Total Familia Mercancía : " + dsfm.Tables[0].Rows.Count;
        //dsfm.Clear();
    }

    private void CargaGeneralSuelta()
    {
        sqlConCGS.Open();
        //SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=EMS;User ID=sa;Password=Jehova-07");
        string QueryCGS = "SELECT * FROM dbo.HIT_Carga_General_Suelta";
        SqlCommand cmdsqlConCGS = new SqlCommand(QueryCGS, sqlConCGS);
        SqlDataAdapter sqlDaCGS = new SqlDataAdapter(cmdsqlConCGS);
        //DataSet dsCGS = new DataSet();
        sqlDaCGS.Fill(dsCGS);

        //this.gvCGS.DataSource = dsCGS;
        //this.gvCGS.DataBind();

        sqlConCGS.Close();

        this.Label7.Visible = true;
        this.Label7.Text = "Total Familia Mercancía : " + dsCGS.Tables[0].Rows.Count;
        //dsCGS.Clear();
    }

    private void CargaGeneralSueltaDistinct()
    {
        sqlConDistinctCGS.Open();
        //SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=EMS;User ID=sa;Password=Jehova-07");
        string QueryDistinctCGS = "SELECT DISTINCT MERCANCIA FROM dbo.HIT_Carga_General_Suelta";
        SqlCommand cmdsqlConDistinctCGS = new SqlCommand(QueryDistinctCGS, sqlConDistinctCGS);
        SqlDataAdapter sqlDaDistinctCGS = new SqlDataAdapter(cmdsqlConDistinctCGS);
        //DataSet dsCGS = new DataSet();
        sqlDaDistinctCGS.Fill(dsDistinctCGS);

        //this.gvCGS.DataSource = dsCGS;
        //this.gvCGS.DataBind();

        sqlConDistinctCGS.Close();
    }

    private void CargaGeneralSueltaInsert()
    {
        #region"Pasando parametros"

        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@DGA", SqlDbType.NVarChar);
        param[0].Value = this.DGACode;

        param[1] = new SqlParameter("@mercancia", SqlDbType.NVarChar);
        param[1].Value = this.CGSMercancia;

        param[2] = new SqlParameter("@paquete", SqlDbType.NVarChar);
        param[2].Value = DBNull.Value;

        param[3] = new SqlParameter("@paqmedida", SqlDbType.NVarChar);
        param[3].Value = "UNKN";

        param[4] = new SqlParameter("@medida", SqlDbType.NVarChar);
        param[4].Value = this.CGSMedida;

        param[5] = new SqlParameter("@unidmedida", SqlDbType.NVarChar);
        param[5].Value = "UNKN";

        param[6] = new SqlParameter("@RNC", SqlDbType.NVarChar);
        param[6].Value = this.IDConsignees.ToString();

        param[7] = new SqlParameter("@consignatario", SqlDbType.NVarChar);
        param[7].Value = this.NameConsignees.ToString();

        #endregion

        try
        {
            //Conectando con StoreProcedure
            sqlConCGS.Open();
            SqlCommand cmdsqlConCGSInsert = new SqlCommand();
            cmdsqlConCGSInsert.Connection = sqlConCGS;
            cmdsqlConCGSInsert.Parameters.AddRange(param);
            cmdsqlConCGSInsert.CommandType = CommandType.StoredProcedure;
            cmdsqlConCGSInsert.CommandText = "sp_CargaGeneralSueltaInsert";
            cmdsqlConCGSInsert.ExecuteNonQuery();
            sqlConCGS.Close();
        }
        catch (Exception ex)
        {
            this.Msg = "Can not insert in General Cargo by sp_CargaGeneralSueltaInsert! " + ex.Message;
            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
            //throw new Exception(ex.Message);
            Jalert.MessageBoxError(this, this.Msg);
        }
        finally
        {
            this.lblMessage.Visible = true;
            this.lblMessage.Text = "Consignatario RNC: " + this.IDConsignees.ToString() + ", NOMBRE: " + this.NameConsignees.ToString() + " ha sido guardado satisfactoriamente!";
        }
    }

    private void ShippersConsignees()
    {
        try
        {
            //Pasando parametros
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@action", SqlDbType.NVarChar);
            param[0].Value = "QueryShippersConsignees";

            //Conectando con StoreProcedure
            sqlConCGS.Open();
            SqlCmdShippersConsignees.Connection = sqlConCGS;
            SqlCmdShippersConsignees.Parameters.AddRange(param);
            SqlCmdShippersConsignees.CommandType = CommandType.StoredProcedure;
            SqlCmdShippersConsignees.CommandText = "dbo.sp_CargaContenedorizada";
            SqlCmdShippersConsignees.ExecuteNonQuery();
            sqlConCGS.Close();

            //SqlDataAdapter
            SqlDataAdapter SqlDtAdrCGS = new SqlDataAdapter(SqlCmdShippersConsignees);

            SqlDtAdrCGS.Fill(dsShippersConsignees);

            this.gvShippersConsignees.DataSource = this.dsShippersConsignees.Tables[0].DefaultView;
            this.gvShippersConsignees.DataBind();

        }
        catch (Exception ex)
        {
            this.Msg = "Can not load DataSet ShippersConsignees from Database! " + ex.Message.ToString();
            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
            Jalert.MessageBoxError(this, this.Msg);
        }
        finally
        {
            string dsCountShippersConsignees = this.gvShippersConsignees.Rows.Count.ToString();
            //Doing count the DataSet.*
            this.Label6.Text = "Total ShippersConsignees : " + dsCountShippersConsignees;
            //this.dsShippersConsignees.Clear();
        }
    }

    private void RoutingPoint()
    {
        try
        {
            //Pasando parametros
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@action", SqlDbType.NVarChar);
            param[0].Value = "QueryRoutingPoints";

            //Conectando con StoreProcedure
            sqlConCGS.Open();
            SqlCmdRoutingPoint.Connection = sqlConCGS;
            SqlCmdRoutingPoint.Parameters.AddRange(param);
            SqlCmdRoutingPoint.CommandType = CommandType.StoredProcedure;
            SqlCmdRoutingPoint.CommandText = "dbo.sp_CargaContenedorizada";
            SqlCmdRoutingPoint.ExecuteNonQuery();
            sqlConCGS.Close();

            //SqlDataAdapter
            SqlDataAdapter SqlDtAdrCGS = new SqlDataAdapter(SqlCmdRoutingPoint);

            SqlDtAdrCGS.Fill(dsRoutingPoint);

            this.gvRoutingPoint.DataSource = this.dsRoutingPoint.Tables[0].DefaultView;
            this.gvRoutingPoint.DataBind();

        }
        catch (Exception ex)
        {
            this.Msg = "can not load dataset routingpoing from database! " + ex.Message.ToString();
            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
            Jalert.MessageBoxError(this, this.Msg);
        }
        finally
        {
            string dsCountRoutingPoint = this.gvRoutingPoint.Rows.Count.ToString();
            //Doing count the DataSet.*
            this.Label2.Text = "Total RoutingPoint : " + dsCountRoutingPoint;
            //this.dsRoutingPoint.Clear();
        }
    }

    private void OEACustomers()
    {
        try
        {
            //Pasando parametros
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@action", SqlDbType.NVarChar);
            param[0].Value = "QueryOEACustomers";

            //Conectando con StoreProcedure
            sqlConCGS.Open();
            SqlCmdOEACustomers.Connection = sqlConCGS;
            SqlCmdOEACustomers.Parameters.AddRange(param);
            SqlCmdOEACustomers.CommandType = CommandType.StoredProcedure;
            SqlCmdOEACustomers.CommandText = "dbo.sp_CargaContenedorizada";
            SqlCmdOEACustomers.ExecuteNonQuery();
            sqlConCGS.Close();

            //SqlDataAdapter
            SqlDataAdapter SqlDtAdrCGS = new SqlDataAdapter(SqlCmdOEACustomers);

            SqlDtAdrCGS.Fill(dsOEACustomers);

            this.gvOEACustomers.DataSource = this.dsOEACustomers.Tables[0].DefaultView;
            this.gvOEACustomers.DataBind();

        }
        catch (Exception ex)
        {
            this.Msg = "Can not load DataSet OEACustomers from Database! " + ex.Message.ToString();
            Jalert.MessageBoxError(this, this.Msg);
        }
        finally
        {
            string dsCountOEACustomers = this.gvOEACustomers.Rows.Count.ToString();
            //Doing count the DataSet.*
            this.Label2.Text = "Total OEACustomers : " + dsCountOEACustomers;
            //this.dsRoutingPoint.Clear();
        }
    }

    protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {
        ScriptManager1.AsyncPostBackErrorMessage = "Se ha producido un error: " + e.Exception.Message;
    }

}