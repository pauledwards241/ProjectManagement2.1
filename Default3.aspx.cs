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
using Reimers.Map;
using Reimers;
using ProjectTableAdapters;
using System.IO;

public partial class Default3 : System.Web.UI.Page
{
    private GoogleIcon i1;
    private string addnew;

    private double Lat
    {
        get
        {
            if (ViewState["Lat"] != null)
            {
                return double.Parse(ViewState["Lat"].ToString());
            }
            else
            {
                return -1;
            }
        }
        set { ViewState["Lat"] = value; }
    }

    private double Lon
    {
        get
        {
            if (ViewState["Lon"] != null)
            {
                return double.Parse(ViewState["Lon"].ToString());
            }
            else
            {
                return -1;
            }
        }
        set { ViewState["Lon"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UpdatePanel2.ChildrenAsTriggers = true;
        if (!IsPostBack)
        {
            GoogleMap1.TypeControl = MapTypeControl.Normal;
            ScriptManager1.RegisterAsyncPostBackControl(Button1);
            ScriptManager1.RegisterAsyncPostBackControl(Button2);
            ScriptManager1.RegisterAsyncPostBackControl(Btn_newProject);
            addnew = "false";
            //ClientScript.RegisterStartupScript( this.GetType(),"IniMap", "initMap()");

        }
        //this.set
        addnew = this.hidden_new.Value;
        Address.Attributes.Add("onkeypress", string.Format("return clickButton(event,\"{0}\")", this.Button1.ClientID));
        GoogleMap1.Click += new ClickHandler(MapClick);
        BindingData("Full", 0);
        ClientScript.RegisterStartupScript(this.GetType(), "ini", "initialize()");
    }

    public void MapClick(GoogleMap Map, GoogleLatLng Position, ref string MapCommand)
    {
        //  = "addmakers(" + Position.Latitude + "," + Position.Longitude + ")";
        MapCommand = "showEntry(" + Position.Latitude + "," + Position.Longitude + ")";

        // MapCommand += "alert(" + Txt_Address.Text + ");";


        //
    }

    public void BindingData(string option, int id)
    {
        ProjectBLL _projectbll = new ProjectBLL();
        Project.ProjectDataTable projectTable;
        switch (option)
        {
            case "Full":
                {
                    projectTable = _projectbll.GetData();

                    break;
                }

            case ("Status"):
                {
                    projectTable = _projectbll.GetDataByStatus(id);
                    break;
                }
            case ("Department"):
                {
                    projectTable = _projectbll.getDatabyDepartment(id);
                    break;
                }
            default:
                {
                    projectTable = _projectbll.GetData();

                    break;
                }
        }

        GridView1.DataSource = projectTable;
        GetMarkers(projectTable);
        GridView1.DataBind();
    }

    public void GetMarkers(Project.ProjectDataTable _table)
    {
        GoogleMap1.PostRenderScript += GoogleMap1.ClearOverlays();
        foreach (Project.ProjectRow ProRow in _table)
        {
            GoogleMarker marker = new GoogleMarker();
            marker.Latitude = ProRow.lat;
            marker.Longitude = ProRow.lon;
            GoogleMarkerOptions option = new GoogleMarkerOptions();
            option.Title = ProRow.Project_Code;

            string path = "GGIcon";
            path += "/" + ProRow.Status.Trim();
            path += "/" + ProRow.Name.Trim() + ".png";
            i1 =
                new GoogleIcon(ProRow.Project_ID.ToString(), path, "", new GoogleSize(30, 30), new GoogleSize(30, 30),
                               new GooglePoint(3, 5), new GooglePoint(4, 5), new GooglePoint(9, 34));
            GoogleMap1.Icons.Add(i1);
            marker.Icon = ProRow.Project_ID.ToString();
            marker.OnClientClick = GoogleMap1.OpenInfoWindowHTML(new GoogleLatLng(ProRow.lat, ProRow.lon), "<B>" + ProRow.Project_Code + "</b>")
            ;
            //marker.OpenInfoWindowHTML(GoogleMap1, ,GoogleInfoWindowOptions);


            // marker.Options = option;
            GoogleMap1.PostRenderScript += GoogleMap1.AddOverlay(marker);
        }
    }


    public void SetupMap(GoogleMap _map)
    {
        _map.MapControl = ControlType.Zoom;
        _map.MapType = MapType.Default;
        _map.ShowScaleControl = true;
        _map.ShowOverviewMap = true;
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string strPoint = this.Hidden1.Value;
        if (string.IsNullOrEmpty(strPoint))
        {
            Label1.Text = "Address not found";
        }
        else
        {
            string[] pagParams = strPoint.Split(',');
            pagParams[0] = pagParams[0].Remove(pagParams[0].IndexOf('('), 1);
            pagParams[1] = pagParams[1].Remove(pagParams[1].IndexOf(')'), 1);
            double lat = double.Parse(pagParams[0].Trim());
            double lon = double.Parse(pagParams[1].ToString());
            GoogleLatLng point = new GoogleLatLng(lat, lon);
            GoogleMap1.PostRenderScript = GoogleMap1.SetCenter(point);
            GoogleMap1.PostRenderScript += GoogleMap1.SetZoom(20);
            GoogleMap1.PostRenderScript += GoogleMap1.OpenInfoWindowHTML(point, "<b>" + this.Address.Text + "</B>");
        }
        GetMarkers((Project.ProjectDataTable)GridView1.DataSource);
        UpdatePanel1.Update();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindingData("Status", int.Parse(DropDownList1.SelectedItem.Value));
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        if (e.CommandName == "go")
        {
            int _selectedIndex = int.Parse(e.CommandArgument.ToString());
            Label _txtLat = (Label)_gridview.Rows[_selectedIndex].FindControl("Txt_Lat");
            Label _txtLon = (Label)_gridview.Rows[_selectedIndex].FindControl("Txt_lon");

            GoogleLatLng center = new GoogleLatLng(double.Parse(_txtLat.Text), double.Parse(_txtLon.Text));
            GoogleMap1.PostRenderScript += GoogleMap1.SetCenter(center);
            GoogleMap1.PostRenderScript += GoogleMap1.SetZoom(16);
            GoogleMap1.PostRenderScript +=
                GoogleMap1.OpenInfoWindowHTML(center, _gridview.Rows[_selectedIndex].Cells[0].Text);
        }

        if (e.CommandName == "List")
        {
            int _selectedIndex = int.Parse(e.CommandArgument.ToString());
            Label _txtID = (Label)_gridview.Rows[_selectedIndex].FindControl("Txt_ID");
            Response.Redirect("detail.aspx?project_id=" + _txtID.Text, false);
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }

    protected void GoogleMap1_MoveEnd(GoogleMap Map, GoogleLatLng Position, GoogleBounds Bounds, ref string MapCommand)
    {
        // 
    }

    protected void Btn_new_Click(object sender, EventArgs e)
    {
        //this.addnew = true;
    }

    protected void Btn_newProject_Click(object sender, EventArgs e)
    {
        bool valid = true;
        if (string.IsNullOrEmpty(Txt_Code.Text))
        {
            projectcode_validate.Visible = true;
            UpdatePanel2.Update();
            valid = false;
            return;
        }
        else
        {
            valid = true;
            projectcode_validate.Visible = false;
        }
        if (string.IsNullOrEmpty(Txt_StartDate.Text))
        {
            Validate_Startdate.Visible = true;
            UpdatePanel2.Update();
            valid = false;
            return;
        }
        else
        {
            valid = true;
            Validate_Startdate.Visible = false;
        }
        if (string.IsNullOrEmpty(TxtEndDate.Text))
        {
            Validate_enddate.Visible = true;
            UpdatePanel2.Update();
            valid = false;
            return;
        }
        else
        {
            valid = true;
            Validate_enddate.Visible = false;
        }
        if (string.IsNullOrEmpty(Txt_city.Text))
        {
            valid = true;
            Validate_city.Visible = true;
            valid = false;
            UpdatePanel2.Update();
            return;
        }
        else
        {
            valid = true;
            Validate_city.Visible = false;
        }

        if (valid == true)
        {
            ProjectBLL project = new ProjectBLL();

            if (project.InsertProject(Txt_Code.Text,
                                      Txt_StartDate.Text,
                                      TxtEndDate.Text,
                                      int.Parse(Dropdown_Status.SelectedValue),
                                      Txt_Contact.Text,
                                      Txt_Address.Text,
                                      Txt_city.Text,
                                      int.Parse(DropDownList_department.SelectedItem.Value),
                                      Txt_desc.Text,
                                      double.Parse(TxtLat.Text),
                                      double.Parse(TxtLon.Text)
                    ) == 1)
            {
                UpdatePanel1.Update();
            }
        }
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Lat = 50454324;
        Lon = 59552252;
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DropDownList2_SelectedIndexChanged1(object sender, EventArgs e)
    {
        BindingData("Department", int.Parse(DropDownList2.SelectedValue));
    }
}
