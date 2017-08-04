using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for Jalert
/// </summary>
public sealed class Jalert
{
    private Jalert()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    //
    public static void MessageBoxSuccess(Page page, String message) {        
        ScriptManager.RegisterStartupScript(page,page.GetType(), 
            "jalert", "successAlert('Success','" + message + "');", true);
    }
    //
    public static void MessageBoxError(Page page, String message)
    {
        ScriptManager.RegisterStartupScript(page, page.GetType(),
            "jalert", "errorAlert('Error','" + message + "');", true);
    }
    //
}