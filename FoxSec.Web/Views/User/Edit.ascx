<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserEditViewModel>" %>
<%@ Import Namespace="FoxSec.Web.Helpers" %>

<style type="text/css">
    .TFtable {
        width: 100%;
        border-collapse: collapse;
    }

        .TFtable th {
            background: #333333;
            color: white
        }

        .TFtable td {
            padding: 3px;
        }
        /* provide some minimal visual accomodation for IE8 and below */
        .TFtable tr {
            background: #CCC;
        }
            /*  Define the background color for all the ODD background rows  */
            .TFtable tr:nth-child(odd) {
                background: white;
            }
            /*  Define the background color for all the EVEN background rows  */
            .TFtable tr:nth-child(even) {
                background: #CCC;
            }
</style>

<div id='edit_dialog_content' style="display:none">
<% String usercompanyId = string.Empty; %>
<%= Html.Hidden("UserId", Model.FoxSecUser.Id) %>
<%= Html.Hidden("UserPermissionGroupId") %>
<%= Html.Hidden("UserBuildingObjectId") %>
<%= Html.Hidden("cards", Model.FoxSecUser.UsersAccessUnits.Any(x=>x.Active==true && x.IsDeleted==false && x.Free == false)) %>
   

<div id='panel_edit_user'>
<ul>
    <li><a href='#tab_user_default'><%:ViewResources.SharedStrings.PersonalDataTabName %></a></li>
    <% if (Model.User.IsCommonUser) { }
        else
        {%>
    <li><a href='#tab_cards'><%:ViewResources.SharedStrings.CardsTabName %></a></li>
    <% if ((Model.FoxSecUser.IsSuperAdmin || Model.FoxSecUser.IsBuildingAdmin) && Model.User.Id == Model.FoxSecUser.Id)
        { %>
        <li><a href='/User/UserRightsTabContent'><%:ViewResources.SharedStrings.UserRightsTabName%></a></li>
    <% }
        else if (Model.User.IsSuperAdmin || Model.User.IsBuildingAdmin)
        { %>
        <li><a href='/User/UserRightsTabContent'><%:ViewResources.SharedStrings.UserRightsTabName%></a></li>
    <% }
        else if (Model.User.Id != Model.FoxSecUser.Id)
        { %>
        <li><a href='/User/UserRightsTabContent'><%:ViewResources.SharedStrings.UserRightsTabName%></a></li>
    <% } %>
    <li><a href='#'>Log</a></li>
    <%} %>
</ul>

<%-- User - User Tab --%>

<div id='tab_user_default'>
<div id='edit_user_personal_data'>
<ul>
    <li><a href="#tab_edit_user_personal_data"><%:ViewResources.SharedStrings.PersonalTabName %></a></li>
    <% if (Model.User.IsCommonUser) { }
        else
        {%>
    <li><a href="#tab_edit_user_roles"><%:ViewResources.SharedStrings.UserRolesTabName %></a></li>
    <%} %>
    <li><a href="#tab_edit_user_contact"><%:ViewResources.SharedStrings.ContactTabName %></a></li>
    <li><a href="#tab_edit_user_work"><%:ViewResources.SharedStrings.WorkTabName %></a></li>
    <li><a href="#tab_edit_user_other"><%:ViewResources.SharedStrings.OtherTabName %></a></li>
</ul>

<%--Edit User - Personal data Tab--%>

