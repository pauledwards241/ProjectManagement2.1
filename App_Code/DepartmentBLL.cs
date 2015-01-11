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
/// Summary description for DepartmentBLL
/// </summary>
public class DepartmentBLL
{
    private DepartmentTableAdapter _adapter = null;
    public DepartmentTableAdapter Adapter
    {
        get
        {
            if (_adapter == null)
            {
                return new DepartmentTableAdapter();
            }
            return _adapter;
        }
        set
        {
            _adapter = value;
        }
    }
    public DepartmentBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Project.DepartmentDataTable GetData()
    {
        return Adapter.GetData();
    }
}
