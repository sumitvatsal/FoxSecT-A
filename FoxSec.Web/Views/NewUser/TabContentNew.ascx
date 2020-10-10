
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_people_default'>
<table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
    <tr>
        <td style='width: 20%; vertical-align: top'>
            <div id='AreaCompanyTree' style='margin: 15px 15px 15px 0; padding: 10px;'></div>
        </td>
        <td style='width: 80%; vertical-align: top'>
            <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
                <tr>
                    
                    <td style='width: 13%; vertical-align: top'>
                        <label for='Search_username'><%:ViewResources.SharedStrings.UsersName %></label><br />
                        <input type='text' id='Search_username' style='width: 85%;' value='' onkeypress="javascript:onPressSubmitPeopleSearch(event);" />
                    </td>
                    <td style='width: 17%; vertical-align: top'>
                        <label for='Search_card_no'><%:ViewResources.SharedStrings.UsersCardNo %></label>
                        <table cellpadding='0' cellspacing='0' style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
                            <tr>
                                <td style='width: 30%'><%:ViewResources.SharedStrings.UsersSerial %>:</td>
                                <td style='width: 70%'>
                                    <input type='text' id='Search_card_ser' style='width: 30px;' value='' maxlength='3' onkeyup="javascript:CardValidation();" />+<input type='text' id='Search_card_dk' maxlength='5' style='width: 47px;' value='' onkeyup="javascript:CardRelationSERDK();" />
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 30%'><%:ViewResources.SharedStrings.UsersCardCode %>:</td>
                                <td style='width: 70%'>
                                    <input type='text' id='Search_card_no' style='width: 148px;' value='' onkeyup="javascript:CardRelationCODE();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <% if (!Model.User.IsCompanyManager && !Model.User.IsDepartmentManager) { %>
                    <td style='width: 10%; vertical-align: top;'>
                        <label for='Search_company'><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                        <input type='text' id='Search_company' style='width: 85%;' value='' onchange="javascript:GetDepartmentByCompany($(this).attr('value'))" onkeypress="javascript:onPressSubmitPeopleSearch(event);" />
                    </td>
                    <% } %>
					<% if (Model.User.IsDepartmentManager) { %>
                    <td style='width: <%= Model.User.IsCompanyManager ? "7%" : "15%" %>; vertical-align: top;'>
                        <label for='Search_title'><%:ViewResources.SharedStrings.UsersTitle %></label><br />
                        <input type='text' id='Search_title' style='width: 85%;' value='' onkeypress="javascript:onPressSubmitPeopleSearch(event);" />
                    </td>
					<%} else{%>

						<td style='width: 10%; vertical-align: top;'>
                        <label for='Comment'><%:ViewResources.SharedStrings.CommonComments %></label><br />
                        <input type='text' id='Comment' style='width: 85%;' value='' /><!-- onchange="javascript:GetDepartmentByCompany($(this).attr('value'))" onkeypress="javascript:onPressSubmitPeopleSearch(event);" />-->

                        </td>

					<%}%>
                    <%if(!Model.User.IsDepartmentManager){%>
						 <td style='width: 20%; vertical-align: top;'>
							<label for='Search_department'><%:ViewResources.SharedStrings.UsersDepartment %></label><br />
							<select id="selectedDepartment"></select>
						</td>
					<%}%>
                   	<% if (Model.User.IsCompanyManager || Model.User.IsDepartmentManager) { %>
					<td style='width: 7%; vertical-align: top;'>
                        <label ><%:ViewResources.SharedStrings.UsersValidation %></label><br />
                    </td>
					 <% } %>
                    <td style='width: 2%; vertical-align: top; padding-top: 20px;'>
                        <span id='button_submit_people_search' class='icon icon_find tipsy_we' style="display: none" title='<%:ViewResources.SharedStrings.UsersSearchPeople %>' 
						onclick="tree_country_id = 0; tree_building_id = 0; tree_location_id = 0; tree_company_id = 0; tree_floor_id = 0; javascript:SubmitPeopleSearch();"></span>
                    </td>
                </tr>
            </table>
            <div style='margin: 10px 0 0 0; text-align: right;'>
            <table  style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
            <tr><td style='width: 10px;'></td>
            <td><%:ViewResources.SharedStrings.UsersUserStatus %>: 
            <select id='UserFilter'>
                <option value="1"><%:ViewResources.SharedStrings.FilterActive %></option>
                <option value="0"><%:ViewResources.SharedStrings.FilterDeactivated %></option>
                <option value="2"><%:ViewResources.SharedStrings.FilterShowAll %></option>
            </select>
            </td><td style='text-align:right'>
                <%if(!Model.User.IsDepartmentManager){%>
					<input type='button' id='button_move_users_to_departament' value='<%:ViewResources.SharedStrings.UsersMoveUser %>' onclick="javascript:MoveUserToDepartament();" style='display:none'/>
				<%}%>
                <input type='button' id='button_delete_user' value='<%:ViewResources.SharedStrings.UsersDeleteUser %>' onclick="javascript:DeleteUser();" style='display:none'/>
                <input type='button' id='button_activate_user' value='<%:ViewResources.SharedStrings.UsersActivateUser %>' onclick="javascript:ActivateUser();" style='display:none'/>
                <input type='button' id='button_deactivate_user' value='<%:ViewResources.SharedStrings.UsersDeactivateUser %>' onclick="javascript:DeactivateUser();" style='display:none'/>
                <input type='button' id='button_add_user' value='<%:ViewResources.SharedStrings.UsersAddNewUser %>' onclick="javascript:AddUser();" />
               

      <%--                <input type='button' id='button_add_csvb' value='AddUser From Csv' onclick="javascript: Addcsv();" /> --%> 
                        
                       
                
                <%if(Model.HRService){%>
                <input type='button' id='button_Integrate' value='HR'  onclick="javascript:Integration();" />
                <%}%>
                              
                     
              
          
            </td>
            </tr>
            </table>
            </div>
            <div id='userPrintControlButtons' style="display:none; text-align:right">
                <a style="cursor:pointer;" onclick="javascript:SetUserExportLink(this,'/Print/UserListPDF')">PDF</a> / <a style="cursor:pointer;" onclick="javascript:SetUserExportLink(this,'/Print/UserListExcel')">XLS</a>
            </div>
            <div id='AreaTabPeopleSearchResultsWait' style='display: none; width: 100%; height:378px; text-align:center'><span style='position:relative,; top:35%' class='icon loader'></span></div>
            <div id='AreaTabPeopleSearchResults' style='display: none; margin: 15px 0; '></div>



        </td>
    </tr>
</table>
</div>
<script>
  $(document).ready(function () {
    	var i = $('#panel_owner_tab_administration li').index($('#companyTab'));
    	var opened_tab = '<%: Session["subTabIndex"] %>';

    	if (opened_tab != '') {
    		i1 = new Number(opened_tab);
    		if (i1 != i) {
    			SetOpenedSubTab(i);
    		}
    	}
    	else {
    		SetOpenedSubTab(i);
    	}
		
		$("input:button").button();
        $.get('/NewUser/GetTree', function (html) {
            $("div#AreaCompanyTree").html(html);
            $("div#AreaCompanyTree").attr("id", function () {
                $(this).corner("bevelfold");
            });
            $("ul#companies").treeview({
                animated: "fast",
                persist: "location",
                collapsed: true,
                unique: true
            });
        });
  });
    </script>