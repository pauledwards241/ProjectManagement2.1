<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default3.aspx.cs" Inherits="Default3" Title="Untitled Page" EnableEventValidation="false"
    EnableViewState="true" %>

<%@ Register Assembly="MattBerseth.WebControls.AJAX" Namespace="MattBerseth.WebControls.AJAX.GridViewControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register TagName="WebUserControl" Src="~/WebUserControl.ascx" TagPrefix="uc" %>
<%@ Register Assembly="GoogleMap" Namespace="Reimers.Map" TagPrefix="Reimers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHoder1" runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Search By Address: "></asp:Label>
    <asp:TextBox ID="Address" runat="server"></asp:TextBox>
    <input id="Hidden1" runat="server" type="hidden" />
    <input id="Button1" runat="server" type="button" value="Find" onclick="showAddress()" />
    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" UseSubmitBehavior="False"
        Text="Button" Visible="False" />&nbsp;
    <input type="text" runat="server" id="hidden_new" enableviewstate="true" visible="false" />
    <script type="text/javascript">
        function Add_new(){
			var hidden_new = $("#ctl00_ContentPlaceHoder1_hidden_new");
			var hidden_new1 = document.getElementById("ctl00_ContentPlaceHoder1_hidden_new");
			hidden_new.val("true");		
				
					//hidden_new.val("true");
						
			}

}
		
		
    </script>
    &nbsp;&nbsp;
    <ajax:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </ajax:ScriptManager>
    <ajax:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            User:
            <%=User.Identity.Name%>
            <div style="width: 1000px; float: left; border: solid 1px #000; background: #fff;">
                <div id="newProject" style="display: none">
                    <ajax:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="table">
                                <thead>
                                </thead>
                                <caption>
                                    Enter the Detail and Save</caption>
                                <tbody>
                                    <tr>
                                        <td>
                                            Project Code
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Code" runat="server" CssClass="LargeInput"></asp:TextBox>
                                            <br />
                                            <asp:Label ID="projectcode_validate" runat="server" Text=" Project code needed to be entered!"
                                                ForeColor="Red" Visible="False"></asp:Label>
                                        <td>
                                            Start Date:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_StartDate" runat="server" Enabled="false" CssClass="LargeInput"></asp:TextBox>
                                            <img src="_Asset/images/calendar1.gif" id="imgCalendar" alt="Pick a date" style="cursor: pointer;
                                                vertical-align: middle;" />
                                            <script type="text/javascript">
                                                Calendar.setup({
                                                    inputField: "<%= this.Txt_StartDate.ClientID %>",           //*
                                                    ifFormat: "%d/%m/%Y",
                                                    showsTime: false,
                                                    button: "imgCalendar",        //*
                                                    step: 1
                                                });
                                            </script>
                                            <br />
                                            <asp:Label ID="Validate_Startdate" runat="server" ForeColor="Red" Text="Please Enter a Start Date "
                                                Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            End Date:
                                        </td>
                                        <td style="width: 235px">
                                            <asp:TextBox ID="TxtEndDate" runat="server" Enabled="false" CssClass="LargeInput"
                                                Text=""></asp:TextBox>
                                            <img src="_Asset/images/calendar1.gif" id="imgEndDate" alt="Pick a date" style="cursor: pointer;
                                                vertical-align: middle;" />
                                            <script type="text/javascript">
                                                Calendar.setup({
                                                    inputField: "<%= this.TxtEndDate.ClientID %>",           //*
                                                    ifFormat: "%d/%m/%Y",
                                                    showsTime: false,
                                                    button: "imgEndDate",        //*
                                                    step: 1
                                                });
                                            </script>
                                            <br />
                                            <asp:Label ID="Validate_enddate" runat="server" ForeColor="Red" Text="Please Enter a End Date"
                                                Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Satus:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="Dropdown_Status" runat="server" DataSourceID="SqlDataSource3"
                                                DataTextField="Status" DataValueField="Status_ID" CssClass="MySelect">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>"
                                                SelectCommand="SELECT [Status_ID], [Status] FROM [status]"></asp:SqlDataSource>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            Department
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList_department" runat="server" DataSourceID="SqlDataSource4"
                                                CssClass="MySelect" DataTextField="Name" DataValueField="Dep_ID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>"
                                                SelectCommand="SELECT [Dep_ID], [Name] FROM [Department]"></asp:SqlDataSource>
                                        </td>
                                        <td>
                                            Contact:
                                        </td>
                                        <td style="width: 235px">
                                            <asp:TextBox ID="Txt_Contact" runat="server" CssClass="LargeInput"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Address" runat="server" CssClass="LargeInput"></asp:TextBox>
                                        </td>
                                        <td>
                                            City:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_city" runat="server" CssClass="LargeInput"></asp:TextBox>
                                            <br />
                                            <asp:Label ID="Validate_city" runat="server" ForeColor="Red" Text="Please Enter the City Name where project is"
                                                Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            Description:
                                        </td>
                                        <td style="width: 235px">
                                            <asp:TextBox ID="Txt_desc" runat="server" CssClass="LargeInput"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 56px">
                                        </td>
                                        <td style="height: 56px">
                                            <asp:TextBox ID="TxtLat" runat="server" Visible="true"></asp:TextBox>
                                            <asp:TextBox ID="TxtLon" runat="server" Visible="true"></asp:TextBox>
                                        </td>
                                        <td style="height: 56px">
                                            <asp:Button ID="Btn_newProject" runat="server" CssClass="Mybutton" Text="Save" OnClick="Btn_newProject_Click" />
                                        </td>
                                        <td style="height: 56px">
                                        </td>
                                        <td style="height: 56px">
                                            <asp:Button ID="btn_cancle" runat="server" CssClass="Mybutton" Text="Cancel" />
                                        </td>
                                        <td style="width: 235px; height: 56px;">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </ajax:UpdatePanel>
                </div>
                <Reimers:GoogleMap ID="GoogleMap1" runat="server" Width="1000px" Height="500px" GoogleKey="ABQIAAAAg_2etgTSZVCmg4kqKUu_5BQeSliVVvi0oBWjPnqtnVsxdn3MFhR-09t_MO6wC-RM3BXSam8DgrQ-yw"
                    ContinuousZoomEnabled="True" DoubleClickZoomEnabled="True" MapControl="Zoom"
                    MapType="Default" ShowScaleControl="True" TypeControl="Normal" Latitude="51.502865812765563"
                    Longitude="-0.12788772583007813" Zoom="10">
                </Reimers:GoogleMap>
            </div>
            <br />
            <div style="width: 1000px; float: left; background: #fff;">
                Filter By Status:
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1"
                    DataTextField="Status" DataValueField="Status_ID" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                    CssClass="MySelect">
                </asp:DropDownList>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Filter By Department :<asp:DropDownList
                    ID="DropDownList2" runat="server" DataSourceID="SqlDataSource2" DataTextField="Name"
                    DataValueField="Dep_ID" CssClass="MySelect" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged1">
                </asp:DropDownList>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CssClass="igoogle igoogle-classic" DataKeyNames="Project_ID" OnPageIndexChanging="GridView1_PageIndexChanging"
                    OnRowCommand="GridView1_RowCommand">
                    <RowStyle CssClass="data-row" />
                    <AlternatingRowStyle CssClass="alt-data-row" />
                    <HeaderStyle CssClass="header-row" />
                    <Columns>
                        <asp:BoundField DataField="Project code" HeaderText="Project" />
                        <asp:BoundField DataField="StartDate" DataFormatString="{0:dd/mm/yy}" HeaderText="Start" />
                        <asp:BoundField DataField="EndDate" DataFormatString="{0:dd/mm/yy}" HeaderText="End" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="Name" HeaderText="Department" />
                        <asp:TemplateField HeaderText="Contact">
                            <ItemTemplate>
                                <asp:Label ID="Txt_lat" runat="server" Text='<%# Eval("lat") %>' Visible="false"></asp:Label>
                                <asp:Label Visible="false" ID="Txt_lon" runat="server" Text='<%# Eval("lon") %>'>
                                </asp:Label>
                                <asp:Label Visible="false" ID="Txt_ID" runat="server" Text='<%# Eval("Project_ID") %>'></asp:Label>
                                <%# Eval("Contact") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField CommandName="go" Text="Map" />
                        <asp:ButtonField CommandName="List" Text="List" />
                    </Columns>
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>"
                    SelectCommand="SELECT [Dep_ID], [Name] FROM [Department]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>"
                    SelectCommand="SELECT [Status_ID], [Status] FROM [status]"></asp:SqlDataSource>
            </div>
            <script type="text/javascript">
                function initMap() {
                    var map = new GMap2(document.getElementById("<%= this.GoogleMap1.ClientSideID %>"));
                    GEvent.addListener(map, "click", function () { alert("You clicked the map."); });
                }
                function addmakers(lat, lon) {


                    var map = new GMap2(document.getElementById("<%= this.GoogleMap1.ClientSideID %>"));
                    alert("tet");
                    var center = new GLatLng(lat, lon);
                    map.setCenter(center, 13);
                    /*
                    var marker = new GMarker(center, {draggable: true});

                    GEvent.addListener(marker, "dragstart", function() {
                    map.closeInfoWindow();
                    });

                    GEvent.addListener(marker, "dragend", function() {
                    marker.openInfoWindowHtml("Just bouncing along...");
                    });

                    map.addOverlay(marker);

   
                    alert("test");*/
                }

                function showEntry(lat, lon) {
                    var newProject = $("#newProject");
                    newProject.css("display", "block");
                    var txtCode = $("#ctl00_ContentPlaceHoder1_Txt_Code");
                    var txtLat = $("#" + "<%= this.TxtLat.ClientID %>");
                    txtLat.val(lat);
                    var txtLon = $("#" + "<%= this.TxtLon.ClientID %>");
                    txtLon.val(lon);
                    //txtCode.val(lat);
                    txtCode.focus();
                }
            </script>
        </ContentTemplate>
    </ajax:UpdatePanel>
    <script type="text/javascript" language="javascript">
   
  function clickButton(e, buttonid){ 

      var evt = e ? e : window.event;

      var bt = document.getElementById(buttonid);

      if (bt){ 

          if (evt.keyCode == 13){ 

                bt.click(); 

                return false; 

          } 

      } 

}
		

   
        function showAddress()
        {
                 
              var TxtAddress=document.getElementById("<%= this.Address.ClientID %>");
                var address = TxtAddress.value;
               var geocoder = new GClientGeocoder();
               geocoder.getLatLng(address,function(point)
                                                {
                                                           if(!point){
                                                            alert(address + "not Found");
                                                           } 
                                                           
                                                           else{
                                                            var strPoint = point;
                                                            var corTxt = document.getElementById("<%= this.Hidden1.ClientID %>");
    corTxt.value = strPoint;
        <%= this.ClientScript.GetPostBackClientHyperlink(this.Button2,"") %>
     //__doPostBack("<%= this.Button2.ClientID %>", '');
                                                           }
                                                });
       
      
    }
        
    </script>
</asp:Content>
