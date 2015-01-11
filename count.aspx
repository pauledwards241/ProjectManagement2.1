<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="count.aspx.cs" Inherits="Images_count" Title="Project Count" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHoder1" runat="Server">
    <div style="padding: 10 0 0 10">
        <asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="True" Font-Size="Large"></asp:Label>
        <br />
        <br />
        <table>
        </table>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
            AllowSorting="True" Width="411px" BorderColor="SteelBlue" BorderStyle="Solid"
            BorderWidth="1px" OnRowDataBound="GridView1_RowDataBound" ShowFooter="True" OnSorting="GridView1_Sorting"
            AutoGenerateColumns="False" OnRowCreated="GridView1_RowCreated">
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#E3EAEB" />
            <EditRowStyle BackColor="#7C6F57" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                <asp:BoundField DataField="Live" HeaderText="Live" SortExpression="Live" />
                <asp:BoundField DataField="Spec" HeaderText="Spec" SortExpression="Spec" />
                <asp:BoundField DataField="Dead" HeaderText="Dead" SortExpression="Dead" />
                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
