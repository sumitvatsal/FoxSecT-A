<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<FoxSec.Web.ViewModels.HomeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">FoxSec WEB</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id='Work' style='display: none'>
        <div id='panel_owner'>
            <ul>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewAdministrationMenu) || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewRoleManadgmentMenu)
                || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTitleMenu) || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewBuildingsMenu)
                || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewCompanyMenu) || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewDepartmentMenu)
                || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewHolidayMenu) || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewCardTypeMenu)
                || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewClassifierMenu) || Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewMyCompanyMenu))
                    {%><li id='administrationTab'><a href='/Administration/TabContent'><%:ViewResources.SharedStrings.AdministrationTabName %></a></li>
                <%}%>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTimeZoneMenu))
                    {%>
                <li id='timeZoneTab'><a href='/TimeZone/Index'><%: ViewResources.SharedStrings.TimeZoneTabName%></a></li>
                <%}%>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewPermissionMenu))
                    {%><li id='permissionsTab'><a href='/Permission/Index'><%= ViewResources.SharedStrings.PermissionTabName %></a></li>
                <%}%>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewPeopleMenu))
                    {%><li id='default_tab'><a href='/User/TabContent'><%:ViewResources.SharedStrings.UsersTabName %></a></li>
                <%}%>
                <%-- <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewPeopleMenu)){%><li id='default_tab1'><a href='/NewUser/TabContentNew'><%:ViewResources.SharedStrings.UsersTabNameNew %></a></li><%}%>--%>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewCardsMenu))
                    {%><li id='cardsTab'><a href='/Card/TabContent'><%:ViewResources.SharedStrings.CardsTabName %></a></li>
                <%}%>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewLogMenu))
                    {%><li id='logTab'><a href='/Log/TabContent'><%:ViewResources.SharedStrings.LogTabName %></a></li>
                <%}%>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewMyLocationMenu))
                    {%>
                <li id='LocationTab'><a href='/Log/Location'><%:ViewResources.SharedStrings.LocationTabName %></a></li>
                <%}%>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTAMenu))
                    {%><li id='TAReportTab'><a href='/TAReport/TabContent'><%:ViewResources.SharedStrings.WorkTabName %></a></li>
                <%}%>
                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.LiveVideo))
                    {
                        { %><li id='TestCamera'><a href='/VideoCamera/Camera'><%:ViewResources.SharedStrings.LiveVideo %></a></li>
                <%}
                    }%>


                <%--     <%if (Model.User.RoleTypeId==3)
     {
          {%> 
<li id='TestCamera'><a href='/VideoCamera/Camera'>Live-Video</a></li>
<%}}%> --%>

                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewMyAccountMenu))
                    {%>
                <li id='myAccountTab'><a href='#'><%:ViewResources.SharedStrings.MyAccountTabName %></a></li>
                <%}%>


                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewVisitorsMenu))
                    {%>
                <li id='myVisitorTab'><a href='/Visitors/TabContent'><%:ViewResources.SharedStrings.ViewVisitorsMenu %></a></li>
                <%}%>

                <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewMyTerminalMenu))
                    {%>
                <li id='myTerminalTab'><a href='/Terminal/TabContent'><%:ViewResources.SharedStrings.ViewMyTerminalMenu %></a></li>
                <%}%>
              <%-- && Model.TALicenseCount > 0--%>
                <%  if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.TARegister))
                    {%>
                <li id='myRemoteRegistrationTab'><a href='/RemoteRegistration/Index'><%:ViewResources.SharedStrings.RemoteRegistration %></a></li>
                <%}%>
            </ul>
        </div>
    </div>
    <div id="Version" style='padding: 10px 25px 0 0; vertical-align: top; text-align: right;'>Version: <%= typeof(FoxSec.Web.Controllers.HomeController).Assembly.GetName().Version.ToString() %></div>

    <script type="text/javascript" lang="javascript">


        $(document).ready(function () {
            var i = $('li').index($('#default_tab'));
            if (i == -1) { i = 0; }
      <%--  when current tab required use this line var opened_tab = '<%: Session["tabIndex"] %>';--%>
            var opened_tab = '<%: Session["user"] %>';

            var myAccountIndex = $('li').index($('#myAccountTab'));
            var VisitorsIndex = $('li').index($('#Li8'));
            $("#panel_owner").tabs({ disabled: [myAccountIndex, VisitorsIndex] });

            if (opened_tab != '') {
                i = new Number(opened_tab);
            }

            $("#panel_owner").attr("id", function () {
                $("#panel_owner").tabs({
                    beforeLoad: function (event, ui) {
                        ui.ajaxSettings.async = false,
                            ui.ajaxSettings.error = function (xhr, status, index, anchor) {
                                $(anchor.hash).html("Couldn't load this tab!");
                            }
                    },
                    fx: {
                        opacity: "toggle",
                        duration: "fast"
                    },
                    active: i
                });
            });
        });

        function SetOpenedTab(tabIndex) {
            $.post('<%: Url.Action("SetOpenedTab", "Home") %>', { tabIndex: tabIndex }, function (data) { });
        }

        function SetOpenedSubTab(tabIndex) {
            $.post('<%: Url.Action("SetOpenedSubTab", "Home") %>', { tabIndex: tabIndex }, function (data) { });
        }

    </script>
</asp:Content>
