using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Map : System.Web.UI.Page
{
    ProjectBLL _projectbll = new ProjectBLL();
    Project.ProjectDataTable projectTable;
    private Int32 total = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterAsyncPostBackControl(BTNaddnew);

        if (!IsPostBack)
            BindingData();

        DeleteConfirmation.Visible = IsPostBack ? false : Request.QueryString["delete"] == "1";
    }

    public string GetPrevious()
    {
        if (string.IsNullOrEmpty(Request.Params["lat"]))
        {
            return "none";
        }
        else if (string.IsNullOrEmpty(Request.Params["lng"]))
        {
            return "none";
        }
        else
        {
            return string.Format("{0},{1},{2}", Request.Params["lat"], Request.Params["lng"], Request.Params["code"]);
        }
    }

    public string GetIconPath()
    {
        return "GGIcon" + "/" + Dropdown_Status.SelectedItem.Text.Trim() + "/" + DropDownList_department.SelectedItem.Text.Trim() + ".png";
    }

    public void BindingData()
    {
        Int32? statusId = Int32.Parse(DropDownList1.SelectedValue);
        Int32? departmentId = Int32.Parse(DropDownList2.SelectedValue);
        Int32? sectorId = Int32.Parse(DropDownList3.SelectedValue);
        string projectSearchText = txtProjectSearch.Text.Trim();

        statusId = statusId >= 0 ? statusId : null;
        departmentId = departmentId >= 0 ? departmentId : null;
        sectorId = sectorId >= 0 ? sectorId : null;
        projectSearchText = !string.IsNullOrEmpty(projectSearchText) ? projectSearchText : null;

        projectTable = _projectbll.GetProjects(statusId, departmentId, sectorId, projectSearchText);

        total = projectTable.Count;
        GridView1.DataSource = projectTable;

        if (projectTable.Count != 0)
        {
            StringBuilder script = new StringBuilder("<script type='text/javascript'>");
            script.AppendLine("var markers = [];");
            foreach (Project.ProjectRow row in projectTable)
            {
                string iconPath = "GGIcon" + "/" + row.Status.Trim() + "/" + row.Name.Trim().Replace('/', '-') + ".png";
                if (row.IsProject_CodeNull())
                {
                    row.Project_Code = string.Empty;
                }
                row.Project_Code = row.Project_Code.Replace("'", "\"");
                script.AppendLine(string.Format("markers.push(addDatamarkers({0},{1},'{2}','{3}','{4}','{5}','{6}'));", row.lat, row.lon, Server.HtmlEncode(row.Project_Code), iconPath, row.Project_ID, row.Status, row.Name));
            }
            script.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "add", script.ToString());
        }
        GridView1.DataBind();
    }

    protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridView _Gridview = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //int _selecteIndex = e.Row.RowIndex;
            LinkButton _mapbutton = (LinkButton)e.Row.FindControl("LBTMap");
            Label _TextLat = (Label)e.Row.FindControl("Txt_lat");
            Label _TextLon = (Label)e.Row.FindControl("Txt_lon");
            Label _TextProjectID = (Label)e.Row.FindControl("Txt_ID");
            string strProjectID = _TextProjectID.Text;
            string status = e.Row.Cells[2].Text;
            string department = e.Row.Cells[4].Text;
            _mapbutton.Attributes.Add("onclick", string.Format("getView({0},{1}, '{2}')", _TextLat.Text, _TextLon.Text, strProjectID));

            DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
            ddlStatus.SelectedValue = status;
            ddlStatus.Attributes.Add("onchange", string.Format("changeStatus({0}, {1}, {2})", strProjectID, status, ddlStatus.ClientID));
            LinkButton _DetailButton = (LinkButton)e.Row.FindControl("LTNDetail");
            Label Detailed_entry = (Label)e.Row.FindControl("LblDetail");
            string strDetail = Detailed_entry.Text;
            if (strDetail == "True")
            {
                _DetailButton.Text = "View Detail";
                _DetailButton.PostBackUrl = string.Format("Detail.aspx?projectID={0}&Mode=View", strProjectID);

            }
            else if (strDetail == "False")
            {
                _DetailButton.Text = "Enter Detail";
                _DetailButton.PostBackUrl = string.Format("Detail.aspx?projectID={0}&Mode=Entry", strProjectID);
            }

        }

        if (e.Row.RowType == DataControlRowType.Pager)
        {
            Label LblTotalPage = (Label)e.Row.FindControl("LblTotalPage");
            LblTotalPage.Text = _Gridview.PageCount.ToString();
            TextBox TxtGoToPage = (TextBox)e.Row.FindControl("TxtGoToPage");
            TxtGoToPage.Text = (_Gridview.PageIndex + 1).ToString();

            DropDownList DDLPageSize = (DropDownList)e.Row.FindControl("DDLPageSize");
            DDLPageSize.SelectedValue = _Gridview.PageSize.ToString();
            Label lblTotal = (Label)e.Row.FindControl("lblTotal");
            lblTotal.Text += total.ToString();
        }
    }

    protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        if (e.NewPageIndex >= 0 && e.NewPageIndex < GridView1.PageCount)
        {
            GridView1.PageIndex = e.NewPageIndex;
        }
        BindingData();
    }

    protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        if (e.CommandName == "go")
        {
            int _selectedIndex = int.Parse(e.CommandArgument.ToString());
            Label _txtLat = (Label)_gridview.Rows[_selectedIndex].FindControl("Txt_Lat");
            Label _txtLon = (Label)_gridview.Rows[_selectedIndex].FindControl("Txt_lon");
            //this.ClientScript.RegisterStartupScript(this.GetType(),"key","<script type=>");
        }
    }

    protected void BTNaddnew_Click(object sender, EventArgs e)
    {

        double lat = double.Parse(this._TxtLat.Value.ToString());
        double lng = double.Parse(this._TxtLng.Value.ToString());
        string project_code = this.TxtProjectCode.Text;
        string project_name = this.TextProjectName.Text;

        int row = _projectbll.InsertBasics(project_code, int.Parse(Dropdown_Status.SelectedValue),
                                 int.Parse(DropDownList_department.SelectedValue),
                                 lat, lng,
                                 project_name);
        UpdatePanel1.Update();
        BindingData();
    }

    protected void Filter_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindingData();
        UpdatePanel1.Update();
    }

    [System.Web.Services.WebMethod]
    public static void UpdateStatus(int projectid, int statusValue)
    {
        ProjectBLL Adapter = new ProjectBLL();
        Adapter.updateProjectStatus(projectid, statusValue);
    }

    [System.Web.Services.WebMethod]
    public static void SaveProjectLatLng(int projectid, double lat, double lng)
    {
        ProjectBLL Adapter = new ProjectBLL();
        Adapter.updateLatLng(projectid, lat, lng);
    }

    [System.Web.Services.WebMethod]
    public static string GetPostCode(string postcode)
    {
        Postcode p = new Postcode();

        Postcode returnP = p.GetLatLng(postcode);

        if (returnP != null)
        {
            return string.Format("{0},{1}", returnP.Lat, returnP.Lng);
        }
        else
        {
            return string.Format("{0} not Found", postcode);
        }
    }

    [System.Web.Services.WebMethod]
    public static bool ValidateProjectCode(string projectCode)
    {
        return new ProjectBLL().ValidateProjectCode(null, projectCode);
    }

    protected void DDLPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDLPageSize = (DropDownList)sender;
        this.GridView1.PageSize = int.Parse(DDLPageSize.SelectedValue);
        BindingData();
    }

    protected void TxtGoToPage_TextChanged(object sender, EventArgs e)
    {
        TextBox TXTGoToPage = (TextBox)sender;
        int pageNumber;
        if (int.TryParse(TXTGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.GridView1.PageCount)
        {
            GridView1.PageIndex = pageNumber - 1;

        }
        else
        {
            GridView1.PageIndex = 0;
        }
        BindingData();
    }
}
