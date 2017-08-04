using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PathHitEdi <add key="PathXSL_Folder" value="\\N4JOBS\HIT_EDI\IN\XSL_Folder\"/>
/// </summary>
public sealed class PathHitEdi
{
    private PathHitEdi()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public  static string Path310 { get { return System.Configuration.ConfigurationManager.AppSettings["Path310"].ToString(); } }
    //
    public static string PathXMLFolder { get { return System.Configuration.ConfigurationManager.AppSettings["PathXML_Folder"].ToString(); } }
    //
    public static string PathXLSFolder { get { return System.Configuration.ConfigurationManager.AppSettings["PathXLS_Folder"].ToString(); } }
    //
    public static string Path301 { get { return System.Configuration.ConfigurationManager.AppSettings["Path301"].ToString(); } }



}