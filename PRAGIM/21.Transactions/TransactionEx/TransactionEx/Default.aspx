<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TransactionEx._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div style="font-family: Arial">
    <table border="1" style="background: brown; color: White">
        <tr>
            <td>
                <b>Account Number </b>
            </td>
            <td>
                <asp:Label ID="lblAccountNumber1" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblAccountNumber2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <b>Customer Name </b>
            </td>
            <td>
                <asp:Label ID="lblName1" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblName2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <b>Balance </b>
            </td>
            <td>
                <asp:Label ID="lblBalance1" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblBalance2" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="btnTransfer" runat="server"
                Text="Transfer $10 from Account A1 to Account A2"
                OnClick="btnTransfer_Click" />
    <br />
    <br />
    <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
    </div>

</asp:Content>