<div id='tab_edit_user_personal_data'>
<form id="editUserPersonalData" action="">
<%=Html.Hidden("Id", Model.FoxSecUser.Id) %>
<%=Html.Hidden("Cardchange", "0") %>
<table cellpadding="3" cellspacing="3" style="margin:0; width:100%; padding:3px; border-spacing:3px;">
<%if (!Model.FoxSecUser.IsBuildingAdmin)
    {%>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='CompanyId'><%:ViewResources.SharedStrings.UsersCompany%></label></td>
    <td style='width:40%; padding:2px;'>
		<%if (!Model.User.IsCompanyManager)
            {%>
        <%=Html.Hidden("UsrCompany", Model.FoxSecUser.CompanyId) %>
       <%
           if (Model.FoxSecUser.UsersAccessUnit.Count > 0)
           {
                usercompanyId = "1";
               
           }
           else
           {
               usercompanyId = "0";
           }

           %>
          <%=Html.Hidden("UsrCompanyacess",usercompanyId) %>

			<%= Html.DropDownList("CompanyId", new SelectList(Model.Companies, "Value", "Text", Model.FoxSecUser.CompanyId), ViewResources.SharedStrings.DefaultDropDownValue, new { @style = "width:219px", @tabindex="1" })%>
		<%}
            else
            { %>
			<%= Html.DropDownList("CompanyId", new SelectList(Model.Companies, "Value", "Text", Model.FoxSecUser.CompanyId), new { @style = "width:219px", @tabindex="1" })%>
		<%} %>
      
    </td>
    <td style='width:40%; padding:2px;'></td>
</tr>
<%}%>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='FirstName'><%:ViewResources.SharedStrings.UsersFirstName %></label></td>
	<td style='width:40%; padding:2px;'>
        <%=Html.TextBox("FirstName", Model.FoxSecUser.FirstName, new { @style="width:80%", @tabindex="2"  })%>
        <span id="FirstName_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
    </td>
    <td style='width:40%; padding:2px;' rowspan='5'>
    <div id='photo_content'>
    <% if (Model.FoxSecUser.Image.Count() > 0)
        { %>
        <table>
        <tr><td align="right"><span class="ui-icon ui-icon-circle-close" style="cursor:pointer" onclick="javascript:RemoveUserPhoto();"></span></td></tr>
        <tr><td><img src="/User/PhotoContent?id=<%=Model.FoxSecUser.Id %>&nocache=<% Random rnd = new Random(); %><%= rnd.Next(100000, 999999) %>" border='0' alt="" style="cursor:pointer;" onclick="javascript:ActivatePhotoUpload();" /></td></tr>
        </table>
    <% }
        else
        {%>
        <table>
        <tr><td align="right"><span class="ui-icon ui-icon-circle-close" style="cursor:pointer;visibility:hidden;" onclick="javascript:RemoveUserPhoto();"></span></td></tr>
        <tr><td><div style="width:90px; height:100px; border:1px solid #333; text-align:center; padding:10px 5px; cursor:pointer;" onclick="javascript:ActivatePhotoUpload();"><%:ViewResources.SharedStrings.UsersUploadFoto%></div></td></tr>
        </table>
    <% } %>
    </div>
    </td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='LastName'><%:ViewResources.SharedStrings.UsersLastName %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("LastName", Model.FoxSecUser.LastName, new { @style = "width:80%", @tabindex="3"  })%>
		<span id="LastName_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
	<td style='width:40%; padding:2px;'></td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='LoginName'><%:ViewResources.SharedStrings.UsersUserName%></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("LoginName", Model.FoxSecUser.LoginName, new { @style="width:80%", @tabindex="4"  })%>
		<span id="UserName_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
	<td style='width:40%; padding:2px;'></td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='Password'><%:ViewResources.SharedStrings.UsersPassword %></label></td>
	<td style='width:40%; padding:2px;'>
        <%=Html.Hidden("Password", Model.FoxSecUser.Password)%>         
		<%=Html.Password("DisplayPassword", string.IsNullOrEmpty(Model.FoxSecUser.Password) ? "" : "***", new { @style = "width:80%", onchange = "PassChange()",title="6-20 characters.At least one number. At lease one capital letter and one small letter.", @tabindex="5",autocomplete="off" })%>
         
		<input type="checkbox" id="ShowPasschbx" onchange="ShowPass(this);"/>
        <span id="Password_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
	<td style='width:40%; padding:2px;'></td>
</tr>
    <tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='RepeatPassword'><%:ViewResources.SharedStrings.UsersPassword %></label></td>
	<td style='width:40%; padding:2px;'>
                                        
		<%=Html.Password("RepeatPassword", string.IsNullOrEmpty(Model.FoxSecUser.Password) ? "" : "***", new { @style = "width:80%",title="6-20 characters.At least one number. At lease one capital letter and one small letter.", @tabindex="6",autocomplete="off" })%>
        
		<span id="Span1" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
	<td style='width:40%; padding:2px;'></td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='PersonalId'><%:ViewResources.SharedStrings.UsersUserId %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("PersonalId", Model.FoxSecUser.PersonalId, new { @style = "width:80%" , @tabindex="7" })%>
		<span id="PersonalId_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
	<td style='width:40%; padding:2px;'></td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='Email'><%:ViewResources.SharedStrings.UsersEmail %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("Email", Model.FoxSecUser.Email, new { @style = "width:80%", @tabindex="8"  })%>
	<span id="Email_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
	</td>
	<td style='width:40%; padding:2px;' rowspan='3'><div id='upload_photo' style="display:none"></div></td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='PersonalCode'><%:ViewResources.SharedStrings.UsersPersonalCode %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("PersonalCode", Model.FoxSecUser.PersonalCode, new { @style = "width:80%", @tabindex="9"  })%>
		<span id="PersonalCode_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
	<td style='width:40%; padding:2px;'></td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='ExternalPersonalCode'><%:ViewResources.SharedStrings.UsersExtPersonalCode %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("ExternalPersonalCode", Model.FoxSecUser.ExternalPersonalCode, new { @style = "width:80%", @tabindex="10"  })%>
		<span id="ExternalPersonalCode_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
	<td style='width:40%; padding:2px;'></td>
