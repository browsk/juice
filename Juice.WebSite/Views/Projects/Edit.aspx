<%@ Page Title="Edit Project" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Juice.WebSite.Views.Projects.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">

<span class="message">
    <%=TempData["Message"] %>
</span>
<% using (Html.BeginForm("Update", "Projects", new {id = ViewData.Model.Id})) %>
<% {  %>
        
        Name: <%= Html.TextBox("Name", ViewData.Model.Name) %> <br />
        Description <br />
        <%=Html.TextArea("Description", ViewData.Model.Description) %> 
    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
    <br />
        <input type="submit" value="Submit" />
<% } %>

    </form>

</asp:Content>
