<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="StronglyTypedDatasets.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div style="font-family:Arial">
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Button" 
        onclick="Button1_Click" />
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
</div>
</asp:Content>