</tr>
<tr>
    <td style='width:20%; padding:2px; text-align:right;'><label for='BirthDayStr'><%:ViewResources.SharedStrings.UsersBirthday %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("BirthDayStr", Model.FoxSecUser.BirthDayStr, new { @style = "width:90px" , @tabindex="11" })%>
        <span id="BirthDayStr_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
		&nbsp;(dd.mm.yyyy)</td>
	<td style='width:40%; padding:2px;'><label for='RegistredDateStr'><%:ViewResources.SharedStrings.UsersRegistered %></label>
		<%=Html.TextBox("RegistredDateStr", Model.FoxSecUser.RegistredDateStr, new { @style = "width:90px", @tabindex="12"  })%>
	</td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='PIN1'>PIN 1:</label></td>
	<td colspan = "4" style='width:40%; padding:2px;'>
		<%=Html.Password("PIN1", Model.FoxSecUser.PIN1, new{@style="display:none"}) %>
		<%=Html.TextBox("DisplayPIN1", string.IsNullOrEmpty(Model.FoxSecUser.PIN1) ? "" : "***", new { @style = "width:30%", onchange = "PinChange(this)", @tabindex="13",autocomplete="off"  })%>
		<span id="PIN1_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
		<label for='PIN2'>PIN 2:</label>&nbsp;
		<%=Html.Password("PIN2", Model.FoxSecUser.PIN2, new{@style="display:none"}) %>
		<%=Html.TextBox("DisplayPIN2", string.IsNullOrEmpty(Model.FoxSecUser.PIN2) ? "" : "***", new { @style = "width:30%", onchange = "PinChange(this)", @tabindex="14",autocomplete="off"  })%>
		<span id="PIN2_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
	</td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='LanguageId'><%:ViewResources.SharedStrings.UsersLanguage %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.DropDownList("LanguageId",
			new SelectList(Model.LanguageItems, "Value", "Text", Model.FoxSecUser.LanguageId), ViewResources.SharedStrings.DefaultDropDownValue,
			new { @style = "width:219px", @tabindex="15"  })%>
	</td>
	<td style='width:40%; padding:2px;'></td>
</tr>
</table>
</form>
<br />
<% if (Model.User.IsCommonUser) { }
    else
    {%>
<input type='button' id='button_save_edit_user' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='javascript: SavePersonalData();' tabindex="16" />
<input type='button' id='button_generate_password' value='<%=ViewResources.SharedStrings.BtnGeneratePassword %>' onclick='SubmitGenerateUserPassword();' tabindex="17" />
<input type='button' id='button_generate_pin' value='<%=ViewResources.SharedStrings.UsersBtnGeneratePin %>' onclick='SubmitGenerateUserPin();' tabindex="18" />
<%} %>
</div>
    <% if (Model.User.IsCommonUser) { }
        else
        {%>
<%--Edit User - User Roles Tab--%>

<div id='tab_edit_user_roles'>
	<% Html.RenderPartial("UserRoles", Model.FoxSecUser.UserRoleItems); %>
</div>

<%--Edit User - User Contact Tab--%>
    <%} %>
<div id='tab_edit_user_contact'>
<form id="editUserContact" action="">
<%=Html.Hidden("Id", Model.FoxSecUser.Id) %>
<table cellpadding="3" cellspacing="3" style="margin:0; width:100%; padding:3px; border-spacing:3px;">
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='Residence'><%=ViewResources.SharedStrings.UsersResidence %></label></td>
	<td style='width:80%; padding:2px;'>
		<%=Html.TextArea("Residence", Model.FoxSecUser.Residence, new { @style = "width:80%" })%>
	</td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='PhoneNumber'><%=ViewResources.SharedStrings.UsersPhone %></label></td>
	<td style='width:80%; padding:2px;'>
		<%=Html.TextBox("PhoneNumber", Model.FoxSecUser.PhoneNumber, new { @style = "width:80%" })%>
	</td>
</tr>
</table>
</form>
<br />
    <% if (Model.User.IsCommonUser) { }
        else
        {%>
<input type='button' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='$.post("/User/EditContactData", $("#editUserContact").serialize()); ShowDialog("Saved", 2000, true);' />
<%} %>
</div>

<%--Edit User - User Work Tab--%>

