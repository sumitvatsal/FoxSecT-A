<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="headerTop">
    <div class="templateTitle">
        <%= Html.ActionLink("Project Title", "Index", "Home") %>
    </div>
    <div class="loginControl">
        <% Html.RenderPartial("LogOnPartialView"); %>
    </div>
</div>
<div class="headerMenu">
        <%-- DXCOMMENT: Configure the header menu --%>
    <% 
        Html.DevExpress().Menu(settings => {
            settings.Name = "HeaderMenu";
            settings.ItemAutoWidth = false;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.Styles.Style.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0);
            settings.Styles.Style.BorderTop.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
        }).BindToXML(HttpContext.Current.Server.MapPath("~/App_Data/TopMenu.xml"), "/items/*").Render();
     %>
</div>