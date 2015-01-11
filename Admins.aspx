<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Admins.aspx.cs" Inherits="Admins" Title="Admin" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHoder1" runat="Server">
    <fieldset>
        <legend>Login </legend>
        <table style="border: solid 0 #fff">
            <tr>
                <td>
                    UserName:
                </td>
                <td>
                    <asp:TextBox ID="TxtUserName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Password
                </td>
                <td>
                    <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Button ID="Button2" runat="server" Text="Login" OnClick="Button2_Click" />
                </td>
            </tr>
        </table>
        <asp:Label runat="server" Text="Label" Visible="false" ID="lblMessage"></asp:Label>
    </fieldset>
    <asp:ScriptManager runat="server" ID="scriptmanager1">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <div style="background-color: White;">
                Find By Department:
                <asp:DropDownList ID="DDLDepartment" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                    DataSourceID="DepartmentBLL" DataTextField="Name" DataValueField="Dep_ID" OnSelectedIndexChanged="DDLDepartment_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="-1">View All</asp:ListItem>
                </asp:DropDownList>
                Find By Status:<asp:DropDownList ID="DDLStatus" runat="server" AppendDataBoundItems="True"
                    AutoPostBack="True" DataSourceID="StatusDataSource" DataTextField="Status" DataValueField="Status_ID"
                    OnSelectedIndexChanged="DDLStatus_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="-1">View All</asp:ListItem>
                </asp:DropDownList>
                Find By Porject Code<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Find" OnClick="Button1_Click" />
                <asp:ObjectDataSource ID="StatusDataSource" runat="server" SelectMethod="GetData"
                    TypeName="StatusBLL"></asp:ObjectDataSource>
                <asp:GridView ID="GridView1" runat="server" CssClass="tablestyle" AutoGenerateColumns="False"
                    OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                    <AlternatingRowStyle CssClass="altrowstyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:BoundField DataField="Project_ID" HeaderText="project ID" />
                        <asp:BoundField DataField="Project code" HeaderText="Project code" HtmlEncode="False" />
                        <asp:BoundField DataField="Project Name" HeaderText=" Project Name" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="Name" HeaderText="Department" />
                        <asp:BoundField DataField="City" HeaderText="City" />
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" CommandName="DeleteProject" CommandArgument='<%# Eval("Project_ID") %>'
                                    Text="Delete" runat="server"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="DepartmentBLL" runat="server" SelectMethod="GetData" TypeName="DepartmentBLL">
    </asp:ObjectDataSource>
</asp:Content>