<div id='tab_edit_user_work'>
<form id="editUserWork" action="">
<%=Html.Hidden("Id", Model.FoxSecUser.Id) %>
<%=Html.Hidden("FoxSecUserId", Model.FoxSecUser.Id) %>
<table id="userBuildingObjectsTable" style="margin:0; width:100%; padding:3px; border-spacing:3px;">
	<thead>
		<tr>
			<td style='width:20%; padding:2px;'>
		<%=ViewResources.SharedStrings.CommonBuilding %>
	</td>
	<td style='width:40%; padding:2px;'>
		<%=ViewResources.SharedStrings.CommonFloor %>
	</td>
	<td style='width:5%; padding:2px;'>
	</td>
	<td style='width:5%; padding:2px;'>
	</td>
	<td style='width:30%; padding:2px;'>
	</td>
		</tr>
	</thead>
	<tbody>
		<%foreach (var ubo in Model.FoxSecUser.UserBuildingObjects)
            {%>
			<tr id=<%:string.Format("userBuildingRow{0}", Model.FoxSecUser.UserBuildingObjects.IndexOf(ubo)) %>>
				<td style='width:20%; padding:2px;'>
					<%if (ubo.IsBuildingAvailable)
                        { %>
						<%=Html.DropDownList(ViewHelper.GetPrefixedName("UserBuildingObjects", "BuildingId", Model.FoxSecUser.UserBuildingObjects.IndexOf(ubo)),
																												new SelectList(Model.FoxSecUser.BuildingItems, "Value", "Text", ubo.BuildingId), ViewResources.SharedStrings.DefaultDropDownValue, new {  @style = "width:120px", onchange = "javascript:ChangeUserBuilding(this);" })%>
					<%}
                        else
                        {%>
						<%=Html.Hidden(ViewHelper.GetPrefixedName("UserBuildingObjects", "BuildingId", Model.FoxSecUser.UserBuildingObjects.IndexOf(ubo)), ubo.BuildingId)%>
						<%=Html.Encode(ubo.BuildingName) %>
					<%} %>
				</td>
				<td style='width:40%; padding:2px;'>
					<%if (ubo.IsBuildingAvailable)
                        { %>
						<%=Html.DropDownList(ViewHelper.GetPrefixedName("UserBuildingObjects","BuildingObjectId", Model.FoxSecUser.UserBuildingObjects.IndexOf(ubo)),
								new SelectList(ubo.FloorItems, "Value", "Text", ubo.BuildingObjectId), new { @style = "width:100px" })%>
					<%}
                        else
                        {%>
						<%=Html.Hidden(ViewHelper.GetPrefixedName("UserBuildingObjects", "BuildingObjectId", Model.FoxSecUser.UserBuildingObjects.IndexOf(ubo)), ubo.BuildingObjectId)%>
						<%=Html.Encode(ubo.FloorName) %>
					<%} %>
				</td>
				<td style='width:5%; padding:2px;'>
					<%if (ubo.IsBuildingAvailable && (Model.FoxSecUser.IsBuildingAdmin || Model.FoxSecUser.IsSuperAdmin || Model.FoxSecUser.IsCompanyManager))
                        {%>
						<span id = 'user_building_delete_'<%= Model.FoxSecUser.UserBuildingObjects.IndexOf(ubo) %>' class="ui-icon ui-icon-circle-close tipsy_we" original-title="Remove Building" style="cursor:pointer;" onclick="javascript:RemoveUserBuilding(this);"></span>
					<%} %>
				</td>
				<td style='width:5%; padding:2px;'>
					<%if (ubo.IsBuildingAvailable && (Model.FoxSecUser.IsBuildingAdmin || Model.FoxSecUser.IsSuperAdmin || Model.FoxSecUser.IsCompanyManager))
                        {%>
						<span id = 'user_building_add_'<%= Model.FoxSecUser.UserBuildingObjects.IndexOf(ubo) %>' class="ui-icon ui-icon-circle-plus tipsy_we" style="cursor:pointer;" original-title = "Add building" onclick="javascript:AddUserBuilding(this);"></span>
					<%}%>
				</td>
				<td style='width:30%; padding:2px;'>
				</td>
			</tr>
        <%}%>
	</tbody>
</table>
<table cellpadding="3" cellspacing="3" style="margin:0; width:100%; padding:3px; border-spacing:3px;">
<tr>
	<td style='width:20%; padding:2px;'>
      <input type='button' id='show_departmens_dialog' style='font-size: 8pt' value='<%=ViewResources.SharedStrings.UsersDepartments %>' onclick="javascript: OpenDepartmentsDialog('<%= Model.FoxSecUser.Id %>');" />
    </td>
	<td style='width:40%; padding:2px;'>
	</td>
	<td style='width:40%; padding:2px;'>
	</td>
</tr>
<!-- develop it on the second phase
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='edit_user_directmanager'><%:ViewResources.SharedStrings.UsersDirectManager %></label></td>
	<td style='width:40%; padding:2px;'>
		<select>
            <option value="empty"><%:ViewResources.SharedStrings.DefaultDropDownValue %></option>
			<option value="Helmes"><%:ViewResources.SharedStrings.UsersDirectManager %></option>
		</select>
	</td>
	<td style='width:40%; padding:2px;'></td>
</tr>
-->
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='edit_user_title'><%:ViewResources.SharedStrings.UsersTitle %></label></td>
	<td style='width:40%; padding:2px;'>
        <%= Html.DropDownList("TitleId", new SelectList(Model.Titles, "Value", "Text", Model.FoxSecUser.TitleId), ViewResources.SharedStrings.DefaultDropDownValue )%>
	</td>
    <td style='width:40%; padding:2px;'></td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='ContractStartDateStr'><%:ViewResources.SharedStrings.UsersContractNr %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("ContractNum", Model.FoxSecUser.ContractNum, new { @style = "width:80%" })%>
	</td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("ContractStartDateStr", Model.FoxSecUser.ContractStartDateStr, new { @style = "width:90px", @class = "date_start" })%> - <%=Html.TextBox("ContractEndDateStr", Model.FoxSecUser.ContractEndDateStr, new { @style = "width:90px", @class = "date_end" })%>
	</td>
