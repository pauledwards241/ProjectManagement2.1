<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewProject.ascx.cs" Inherits="NewProject" %>
<table>
    <tr>
        <td> Porject Code:
        </td>
        <td >
            <input id="TxtProjectCode" type="text" />
        </td>
       
    </tr>
    <tr>
        <td>
            Owner|:</td>
        <td style="width: 3px">
            <input id="Text1" type="text" /></td>
       
    </tr>
    <tr>
        <td style="height: 21px">
            Description:</td>
        <td style="width: 3px; height: 21px;">
        </td>

    </tr>
    <tr>
        <td colspan="2" style="text-align: center">
            <asp:Button ID="Button1" runat="server" Text="Button" /></td>
    </tr>
</table>
