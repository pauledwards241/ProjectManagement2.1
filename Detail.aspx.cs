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
using GemBox.Spreadsheet;
using System.Net.Mail;

public partial class Detail : System.Web.UI.Page
{
    private ProjectBLL projectBLL = new ProjectBLL();
    private Hashtable myHashtable;

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

            DetailsView2.DataBind();
            DetailsView2.CssClass = DetailsView2.CurrentMode.ToString().ToLower();

            if (p.Rows[0]["AddedAt"] == DBNull.Value)
                DetailsView2.Fields[17].Visible = false;
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
        if (FileJobSheet.PostedFile.ContentLength == 0 || SubmittedBy.Text.Trim().Length == 0)
            return;

        System.Collections.Generic.List<Attachment> attachments = new System.Collections.Generic.List<Attachment>();

        attachments.Add(new Attachment(FileJobSheet.PostedFile.InputStream, FileJobSheet.PostedFile.FileName));

        if (FileOriginalFeeProposal.PostedFile.ContentLength > 0)
            attachments.Add(new Attachment(FileOriginalFeeProposal.PostedFile.InputStream, FileOriginalFeeProposal.PostedFile.FileName));

        if (FileAcceptanceOfService.PostedFile.ContentLength > 0)
            attachments.Add(new Attachment(FileAcceptanceOfService.PostedFile.InputStream, FileAcceptanceOfService.PostedFile.FileName));

        String[] bodyParameters = { SubmittedBy.Text, ProjectDetailsComments.Text };

        EmailService.SendEmail("jobSheet", bodyParameters, attachments);

        lblEmailSuccess.Visible = true;
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void BtnSheet_Click(object sender, EventArgs e)
    {
        //FileStream file = new FileStream(Server.MapPath("jobsheet.xls"), FileMode.Open);
        string ProjectID = Request.QueryString["projectID"];
        Project.ProjectDataTable p = projectBLL.GetDataByID(ProjectID);
        Project.ProjectRow row = p.Rows[0] as Project.ProjectRow;

        ExcelFile ex = new ExcelFile();
        ex.LoadXls(Server.MapPath("jobsheet.xls"));
        ExcelWorksheet jobsheet = ex.Worksheets.ActiveWorksheet;
        if (!row.IsStatusNull())
        {
            jobsheet.Cells[2, 6].Value = row.Status;
        }

        if (!row.IsProject_CodeNull())
        {
            jobsheet.Cells[3, 1].Value = row.Project_Code;
        }

        if (!row.IsDescriptionNull())
        {
            jobsheet.Cells[5, 0].Value = row.Description;
        }
        if (!row.IsContactNull())
        {
            jobsheet.Cells[10, 1].Value = row.Contact;
        }
        if (!row.IsAddressNull())
        {
            jobsheet.Cells[12, 0].Value = row.Address;
        }
        if (!row.IsCityNull())
        {
            jobsheet.Cells[13, 0].Value = row.City;
        }
        if (!row.IsProjectManagerNull())
        {
            jobsheet.Cells[29, 2].Value = row.ProjectManager;
        }
        jobsheet.Cells[30, 5].Value = DateTime.Today.ToShortDateString();
        if (File.Exists(MapPath("newJobsheet1.xls")))
        {
            File.Delete(MapPath("newJobsheet1.xls"));
        }
        ex.SaveXls(Server.MapPath("newJobsheet1.xls"));
        //file.Close();
        Response.Redirect("newJobsheet1.xls");

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //Excel.Application app = new Excel.Application();
        //string ProjectID = Request.QueryString["projectID"];
        //Project.ProjectDataTable p = projectBLL.GetDataByID(ProjectID);
        //Project.ProjectRow row = p.Rows[0] as Project.ProjectRow;

        //try
        //{
        //    Excel.Workbook book =
        //   app.Workbooks.Open(Server.MapPath("jobsheet_design.xls"), 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
        //                      Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //    app.Visible = false;
        //    Worksheet sheet = (Worksheet)book.ActiveSheet;
        //    if (!row.IsProject_CodeNull())
        //    {
        //        sheet.Cells[3, 7] = row.Project_Code;
        //    }
        //    if (!row.IsStartDateNull())
        //    {
        //        sheet.Cells[6, 7] = row.StartDate;
        //    }

        //    if (!row.IsEndDateNull())
        //    {
        //        sheet.Cells[7, 7] = row.EndDate;
        //    }

        //    if (!row.IsDescriptionNull())
        //    {
        //        sheet.Cells[9, 7] = row.Description;
        //    }

        //    if (!row.IsContactNull())
        //    {
        //        sheet.Cells[17, 1] = row.Contact;
        //    }
        //    if (!row.IsAddressNull())
        //    {
        //        sheet.Cells[20, 1] = row.Address;
        //    }

        //    if (!row.IsCityNull())
        //    {
        //        sheet.Cells[21, 1] = row.City;
        //    }
        //    if (File.Exists(MapPath("newJobsheet_design.xls")))
        //    {
        //        File.Delete(MapPath("newJobsheet_design.xls"));
        //    }
        //    book.SaveAs(MapPath("newJobsheet_design.xls"), XlFileFormat.xlExcel12, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //    app.Quit();
        //}
        //catch (Exception ex)
        //{
        //    System.Diagnostics.Debug.Write(ex.Message);
        //    System.Diagnostics.Debug.Write(ex.StackTrace);
        //}
        //finally
        //{
        //    foreach (Workbook book in app.Workbooks)
        //    {

        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(book);

        //    }
        //    app.Workbooks.Close();
        //    app.DisplayAlerts = false;
        //    app.Quit();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        //    app = null;
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //    //KillExcel();
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.Redirect("newJobsheet_design.xls", true);
        //}

        string ProjectID = Request.QueryString["projectID"];
        Project.ProjectDataTable p = projectBLL.GetDataByID(ProjectID);
        Project.ProjectRow row = p.Rows[0] as Project.ProjectRow;

        ExcelFile ex = new ExcelFile();
        ex.LoadXls(Server.MapPath("jobsheet_design.xls"));
        ExcelWorksheet sheet = ex.Worksheets.ActiveWorksheet;

        if (!row.IsProject_CodeNull())
        {
            sheet.Cells[2, 6].Value = row.Project_Code;
        }
        if (!row.IsStartDateNull())
        {
            sheet.Cells[5, 6].Value = row.StartDate;
        }

        if (!row.IsEndDateNull())
        {
            sheet.Cells[6, 6].Value = row.EndDate;
        }

        if (!row.IsDescriptionNull())
        {
            sheet.Cells[8, 6].Value = row.Description;
        }

        if (!row.IsContactNull())
        {
            sheet.Cells[16, 0].Value = row.Contact;
        }
        if (!row.IsAddressNull())
        {
            sheet.Cells[19, 0].Value = row.Address;
        }

        if (!row.IsCityNull())
        {
            sheet.Cells[20, 0].Value = row.City;
        }
        if (File.Exists(MapPath("newJobsheet_design.xls")))
        {
            File.Delete(MapPath("newJobsheet_design.xls"));
        }

        ex.SaveXls(MapPath("newJobsheet_design.xls"));
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