</tr>
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='PermitOfWorkStr'><%:ViewResources.SharedStrings.UsersPermitOfWork %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("PermitOfWorkStr", Model.FoxSecUser.PermitOfWorkStr, new { @style = "width:90px", @class = "select_date" })%>
	</td>
	<td style='width:40%; padding:2px;'></td>
</tr>
<tr>
    <td style='width:20%; padding:2px; text-align:right;'><label for='WorkTime'><%:ViewResources.SharedStrings.UsersWorkTime %></label></td>
	<td style='width:40%; padding:2px;'>
		<% bool wt = false; if (Model.FoxSecUser.WorkTime.HasValue) { wt = Model.FoxSecUser.WorkTime.Value; } %><%=Html.CheckBox("WorkTime", wt)%>
	</td>
    <td style='width:40%; padding:2px;'></td>
</tr>
<tr>
    <td style='width:20%; padding:2px; text-align:right;'><label for='TableNumber'><%:ViewResources.SharedStrings.UsersTableNr %></label></td>
	<td style='width:40%; padding:2px;'>
		<%=Html.TextBox("TableNumber", Model.FoxSecUser.TableNumber, new { @style = "width:90px" })%>
	</td>
	<td style='width:40%; padding:2px;'></td>
</tr>
</table>
</form>
<br />
    <% if (Model.User.IsCommonUser) { }
        else
        {%>
<input type='button' id='button3' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='javascript: SaveWorkData();' />
    <%} %>

    <%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTAReportMenu))
        {%>
    <input type='button' id='btnatwork' value='<%=ViewResources.SharedStrings.Atwork %>' onclick='javascript: Atwork();'/>
    <input type='button' id='btnleaving' value='<%=ViewResources.SharedStrings.Leaving %>' onclick='javascript: leaving();'/>
    <% }%>
</div>

<%--Edit User - User Other Tab--%>

<div id='tab_edit_user_other'>
    <form id="EditOtherData">
        <%=Html.Hidden("Id", Model.FoxSecUser.Id) %>

<table cellpadding="3" cellspacing="3" style="margin:0; width:100%; padding:3px; border-spacing:3px;">
<tr>
    <td style='width:20%; padding:2px; text-align:right;'><label for='edit_user_coffeecups'><%:ViewResources.SharedStrings.CommonComments %></label></td>
	<td style='width:70%; padding:2px;'>
        <%=Html.TextArea("Residence", Model.FoxSecUser.Comment, new { @style = "width:100%" })%>
        </td>
    <td style='width:10%; padding:2px;'></td>
</tr><tr>
    <td style='width:20%; padding:2px; text-align:right;'><label for='edit_user_coffeecups'><%:ViewResources.SharedStrings.UsersCoffeCups %></label></td>
	<td style='width:70%; padding:2px;'><%=Html.TextBox("Coffe cups", "", new { @style = "width:100%", @disabled = "disabled" })%></td>
    <td style='width:10%; padding:2px;'></td>
</tr>
<tr>
    <td style='width:20%; padding:2px; text-align:right;'><label for='edit_user_worktime'><%:ViewResources.SharedStrings.UsersPermisionCallQuests %></label></td>
	<td style='width:70%; padding:2px;'><%=Html.CheckBox("PermissionCallGuests", Model.FoxSecUser.PermissionCallGuests)%></td>
    <td style='width:10%; padding:2px;'></td>
</tr>
</table>
</form>
<br />
    <% if (Model.User.IsCommonUser) { }
        else
        {%>
<input type='button' id='button4' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='$.post("/User/EditOtherData", $("#EditOtherData").serialize()); ShowDialog("Saved", 2000, true);' />
<%} %>
    <br/>
    <br/>
    <div id="divdetails"></div>
</div>
</div>
</div>
        <% if (Model.User.IsCommonUser) { }
            else
            {%>
<%-- User Cards Tab --%>
<div id='tab_cards'>

<div id='edit_user_card_data'>
<ul>
    <li><a href="/User/UserCardsTabContent"><%:ViewResources.SharedStrings.UsersUserCards %></a></li>
</ul>

<%-- User Cards - Card List Tab --%>

</div>
</div>

<%-- User Rights Tab --%>
    <%} %>
</div>
</div>

