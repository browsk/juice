<%@ Page Title="Create Project" Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Site.Master"  CodeBehind="Create.aspx.cs" Inherits="Juice.WebSite.Views.Projects.Create" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% using (Html.BeginForm("CreateNew", "Projects")) %>
<% {  %>
        
        Name: <%= Html.TextBox("name") %> <br />
        Description <br />
        <%=Html.TextArea("description") %> <br />
        <input type="submit" value="Submit" />
<% } %>

</asp:Content>
