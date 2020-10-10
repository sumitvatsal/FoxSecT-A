<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
	<%: ViewResources.SharedStrings.AboutPageTitle %>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: ViewResources.SharedStrings.AboutTabName %></h2>
	<p>
		<%: ViewResources.SharedStrings.AboutPageContentPlaceholder %>
	</p>
</asp:Content>