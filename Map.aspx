<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="Map"
    EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="MattBerseth.WebControls.AJAX" Namespace="MattBerseth.WebControls.AJAX.GridViewControl"
    TagPrefix="mb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mayer Brown Project Management System (Beta)</title>
    <link type="text/css" rel="stylesheet" href="_Asset/layout1.css?v=1" />
    <link href="niceforms/niceforms-default.css" rel="stylesheet" type="text/css" />
    <link href="_Asset/GridView.css" rel="stylesheet" type="text/css" />
    <link href="_Asset/Progress.css" rel="stylesheet" type="text/css" />
    <link href="_Asset/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false&libraries=places"></script>
    <script type="text/javascript" src="js/markerclusterer_compiled.js"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        String.prototype.trim = function () {
            return this.replace(/^\s*/, "").replace(/\s*$/, "");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <div id="header">
        </div>
        <!-- end: #header -->
        <div id="prim_links">
            <div class="block block-menu" id="block-menu-2">
                <h2 class="title">
                    Primary links</h2>
                <div class="content">
                    <ul class="menu">
                        <li class="leaf"><a href="map.aspx" title=""><span>Home</span></a></li>
                        <li class="leaf"><a href="count.aspx" title=""><span>Project Counts</span></a></li>
                        <li class="leaf"><a href="#" title=""><span>Projects</span></a></li>
                        <li class="leaf"><a href="#" title=""><span>Services</span></a></li>
                        <li class="leaf"><a href="#" title=""><span>Careers</span></a></li>
                        <li class="leaf"><a href="#" title=""><span>Contact Us</span></a></li>
                    </ul>
                </div>
            </div>
        </div>
        <!-- end: #prim_links -->
        <asp:Label ID="DeleteConfirmation" runat="server" CssClass="success" Text="* Project successfully deleted" Visible="false"></asp:Label>
        <div id="content">
            <div id="conndiv" style="width: 1000px; position: relative;">

                <div id="newProject">

                    <asp:HiddenField ID="_TxtLat" runat="server" />
                    <asp:HiddenField ID="_TxtLng" runat="server" />

                    <div id="new-project-validation" class="validation-summary"></div>

                    <div class="form-row">
                        <label for="TextProjectName">Project Name *</label>
                        <asp:TextBox ID="TextProjectName" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-row">
                        <label for="TxtProjectCode">Project Code *</label>
                        <asp:TextBox ID="TxtProjectCode" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-row">
                        <label for="Dropdown_Status">Project Status</label>
                        <asp:DropDownList ID="Dropdown_Status" runat="server" DataSourceID="SqlDataSource3" DataTextField="Status" DataValueField="Status_ID" CssClass="mySelect"> </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>" SelectCommand="SELECT [Status_ID], [Status] FROM [status] ORDER By [Status_ID]"> </asp:SqlDataSource>
                    </div>

                    <div class="form-row">
                        <label for="DropDownList_department">Department</label>
                        <asp:DropDownList ID="DropDownList_department" runat="server" DataSourceID="SqlDataSource4" CssClass="MySelect" DataTextField="Name" DataValueField="Dep_ID"> </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>" SelectCommand="SELECT [Dep_ID], [Name] FROM [Department] order by [Name]"></asp:SqlDataSource>
                    </div>

                    <div class=" form-buttons">
                        <input type="button" id="savePro" title="save" value="Save" onclick="saveProject()" class="buttonSubmit" />
                        <input type="button" id="Cancel" value="Cancel" onclick="cancelSaving()" class="buttonSubmit" />
                    </div>

                </div>

                <div style="width: 1000px; background: white; margin-left: 0px; margin-top: 0px;
                    border: solid 1px black">
                    <input type="text" value="" id="_Txt_address" style="width: 800px; height: 30px;
                        font-size: 15px;" placeholder="Enter a landmark, location or postcode!">&nbsp;
                    <div id="map_canvas" style="width: 1000px; height: 500px;">
                    </div>
                </div>
                <div id="data_canvas" class="map-grid-results">
                    <script type="text/javascript">
                        var mapProp = {
                            center: new google.maps.LatLng(51.502865812765563, -0.12788772583007813),
                            zoom: 13,
                            mapTypeId: google.maps.MapTypeId.ROADMAP,
                            mapTypeControl: true,
                            scaleConrol: true
                        };
                        var map = new google.maps.Map(document.getElementById("map_canvas"), mapProp);

                        $(document).ready(function () {
                            var oldLatLng;

                            var mc = new MarkerClusterer(map, markers);
                            var previous = new String("<%= this.GetPrevious() %>");

                            if (previous != "none") {
                                cord = new Array();
                                cord = previous.split(",");
                                var Centre = new google.maps.LatLng(cord[0], cord[1]);
                                map.setCenter(Centre, 16);
                            }

                            var input = document.getElementById('_Txt_address');

                            var defaultBounds = new google.maps.LatLngBounds(
                                new google.maps.LatLng(49.00, -13.00),
                                new google.maps.LatLng(60.00, 3.00)
                            );

                            var searchBox = new google.maps.places.SearchBox(input, {
                                bounds: defaultBounds
                            });

                            var rightclicklistener = google.maps.event.addListener(map, "rightclick", function (event) {
                                //google.maps.event.removeListener(rightclicklistener);
                                if (confirm("Would you like to create a project here?")) {
                                    CreateMarker(event.latLng);
                                }
                            });

                            google.maps.event.addListener(searchBox, 'places_changed', function () {
                                var places = searchBox.getPlaces();
                                var bounds = new google.maps.LatLngBounds();
                                for (var i = 0, place; place = places[i]; i++) {
                                    bounds.extend(place.geometry.location);
                                }

                                map.fitBounds(bounds);

                                google.maps.event.addListenerOnce(map, "idle", function () {
                                    if (map.getZoom() > 16)
                                        map.setZoom(16); // Was 13
                                });
                            });
                        });

                        function clickButton(e, buttonid) {
                            var evt = e ? e : window.event;
                            var bt = document.getElementById(buttonid);
                            if (bt) {
                                if (evt.keyCode == 13) {
                                    bt.click();
                                    return false;
                                }
                            }
                        }

                        function addDatamarkers(lat, lon, project_code, image, project_id, status, department) {
                            var path = '<b><a href=Detail.aspx?projectID=' + project_id + '&Mode=View>View Detail</a></b>';

                            var Bicon = new google.maps.MarkerImage(image,
                                                                    google.maps.Size(34, 34),
                                                                    google.maps.Point(5, 34),
                                                                    google.maps.Point(9, 2)
                                                                    );

                            var markerOptions = {
                                icon: Bicon,
                                draggable: true,
                                title: project_id,
                                position: new google.maps.LatLng(lat, lon)
                            };
                            var datamarker = new google.maps.Marker(markerOptions);

                            google.maps.event.addListener(datamarker, "click", function () {
                                infowindow1.open(map, datamarker);
                            });

                            var infowindow1 = new google.maps.InfoWindow({
                                content: createInfo('Project', "<b>" + project_code + "</B><br/>Status: " + status + "<br/>Department: " + department + "<br/>" + path)
                            });

                            function createInfo(title, content) {
                                return '<div class="infowindow"><strong>' + title + '</strong>' + content + '</div>';
                            }

                            google.maps.event.addListener(datamarker, "dragstart", function () {
                                oldLatLng = datamarker.getPosition();
                            });

                            google.maps.event.addListener(datamarker, "dragend", function (position) {
                                if (confirm("Are you sure you want to move this marker?")) {
                                    PageMethods.SaveProjectLatLng(datamarker.getTitle(), position.latLng.lat(), position.latLng.lng());
                                }
                                else {
                                    datamarker.setPosition(oldLatLng);
                                }
                            });
                            return datamarker;
                        }

                        var newProjectModal = $('#newProject');
                        var projectCode = document.getElementById("<%=this.TxtProjectCode.ClientID %>");
                        var projectName = document.getElementById('<%= TextProjectName.ClientID %>');
                        var validationSummary = $('#new-project-validation');

                        function CreateMarker(point) {
                            SaveLatLng(point.lat(), point.lng());
                            resetForm();
                            newProjectModal.fadeIn();
                        }

                        function resetForm() {
                            projectCode.value = projectCode.className = '';
                            projectName.value = projectName.className = '';
                            validationSummary.hide();
                        }

                        function saveProject() {

                            // Required validation
                            var projectCodeValue = $.trim(projectCode.value);
                            var projectNameValue = $.trim(projectName.value);

                            projectCode.className = projectCodeValue ? '' : 'invalid';
                            projectName.className = projectNameValue ? '' : 'invalid';                            

                            if (!projectCodeValue || !projectNameValue) {
                                validationSummary.html('Please check required (*) fields.');
                                validationSummary.slideDown();
                                return;
                            }

                            // Validate project code
                            PageMethods.ValidateProjectCode(projectCodeValue, function (result) {

                                projectCode.className = result ? '' : 'invalid';

                                if (!result) {
                                    validationSummary.html('Your chosen project code already exists, please choose another.');
                                    validationSummary.slideDown();
                                    return;
                                }

                                validationSummary.slideUp();

                                var lat = parseFloat(document.getElementById("<%= this._TxtLat.ClientID %>").value);
                                var lng = parseFloat(document.getElementById("<%= this._TxtLng.ClientID %>").value);                           

                                var DDLstatus = document.getElementById("<%= this.Dropdown_Status.ClientID%>");
                                var status = new String(DDLstatus.options[DDLstatus.selectedIndex].text);
                                status = status.trim();

                                var DDLDept = document.getElementById("<%= this.DropDownList_department.ClientID %>");
                                var dep = new String(DDLDept.options[DDLDept.selectedIndex].text);
                                dep = dep.trim().replace('/', '-');

                                var myMarker = "GGIcon/" + status + "/" + dep + ".png";

                                var markers = [];
                                markers.push(addDatamarkers(lat, lng, projectCodeValue, myMarker, 'null', '', ''));
                                var mc = new MarkerClusterer(map, markers);

                                newProjectModal.fadeOut();
                                __doPostBack('<%= this.BTNaddnew.UniqueID %>', '')
                            });                            
                        }

                        function cancelSaving() {
                            newProjectModal.fadeOut();
                        }

                        function SaveLatLng(lat, lng) {

                            txt_lat = document.getElementById("<%=this._TxtLat.ClientID %>");
                            txt_lat.value = lat;
                            txt_lng = document.getElementById("<%= this._TxtLng.ClientID %>");
                            txt_lng.value = lng;
                        }

                        function getView(lat, lng, project_code, status, department, path) {
                            var point = new google.maps.LatLng(lat, lng);
                            map.setCenter(point, 20);
                            map.openInfoWindowHtml(point, "<b>" + project_code + "</B><br/>Status: " + status + "<br/>Department: " + department + "<br/>" + path);
                        }

                        function FindAddress() {
                            var address = document.getElementById("_Txt_address").value;
                            if (address == undefined) {
                                return;
                            }
                            else {
                                var geocoder = new GClientGeocoder();
                                var zoom;
                                if (address.indexOf(",") != -1) {
                                    zoom = 20;
                                }
                                else {
                                    zoom = 13;
                                }

                                if (geocoder) {
                                    geocoder.getLatLng(address, function (point) {
                                        if (!point) {
                                            alert(address + "not found");
                                        }
                                        else {
                                            map.setCenter(point, zoom);
                                            map.openInfoWindowHtml(point, address);
                                        }
                                    });
                                }
                            }
                        }
                    </script>
                    <ajax:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
                    </ajax:ScriptManager>
                    &nbsp;Status:
                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource3"
                        DataTextField="Status" DataValueField="Status_ID" CssClass="mySelect" OnSelectedIndexChanged="Filter_SelectedIndexChanged"
                        AutoPostBack="true" AppendDataBoundItems="True">
                        <asp:ListItem Selected="True" Value="-1">View All</asp:ListItem>
                    </asp:DropDownList>
                    Department:
                    <asp:DropDownList ID="DropDownList2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSource4"
                        DataTextField="Name" DataValueField="Dep_ID" AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="-1">View All</asp:ListItem>
                    </asp:DropDownList>
                    Sector:
                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:MBProjectConnectionString %>"
                        SelectCommand="SELECT [Sector_ID], [Name] FROM [Sector] order by [Name]"></asp:SqlDataSource>
                    <asp:DropDownList ID="DropDownList3" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSource5"
                        DataTextField="Name" DataValueField="Sector_ID" AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="-1">View All</asp:ListItem>
                    </asp:DropDownList>
                    <script type="text/javascript">

                        function onUpdating() {
                            // get the update progress div
                            var pnlPopup = $get('<%= this.pnlPopup.ClientID %>');

                            //  get the gridview element        
                            var gridView = $get('<%= this.GridView1.ClientID %>');

                            // make it visible
                            pnlPopup.style.display = '';

                            // get the bounds of both the gridview and the progress div
                            var gridViewBounds = Sys.UI.DomElement.getBounds(gridView);
                            var pnlPopupBounds = Sys.UI.DomElement.getBounds(pnlPopup);

                            //  center of gridview
                            var x = gridViewBounds.x + Math.round(gridViewBounds.width / 2) - Math.round(pnlPopupBounds.width / 2);
                            var y = gridViewBounds.y + Math.round(gridViewBounds.height / 2) - Math.round(pnlPopupBounds.height / 2);

                            //	set the progress element to this position
                            Sys.UI.DomElement.setLocation(pnlPopup, x, y);
                        }

                        function onUpdated() {
                            // get the update progress div
                            var pnlPopup = $get('<%= this.pnlPopup.ClientID %>');
                            // make it invisible
                            pnlPopup.style.display = 'none';
                        }

                        function changeStatus(_projectID, defaultStatus, ClientID) {
                            if (confirm("Are you sure you want to change the project status?")) {
                                PageMethods.UpdateStatus(_projectID, ClientID.value);
                            } else {
                                ClientID.value = defaultStatus;
                            }
                        }
            
                    </script>
                    &nbsp;
                    <ajax:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:GridView ID="GridView1" runat="server" CssClass="tablestyle" OnRowCommand="GridView1_RowCommand"
                                OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="Project_ID" AutoGenerateColumns="False"
                                AllowPaging="True" OnRowDataBound="GridView1_RowDataBound" Width="100%" PageSize="5">
                                <AlternatingRowStyle CssClass="altrowstyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <RowStyle CssClass="rowstyle" />
                                <Columns>
                                    <asp:BoundField DataField="Project code" HeaderText="Project code" />
                                    <asp:BoundField DataField="Project Name" HeaderText=" Project Name" />
                                    <asp:BoundField DataField="StatusID" HeaderText="Status" />
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="SqlDataSource3" DataTextField="Status"
                                                DataValueField="Status_ID" CssClass="mySelect" AutoPostBack="true" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Department" />
                                    <asp:BoundField DataField="City" HeaderText="City" />
                                    <asp:BoundField DataField="Authority" HeaderText="Authority" />
                                    <asp:TemplateField HeaderText="Detail">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDetail" Text='<%# Eval("Detailed") %>' runat="server" Visible="false"></asp:Label>
                                            <asp:LinkButton ID="LTNDetail" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Detailed" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="Txt_lat" runat="server" Text='<%# Eval("lat") %>' Visible="false"></asp:Label>
                                            <asp:Label Visible="true" ID="Txt_lon" runat="server" Text='<%# Eval("lon") %>'>
                                            </asp:Label>
                                            <asp:Label Visible="true" ID="Txt_ID" runat="server" Text='<%# Eval("Project_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Map">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBTMap" Text="Map" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pagerstyle" />
                                <PagerTemplate>
                                    <div style="float: left; width: 50%; text-align: left;">
                                        <asp:Label Font-Size="Medium" Font-Bold="true" ID="lblTotal" runat="server" Text="Total Jobs: "></asp:Label></div>
                                    <div style="float: right; width: 50%; text-align: right;">
                                        Show rows:
                                        <asp:DropDownList ID="DDLPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLPageSize_SelectedIndexChanged">
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                        Page:
                                        <asp:TextBox ID="TxtGoToPage" runat="server" Width="22px" OnTextChanged="TxtGoToPage_TextChanged"
                                            CssClass="gotopage"></asp:TextBox>
                                        of
                                        <asp:Label ID="LblTotalPage" runat="server" Text=""></asp:Label>
                                        <asp:Button ID="BtnPrev" runat="server" Text="" Width="23px" CommandArgument="Prev"
                                            CommandName="Page" ToolTip="Prev" CssClass="previous" />
                                        <asp:Button ID="BtnNExt" CommandArgument="Next" ToolTip="Next Page" runat="server"
                                            Text="" Width="23px" CommandName="Page" CssClass="next" /></div>
                                </PagerTemplate>
                            </asp:GridView>
                            <div style="display: none">
                                <asp:Button ID="BTNaddnew" runat="server" Text="Button" OnClick="BTNaddnew_Click"
                                    UseSubmitBehavior="False" Visible="true"></asp:Button></div>
                            <mb:GridViewControlExtender ID="GridViewControlExtender1" runat="server" TargetControlID="GridView1"
                                RowHoverCssClass="row-over" RowSelectCssClass="row-select" />
                        </ContentTemplate>
                    </ajax:UpdatePanel>
                </div>
                <!-- end: #conn -->
                <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server"
                    TargetControlID="UpdatePanel1">
                    <Animations>
                        <OnUpdating>
                            <Parallel duration="0">
                                <%-- place the update progress div over the gridview control --%>
                                <ScriptAction Script="onUpdating();" />  
                            </Parallel>
                        </OnUpdating>
                        <OnUpdated>
                            <Parallel duration="0">
                                <%--find the update progress div and place it over the gridview control--%>
                                <ScriptAction Script="onUpdated();" /> 
                            </Parallel> 
                        </OnUpdated>
                    </Animations>
                </cc1:UpdatePanelAnimationExtender>
                <asp:Panel ID="pnlPopup" runat="server" CssClass="progress" Style="display: none;">
                    <div class="container">
                        <div class="header">
                            Loading, please wait...</div>
                        <div class="body">
                            <img src="_Asset/images/activity.gif" alt="Loading..." />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <!-- end: #conndiv -->
        </div>
    </div>
    </form>
</body>
</html>
