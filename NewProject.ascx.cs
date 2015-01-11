using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class NewProject : System.Web.UI.UserControl
{
    private double langitude;
    public double Langitude
    {
        get
        {
            return langitude;
        }
        set
        {
            langitude = value;
        }
    }

    private double latitude;
    public  double Latitude
    {
        get
        {
            return latitude;
        }
        set
        {
            latitude = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    

}
