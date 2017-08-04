using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
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


public partial class wfrmBookings : System.Web.UI.Page
{
    //Public Variable.-
    # region "Decaration Public"

    string line, voyageNo, vessel, Container1, Container2;
    string callsign, ContainerNoLetter, ContainerNoNumber;
    int longitudContainer;

    string strFileName;
    string strFilePath;
    string strFolder;
    //string localComputerName;
    //IPAddress[] localIPs;

    string fecha = DateTime.Today.ToString("yyMMdd");

    //DataSet Excel.*
    DataSet dsMsExcel = new DataSet("ExcelBooks DataSet");
    GridView gvdsMsExcel = new GridView();

    //DataSet
    DataSet ds = new DataSet("Books DataSet");
    //GridView
    GridView gvManifestBL = new GridView();
    GridView gvManifestVehicleBL = new GridView();

    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.MyFile.Value.DefaultIfEmpty();
            this.lblMessage.Text = "";
            this.lblPath.Text = "";
            this.hfPath.Value = "";
        }
    }

    protected void btnLoadXML_Click(object sender, EventArgs e)
    {
        //Execute private function.....
        this.LoadingFile();
        this.LoadingXLStoDataSet();
    }

    protected void btnCreateEDI_Click(object sender, EventArgs e)
    {
        if (this.lblPath.Text == "")
        {
            //this.lblMessage.Text = "There not file";
            Jalert.MessageBoxError(this, "There not file");
            return;
        }
        else
        {
            //Execute function......
            this.LoadingXLStoDataSet();

            #region "Validate"

            if (txtVessel.Text == "")
            {
                Jalert.MessageBoxError(this, "The field Vessel is required");
                return;
            }
            else
            {
                vessel = txtVessel.Text;
            }

            if (txtCallsign.Text == "")
            {
                Jalert.MessageBoxError(this, "The field Lloyds Id is required");
                return;
            }
            else
            {
                callsign = txtCallsign.Text;
            }

            if (txtLine.Text == "")
            {
                Jalert.MessageBoxError(this, "The field Line is required");
                return;
            }
            else
            {
                line = txtLine.Text;
            }

            if (txtVoyage.Text == "")
            {

                Jalert.MessageBoxError(this, "The field Voyage is required");
                return;
            }
            else
            {
                voyageNo = txtVoyage.Text;
            }

            #endregion

            //Creating File in directory.
            //string fileName = @"C:\HIT_EDI\IN\" + this.line + @"\301\" + this.line + "_booking_" + this.voyageNo + "_from_app.edi";

            //string fileName = String.Format(PathHitEdi.Path310, this.line) + ManifestNo + "_from_app.edi";


            //string fileName = @"\\N4JOBS\HIT_EDI\IN\" + this.line + @"\301\" + this.line + "_booking_" + this.voyageNo + "_from_app.edi";

            string fileName = String.Format(PathHitEdi.Path301, this.line) + this.line + "_booking_" + this.voyageNo + "_from_app.edi";


            if (File.Exists(fileName))
            {
                //this.lblMessage.Text = "{0} already exists., " + fileName + "";
                Jalert.MessageBoxError(this, "{0} already exists., " + fileName + "");
                return;
            }
            using (StreamWriter sw = File.CreateText(fileName))
            {

                # region "Writing Container to Container"

                //Loading GridView from DataSet.*
                //this.gvdsMsExcel.DataSource = this.dsMsExcel.Tables[0].DefaultView;
                //this.gvdsMsExcel.DataBind();

                //Count to rows.-
                int rowcount = this.gvdsMsExcel.Rows.Count;
                rowcount = rowcount + 1;
                for (int i = 0; i < rowcount - 1; i++)
                {
                    #region "Height to Container"

                    if (this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() == null)
                    {
                        Container2 = "la variable esta en blanco";
                    }
                    else
                    {
                        longitudContainer = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Length;

                        if (longitudContainer >= 12)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 11);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(4, 7);
                        }
                        if (longitudContainer == 11)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 11);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(4, 7);
                        }
                        if (longitudContainer == 10)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 10);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(4, 6);
                        }
                        if (longitudContainer == 9)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 9);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(4, 5);
                        }
                        if (longitudContainer == 8)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 8);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(4, 4);
                        }
                        if (longitudContainer == 7)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 7);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(4, 3);
                        }
                        if (longitudContainer == 6)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 6);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(4, 2);
                        }
                        if (longitudContainer == 5)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 5);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(4, 1);
                        }
                        if (longitudContainer == 4)
                        {
                            Container1 = " " + this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() + " ";
                            Container2 = Container1.Substring(0, 4);
                            ContainerNoLetter = this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString().Substring(0, 4);
                            ContainerNoNumber = "";
                        }
                    }

                    #endregion

                    #region "Writing Container in EDI file"

                    {
                        if (i == 0)
                        {
                            //First Block.*
                            sw.WriteLine("ISA*00*1000008   *00*          *ZZ*" + line.ToUpper() + "            *ZZ*HIT            *060701*1911*U*00200*000012452*0*P*>~");
                            sw.WriteLine("GS*RO*" + line.ToUpper() + "*HIT*20060701*2201*20983*X*004020~");
                            sw.WriteLine("ST*301*0001~");
                            sw.WriteLine("B1*" + this.line.ToUpper() + "*" + this.gvdsMsExcel.Rows[i].Cells[1].Text.ToString() + "*" + this.fecha + "*N~");
                            sw.WriteLine("Y3*" + this.gvdsMsExcel.Rows[i].Cells[1].Text.ToString() + "*" + line.ToUpper() + "~");
                            sw.WriteLine("Y4*" + this.gvdsMsExcel.Rows[i].Cells[1].Text.ToString() + "****" + this.gvdsMsExcel.Rows[i].Cells[8].Text.ToString() + "*" + this.gvdsMsExcel.Rows[i].Cells[2].Text.ToString() + "~");
                            sw.WriteLine("N9*BN*" + this.gvdsMsExcel.Rows[i].Cells[1].Text.ToString() + "~");
                            sw.WriteLine("N1*SH*" + this.gvdsMsExcel.Rows[i].Cells[4].Text.ToString() + "~");
                            sw.WriteLine("R4*L*K*24741*HAINA TERMINAL~");
                            sw.WriteLine("R4*D*K*" + this.gvdsMsExcel.Rows[i].Cells[5].Text.ToString() + "*INTERNATIONAL TERMINAL*PA~");
                            sw.WriteLine("H3*MTE*SC31054438M101MMB~");
                            sw.WriteLine("LX*2000~");
                            sw.WriteLine("N7*" + ContainerNoLetter + "*" + ContainerNoNumber + "*" + this.gvdsMsExcel.Rows[i].Cells[6].Text.ToString() + "*G*******CN*" + this.line.ToUpper() + "*****G*****" + this.gvdsMsExcel.Rows[i].Cells[2].Text.ToString() + "~");
                            sw.WriteLine("L0*10****G*0*E****K~");
                            sw.WriteLine("L5*1*" + this.gvdsMsExcel.Rows[i].Cells[3].Text.ToString() + "*" + this.gvdsMsExcel.Rows[i].Cells[3].Text.ToString() + "~");
                            sw.WriteLine("V1*" + this.callsign + "*" + this.vessel + "**" + this.voyageNo + "****L~");
                            sw.WriteLine("SE*225*0001~");
                            sw.WriteLine("GE*4*000012452~");
                            sw.WriteLine("IEA*1*000012452~");
                        }
                        else
                        {
                            //Continue Block.*
                            sw.WriteLine("ISA*00*1000008   *00*          *ZZ*" + line.ToUpper() + "            *ZZ*HIT            *060701*1911*U*00200*000012452*0*P*>~");
                            sw.WriteLine("GS*RO*" + line.ToUpper() + "*HIT*20060701*2201*20983*X*004020~");
                            sw.WriteLine("ST*301*0001~");
                            sw.WriteLine("B1*" + this.line.ToUpper() + "*" + this.gvdsMsExcel.Rows[i].Cells[1].Text.ToString() + "*" + this.fecha + "*A~");
                            sw.WriteLine("Y3*" + this.gvdsMsExcel.Rows[i].Cells[1].Text.ToString() + "*" + line.ToUpper() + "~");
                            sw.WriteLine("Y4*" + this.gvdsMsExcel.Rows[i].Cells[1].Text.ToString() + "****" + this.gvdsMsExcel.Rows[i].Cells[8].Text.ToString() + "*" + this.gvdsMsExcel.Rows[i].Cells[2].Text.ToString() + "~");
                            sw.WriteLine("N9*BN*" + this.gvdsMsExcel.Rows[i].Cells[1].Text.ToString() + "~");
                            sw.WriteLine("N1*SH*" + this.gvdsMsExcel.Rows[i].Cells[4].Text.ToString() + "~");
                            sw.WriteLine("R4*L*K*24741*HAINA TERMINAL~");
                            sw.WriteLine("R4*D*K*" + this.gvdsMsExcel.Rows[i].Cells[5].Text.ToString() + "*INTERNATIONAL TERMINAL*PA~");
                            sw.WriteLine("H3*MTE*SC31054438M101MMB~");
                            sw.WriteLine("LX*2000~");
                            sw.WriteLine("N7*" + ContainerNoLetter + "*" + ContainerNoNumber + "*" + this.gvdsMsExcel.Rows[i].Cells[6].Text.ToString() + "*G*******CN*" + this.line.ToUpper() + "*****G*****" + this.gvdsMsExcel.Rows[i].Cells[2].Text.ToString() + "~");
                            sw.WriteLine("L0*10****G*0*E****K~");
                            sw.WriteLine("L5*1*" + this.gvdsMsExcel.Rows[i].Cells[3].Text.ToString() + "*" + this.gvdsMsExcel.Rows[i].Cells[3].Text.ToString() + "~");
                            sw.WriteLine("V1*" + this.callsign + "*" + this.vessel + "**" + this.voyageNo + "****L~");
                            sw.WriteLine("SE*225*0001~");
                            sw.WriteLine("GE*4*000012452~");
                            sw.WriteLine("IEA*1*000012452~");
                        }
                    }
                }

                #endregion

                #endregion

                sw.Close(); //Don't Forget Close the TextWriter Object(sw)      
                //this.lblMessage.Text = "Data Successfully Exported";
                Jalert.MessageBoxSuccess(this, "Data Successfully Exported");
            }
        }
    }

    private void LoadingXLStoDataSet()
    {
        try
        {
            //DataTable Worksheets.*
            # region "Connect to file XLS"

            DataTable worksheets;

            string connectionString = "";

            //server
            //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + this.hfPath.Value + ";Extended Properties=\"Excel 8.0;\"";


            connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + this.hfPath.Value + "';Extended Properties=Excel 8.0;";

            
            //You must use the $ after the object and you reference in the spreadsheet
            OleDbDataAdapter da = new OleDbDataAdapter
            ("SELECT * FROM [Hoja1$]", connectionString);

            # endregion

            //da.TableMappings.Add("Table", "ExcelTest");
            da.Fill(this.dsMsExcel);

            //Delete rows null.*
            #region "Delete rows null"

            //Count to rows.-
            int rowcount = this.dsMsExcel.Tables[0].Rows.Count;
            rowcount = rowcount + 1;
            for (int i = 0; i < rowcount - 1; i++)
            {
                if (this.dsMsExcel.Tables[0].Rows[i].ItemArray[0].ToString() == "")
                {
                    this.dsMsExcel.Tables[0].Rows[i].Delete();
                }
            }

            # endregion

            if (this.dsMsExcel.Tables[0].Rows.Count > 0)
            {
                this.gvdsMsExcel.DataSource = this.dsMsExcel.Tables[0].DefaultView;
                this.gvdsMsExcel.DataBind();
            }
            else {
                Jalert.MessageBoxError(this, "Can not load DataSet! ");
            }

        }
        catch (Exception ex)
        {
            //this.lblMessage.Text = "Can not load DataSet! " + ex.Message.ToString();
            Jalert.MessageBoxError(this, "Can not load DataSet! " + ex.Message.ToString());
        }
        finally
        {
            string dsMsExcelCount = this.gvdsMsExcel.Rows.Count.ToString();
            //Doing count the DataSet.*
            this.Label1.Text = "Total Bookings : " + dsMsExcelCount;
        }
    }

    private void LoadingFile()
    {
        //this.localComputerName = Dns.GetHostName();
        //this.localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        //strFolder = (@"C:\HIT_EDI\IN\XLS_Folder\");
        //strFolder = (@"\\N4JOBS\HIT_EDI\IN\XLS_Folder\");

        strFolder = PathHitEdi.PathXLSFolder;

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
                    //this.lblMessage.Text = strFileName + " already exists on the server!";
                    Jalert.MessageBoxError(this, strFileName + " already exists on the server!");
                }
                else
                {
                    this.MyFile.PostedFile.SaveAs(strFilePath);
                    //this.lblMessage.Text = strFileName + " has been successfully uploaded.";
                    Jalert.MessageBoxSuccess(this, strFileName + " has been successfully uploaded.");
                }

            }
            else
            {
                //this.lblMessage.Text = "Click 'Browse' to select the file to upload.";
                Jalert.MessageBoxError(this, "Click 'Browse' to select the file to upload.");
            }
        }
        catch (Exception ex)
        {
            //this.lblMessage.Text = "ERROR: " + ex.Message.ToString();
            //this.PanelMessage.Visible = true;
            Jalert.MessageBoxError(this, "ERROR: " + ex.Message.ToString());
        }
        finally
        {
            // Display the result of the upload.
            //this.PanelMessage.Visible = false;
            //this.PanelPath.Visible = true;
            //this.lblPath.Text = this.strFilePath;
            this.hfPath.Value = this.strFilePath;
        }
    }

}