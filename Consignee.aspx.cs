using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;


public partial class Consignee : System.Web.UI.Page
{
    #region "SqlConnection"
    //private SqlConnection ConnSqlRegistro = new SqlConnection(@"Server=TECN-06\SQLEXPRESS;Database=CONTROLHIT;User ID=sa;Password=Jehova-07;");
    //private SqlConnection SqlConConsignee = new SqlConnection("Server=HIT-SQL01-1;Database=testn4;User ID=navistest;Password=navistest;");
    private SqlConnection SqlConConsignee = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;");
    private DataSet DtStConsignee = new DataSet();
    private SqlCommand SqlCmdConsignee = new SqlCommand();

    string Query;
    string Msg;
    int Mssg = 0;
    int IDUpdate;

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

            this.lblMessage.Text = "Insert";
            this.lblMessage.Visible = true;
            //this.lblIDUpdate.Text = "Null";
        }

        //Boton Guardar........
        if (Menu1.SelectedItem.Text == "Save")
        {
            if (this.TextBox1.Text == "")
            {
                this.lblMessage.Text = "There are textbox empty!";
                this.lblMessage.Visible = true;
                return;
            }

            #region"Pasando parametros"

            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@action", SqlDbType.NVarChar);
            param[0].Value = this.lblMessage.Text;
            //param[0].Value = "Insert";

            param[1] = new SqlParameter("@DGA", SqlDbType.NVarChar);
            param[1].Value = this.TextBox1.Text;

            param[2] = new SqlParameter("@MERCANCIA", SqlDbType.NVarChar);
            param[2].Value = this.TextBox2.Text;

            param[3] = new SqlParameter("@PAQUETE", SqlDbType.NVarChar);
            param[3].Value = this.TextBox3.Text;

            param[4] = new SqlParameter("@PAQMEDIDA", SqlDbType.NVarChar);
            param[4].Value = this.TextBox4.Text;

            param[5] = new SqlParameter("@MEDIDA", SqlDbType.NVarChar);
            param[5].Value = this.TextBox5.Text;

            param[6] = new SqlParameter("@UNIDMEDIDA", SqlDbType.NVarChar);
            param[6].Value = this.TextBox6.Text;

            param[7] = new SqlParameter("@RNC", SqlDbType.NVarChar);
            param[7].Value = this.TextBox7.Text;

            param[8] = new SqlParameter("@CONSIGNATARIO", SqlDbType.NVarChar);
            param[8].Value = this.TextBox8.Text;

            if (this.lblMessage.Text == "Insert")
            {
                param[9] = new SqlParameter("@ID", SqlDbType.Int);
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9] = new SqlParameter("@ID", SqlDbType.Int);
                param[9].Value = Convert.ToInt32(this.lblIDUpdate.Text);
            }

            #endregion

            try
            {
                //Conectando con StoreProcedure
                SqlConConsignee.Open();
                SqlCmdConsignee.Connection = SqlConConsignee;
                SqlCmdConsignee.Parameters.AddRange(param);
                SqlCmdConsignee.CommandType = CommandType.StoredProcedure;
                SqlCmdConsignee.CommandText = "sp_CargaGeneralSuelta";
                SqlCmdConsignee.ExecuteNonQuery();
                SqlConConsignee.Close();

                if (this.lblMessage.Text == "Insert")
                {
                    this.lblMessage.Visible = true;
                    this.lblMessage.Text = "Register completed!";
                    Jalert.MessageBoxSuccess(this, "Register completed!");
                }
            }
            catch (Exception ex)
            {
                this.Msg = ex.Message;
                //throw new Exception(ex.Message);
                this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
                Jalert.MessageBoxError(this, this.Msg);

            }
            finally
            {
                //Inhabilitando TextBox
                InhabilitandoTextBox();

                //Limpiar TextBox
                LimpiandoTextBox();

                if (this.lblMessage.Text == "Update")
                {
                    SqlConConsignee.Open();
                    string QueryEdit = "SELECT * FROM dbo.hit_carga_general_suelta WHERE ID = ('" + Convert.ToInt32(this.lblIDUpdate.Text) + "')";
                    SqlCommand CmdEdit = new SqlCommand(QueryEdit, SqlConConsignee);
                    SqlDataAdapter SqlDataAdapterEdit = new SqlDataAdapter(CmdEdit);
                    DataSet DataSetEdit = new DataSet();
                    SqlDataAdapterEdit.Fill(DataSetEdit);
                    this.GridView1.DataSource = DataSetEdit;
                    //this.GridView1.DataSource = DataSetEdit.Tables[0].DefaultView;
                    this.GridView1.DataBind();
                    SqlConConsignee.Close();

                    this.lblMessage.Text = "Register Updated!";
                    Jalert.MessageBoxSuccess(this, "Register Updated!");
                }
            }
        }

        //Boton Cancelar.........
        if (Menu1.SelectedItem.Text == "Cancel")
        {
            //Limpiando TextBox
            LimpiandoTextBox();
            this.lblMessage.Text = "Canceled";
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
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (e.NewPageIndex != -1)
            {
                GridView1.PageIndex = e.NewPageIndex;
                SqlCmdConsignee.Parameters.Clear();
                LoadGridView();
            }
        }
        catch (Exception ex)
        {
            this.Msg = ex.Message;
            this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
            Jalert.MessageBoxError(this, this.Msg);
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //int index = Convert.ToInt32(e.CommandArgument);
            //ImageButton boton = (ImageButton)GridView1.Rows[index].Cells[3].Controls[0];
            int ID = Convert.ToInt32(e.CommandArgument.ToString());
            string Comando = e.CommandName;

            if (Comando == "Delete")
            {
                #region "Obtener Pagina"

                //Int32 index = Convert.ToInt32(e.CommandArgument);
                //GridViewRow row;
                //if (GridView1.PageIndex == 0)
                //{
                //    row = GridView1.Rows[index];
                //}
                //else
                //{
                //    Int32 totalPag = Convert.ToInt32(GridView1.PageCount);
                //    Int32 itemActl = (totalPag - (totalPag - Convert.ToInt32(GridView1.PageIndex))) * Convert.ToInt32(GridView1.PageSize);
                //    row = GridView1.Rows[index - itemActl];
                //}

                //DataKey key = GridView1.DataKeys[row.RowIndex];
                #endregion

                //Procedimiento para eliminar.
                SqlConConsignee.Open();
                string QueryDelete = "DELETE dbo.hit_carga_general_suelta WHERE ID = ('" + ID + "')";
                SqlCommand CmdDelete = new SqlCommand(QueryDelete, SqlConConsignee);
                SqlDataAdapter SqlDataAdapterDelete = new SqlDataAdapter(CmdDelete);
                DataSet DataSetDelete = new DataSet();
                SqlDataAdapterDelete.Fill(DataSetDelete);
                this.GridView1.DataSource = DataSetDelete;
                //this.GridView1.DataBind();
                SqlConConsignee.Close();

                lblMessage.Text = "Delete";
            }

            if (Comando == "Update")
            {
                //Procedimiento para Actualizar.
                SqlConConsignee.Open();
                string QueryEdit = "SELECT * FROM dbo.hit_carga_general_suelta WHERE ID = ('" + ID + "')";
                SqlCommand CmdEdit = new SqlCommand(QueryEdit, SqlConConsignee);
                SqlDataAdapter SqlDataAdapterEdit = new SqlDataAdapter(CmdEdit);
                DataSet DataSetEdit = new DataSet();
                SqlDataAdapterEdit.Fill(DataSetEdit);
                this.GridView1.DataSource = DataSetEdit;
                //this.GridView1.DataSource = DataSetEdit.Tables[0].DefaultView;
                this.GridView1.DataBind();
                SqlConConsignee.Close();

                lblMessage.Text = "Update";

                this.HabilitandoTextBox();
                this.LimpiandoTextBox();

                this.IDUpdate = Convert.ToInt32(DataSetEdit.Tables["Table"].Rows[0].ItemArray[0].ToString().Trim());
                TextBox1.Text = DataSetEdit.Tables["Table"].Rows[0].ItemArray[1].ToString().Trim();
                TextBox2.Text = DataSetEdit.Tables["Table"].Rows[0].ItemArray[2].ToString().Trim();
                TextBox3.Text = DataSetEdit.Tables["Table"].Rows[0].ItemArray[3].ToString().Trim();
                TextBox4.Text = DataSetEdit.Tables["Table"].Rows[0].ItemArray[4].ToString().Trim();
                TextBox5.Text = DataSetEdit.Tables["Table"].Rows[0].ItemArray[5].ToString().Trim();
                TextBox6.Text = DataSetEdit.Tables["Table"].Rows[0].ItemArray[6].ToString().Trim();
                TextBox7.Text = DataSetEdit.Tables["Table"].Rows[0].ItemArray[7].ToString().Trim();
                TextBox8.Text = DataSetEdit.Tables["Table"].Rows[0].ItemArray[8].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            this.Msg = ex.Message;
            this.lblMessage.Text = string.Concat(this.lblMessage.Text, " [ ", this.Mssg = this.Mssg + 1, " ] --> ", this.Msg, " <-- ");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error!')", true);
            Jalert.MessageBoxError(this, this.Msg);

        }
        finally
        {
            if (lblMessage.Text == "Delete")
            {
                //Response.Redirect("~/Clientes.aspx?ID=" + (string)e.CommandArgument);
                string url = "~/Consignee.aspx?";
                //url += "Msg=" + "Este registro ha sido actualizado";
                Response.Redirect(url + (string)e.CommandArgument);
                lblMessage.Text = "Row deleted!";
            }
            if (lblMessage.Text == "Update")
            {
                lblIDUpdate.Text = Convert.ToString(this.IDUpdate);
            }
        }
    }

    #region "IHL TEXTBOX"

    private void InhabilitandoTextBox()
    {
        //Inhabilitando Campos
        this.TextBox1.Enabled = false;
        this.TextBox2.Enabled = false;
        this.TextBox3.Enabled = false;
        this.TextBox4.Enabled = false;
        this.TextBox5.Enabled = false;
        this.TextBox6.Enabled = false;
        this.TextBox7.Enabled = false;
        this.TextBox8.Enabled = false;
    }

    private void HabilitandoTextBox()
    {
        //Habilitando Campos
        this.TextBox1.Enabled = true;
        this.TextBox2.Enabled = true;
        this.TextBox3.Enabled = true;
        this.TextBox4.Enabled = true;
        this.TextBox5.Enabled = true;
        this.TextBox6.Enabled = true;
        this.TextBox7.Enabled = true;
        this.TextBox8.Enabled = true;
    }

    private void LimpiandoTextBox()
    {
        //Limpiar Campos
        this.TextBox1.Text = "";
        this.TextBox2.Text = "";
        this.TextBox3.Text = "";
        this.TextBox4.Text = "";
        this.TextBox5.Text = "";
        this.TextBox6.Text = "";
        this.TextBox7.Text = "";
        this.TextBox8.Text = "";
        this.TextBox1.Focus();
    }

    #endregion

    #region "FUNCTION"

    private void LoadGridView()
    {
        //Pasando parametros
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@action", SqlDbType.NVarChar);
        param[0].Value = "Query";

        param[1] = new SqlParameter("@ID", SqlDbType.Int);
        param[1].Value = DBNull.Value;

        //Conectando con StoreProcedure
        SqlConConsignee.Open();
        SqlCmdConsignee.Connection = SqlConConsignee;
        SqlCmdConsignee.Parameters.AddRange(param);
        SqlCmdConsignee.CommandType = CommandType.StoredProcedure;
        SqlCmdConsignee.CommandText = "dbo.sp_CargaGeneralSuelta";
        SqlCmdConsignee.ExecuteNonQuery();
        SqlConConsignee.Close();

        //SqlDataAdapter
        SqlDataAdapter SqlDataAdapterRegistro = new SqlDataAdapter(SqlCmdConsignee);

        SqlDataAdapterRegistro.Fill(DtStConsignee);

        //Llenando el GridView
        this.GridView1.DataSource = DtStConsignee.Tables[0].DefaultView;
        this.GridView1.DataBind();
        SqlConConsignee.Close();

        //Cargando el Combobox de Buscar con los campos de la Tabla
        // ''''''''''''''
        if (this.DropDownListFields.Items.Count <= 1)
        {
            foreach (DataColumn dc in DtStConsignee.Tables[0].Columns)
            {
                this.DropDownListFields.Items.Add(new ListItem(dc.ColumnName));
            }
        }

        //Limpiando el DataSet
        DtStConsignee.Clear();

        //Asignando el index al DropDownList
        //this.DropDownList2.Items[0].Text = "--";
    }

    private void Buscar()
    {
        if ((this.txtFind.Text != "") && (this.DropDownListFields.Text != "--"))
        {

            SqlConConsignee.Open();
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=EMS;User ID=sa;Password=Jehova-07");
            if (this.DropDownListFields.SelectedItem.Text == "RNC")
            {
                Query = "SELECT * FROM dbo.hit_carga_general_suelta WHERE STR(" + this.DropDownListFields.SelectedItem.Text + ") LIKE ('%" + this.txtFind.Text + "%') ";
            }
            else
            {
                Query = "SELECT * FROM dbo.hit_carga_general_suelta WHERE " + this.DropDownListFields.SelectedItem.Text + " LIKE ('%" + this.txtFind.Text + "%') ";
            }
            SqlCommand SqlCmdFind = new SqlCommand(Query, SqlConConsignee);
            SqlDataAdapter SqlDtAprFind = new SqlDataAdapter(SqlCmdFind);
            DataSet DtStFind = new DataSet();
            SqlDtAprFind.Fill(DtStFind);
            this.GridView1.DataSource = DtStFind;
            this.GridView1.DataBind();

            SqlConConsignee.Close();

            this.lblTitulo.Visible = true;
            this.lblTitulo.Text = "" + DtStFind.Tables[0].Rows.Count;
            DtStFind.Clear();
        }
    }

    # endregion

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetListofDGACode(string prefixText)
   {
        using (SqlConnection sqlconn = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;"))
        {
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT DGA FROM dbo.hit_carga_general_suelta WHERE DGA LIKE '" + prefixText + "%' ", sqlconn);
            cmd.Parameters.AddWithValue("@DGA", prefixText);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> Lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Lst.Add(dt.Rows[i]["DGA"].ToString());
            }
            return Lst;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetListofMercancia(string prefixText)
    {
        using (SqlConnection sqlconn = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;"))
        {
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT MERCANCIA FROM dbo.hit_carga_general_suelta WHERE MERCANCIA LIKE '" + prefixText + "%' ", sqlconn);
            cmd.Parameters.AddWithValue("@MERCANCIA", prefixText);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> Lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Lst.Add(dt.Rows[i]["MERCANCIA"].ToString());
            }
            return Lst;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetListofPaqMedida(string prefixText)
    {
        using (SqlConnection sqlconn = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;"))
        {
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT PAQMEDIDA FROM dbo.hit_carga_general_suelta WHERE PAQMEDIDA LIKE '" + prefixText + "%' ", sqlconn);
            cmd.Parameters.AddWithValue("@PAQMEDIDA", prefixText);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> Lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Lst.Add(dt.Rows[i]["PAQMEDIDA"].ToString());
            }
            return Lst;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetListofUnidMedida(string prefixText)
    {
        using (SqlConnection sqlconn = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;"))
        {
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT UNIDMEDIDA FROM dbo.hit_carga_general_suelta WHERE UNIDMEDIDA LIKE '" + prefixText + "%' ", sqlconn);
            cmd.Parameters.AddWithValue("@UNIDMEDIDA", prefixText);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> Lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Lst.Add(dt.Rows[i]["UNIDMEDIDA"].ToString());
            }
            return Lst;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetListofRNC(string prefixText)
    {
        using (SqlConnection sqlconn = new SqlConnection("Server=172.16.0.32;Database=apex;User ID=N4edi;Password=N4edi.2014;"))
        {
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT RNC FROM dbo.hit_carga_general_suelta WHERE RNC LIKE '" + prefixText + "%' ", sqlconn);
            cmd.Parameters.AddWithValue("@RNC", prefixText);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> Lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Lst.Add(dt.Rows[i]["RNC"].ToString());
            }
            return Lst;
        }
    }


    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //this.GridView1.EditIndex = e.NewEditIndex;
        //this.LoadGridView();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //// find student id of edit row
        //string id = GridView1.DataKeys[e.RowIndex].Value.ToString();
        //// find updated values for update
        //TextBox DGA = (TextBox)GridView1.Rows[e.RowIndex].FindControl("DGA");
        //TextBox MERCANCIA = (TextBox)GridView1.Rows[e.RowIndex].FindControl("MERCANCIA");
        //TextBox PAQUETE = (TextBox)GridView1.Rows[e.RowIndex].FindControl("PAQUETE");
        //TextBox PAQMEDIDA = (TextBox)GridView1.Rows[e.RowIndex].FindControl("PAQMEDIDA");
        //TextBox MEDIDA = (TextBox)GridView1.Rows[e.RowIndex].FindControl("MEDIDA");
        //TextBox UNIDMEDIDA = (TextBox)GridView1.Rows[e.RowIndex].FindControl("UNIDMEDIDA");
        //TextBox RNC = (TextBox)GridView1.Rows[e.RowIndex].FindControl("RNC");
        //TextBox CONSIGNATARIO = (TextBox)GridView1.Rows[e.RowIndex].FindControl("CONSIGNATARIO");

        //SqlCommand cmd = new SqlCommand("Update hit_carga_general_suelta set DGA='" + DGA.Text + "', MERCANCIA='" + MERCANCIA.Text + "', PAQUETE='" + PAQUETE.Text + "', PAQMEDIDA='" + PAQMEDIDA.Text + "', MEDIDA='" + MEDIDA.Text + "', UNIDMEDIDA='" + UNIDMEDIDA.Text + "' , RNC='" + RNC.Text + "' , CONSIGNATARIO='" + CONSIGNATARIO.Text + "' where ID=" + id, SqlConConsignee);
        //SqlConConsignee.Open();
        //cmd.ExecuteNonQuery();
        //SqlConConsignee.Close();

        //GridView1.EditIndex = -1;
        //this.LoadGridView();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //this.GridView1.EditIndex = -1;
        //this.LoadGridView();
    }

}