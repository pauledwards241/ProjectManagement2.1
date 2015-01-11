using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for GetForms
/// </summary>
public class GetForms:IHttpHandler
{
	public GetForms()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        ViewManager<UserControl>  viewmaManager = new ViewManager<UserControl>();
        UserControl control = viewmaManager.LoadViewControl("~/NewProject.ascx");
        context.Response.Write(viewmaManager.RenderView(control));
    }


    
#region IHttpHandler Members

bool  IHttpHandler.IsReusable
        {
            get
            {
                return true;
            }
        }
    


#endregion




}
