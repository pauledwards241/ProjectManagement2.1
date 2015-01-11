using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ProjectTableAdapters;
/// <summary>
/// Summary description for StatusBLL
/// </summary>
public class StatusBLL
{
	public StatusBLL()
	{
	}

    private statusTableAdapter _adapter;
    public statusTableAdapter Adapter
    {
        get
        {
            if(_adapter!=null)
            {
                return _adapter;
            }
            else
            {
                return new statusTableAdapter();
            }
        }
        set
        {
            _adapter= value;
        }
    }

    public  Project.statusDataTable GetData()
    {
        return Adapter.GetData();
    }
}
