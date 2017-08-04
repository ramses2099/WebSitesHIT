using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web;

namespace WebSitesHIT
{
    /// <summary>
    /// Summary description for ClassConnection
    /// </summary>
    public class ClassConnection
    {
        public static SqlConnection conn = null;

        public void Connection_Today()
        {
            //
            // TODO: Add constructor logic here
            //
            conn = new SqlConnection("Server=HIT-SQL01;Database=XML_DBF;User ID=ConsultHit;Password=Jehova-07;");

            conn.Open();
        }
    }
}