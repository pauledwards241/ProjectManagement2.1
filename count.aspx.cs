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
using ProjectTableAdapters;
using System.Collections.Generic;
public partial class Images_count : System.Web.UI.Page
{
    ProjectTableAdapter _adapter = new ProjectTableAdapter();
    DepartmentBLL dBll = new DepartmentBLL();
    StatusBLL statusBll = new StatusBLL();
    private const string ASCENDING = " ASC";

    private const string DESCENDING = " DESC";

    private SortDirection GridViewSortDirection
    {

        get
        {

            if (ViewState["sortDirection"] == null)

                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];

        }

        set { ViewState["sortDirection"] = value; }

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = string.Format("Total project count: {0}", _adapter.projectcount());

        DataTable table = new DataTable();
        DataColumn departmentcol = new DataColumn("Department", typeof(string));
        table.Columns.Add(departmentcol);
        Project.statusDataTable statusTable = statusBll.GetData();
        foreach (Project.statusRow statusRow in statusTable.Rows)
        {
            DataColumn statusCol = new DataColumn(statusRow.Status.Trim(), typeof(int));
            table.Columns.Add(statusCol);
        }
        DataColumn totalCol = new DataColumn("Total", typeof(int));
        table.Columns.Add(totalCol);

        Project.DepartmentDataTable dTable = dBll.GetData();
        Project.statusDataTable sTable = statusBll.GetData();
        foreach (Project.DepartmentRow department in dTable.Rows)
        {
            DataRow row = table.NewRow();
            row["Department"] = department.Name;
            row["Total"] = _adapter.DepartmentCount(department.Dep_ID);
            foreach (Project.statusRow sRow in sTable.Rows)
            {
                foreach (DataColumn c in table.Columns)
                {
                    if (sRow.Status.Trim() == c.ColumnName)
                    {
                        row[c] = _adapter.detailprojectcount(sRow.Status_ID, department.Dep_ID);
                    }
                }
            }
            table.Rows.Add(row);

        }

        table.AcceptChanges();
        GridView1.DataSource = table;
        GridView1.DataBind();

        /* List<ProjectCount> list = new List<ProjectCount>();

         Project.DepartmentDataTable dTable = dBll.GetData();
        
         foreach(Project.DepartmentRow department in dTable.Rows)
         {
             ProjectCount c = new ProjectCount();
             c.Department = department.Name;
             Project.statusDataTable sTable = statusBll.GetData();
             foreach (Project.statusRow status in sTable.Rows)
             {
                 if(status.Status_ID==1)
                 {
                     c.LiveTotal = (int)(_adapter.detailprojectcount(status.Status_ID,department.Dep_ID));
                     System.Diagnostics.Debug.WriteLine("dfas"+_adapter.detailprojectcount(status.Status_ID, department.Dep_ID));
                 }
                 else if(status.Status_ID ==2)
                 {
                     c.SpecTotal =(int) _adapter.detailprojectcount(status.Status_ID,department.Dep_ID);

                 }
                 else if(status.Status_ID ==3)
                 {
                     c.DeadTotal =(int) _adapter.detailprojectcount(status.Status_ID,department.Dep_ID);
                 }
                 c.Total = (int)_adapter.DepartmentCount(department.Dep_ID);
             }

            
         }

         Reapter1.DataSource = list;
         Reapter1.DataBind();
         */


    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = string.Format("Total");
            e.Row.Cells[1].Text = _adapter.StatusCount(1).ToString();
            e.Row.Cells[2].Text = _adapter.StatusCount(2).ToString();
            e.Row.Cells[3].Text = _adapter.StatusCount(3).ToString();
            e.Row.Cells[4].Text = _adapter.projectcount().ToString();
        }
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        /*DataTable table = GridView1.DataSource as DataTable;

        DataView dataview = new DataView(table);
        string sortExpression = e.SortExpression;
        ViewState["SortExpression"] = sortExpression;

        if (GridView1.SortExpression == e.SortExpression)
        {
            //dataview.Sort= e.SortExpression + " "+
        }
        

        dataview.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        GridView1.DataSource = dataview;
        GridView1.DataBind();*/

        string sortExpression = e.SortExpression;
        ViewState["SortExpression"] = sortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }

        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }

    private void SortGridView(string sortExpression, string p)
    {
        DataTable table = GridView1.DataSource as DataTable;

        DataView dataview = new DataView(table);

        dataview.Sort = sortExpression + p;
        GridView1.DataSource = dataview;
        GridView1.DataBind();
    }





    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = string.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                break;

            case SortDirection.Descending:
                newSortDirection = "DESC";
                break;
        }

        return newSortDirection;

    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int sortColumIndex = GetSortColumnIndex();
            if (sortColumIndex != -1)
            {
                AddSortImage(sortColumIndex, e.Row);
            }
        }
    }

    private int GetSortColumnIndex()
    {

        foreach (DataControlField field in GridView1.Columns)
        {

            if (field.SortExpression ==

                         (string)ViewState["SortExpression"])
            {

                return GridView1.Columns.IndexOf(field);

            }

        }

        return -1;

    }

    private void AddSortImage(int columnIndex, GridViewRow headerRow)
    {

        // Create the sorting image based on the sort direction.

        Image sortImage = new Image();

        if (GridViewSortDirection == SortDirection.Ascending)
        {

            sortImage.ImageUrl = "~/images/dt-arrow-up.png";

            sortImage.AlternateText = "Ascending Order";

        }

        else
        {

            sortImage.ImageUrl = "~/images/dt-arrow-dn.png";

            sortImage.AlternateText = "Descending Order";

        }

        // Add the image to the appropriate header cell.

        headerRow.Cells[columnIndex].Controls.Add(sortImage);

    }




}


class ProjectCount
{
    private int _liveTotal;
    private int _specTotal;
    private int _deadTotal;
    private int _total;
    private string _department;

    public int LiveTotal
    {
        get
        {
            return _liveTotal;
        }
        set
        {
            _liveTotal = value;
        }
    }

    public int SpecTotal
    {
        get
        {
            return _specTotal;
        }
        set
        {
            _specTotal = value;
        }
    }

    public int DeadTotal
    {
        get
        {
            return _deadTotal;
        }
        set
        {
            _deadTotal = value;
        }
    }

    public int Total
    {
        get
        {
            return _total;
        }
        set
        {
            _total = value;
        }
    }

    public string Department
    {
        get
        {
            return _department;
        }
        set
        {
            _department = value;
        }

    }
}