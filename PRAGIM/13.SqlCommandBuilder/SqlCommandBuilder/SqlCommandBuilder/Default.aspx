<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SqlCommandBuilder._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div style="font-family: Arial">
    <table border="1">
        <tr>
            <td>
                Student ID
            </td>
            <td>
                <asp:TextBox ID="txtStudentID" runat="server"></asp:TextBox>
                <asp:Button ID="btnGetStudent" runat="server" Text="Load" 
                    OnClick="btnGetStudent_Click" />
            </td>
        </tr>
        <tr>
            <td>
                Name
            </td>
            <td>
                <asp:TextBox ID="txtStudentName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Gender
            </td>
            <td>
                <asp:DropDownList ID="ddlGender" runat="server">
                    <asp:ListItem Text="Select Gender" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                  <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Total Marks
            </td>
            <td>
                <asp:TextBox ID="txtTotalMarks" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" 
                    OnClick="btnUpdate_Click" />
                <asp:Label ID="lblStatus" runat="server" Font-Bold="true">
                </asp:Label>
            </td>
        </tr>
    </table>
    </div>

</asp:Content>
