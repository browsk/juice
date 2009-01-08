<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Site.Master"  CodeBehind="Index.aspx.cs" Inherits="Juice.WebSite.Views.Projects.Index" %>
<asp:Content ID="ProjectIndex" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="Scripts/jquery-1.2.6.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $('table tbody tr:odd').addClass('odd');
            $('table tbody tr:even').addClass('even');
        });
    </script>
    <div>
        <h1>Projects</h1>
        <table id="projectList">
        <thead>
            <tr>
                <th>Name</th>
                <th>Description</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
        <%
            foreach (var project in ViewData.Model)
            {
                 %>
                 
                <tr>
                    <td><%=Html.ActionLink(project.Name, "Select", new {id = project.Id}) %></td>
                    <td><%=project.Description %></td>
                    <td>
                        <div>
                            <%=Html.ActionLink("Edit", "Edit", new {id = project.Id}) %>
                        </div>
                        <div>
                            <%=Html.ActionLink("Delete", "Delete", new {id = project.Id}) %>
                        </div>
                    </td>
                </tr>
        <%
            }
            %>
        </tbody>
        </table>
        <a href="/Projects/Create">Create Project</a>
    </div>
</asp:Content>