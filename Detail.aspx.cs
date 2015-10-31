using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using Table = iTextSharp.text.Table;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;
using System.Collections.Generic;
using GemBox.Spreadsheet;
using System.Net.Mail;
using System.Data.OleDb;

public partial class Detail : System.Web.UI.Page
{
    private ProjectBLL projectBLL = new ProjectBLL();
    private Hashtable myHashtable;

    const String ExcelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=No;'";

    protected Boolean HasJobSheet
    {
        get
        {
            return ViewState["HasJobSheet"] != null && (Boolean)ViewState["HasJobSheet"];
        }
        set
        {
            ViewState["HasJobSheet"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(Button1);

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.Params["Mode"]))
            {
                if (Request.Params["Mode"] == "View")
                {
                    DetailsView2.ChangeMode(DetailsViewMode.ReadOnly);
                }
                else if (Request.Params["Mode"] == "Entry")
                {
                    DetailsView2.ChangeMode(DetailsViewMode.Edit);
                }
            }

            DetailsView2Databinding();
        }

        lblEmailSuccess.Visible = false;
    }

    private void DetailsView2Databinding()
    {
        string ProjectID = Request.Params["ProjectID"];

        if (!string.IsNullOrEmpty(ProjectID))
        {
            Project.ProjectDataTable p = projectBLL.GetDataByID(ProjectID);

            DetailsView2.DataSource = p;

            GenerateJobSheet(p);

            Dictionary<String, Int16> FieldColumns = new Dictionary<String, Int16>();
            FieldColumns.Add( "AddedAt", 17 );
            FieldColumns.Add( "JobSheetSubmitted", 18 );
            FieldColumns.Add( "FeeProposalSubmitted", 19 );
            FieldColumns.Add( "AcceptanceOfServiceSubmitted", 20 );

            foreach (KeyValuePair<String, Int16> item in FieldColumns)
                DetailsView2.Fields[item.Value].Visible = p.Rows[0][item.Key] != DBNull.Value;

            DetailsView2.DataBind();
            DetailsView2.CssClass = DetailsView2.CurrentMode.ToString().ToLower();

            HasJobSheet = p.Rows[0]["JobSheetSubmitted"] != DBNull.Value;
            JobSheetMandatoryMarker.Visible = !HasJobSheet;
        }
    }

    private void GenerateJobSheet(Project.ProjectDataTable p)
    {

        Project.ProjectRow row = p.Rows[0] as Project.ProjectRow;
        TxtJobCode.Text = "";
        TxtManager.Text = "";
        TxtDetails.Text = "";
        TxtAddress.Text = "";
        TxtJobCode.Text = row.Project_Code;
        if (!row.IsContactNull())
        {
            TxtContact.Text = row.Contact;
        }
        TxtDate.Text = DateTime.Today.ToShortDateString();
        if (!row.IsDescriptionNull())
            TxtDetails.Text = row.Description;

        if (!row.IsAddressNull())
        {
            TxtAddress.Text = row.Address;
        }
        if (!row.IsCityNull())
        {
            TxtAddress.Text += "\n" + row.City;
        }
        if (!row.IsProjectManagerNull())
        {
            TxtManager.Text = row.ProjectManager;
        }


    }

    protected void DetailsView2_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        DetailsView2.ChangeMode(e.NewMode);
        DetailsView2Databinding();
    }

    protected void DetailsView2_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        DetailsView2.ChangeMode(DetailsViewMode.ReadOnly);
        DetailsView2.DataBind();
    }

    protected void DDLSector_DataBound(object sender, EventArgs e)
    {
        Label lblSector = (Label)DetailsView2.FindControl("lblSector");
        System.Web.UI.WebControls.ListBox ddlSector = (System.Web.UI.WebControls.ListBox)DetailsView2.FindControl("ddlSector");
        string[] arrSectors = lblSector.Text.Split(',');
        foreach (string szSector in arrSectors)
        {
            for (int i = 0; i < ddlSector.Items.Count; i++)
            {
                if (ddlSector.Items[i].Text == szSector.Trim())
                    ddlSector.Items[i].Selected = true;
            }
        }
    }

    protected void DetailsView2_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        TextBox TxtProjectCode = (TextBox)DetailsView2.FindControl("TxtProjectCode");
        TextBox TxtProjectname = (TextBox)DetailsView2.FindControl("TxtPorjectname");
        TextBox TxtStartDate = (TextBox)DetailsView2.FindControl("TxtStartdate");
        TextBox TxtEndDate = (TextBox)DetailsView2.FindControl("TxtEndDate");
        DropDownList DDLstatus = (DropDownList)DetailsView2.FindControl("DDlstatus");
        DropDownList DDLDepartment = (DropDownList)DetailsView2.FindControl("DDLDepartment");
        System.Web.UI.WebControls.ListBox DDLSector = (System.Web.UI.WebControls.ListBox)DetailsView2.FindControl("DDLSector");
        TextBox TxtContact = (TextBox)DetailsView2.FindControl("TxtContact");
        TextBox TxtAddress = (TextBox)DetailsView2.FindControl("TxtAddress");
        TextBox TxtCity = (TextBox)DetailsView2.FindControl("TxtCity");
        TextBox TxtAuthority = (TextBox)DetailsView2.FindControl("TxtAuthority");
        CheckBox ChkDetailed = (CheckBox)DetailsView2.FindControl("ChkDetailed");
        TextBox TxtDescription = (TextBox)DetailsView2.FindControl("TxtDescription");
        string project_id = ((Label)DetailsView2.FindControl("LBLProjectID")).Text;
        string ProjectManager = ((TextBox)DetailsView2.FindControl("TxtManager")).Text;

        Nullable<DateTime> startDate = null;
        Nullable<DateTime> endDate = null;
        if (!string.IsNullOrEmpty(TxtStartDate.Text))
        {
            string[] date = TxtStartDate.Text.Split('/');
            startDate = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));

        }
        if (!string.IsNullOrEmpty(TxtEndDate.Text))
        {
            string[] date = TxtEndDate.Text.Split('/');
            endDate = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));
        }

        string selectedSectors = "";
        int[] selectedIndices = DDLSector.GetSelectedIndices();
        foreach (int selectedIndex in selectedIndices)
        {
            selectedSectors = selectedSectors + "," + DDLSector.Items[selectedIndex].Value;
        }
        selectedSectors = selectedSectors.Trim(',');

        projectBLL.updateProject(TxtProjectCode.Text, int.Parse(DDLstatus.SelectedValue), TxtAddress.Text, TxtCity.Text,
                                int.Parse(DDLDepartment.SelectedValue), selectedSectors, TxtDescription.Text, int.Parse(project_id), TxtProjectname.Text,
                                 startDate, endDate, TxtContact.Text, TxtAuthority.Text, ProjectManager);
        DetailsView2.ChangeMode(DetailsViewMode.ReadOnly);
        DetailsView2Databinding();

    }

    protected void DetailsView2_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "GoBack")
        {
            string lat = ((Label)DetailsView2.FindControl("LblLat")).Text;
            string lng = ((Label)DetailsView2.FindControl("LblLng")).Text;
            string projectcode = ((Label)DetailsView2.FindControl("LblCode")).Text;
            Response.Redirect(string.Format("map.aspx?lat={0}&lng={1}&code={2}", lat, lng, projectcode), false);
        }
    }

    protected void DeleteProject_Click(object sender, EventArgs e)
    {
        String projectId = ((Label)DetailsView2.FindControl("LBLProjectID")).Text;
        projectBLL.DeleteProject(Int32.Parse(projectId));
        Response.Redirect("map.aspx?delete=1");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Document doc = new Document();
        string path = Server.MapPath("PDFs");

        PdfWriter.GetInstance(doc, new FileStream(path + "/jobsheet.pdf", FileMode.Create));
        doc.Open();

        Table table = new Table(4);
        table.TableFitsPage = true;
        table.CellsFitPage = true;
        table.BorderWidthBottom = 1;
        table.BorderWidthLeft = 1;
        table.BorderWidthRight = 1;
        table.BorderWidthTop = 1;

        table.BorderColor = Color.BLACK;
        table.Cellpadding = 2;
        table.Cellspacing = 2;

        Cell cell = new Cell("NEW PROJECT DETAILS"); cell.Header = true;
        cell.SetHorizontalAlignment("Center");
        cell.Colspan = 4;
        table.AddCell(cell);

        cell = new Cell("JOB DETAILS"); cell.Header = true;
        cell.BackgroundColor = Color.GRAY;
        cell.SetHorizontalAlignment("Center");
        cell.Colspan = 4;
        table.AddCell(cell);
        // row 2
        //cell = new Cell("Job Status");
        //cell.BackgroundColor = Color.LIGHT_GRAY;
        //table.AddCell(cell);
        // row 3
        cell = new Cell("Job Code");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtJobCode.Text);
        table.AddCell(cell);

        cell = new Cell("Job No");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtJobNo.Text);
        table.AddCell(cell);

        // row 4
        cell = new Cell("Job Details");
        cell.Colspan = 2;
        cell.SetHorizontalAlignment("Center");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);



        cell = new Cell("for");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(Txtfor.Text);
        table.AddCell(cell);

        // row 6
        cell = new Cell(TxtDetails.Text);
        cell.Colspan = 4;
        table.AddCell(cell);

        // row 7
        cell = new Cell("Client Details");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        cell.Header = true;
        cell.SetHorizontalAlignment("Center");
        cell.Colspan = 2;
        table.AddCell(cell);



        cell = new Cell("Ref");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtRef.Text);
        table.AddCell(cell);

        // row 6

        cell = new Cell("Client");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtClient.Text);

        table.AddCell(cell);

        cell = new Cell("Order No");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtOrderNo.Text);
        table.AddCell(cell);

        // row 7

        cell = new Cell("Contact");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtContact.Text);
        table.AddCell(cell);

        cell = new Cell("Invoice Contact");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtInvoiceContact.Text);
        table.AddCell(cell);

        //row8
        cell = new Cell("Address");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(true);
        table.AddCell(cell);

        cell = new Cell("Invoice Address");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(true);
        table.AddCell(cell);

        // row 9 
        cell = new Cell(TxtAddress.Text);
        cell.Colspan = 2;
        table.AddCell(cell);

        cell = new Cell(TxtInvoiceAddress.Text);
        cell.Colspan = 2;
        table.AddCell(cell);

        // row 10

        cell = new Cell("Tel");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtTel.Text);
        table.AddCell(cell);

        cell = new Cell("Tel");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtInvoiceTel.Text);
        table.AddCell(cell);

        // row 11
        cell = new Cell("Fax");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtFax.Text);
        table.AddCell(cell);

        cell = new Cell("Fax");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtInvoiceFax.Text);
        table.AddCell(cell);

        // row 12

        cell = new Cell("CONTRACT/ORDER DETAILS");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        cell.Colspan = 4;
        cell.SetHorizontalAlignment("Center");
        cell.Header = true;
        table.AddCell(cell);

        //row 13
        cell = new Cell("Estimated Hours");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtHours.Text);
        table.AddCell(cell);

        cell = new Cell("Estimated Completion Date or Final Invoice Date");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtCompletionDate.Text);
        table.AddCell(cell);

        //row 14
        cell = new Cell("Estimated Fee/OrderValue");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtOrderValue.Text);
        table.AddCell(cell);

        cell = new Cell(true);
        table.AddCell(cell);
        cell = new Cell(true);
        table.AddCell(cell);

        //row 15
        cell = new Cell("Variation Orders");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        cell.Colspan = 4;
        cell.SetHorizontalAlignment("Center");
        table.AddCell(cell);

        //row 16
        cell = new Cell(TxtVariationOrders.Text);
        cell.Colspan = 4;
        table.AddCell(cell);

        // row 17

        cell = new Cell("Project Manager");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtManager.Text);
        table.AddCell(cell);

        cell = new Cell("Director");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);

        cell = new Cell(TxtDirector.Text);
        table.AddCell(cell);

        // row 18

        cell = new Cell(true);
        cell.Colspan = 2;
        table.AddCell(cell);


        cell = new Cell("Date");
        cell.BackgroundColor = Color.LIGHT_GRAY;
        table.AddCell(cell);
        cell = new Cell(TxtDate.Text);
        table.AddCell(cell);

        try
        {
            doc.Add(table);
        }
        catch (Exception ex)
        {
            //Display parser errors in PDF.
            //Parser errors will also be wisible in Debug.Output window in VS
            Paragraph paragraph = new Paragraph("Error! " + ex.Message);
            paragraph.SetAlignment("center");
            Chunk text = paragraph.Chunks[0] as Chunk;

            if (text != null)
            {
                text.Font.Color = Color.RED;
            }
            doc.Add(paragraph);

        }
        finally
        {
            doc.Close();
            //Response.ContentType = "application/pdf";
            //Response.Redirect("PDFs/jobsheet.pdf");
            //Response.End();
            //ClientScriptManager cs = this.ClientScriptManager();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "OpenPDF", "window.open('PDFs/Jobsheet.pdf','_blank');", true);


        }
    }

    protected void UploadJobSheet_Click(object sender, EventArgs e)
    {
        Boolean jobSheetIncluded = FileJobSheet.PostedFile.ContentLength > 0;
        String submittedBy = SubmittedBy.Text.Trim();

        if ((!HasJobSheet && !jobSheetIncluded) || String.IsNullOrEmpty(submittedBy))
            return;
        
        Boolean originalFeeProposalIncluded = FileOriginalFeeProposal.PostedFile.ContentLength > 0;
        Boolean acceptanceOfServiceIncluded = FileAcceptanceOfService.PostedFile.ContentLength > 0;

        System.Collections.Generic.List<Attachment> attachments = new System.Collections.Generic.List<Attachment>();

        if (jobSheetIncluded)
            attachments.Add(new Attachment(FileJobSheet.PostedFile.InputStream, FileJobSheet.PostedFile.FileName));

        if (originalFeeProposalIncluded)
            attachments.Add(new Attachment(FileOriginalFeeProposal.PostedFile.InputStream, FileOriginalFeeProposal.PostedFile.FileName));

        if (acceptanceOfServiceIncluded)
            attachments.Add(new Attachment(FileAcceptanceOfService.PostedFile.InputStream, FileAcceptanceOfService.PostedFile.FileName));

        String comments = ProjectDetailsComments.Text.Trim();

        // Don't send email if no information has been provided
        if (attachments.Count == 0 && String.IsNullOrEmpty(comments)) return;

        String[] bodyParameters = { submittedBy, comments };

        EmailService.SendEmail("jobSheet", bodyParameters, attachments);

        // Update database with submission dates
        String projectId = ((Label)DetailsView2.FindControl("LBLProjectID")).Text;
        DateTime? now = DateTime.Now;
        projectBLL.UpdateProjectSubmissions(Int32.Parse(projectId), jobSheetIncluded ? now : null, originalFeeProposalIncluded ? now : null, acceptanceOfServiceIncluded ? now : null);

        DetailsView2Databinding();

        lblEmailSuccess.Visible = true;
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void BtnSheet_Click(object sender, EventArgs e)
    {
        string ProjectID = Request.QueryString["projectID"];
        Project.ProjectDataTable p = projectBLL.GetDataByID(ProjectID);
        Project.ProjectRow row = p.Rows[0] as Project.ProjectRow;

        String originalJobSheetPath = Server.MapPath("JobSheet.xls");
        String modifiedJobSheetPath = Server.MapPath("newJobsheet1.xls");

        File.Copy(originalJobSheetPath, modifiedJobSheetPath, true);

        String conn = String.Format(ExcelConnectionString, modifiedJobSheetPath);
        String updateQuery = "update [Jobsheet${0}:{0}] set F1 = '{1}'";

        using (OleDbConnection connection = new OleDbConnection(conn))
        {
            connection.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;

            if (!row.IsStatusNull())
            {
                cmd.CommandText = String.Format(updateQuery, "E3", row.Status);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsProject_CodeNull())
            {
                cmd.CommandText = String.Format(updateQuery, "B4", row.Project_Code);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsDescriptionNull())
            {
                cmd.CommandText = String.Format(updateQuery, "A7", row.Description);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsContactNull())
            {
                cmd.CommandText = String.Format(updateQuery, "B12", row.Contact);
                cmd.ExecuteNonQuery();
            }

            String address = String.Empty;

            if (!row.IsAddressNull())
                address = row.Address;

            if (!row.IsAddressNull() && !row.IsCityNull() && !String.IsNullOrEmpty(address))
                address += "\n";

            if (!row.IsCityNull())
                address += row.City;

            if (!String.IsNullOrEmpty(address))
            {
                cmd.CommandText = String.Format(updateQuery, "A14", address);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsProjectManagerNull())
            {
                cmd.CommandText = String.Format(updateQuery, "C31", row.ProjectManager);
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = String.Format(updateQuery, "F32", DateTime.Today.ToShortDateString());
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        Response.Redirect("newJobsheet1.xls");
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string ProjectID = Request.QueryString["projectID"];
        Project.ProjectDataTable p = projectBLL.GetDataByID(ProjectID);
        Project.ProjectRow row = p.Rows[0] as Project.ProjectRow;

        String originalJobSheetDesignPath = Server.MapPath("JobSheetDesign.xls");
        String modifiedJobSheetDesignPath = Server.MapPath("newJobsheet_design.xls");

        File.Copy(originalJobSheetDesignPath, modifiedJobSheetDesignPath, true);

        String conn = String.Format(ExcelConnectionString, modifiedJobSheetDesignPath);
        String updateQuery = "update [Project Sheet${0}:{0}] set F1 = '{1}'";

        using (OleDbConnection connection = new OleDbConnection(conn))
        {
            connection.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;

            if (!row.IsProject_CodeNull())
            {
                cmd.CommandText = String.Format(updateQuery, "G5", row.Project_Code);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsStartDateNull())
            {
                cmd.CommandText = String.Format(updateQuery, "G10", row.StartDate);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsEndDateNull())
            {
                cmd.CommandText = String.Format(updateQuery, "G11", row.EndDate);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsDescriptionNull())
            {
                cmd.CommandText = String.Format(updateQuery, "G13", row.Description);
                cmd.ExecuteNonQuery();
            }

            if (!row.IsContactNull())
            {
                cmd.CommandText = String.Format(updateQuery, "A21", row.Contact);
                cmd.ExecuteNonQuery();
            }

            // Clear cells A24 to A29
            cmd.CommandText = "update [Project Sheet$A24:A29] set F1 = ''";
            cmd.ExecuteNonQuery();

            Int16 rowToUpdate = 24;

            if (!row.IsAddressNull() && !String.IsNullOrEmpty(row.Address))
            {
                String[] addressParts = row.Address.Split(',');

                foreach (String part in addressParts)
                {
                    cmd.CommandText = String.Format(updateQuery, "A" + rowToUpdate, part.Trim());
                    cmd.ExecuteNonQuery();
                    rowToUpdate++;
                }
            }

            if (!row.IsCityNull() && !String.IsNullOrEmpty(row.City))
            {
                String[] cityParts = row.City.Split(',');

                foreach (String part in cityParts)
                {
                    cmd.CommandText = String.Format(updateQuery, "A" + rowToUpdate, part.Trim());
                    cmd.ExecuteNonQuery();
                    rowToUpdate++;
                }
            }

            connection.Close();
        }
        
        Response.Redirect("newJobsheet_design.xls");
    }

    private void CheckExcellProcesses()
    {
        Process[] AllProcesses = Process.GetProcessesByName("excel");
        myHashtable = new Hashtable();
        int iCount = 0;

        foreach (Process ExcelProcess in AllProcesses)
        {
            myHashtable.Add(ExcelProcess.Id, iCount);
            iCount = iCount + 1;
        }
    }

    private void KillExcel()
    {

        Process[] AllProcesses = Process.GetProcessesByName("excel");

        // check to kill the right process
        foreach (Process ExcelProcess in AllProcesses)
        {
            if (myHashtable.ContainsKey(ExcelProcess.Id) == false)
                ExcelProcess.Kill();
        }

        AllProcesses = null;
    }
}