<script type="text/javascript" language="javascript">
    $(function () {
        $(document).tooltip();
    });
    $(document).ready(function () {
        pin1Changed = false;
        pin2Changed = false;
        $("#panel_edit_user").attr("id", function () {
            passChecked = true;
            $("#panel_edit_user").tabs({
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
                active: 0
            });
        });

        $("#panel_edit_user").tabs("option", "disabled", [3]);

        $("#edit_user_personal_data").tabs({
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
            active: 0
        });

        $("#edit_user_card_data").tabs({
            
            beforeLoad: function (event, ui) {
                debugger;
                ui.ajaxSettings.async = false,
                    ui.ajaxSettings.error = function (xhr, status, index, anchor) {
                        $(anchor.hash).html("Couldn't load this tab!");
                    }
            },
            fx: {
                opacity: "toggle",
                duration: "fast"
            },
            active: 0
        });

        $(".select_date").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true
        });

        $(".date_start").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function (dateText, inst) {
                $(".date_end").datepicker("option", "minDate", dateText);
            }
        });

        $(".date_end").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            minDate: $(".date_end").val()
        });

        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });

        $("input:button").button();
        $.get('/Card/UserCardsList', { id: $("div#edit_dialog_content").find("input#UserId").val(), filter: 1 }, function (html) { $("div#userCardsList").html(html); });
        $("div#edit_dialog_content").fadeIn("fast");

        $.ajax({
            type: 'GET',
            url: '/User/GetFsProjectsUsersMemoryList',
            cache: false,
            data: {
                id:"<%=  Model.FoxSecUser.Id %>"
            },
            success: function (html) {
                $("div#divdetails").html("");
                $("div#divdetails").html(html);
            }
        });
        return false;
    });

    function ShowPass(chbx) {
        if (chbx.checked) {
            var element = document.getElementById("DisplayPassword");
            if (element.type == 'password') {
                element.type = "text";
            }
        }
        else {
            var element = document.getElementById("DisplayPassword");
            if (element.type == 'text') {
                element.type = "password";
            }
        }
    }

    function SaveWorkData() {
        debugger;
        var BuildingId = $("#Buildingval").val();
        var serial = $("#editUserWork").serialize();
        if (BuildingId == "") {
               ShowDialog('<%=ViewResources.SharedStrings.BuildingMessage %>', 3000, false);
            
        }
        else {


            $.ajax({
                type: "Post",
                url: "/User/EditWorkData",
                dataType: 'json',
                traditional: true,
                data: $("#editUserWork").serialize(),
                success: function (data) {
                    if (data.IsSucceed == false) {
                        ShowDialog(data.Msg, 2000);
                    }
                    else {
                        ShowDialog(data.Msg, 2000, true);
                    }
                }
            });
        }
    }

    function SubmitGenerateUserPassword() {
        $.getJSON('/User/GeneratePassword', function (data) {
            $("input#Password").val(data.passw);
            $("input#DisplayPassword").val(data.passw);
        });
        passChecked = true;
        return false;
    }

    function SubmitGenerateUserPin() {
        $.getJSON('/User/GeneratePin', function (data) {
            $("input#PIN1").val(data.pin1);
            $("input#PIN2").val(data.pin2);
            $("input#DisplayPIN1").val(data.pin1);
            $("input#DisplayPIN2").val(data.pin2);
        });
        return false;
    }

    function ActivatePhotoUpload() {
        $.get('/User/UploadImage',
            { userId: "<%= Model.FoxSecUser.Id %>" },
            function (html) {
                $("div#upload_photo").html(html);
                $("div#upload_photo").toggle("slow");
            });
        return false;
    }

    function IsValidPin(pin) {
        if (pin == "") {
            return true;
        }
        var re4digit = /^(\d{4}|\d{6})$/;
        if (pin.search(re4digit) == -1) {
            return false;
        }

        return true;
    }

    function PassChange() {
        $("input#Password").val($("input#DisplayPassword").val());
        passChecked = false;
    }

    function PinChange(cntr) {
        var pin_id = '#' + $(cntr).attr('id').substr(7);
        var pin = $(cntr).val();
        $(pin_id).attr('value', pin);
        $(cntr).attr('value', pin);

        var testpinx = $(pin_id).val();

        if ($(cntr).attr('id').substr(7) == 'PIN1') {
            pin1Changed = true;
        }
        else {
            pin2Changed = true;
        }
        pinChanged = true;
    }

    function ValidatePassword(cntr) {
        pass_str = $(cntr).attr('value');

        if (pass_str == "") {
            return true;
        }

        regexp = /^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$/;

        if (pass_str.search(regexp) == -1) {
            return false;
        }


        return true;
    }

    function SavePersonalData() {

        var isValid = new Boolean(true);
        var repeatpass = $("input#Password").val() == $("input#RepeatPassword").val();
        if (passChecked == false && !repeatpass) {
            isValid = false;
            ShowDialog('<%=ViewResources.SharedStrings.UsersPasswordErrorMessage %>', 4000, false);//!!!
        }
        if ($("input#FirstName").val() == "" || ($("input#FirstName").val() != "" && !$("input#FirstName").val().match(/^.{1,20}$/))) { $("#FirstName_val").text('!').show().fadeOut(3000); isValid = false; }
        if ($("input#LastName").val() == "" || ($("input#LastName").val() != "" && !$("input#LastName").val().match(/^.{1,20}$/))) { $("#LastName_val").text('!').show().fadeOut(3000); isValid = false; }
        if ($("input#LoginName").val() == "") { $("#UserName_val").text('!').show().fadeOut(3000); isValid = false; }
        if (passChecked == false && ValidatePassword($("input#Password")) == false) {
            $("#Password_val").text('!').show().fadeOut(3000);
            isValid = false;
            ShowDialog('<%=ViewResources.SharedStrings.UsersPasswordErrorMessage %>', 4000, false);
        }

        if ($("input#Email").val() != "" && !$("input#Email").val().match(/[\w-]+@([\w-]+\.)+[\w-]+/)) { $("#Email_val").text('!').show().fadeOut(3000); isValid = false; }
        if ($("input#PersonalCode").val())
            if ($("input#ExternalPersonalCode").val())
                if ($("input#BirthDayStr").val() != "" && (!$("input#BirthDayStr").val().match(/^(0[1-9]|[12][0-9]|3[01])[.](0[1-9]|1[012])[.](19|20)\d\d$/))) { $("#BirthDayStr_val").text('!').show().fadeOut(3000); isValid = false; }

        if (IsValidPin($("input#PIN1").val()) == false && pin1Changed == true) {
            $("#PIN1_val").text('!').show().fadeOut(3000);
            isValid = false;
            ShowDialog('<%=ViewResources.SharedStrings.UsersIncorrectPinErrorMessage %>', 2000);
        }
        if (IsValidPin($("input#PIN2").val()) == false && pin2Changed == true) {
            $("#PIN2_val").text('!').show().fadeOut(3000);
            isValid = false;
            ShowDialog('<%=ViewResources.SharedStrings.UsersIncorrectPinErrorMessage %>', 2000);
        }

        if (!isValid) {
            ShowDialog('<%=ViewResources.SharedStrings.UsersValidator %>', 5000);
            return false;
        }

        var usId = $('#tab_edit_user_personal_data').find('#Id').attr('value');

        $.getJSON('/User/CheckLoginName', { login: $("input#LoginName").val(), userId: usId },
            function (data) {
               
                if (data.isInUse) {
                    ShowDialog('<%=ViewResources.SharedStrings.UsersMessageLoginIsInUse %>', 5000); return false;
                }
                else {
                    //tesa
                   debugger;
                    var company = $('#CompanyId').val();
                    var usrcompany = $('#UsrCompany').val();
                    var cards = $('#cards').val();
                    var useraceessid = $('#UsrCompanyacess').val();
                    //var splitval = useraceessid.split(",");
                   
                    var Flag = 0;
                    if (useraceessid == "1") {
                        Flag = 1;
                       
                    }
                    else {

                    }
                    
                    if (Flag==1) {
                        $('#button_save_edit_user').hide();
                        $("div#user-modal-dialog").dialog({
                            open: function () {
                                $("div#user-modal-dialog").html('What to do with cards?</br><select id="CardChangeStausReaon"><option value ="1">Change Card company</option><option value ="2">Move to Free cards</option></select>');
                            },
                            close: function () {
                                $('#button_save_edit_user').show();
                            },
                            resizable: false,
                            width: 300,
                            height: 250,
                            modal: true,
                            title: "Cards",
                            buttons: {
			                     '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                                    ShowDialog('<%=ViewResources.SharedStrings.DepartmentsUserDepartmentSavingMessage %>', 2000, true);
                                     $(this).dialog("close");
                                     var crd = $('#CardChangeStausReaon').val();
                                     $("#Cardchange").val(crd);
                                     $.post("/User/EditPersonalData", $("#editUserPersonalData").serialize(), function () {
                                         newDialogTitle = "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + $("input#FirstName").val() + ' ' + $("input#LastName").val();
                                         $('#ui-dialog-title-modal-dialog').html(newDialogTitle);
                                         ReloadEditPage();
                                         $('#button_save_edit_user').show();
                                         ShowDialog('<%=ViewResources.SharedStrings.CommonDataSavedMessage %>', 2000, true);
                                    });
                                    return false;
                                },
                                '<%=ViewResources.SharedStrings.BtnBack %>': function () {
                                    $('#button_save_edit_user').show();
                                    $(this).dialog("close");
                                    return false;
                                }
                            }
                        });
                    } else {

                        $.post("/User/EditPersonalData", $("#editUserPersonalData").serialize(), function () {
                            newDialogTitle = "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + $("input#FirstName").val() + ' ' + $("input#LastName").val();
                            $('#ui-dialog-title-modal-dialog').html(newDialogTitle);
                            ReloadEditPage();
                            ShowDialog('<%=ViewResources.SharedStrings.CommonDataSavedMessage %>', 2000, true);
                        });
                        return false;
                    }
                }
            });
    }

    function OpenDepartmentsDialog(id) {
        $("div#user-modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $.get('/Department/UserDepartmentList', { userId: id }, function (html) {
                    $("div#user-modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 640,
            height: 480,
            modal: true,
            title:  "<%=string.Format("<span class={1}ui-icon ui-icon-pencil{1} style={1}float:left; margin:1px 5px 0 0{1} ></span>{0}",ViewResources.SharedStrings.DialogTitleUserDepartments, "'") %>",
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    $.post("/Department/SaveUserDepartments", $("#userDepartmentsForm").serialize());
                    ShowDialog('<%=ViewResources.SharedStrings.DepartmentsUserDepartmentSavingMessage %>', 2000, true);
                    $(this).dialog("close");
                },
                '<%=ViewResources.SharedStrings.BtnBack %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    function ChangeUserBuilding(cntr) {
        debugger;
        rowObj = $(cntr).parents('[id*=userBuildingRow]');
        floorControl = rowObj.find('[id*=_BuildingObjectId]');
       
        $.get('/User/GetFloorsByBuilding', { userId: $("#FoxSecUserId").attr('value'), buildingId: $(cntr)[0].value }, function (data) { floorControl.html(data); }, 'json');
            
        return false;
    }

    function RemoveUserBuilding(cntr) {
        if ($('[id*=userBuildingRow]').size() == 1 || $('[id*=user_building_delete_]').size() == 1) {
            return;
        }
        else {
            rowObj = $(cntr).parents('[id*=userBuildingRow]');
            rowObj.remove();
            SetUpUserBuildingObjectIds();
            return false;
        }
    }

    function AddUserBuilding(cntr) {
        rowObj = $(cntr).parents('[id*=userBuildingRow]');
        newRow = rowObj.clone();
        newRow.insertAfter(rowObj);
        buildControl = newRow.find('[id*=_BuildingId]');
        buildControl.find('option:first').attr('selected', 'selected');
        ChangeUserBuilding(buildControl);
        SetUpUserBuildingObjectIds();
        return false;
    }

    function SetUpUserBuildingObjectIds() {
        ind = 0;
        $('[id*=userBuildingRow]').each(function () {
            buildControl = $(this).find('[id*=_BuildingId]');
            buildName = 'UserBuildingObjects[' + ind + '].BuildingId';
            buildControl.attr('name', buildName);

            floorControl = $(this).find('[id*=_BuildingObjectId]');
            floorName = 'UserBuildingObjects[' + ind + '].BuildingObjectId';
            floorControl.attr('name', floorName);

            $(this).attr('value', 'userBuildingRow' + ind);
            ind++;
        });
        return false;
    }

    function ReloadEditPage() {
        $.get('/User/Edit', { id: $('#UserId').attr('value') }, function (html) { $("div#modal-dialog").html(html); });
        
      
        return false;
    }

    function RemoveUserPhoto() {
        $("div#delete-modal-dialog").dialog({
            open: function (event, ui) {
                $("div#delete-modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 300,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.UsersDeletingPhoto %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnDelete %>': function () {
                    $(this).dialog("close");
                    $.ajax({
                        type: "Post",
                        url: "/User/RemovePhoto",
                        data: { id: '<%= Model.FoxSecUser.Id %>' },
                        success: function () {
                            $("div#photo_content").html('<table><tr><td align="right"><span class="ui-icon ui-icon-circle-close" style="cursor:pointer;visibility:hidden;" onclick="javascript:RemoveUserPhoto();"></span></td></tr><tr><td><div style="width:90px; height:100px; border:1px solid #333; text-align:center; padding:10px 5px; cursor:pointer;" onclick="javascript:ActivatePhotoUpload();"><%:ViewResources.SharedStrings.UsersUploadFoto%></div></td></tr></table>');
                        }
                    });
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    function leaving() {
        var id = $("div#edit_dialog_content").find("input#UserId").val();
        $.ajax({
            type: "Get",
            url: "/User/SaveWorkLeaving",
            data:
            {
                boid: 0,
                type: 2,
                id: id
            },
            success: function (response) {
                ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>', 2000, true);
                $("div#user-modal-dialog").dialog("close");
            }
        })
        return false;
    }
    function Atwork() {
        var id = $("div#edit_dialog_content").find("input#UserId").val();
        $("div#user-modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $.get('/User/AtWork', { userId: id }, function (html) {
                    $("div#user-modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 600,
            height: 150,
            modal: true,
            title:  "<%=string.Format("<span class={1}ui-icon ui-icon-pencil{1} style={1}float:left; margin:1px 5px 0 0{1} ></span>{0}",ViewResources.SharedStrings.Atwork, "'") %>",
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    var bid = $('#UserBuildingObjectsItems').val();
                    if (bid == "" || bid == null) {
                        ShowDialog('Please select Building Object!!', 3000, false);
                        return false;
                    }
                    button = $(this).parent().find("button:contains('<%=ViewResources.SharedStrings.BtnOk %>')");
                    button.unbind();
                    button.addClass('ui-state-disabled');
                    $.ajax({
                        type: "Get",
                        url: "/User/SaveWorkLeaving",
                        data:
                        {
                            boid: bid,
                            type: 1,
                            id: id
                        },
                        success: function (response) {
                            ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>', 2000, true);
                            $("div#user-modal-dialog").dialog("close");
                        }
                    })
                }
            }
        });
        return false;
    }

</script>