<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true"
    CodeFile="Detail.aspx.cs" Inherits="Detail" Title="Mayer Brown Project Management System (Beta)"
    EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="Server">
    <link rel="Stylesheet" type="text/css" href="_Asset/bootstrap.css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHodler1" runat="Server">

    <div runat="server" id="ViewPanel">
        <fieldset title="Project Detail">
            <legend style="font-size: medium; font-weight: bold">Project Detail</legend>
            <ajax:ScriptManager ID="ScriptManager1" runat="server">
            </ajax:ScriptManager>
            <ajax:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                        <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                            <HeaderTemplate>
                                Project Detail
                            </HeaderTemplate>
                            <ContentTemplate>

                                <asp:Label ID="lblEmailSuccess" runat="server" CssClass="success" Text="* Email sent successfully" Visible="false"></asp:Label>

                                <asp:DetailsView ID="DetailsView2" runat="server" AutoGenerateRows="False" CellPadding="4"
                                    DataKeyNames="Project_ID" ForeColor="#333333" GridLines="None" Height="50px"
                                    Width="510px" AutoGenerateEditButton="False" BackImageUrl="niceforms/images/button.gif"
                                    OnModeChanging="DetailsView2_ModeChanging" OnItemUpdated="DetailsView2_ItemUpdated"
                                    OnItemUpdating="DetailsView2_ItemUpdating" OnItemCommand="DetailsView2_ItemCommand">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <Fields>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Project_ID") %>' ID="LBLProjectID"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project Code">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Project Code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtProjectCode" runat="server" Text='<%# Eval("Project Code") %>'
                                                    Width="200"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Project_ID" HeaderText="Project_ID" InsertVisible="False"
                                            Visible="False" ReadOnly="True" SortExpression="Project_ID" />
                                        <asp:TemplateField HeaderText="Project Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Project Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" Width="200" ID="TxtPorjectname" Text='<%# Eval("Project Name") %>'>
                                                </asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start Date">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLStartDate" runat="server" Text='<%# Eval("StartDate","{0:d}")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox Width="200" ID="TxtStartdate" runat="server" Text='<%# Eval("StartDate","{0:d}")  %>'></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="TxtStartdate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End Date">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLEndDate" runat="server" Text='<%# Eval("EndDate","{0:d}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox Width="200" ID="TxtEndDate" runat="server" Text='<%# Eval("EndDate","{0:d}")%>'></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true" TargetControlID="TxtEndDate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList Width="200px" ID="DDLStatus" runat="server" DataSourceID="StatusDataSource"
                                                    SelectedValue='<%# Eval("StatusID") %>' DataTextField="Status" DataValueField="Status_ID">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="StatusDataSource" runat="server" SelectMethod="GetData"
                                                    TypeName="StatusBLL" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLDepartment" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList Width="200" ID="DDLDepartment" runat="server" DataSourceID="DepartmentDataSource"
                                                    DataTextField="Name" DataValueField="Dep_ID" SelectedValue='<%# Eval("DepartmentID") %>'>
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="DepartmentDataSource" runat="server" SelectMethod="GetData"
                                                    TypeName="DepartmentBLL"></asp:ObjectDataSource>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sector">
                                            <ItemTemplate>
                                                <asp:Label ID="LBLSector" runat="server" Text='<%# Eval("SectorList") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSector" runat="server" Text='<%# Eval("SectorList") %>' Visible="false"></asp:Label>
                                                <asp:ListBox ID="DDLSector" SelectionMode="Multiple" runat="server" DataSourceID="SqlDataSource5"
                                                    CssClass="MySelect" DataTextField="Name" OnDataBound="DDLSector_DataBound" DataValueField="Sector_ID"></asp:ListBox>
                                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>"
                                                    SelectCommand="SELECT [Sector_ID], [Name] FROM [Sector]"></asp:SqlDataSource>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project Manager[MBL]">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ProjectManager") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtManager" Width="200" Text='<%#  Eval("ProjectManager")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client Contact">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Contact") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtContact" Width="200" Text='<%# Eval("Contact") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Address">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtAddress" Width="200" Text='<%# Eval("Address") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="City">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("City") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtCity" Text='<%# Eval("City") %>' Width="200"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Authority">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Authority")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="TxtAuthority" Text='<%# Eval("Authority")%>' Width="200"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Detailed">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Detailed") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox runat="server" ID="ChkDetailed" Checked='<%# Eval("Detailed") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCode" runat="server" Text='<%# Bind("Project_ID") %>'></asp:Label>
                                                <asp:Label ID="LblLat" runat="server" Text='<%# Bind("lat") %>'> </asp:Label>
                                                <asp:Label ID="LblLng" runat="server" Text='<%# Bind("lon") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" TextMode="MultiLine" Width="200" Rows="10" Text='<%# Eval("Description") %>'
                                                    ID="TxtDescription"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Created on" DataField="AddedAt" ReadOnly="true" DataFormatString="{0:d}" ItemStyle-CssClass="readonly" />
                                    </Fields>
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Fields>
                                        <asp:CommandField ButtonType="Button" ShowEditButton="True" ShowCancelButton="True"
                                            UpdateText="Update" />
                                    </Fields>
                                    <FooterTemplate>
                                        <asp:Button CommandName="GoBack" Text="Go Back" runat="server" ID="BtnBack" />
                                        <asp:Button CommandName="JobSheet" Text="Job Sheet" runat="server" ID="BtnSheet" OnClick="BtnSheet_Click" />
                                        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Design Job Sheet" />
                                        <button type="button" data-toggle="modal" data-target="#upload-job-sheet">Submit Job Sheet/Fee/Acceptance</button>
                                    </FooterTemplate>
                                </asp:DetailsView>
                                &nbsp;
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel runat="server" Visible="false" HeaderText="" Enabled="false" EnableViewState="false"
                            ID="TabPanel2">
                            <ContentTemplate>
                                <table id="Table1" class="jobsheet" runat="server">
                                    <tr>
                                        <td colspan="4" class="heading1">New Project Details
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="heading2">Job Detail
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 26px">Job Code
                                        </td>
                                        <td style="height: 26px">
                                            <asp:TextBox ID="TxtJobCode" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="height: 26px">Job No
                                        </td>
                                        <td style="width: 25%; height: 26px">
                                            <asp:TextBox ID="TxtJobNo" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Job Details
                                        </td>
                                        <td></td>
                                        <td>For
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="Txtfor" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="TxtDetails" TextMode="MultiLine" Rows="3" runat="server" Width="547px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading2">Client Details
                                        </td>
                                        <td></td>
                                        <td>Ref
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtRef" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Client
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtClient" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Order No
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtOrderNo" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 26px">Contact
                                        </td>
                                        <td style="height: 26px">
                                            <asp:TextBox ID="TxtContact" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="height: 26px">Invoice Contact
                                        </td>
                                        <td style="width: 25%; height: 26px;">
                                            <asp:TextBox ID="TxtInvoiceContact" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Address:
                                        </td>
                                        <td></td>
                                        <td>Invoice Address
                                        </td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="TxtAddress" runat="server" TextMode="MultiLine" Width="272px" Rows="5"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="TxtInvoiceAddress" runat="server" TextMode="MultiLine" Width="269px"
                                                Rows="5"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tel
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtTel" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Tel
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtInvoiceTel" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Fax
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFax" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Fax
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtInvoiceFax" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="heading2">Contract / Order Details
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 51px">Estimated Hours
                                        </td>
                                        <td style="height: 51px">
                                            <asp:TextBox ID="TxtHours" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="height: 51px">Estimated Completion Date/<br />
                                            Finanl Invoice Date
                                        </td>
                                        <td style="width: 25%; height: 51px">
                                            <asp:TextBox ID="TxtCompletionDate" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td>Estimated Fee/
                                            <br />
                                            Order Value
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtOrderValue" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td>Variation Orders
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="TxtVariationOrders" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Project Manager
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtManager" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td>Director
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtDirector" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>Date
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="TxtDate" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <asp:Button ID="Button1" runat="server" Text="Generate PDF" OnClick="Button1_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </ContentTemplate>
            </ajax:UpdatePanel>
            <br />
        </fieldset>
    </div>
    <div id="editpanel" runat="server">
    </div>

    <div class="modal fade" id="upload-job-sheet" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Submit Project Details</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group first">
                        <label for="FileJobSheet">
                            * Job Sheet
                            <span class="validation-label">* Required field</span>
                        </label>
                        <asp:FileUpload ID="FileJobSheet" runat="server" />
                    </div>     
                    <div class="form-group">
                        <label for="FIleOriginalFeeProposal">
                            Original Fee Proposal 
                        </label>
                        <asp:FileUpload ID="FileOriginalFeeProposal" runat="server" />
                    </div>    
                    <div class="form-group">
                        <label for="FIleAcceptanceOfService">
                            Client Acceptance of Service
                        </label>
                        <asp:FileUpload ID="FileAcceptanceOfService" runat="server" />
                    </div>   
                    <div class="form-group">
                        <label for="SubmittedBy">
                            * Submitted by
                            <span class="validation-label">* Required field</span>
                        </label>
                        <asp:Textbox  ID="SubmittedBy" runat="server" CssClass="form-control" placeholder="Insert your name..." />
                    </div>
                    <div class="form-group last">
                        <label for="ProjectDetailsComments">
                            Additional Comments
                        </label>
                        <asp:TextBox ID="ProjectDetailsComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Insert any additional comments..." />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button ID="UploadJobSheet" CssClass="btn btn-primary" runat="server" OnClick="UploadJobSheet_Click" OnClientClick="return validateUpload()" Text="Email Project Details" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="ContentScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <script type="text/javascript">

        $('#upload-job-sheet').on('show.bs.modal', function (e) {
            $('.validation-label').hide();
            $('.form-group input, .form-group textarea').val('');
        })

        function validateUpload() {

            var validateField = function (id) {
                var element = $('input[id$="' + id + '"]');
                var validationLabel = element.parent().find('.validation-label');

                if (element.val().trim() === '')
                    validationLabel.fadeIn();
                else
                    validationLabel.hide();
            }

            validateField('SubmittedBy');
            validateField('FileJobSheet');

            return ($('.validation-label:visible').length === 0);
        }

        //$('#upload-job-sheet').modal('show');

    </script>
</asp:Content>
