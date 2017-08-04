using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;

public partial class OEACustomers : System.Web.UI.Page
{
    #region "SqlConnection"
    //private SqlConnection ConnSqlRegistro = new SqlConnection(@"Server=TECN-06\SQLEXPRESS;Database=CONTROLHIT;User ID=sa;Password=Jehova-07;");
    //private SqlConnection SqlConConsignee = new SqlConnection("Server=HIT-SQL01-1;Database=testn4;User ID=navistest;Password=navistest;");
    private SqlConnection SqlConOEACustomers = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;");
    private DataSet DtStOEACustomers = new DataSet();
    private SqlCommand SqlCmdOEACustomers = new SqlCommand();

    string Msg;
    int Mssg = 0;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack == false)
        {
            //Inicializando.....
            this.txtFind.Focus();

            //Cargando el GridView
            LoadGridView();

            //Limpiando Textbox
            LimpiandoTextBox();

            //Inhabilitando Textbox
            InhabilitandoTextBox();
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        //Boton Nuevo.........
        if (Menu1.SelectedItem.Text == "New")
        {
            //Limpiando TextBox
            LimpiandoTextBox();

            //Habilitando TextBox
            HabilitandoTextBox();

            //this.lblMessage.Text = "New";
            //this.lblMessage.Visible = true;
        }

        //Boton Guardar........
        if (Menu1.SelectedItem.Text == "Save")
        {
            if (this.TextBox1.Text == "")
            {
                this.Msg = "There are textbox empty!";
                Jalert.MessageBoxError(this, this.Msg);
                return;
            }
            if (this.TextBox2.Text == "")
            {
                this.Msg = "There are textbox empty!";
                Jalert.MessageBoxError(this, this.Msg);
                return;
            }
            
            #region"Pasando parametros"

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@action", SqlDbType.NVarChar);
            param[0].Value = "InsertOEACustomers";

            param[1] = new SqlParameter("@RNC", SqlDbType.NVarChar);
            param[1].Value = this.TextBox1.Text;

            param[2] = new SqlParameter("@CUSTOMER", SqlDbType.NVarChar);
            param[2].Value = this.TextBox2.Text;

            param[3] = new SqlParameter("@STATUS", SqlDbType.NVarChar);
            param[3].Value = this.DropDownList1.Text;

            #endregion

            try
            {
                //Conectando con StoreProcedure
                SqlConOEACustomers.Open();
                SqlCmdOEACustomers.Connection = SqlConOEACustomers;
                SqlCmdOEACustomers.Parameters.AddRange(param);
                SqlCmdOEACustomers.CommandType = CommandType.StoredProcedure;
                SqlCmdOEACustomers.CommandText = "sp_CargaContenedorizada";
                SqlCmdOEACustomers.ExecuteNonQuery();
                //this.lblMessage.Visible = true;
                //this.lblMessage.Text = "Register completed!";
                SqlConOEACustomers.Close();
            }
            catch (Exception ex)
            {
                this.Msg = ex.Message;
                //throw new Exception(ex.Message);
                //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);

                Jalert.MessageBoxError(this, this.Msg);

            }
            finally
            {
                //Inhabilitando TextBox
                InhabilitandoTextBox();

                //Limpiar TextBox
                LimpiandoTextBox();
                //
                Jalert.MessageBoxSuccess(this, "Register completed!");

            }
        }

        //Boton Cancelar.........
        if (Menu1.SelectedItem.Text == "Cancel")
        {
            //Limpiando TextBox
            LimpiandoTextBox();
            Jalert.MessageBoxSuccess(this, "Canceled");
        }
    }

    protected void btnFind_Click(object sender, EventArgs e)
    {
        Buscar();
    }

    protected void Menu2_MenuItemClick(object sender, MenuEventArgs e)
    {
        //Boton Refresh........
        if (Menu2.SelectedItem.Text == "Refresh")
        {
            //Cargando el GridView
            LoadGridView();
            //
            this.TextBox1.Text = "";
            this.TextBox2.Text = "";
        }
    }

    protected void  GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (e.NewPageIndex != -1)
            {
                 this.GridView1.PageIndex = e.NewPageIndex;
                SqlCmdOEACustomers.Parameters.Clear();
                LoadGridView();
            }
        }
        catch (Exception ex)
        {
            this.Msg = ex.Message;
            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);

            Jalert.MessageBoxError(this, this.Msg);
        }
    }

    protected void  GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        bool isDelete = false;

        try
        {
            //int index = Convert.ToInt32(e.CommandArgument);
            //ImageButton boton = (ImageButton) this.GridView1.Rows[index].Cells[3].Controls[0];
            int ID = Convert.ToInt32(e.CommandArgument.ToString());
            string Comando = e.CommandName;

            if (Comando == "Delete")
            {
                #region "Obtener Pagina"

                //Int32 index = Convert.ToInt32(e.CommandArgument);
                //GridViewRow row;
                //if ( this.GridView1.PageIndex == 0)
                //{
                //    row =  this.GridView1.Rows[index];
                //}
                //else
                //{
                //    Int32 totalPag = Convert.ToInt32( this.GridView1.PageCount);
                //    Int32 itemActl = (totalPag - (totalPag - Convert.ToInt32( this.GridView1.PageIndex))) * Convert.ToInt32( this.GridView1.PageSize);
                //    row =  this.GridView1.Rows[index - itemActl];
                //}

                //DataKey key =  this.GridView1.DataKeys[row.RowIndex];
                #endregion

                //Procedimiento para eliminar.
                SqlConOEACustomers.Open();
                string QueryDelete = "DELETE dbo.hit_oea_customers WHERE ID = ('" + ID + "')";
                SqlCommand CmdDelete = new SqlCommand(QueryDelete, SqlConOEACustomers);
                SqlDataAdapter SqlDataAdapterDelete = new SqlDataAdapter(CmdDelete);
                DataSet DataSetDelete = new DataSet();
                SqlDataAdapterDelete.Fill(DataSetDelete);
                 this.GridView1.DataSource = DataSetDelete;
                //this. this.GridView1.DataBind();
                SqlConOEACustomers.Close();

                Jalert.MessageBoxSuccess(this, "Delete");
                isDelete = true;
            }
        }
        catch (Exception ex)
        {
            this.Msg = ex.Message;
            //this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);

            Jalert.MessageBoxError(this, this.Msg);
        }
        finally
        {
            if (isDelete)
            {
                //Response.Redirect("~/Clientes.aspx?ID=" + (string)e.CommandArgument);
                string url = "~/OEACustomers.aspx?";
                //url += "Msg=" + "Este registro ha sido actualizado";
                Response.Redirect(url + (string)e.CommandArgument);
                lblMessage.Text = "Row deleted!";
            }
        }
    }

    #region "IHL TEXTBOX"

    private void InhabilitandoTextBox()
    {
        //Inhabilitando Campos
        this.TextBox1.Enabled = false;
        this.TextBox2.Enabled = false;
    }

    private void HabilitandoTextBox()
    {
        //Habilitando Campos
        this.TextBox1.Enabled = true;
        this.TextBox2.Enabled = true;
    }

    private void LimpiandoTextBox()
    {
        //Limpiar Campos
        this.TextBox1.Text = "";
        this.TextBox2.Text = "";
        this.TextBox1.Focus();
    }

    #endregion

    #region "FUNCTION"

    private void LoadGridView()
    {
        //Pasando parametros
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@action", SqlDbType.NVarChar);
        param[0].Value = "QueryOEACustomers";

        //Conectando con StoreProcedure
        SqlConOEACustomers.Open();
        SqlCmdOEACustomers.Connection = SqlConOEACustomers;
        SqlCmdOEACustomers.Parameters.AddRange(param);
        SqlCmdOEACustomers.CommandType = CommandType.StoredProcedure;
        SqlCmdOEACustomers.CommandText = "dbo.sp_CargaContenedorizada";
        SqlCmdOEACustomers.ExecuteNonQuery();
        SqlConOEACustomers.Close();

        //SqlDataAdapter
        SqlDataAdapter SqlDataAdapterRegistro = new SqlDataAdapter(SqlCmdOEACustomers);

        SqlDataAdapterRegistro.Fill(DtStOEACustomers);

        //Llenando el GridView
        this.GridView1.DataSource = DtStOEACustomers.Tables[0].DefaultView;
        this.GridView1.DataBind();
        SqlConOEACustomers.Close();

        //Cargando el Combobox de Buscar con los campos de la Tabla
        // ''''''''''''''
        if (this.DropDownListFields.Items.Count <= 1)
        {
            foreach (DataColumn dc in DtStOEACustomers.Tables[0].Columns)
            {
                this.DropDownListFields.Items.Add(new ListItem(dc.ColumnName));
            }
        }

        //Limpiando el DataSet
        DtStOEACustomers.Clear();

        //Asignando el index al DropDownList
        //this.DropDownList2.Items[0].Text = "--";
    }

    private void Buscar()
    {
        if ((this.txtFind.Text != "") && (this.DropDownListFields.Text != "--"))
        {

            SqlConOEACustomers.Open();
            //SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=EMS;User ID=sa;Password=Jehova-07");
            string Query = "SELECT * FROM dbo.hit_oea_customers WHERE " + this.DropDownListFields.SelectedItem.Text + " LIKE ('" + this.txtFind.Text + "') ";
            SqlCommand SqlCmdFind = new SqlCommand(Query, SqlConOEACustomers);
            SqlDataAdapter SqlDtAprFind = new SqlDataAdapter(SqlCmdFind);
            DataSet DtStFind = new DataSet();
            SqlDtAprFind.Fill(DtStFind);
            this.GridView1.DataSource = DtStFind;
            this.GridView1.DataBind();

            SqlConOEACustomers.Close();

            this.lblTitulo.Visible = true;
            this.lblTitulo.Text = "" + DtStFind.Tables[0].Rows.Count;
            DtStFind.Clear();
        }
    }

    # endregion

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
      
        try
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";


            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                 this.GridView1.AllowPaging = false;
                LoadGridView();

                 this.GridView1.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in  this.GridView1.HeaderRow.Cells)
                {
                    cell.BackColor =  this.GridView1.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in  this.GridView1.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor =  this.GridView1.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor =  this.GridView1.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

               this.GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            

        }
        catch (Exception ex)
        {

           
        }

      
    }
}