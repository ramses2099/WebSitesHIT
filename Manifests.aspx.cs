using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using WebSitesHIT;
  
public partial class Manifests : System.Web.UI.Page
{      
    protected void Page_Load(object sender, EventArgs e)
    {
        ClassConnection NewConnection = new ClassConnection();
        NewConnection.Connection_Today();

        SqlCommand comm = new SqlCommand();
        comm.Connection = ClassConnection.conn;

        // There are two field in the table but int is (1,1) so parameter
        // is not required.
        comm.CommandText = "insert into Testing values (@name)";
        comm.Parameters.AddWithValue("name", TextBox1.Text);
        comm.ExecuteNonQuery();
                    
    }
}