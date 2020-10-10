<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>

<div id='panel_owner_tab_administration'> 
<ul>
<% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewRoleManadgmentMenu)) {%>
<li id='roleManagmentTab'><a href='/Role/TabContent'><%: ViewResources.SharedStrings.RoleManagementTabName %></a></li>
<%}%>
<%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTitleMenu)) {%>
<li id='titleTab'><a href='/Title/TabContent'><%: ViewResources.SharedStrings.TitleManagementTabName %></a></li>
<%}%>
<%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewBuildingsMenu)) {%>
<li id='buildingTab'><a href='/Building/Index'><%: ViewResources.SharedStrings.BuildingsTabName %></a></li>
<%}%>
<%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewCompanyMenu)) {%>
<li id='companyTab'><a href='/Company/TabContent'><%:ViewResources.SharedStrings.CompaniesManagementTabName %></a></li>
<%}%>
<%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewDepartmentMenu)) {%>
<li id='departmentTab'><a href='/Department/TabContent'><%: ViewResources.SharedStrings.DepartmentsManagementTabName %></a></li>
<%}%>
<%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewHolidayMenu)) {%>
<li id='holidayTab'><a href='/Holiday/TabContent'><%: ViewResources.SharedStrings.HolidaysManagementTabName %></a></li>
<%}%>
<%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewCardTypeMenu)) {%>
<li id='cardTypeTab'><a href='/Card/TypeTabContent'><%: ViewResources.SharedStrings.CardsManagementTabName %></a></li>
<%}%>
<%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewClassifierMenu)) {%>
<li id='classifierTab'><a href='/Classifiers/TabContent'><%: ViewResources.SharedStrings.ClassifiersManagementTabName %></a></li>
<%}%>
    
     <%if (Model.User.RoleTypeId==1)
     {
          {%> 
    
<li id='LiveVideo'><a href='/LiveVideo/LiveCamera'>Video-Comapny</a></li>


<%}}%> 



<%if (Model.User.RoleTypeId==1)
    {
        string b = Model.User.RoleTypeId.ToString();
    }%>
 <%else
     {
         if((Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewMyCompanyMenu))) {%> 
 <li id='myCompanyTab'><a href='/Company/MyCompany'><%:ViewResources.SharedStrings.MyCompanyTabName %></a></li>
<%}}%>  

           
</ul>

<script type="text/javascript" lang="javascript">

	$(document).ready(function () {
		var i = $('#panel_owner li').index($('#administrationTab'));
		var opened_tab = '<%: Session["tabIndex"] %>';

		if (opened_tab != '') {
			i1 = new Number(opened_tab);
			if (i1 != i) {
				SetOpenedTab(i);
			}
		}
		else {
			SetOpenedTab(i);
		}

		var i1 = $('#panel_owner_tab_administration li').index($('#roleManagmentTab'));
		if (i1 == -1) { i1 = 0; }
		var opened_sub_tab = '<%: Session["subTabIndex"] %>';

		if (opened_sub_tab != '') {
			i1 = new Number(opened_sub_tab);
		}

		$("#panel_owner_tab_administration").attr("id", function () {
			$("#panel_owner_tab_administration").tabs({
			    beforeLoad: function( event, ui )  {
			        ui.ajaxSettings.async = false,
					ui.ajaxSettings.error= function (xhr, status, index, anchor) {
						$(anchor.hash).html("Couldn't load this tab!");
					}
				},
				fx: {
					opacity: "toggle",
					duration: "fast"
				},
				active: i1
			});
		});

		$('div#Work').fadeIn("slow");
	});

</script>
</div>