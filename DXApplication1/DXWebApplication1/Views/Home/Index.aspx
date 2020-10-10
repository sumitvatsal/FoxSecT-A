<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Main.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.IEnumerable>" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
<% Html.RenderPartial("GridViewPartialView", Model); %>
</asp:Content>