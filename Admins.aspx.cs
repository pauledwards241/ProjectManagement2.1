using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ProjectTableAdapters;
public partial class Admins : System.Web.UI.Page
{
    ProjectBLL projectbll = new ProjectBLL();
    private bool login = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        //lblMessage.Text = "You are not authorised to see the content of this page, please login";
        if (login)
        {
            UpdatePanel1.Visible = false;
        }
        else
        {
            UpdatePanel1.Visible = true;
        }

        /*DDLDepartment.Visible = false;
        DDLStatus.Visible = false;
        Button1.Visible = false;*/

    }

    private void ProjectBind()
    {
        int depID = int.Parse(DDLDepartment.SelectedValue);
        int statusID = int.Parse(DDLStatus.SelectedValue);
        Project.ProjectDataTable table = null;
        if (statusID == -1 && depID == -1)
        {
            table = projectbll.GetData();
        }
        else
        {
            if (statusID == -1)
            {
                table = projectbll.getDatabyDepartment(depID);
            }
            else if (depID == -1)
            {
                table = projectbll.GetDataByStatus(statusID);
            }
            else
            {
                table = projectbll.GetDataBydepandStatus(statusID, depID);
            }
        }
        GridView1.DataSource = table;
        GridView1.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string projectcode = TextBox1.Text;
        if (!string.IsNullOrEmpty(projectcode))
        {
            Project.ProjectDataTable table = projectbll.GetDataByprojectCode(projectcode);
            GridView1.DataSource = table;
            GridView1.DataBind();
        }
    }
    protected void DDLDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProjectBind();
    }
    protected void DDLStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProjectBind();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton del = (LinkButton)e.Row.FindControl("btnDelete");
            del.Attributes.Add("onclick", "javascript:return " + "confirm('are you sure you want to delete this record ')");
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteProject")
        {
            int projectid = int.Parse(e.CommandArgument.ToString());
            projectbll.DeleteProject(projectid);
            ProjectBind();
        }

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        if (TxtUserName.Text == "admin" && TxtPassword.Text == "password")
        {
            lblMessage.Visible = false;
            ProjectBind();
            UpdatePanel1.Visible = true;
            login = true;
        }
        else
        {
            lblMessage.Text = "Login Failed! Try Agina";
            lblMessage.Visible = true;
            UpdatePanel1.Visible = false;
            login = false;

        }
    }
}
