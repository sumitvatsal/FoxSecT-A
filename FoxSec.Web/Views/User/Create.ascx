<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserEditViewModel>" %>

<div id='create_dialog_content' style="display: none">

    <div id='panel_create_user'>
        <ul>
            <li><a href="#tab_user_default"><%:ViewResources.SharedStrings.PersonalDataTabName %></a></li>
        </ul>
        <div id='tab_user_default'>

            <div id='create_user_personal_data'>
                <ul>
                    <li><a href="#tab_create_user_personal_data"><%:ViewResources.SharedStrings.PersonalTabName %></a></li>
                    <li><a href="#tab_create_user_roles"><%:ViewResources.SharedStrings.UserRolesTabName %></a></li>
                    <li><a href="#tab_create_user_contact"><%:ViewResources.SharedStrings.ContactTabName %></a></li>
                    <li><a href="#tab_ceate_user_work"><%:ViewResources.SharedStrings.WorkTabName %></a></li>
                    <li><a href="#tab_create_user_other"><%:ViewResources.SharedStrings.OtherTabName %></a></li>
                </ul>

                <%--Create User - Personal data Tab--%>

                <div id='tab_create_user_personal_data'>
                    <form id="editUserPersonalData" action="">
                        <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px">
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_company'><%:ViewResources.SharedStrings.UsersCompany %></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%if (!Model.User.IsCompanyManager)
                                        {%>
                                    <%= Html.DropDownList("CompanyId", new SelectList(Model.Companies, "Value", "Text", Model.FoxSecUser.CompanyId), ViewResources.SharedStrings.DefaultDropDownValue, new { @style = "width:219px" })%>
                                    <%}
                                        else
                                        { %>
                                    <%= Html.DropDownList("CompanyId", new SelectList(Model.Companies, "Value", "Text", Model.FoxSecUser.CompanyId), new { @style = "width:219px" })%>
                                    <%} %>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='FirstName'><%:ViewResources.SharedStrings.UsersFirstName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("FirstName", Model.FoxSecUser.FirstName, new { @style = "width:80%c", @OnKeyUp = "FirstNameUp()" })%> <span id="FirstName_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 0px;' rowspan='5'>
                                    <div id='photo_content'>
                                        <table>
                                            <tr>
                                                <td align="right"><span class="ui-icon ui-icon-circle-close" style="cursor: pointer; visibility: hidden;" onclick="javascript:RemoveUserPhoto();"></span></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 90px; height: 100px; border: 1px solid #333; text-align: center; padding: 10px 5px; cursor: pointer;" onclick="javascript:ActivatePhotoUpload();"><%:ViewResources.SharedStrings.UsersUploadFoto%></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_lastname'><%:ViewResources.SharedStrings.UsersLastName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("LastName", Model.FoxSecUser.LastName, new { @style = "width:80%;text-transform: capitalize;", @OnKeyUp = "LastNameUp()" })%> <span id="LastName_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_username'><%:ViewResources.SharedStrings.UsersUserName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("LoginName", Model.FoxSecUser.LoginName, new { @style="width:80%;text-transform: capitalize;" })%> <span id="UserName_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_password'><%:ViewResources.SharedStrings.UsersPassword %></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.TextBox("Password", Model.FoxSecUser.Password, new { @style = "width:80%",onchange = "PassChange()",title="6-20 characters.At least one number. At lease one capital letter and one small letter."})%> <span id="Password_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='PersonalCode'><%:ViewResources.SharedStrings.UsersUserId%></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.TextBox("PersonalId", Model.FoxSecUser.PersonalId, new { @style = "width:80%" })%>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_email'><%:ViewResources.SharedStrings.UsersEmail %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("Email", Model.FoxSecUser.Email, new { @style = "width:80%" })%> <span id="Email_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 2px;' rowspan='3'>
                                    <div id='upload_photo'></div>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_personalcode'><%:ViewResources.SharedStrings.UsersPersonalCode %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("PersonalCode", Model.FoxSecUser.PersonalCode, new { @style = "width:80%" })%> <span id="PersonalCode_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_externalpersonalcode'><%:ViewResources.SharedStrings.UsersExtPersonalCode %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("ExternalPersonalCode", Model.FoxSecUser.ExternalPersonalCode, new { @style = "width:80%" })%> <span id="ExternalPersonalCode_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_birthday'><%:ViewResources.SharedStrings.UsersBirthday %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("BirthDayStr", Model.FoxSecUser.BirthDayStr, new { @style = "width:90px" })%>&nbsp;(dd.mm.yyyy)
    <span id="BirthDayStr_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_pin1'>PIN 1:</label></td>
                                <td colspan="4" style='width: 40%; padding: 2px;'>
                                    <%=Html.TextBox("PIN1", Model.FoxSecUser.PIN1, new { @style = "width:30%" })%><span id="PIN1_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                    <label for='edit_user_pin1'>PIN 2:</label>&nbsp;
		<%=Html.TextBox("PIN2", Model.FoxSecUser.PIN2, new { @style = "width:30%" })%>
                                    <span id="PIN2_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_language'><%:ViewResources.SharedStrings.UsersLanguage %> </label>
                                </td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.DropDownList("LanguageId",
			new SelectList(Model.LanguageItems, "Value", "Text", Model.FoxSecUser.LanguageId), ViewResources.SharedStrings.DefaultDropDownValue,
			new { @style = "width:219px" })%>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                        </table>
                    </form>
                    <br />
                    <input type='button' id='button_save_edit_user' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='javascript: SavePersonalData();' />
                    <input type='button' id='button_generate_password' value='<%=ViewResources.SharedStrings.BtnGeneratePassword %>' onclick='SubmitGenerateUserPassword();' />
                    <input type='button' id='button_generate_pin' value='<%=ViewResources.SharedStrings.UsersBtnGeneratePin %>' onclick='SubmitGenerateUserPin();' />
                </div>

                <%--Create User - User Roles Tab--%>

                <div id='tab_create_user_roles'>
                    <form id="editUserRoles" action="">
                        <input type="hidden" name="userid" value="0" />
                        <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px">
                            <tr>
                                <th style='width: 20px; padding: 2px;'><%:ViewResources.SharedStrings.CommonId %></th>
                                <th style='width: 20%; padding: 2px;'><%:ViewResources.SharedStrings.UsersRoleTitle %></th>
                                <th style='width: 60%; padding: 2px; text-align: center;'><%:ViewResources.SharedStrings.CommonValidationPeriod %></th>
                                <th style='width: 20%; padding: 2px; text-align: right;'><%:ViewResources.SharedStrings.CommonIsAllowed %></th>
                            </tr>

                            <% var i = 1; foreach (var role in Model.FoxSecUser.RoleItems)
                                { %>
                            <tr>
                                <td style='width: 20px; padding: 2px;'><%= Html.Encode(i++) %></td>
                                <td style='width: 20%; padding: 2px; cursor: help;' class='tipsy_we' original-title='<%= Html.Encode(role.Text.Split('#')[1]) %>'><%= Html.Encode(role.Text.Split('#')[0]) %></td>
                                <td style='width: 60%; padding: 2px; text-align: center;'>
                                    <input id='from_<%= i %>' type='text' class="date_start" style="width: 90px" name='from' value='' />
                                    -
                                    <input id='to_<%= i %>' type='text' class="date_end" style="width: 90px" name='to' value='' /></td>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <input type="hidden" name="id" value="<%= role.Value %>" /><input type='checkbox' name='selectedId' value='<%=role.Value%>' <% if (role.Selected)
                                                                                                   {%>
                                        checked='checked' <% } %> /></td>
                            </tr>
                            <% } %>
                        </table>
                    </form>
                    <br />
                    <input type='button' id='button1' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='$.post("/User/EditRoles", $("#editUserRoles").serialize()); ShowDialog("Saved", 2000, true);' />
                </div>

                <%--Create User - User Contact Tab--%>

                <div id='tab_create_user_contact'>
                    <form id="editUserContact" action="">
                        <%=Html.Hidden("Id", 0) %>
                        <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px">
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_residence'><%:ViewResources.SharedStrings.UsersResidence %></label></td>
                                <td style='width: 80%; padding: 2px;'><%=Html.TextArea("Residence", Model.FoxSecUser.Residence, new { @style = "width:80%" })%></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_phone'><%:ViewResources.SharedStrings.UsersPhone %></label></td>
                                <td style='width: 80%; padding: 2px;'><%=Html.TextBox("PhoneNumber", Model.FoxSecUser.PhoneNumber, new { @style = "width:80%" })%></td>
                            </tr>
                        </table>
                    </form>
                    <br />
                    <input type='button' id='button2' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='$.post("/User/EditContactData", $("#editUserContact").serialize()); ShowDialog("Saved", 2000, true);' />
                </div>

                <%--Create User - User Work Tab--%>

                <div id='tab_ceate_user_work'>
                    <form id="editUserWork" action="">
                        <%=Html.Hidden("Id", 0) %>
                        <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px">
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_building'>Building</label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <select>
                                        <option value="Helmes">Building</option>
                                        <option value="empty"></option>
                                    </select>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_floor'>Floor</label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <select>
                                        <option value="1">1</option>
                                        <option value="2">2</option>
                                        <option value="3">3</option>
                                        <option value="4">4</option>
                                        <option value="5">5</option>
                                    </select>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_department'>Department</label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <select>
                                        <option value="Helmes">Department</option>
                                        <option value="empty"></option>
                                    </select></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <!-- develop it on the second phase
<tr>
	<td style='width:20%; padding:2px; text-align:right;'><label for='edit_user_directmanager'><%:ViewResources.SharedStrings.UsersDirectManager %></label></td>
	<td style='width:40%; padding:2px;'>
    <select>
        <option value="empty"><%:ViewResources.SharedStrings.DefaultDropDownValue %></option>
        <option value="Helmes"><%:ViewResources.SharedStrings.UsersDirectManager %></option>
    </select></td>
    <td style='width:40%; padding:2px;'></td>
</tr>
-->
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_title'><%:ViewResources.SharedStrings.UsersTitle %></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%= Html.DropDownList("TitleId", new SelectList(Model.Titles, "Value", "Text", Model.FoxSecUser.TitleId), ViewResources.SharedStrings.DefaultDropDownValue )%>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_contractnr'><%:ViewResources.SharedStrings.UsersContractNr %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("ContractNum", Model.FoxSecUser.ContractNum, new { @style = "width:80%" })%></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.TextBox("ContractStartDateStr", Model.FoxSecUser.ContractStartDateStr, new { @style = "width:90px", @class = "date_start" })%> - <%=Html.TextBox("ContractEndDateStr", Model.FoxSecUser.ContractEndDateStr, new { @style = "width:90px", @class = "date_end" })%>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_permitofwork'><%:ViewResources.SharedStrings.UsersPermitOfWork %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("PermitOfWorkStr", Model.FoxSecUser.PermitOfWorkStr, new { @style = "width:90px", @class = "select_date" })%></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_worktime'><%:ViewResources.SharedStrings.UsersWorkTime %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("WorkTime", false)%></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_building'><%:ViewResources.SharedStrings.UsersESeriviceAllowed %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("EServiceAllowed", true)%></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_tablenr'><%:ViewResources.SharedStrings.UsersTableNr %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("TableNumber", Model.FoxSecUser.TableNumber, new { @style = "width:90px" })%></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                        </table>
                    </form>
                    <br />
                    <input type='button' id='button3' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='$.post("/User/EditWorkData", $("#editUserWork").serialize()); ShowDialog("Saved", 2000, true);' />
                </div>

                <%--Create User - User Other Tab--%>

                <div id='tab_create_user_other'>
                    <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px">
                        <tr>
                            <td style='width: 20%; padding: 2px; text-align: right;'>
                                <label for='edit_user_coffeecups'><%:ViewResources.SharedStrings.UsersCoffeCups %></label></td>
                            <td style='width: 40%; padding: 2px;'><%=Html.TextBox("Coffe cups", "", new { @style = "width:80%", @readonly = "readonly" })%></td>
                            <td style='width: 40%; padding: 2px;'></td>
                        </tr>
                        <tr>
                            <td style='width: 20%; padding: 2px; text-align: right;'>
                                <label for='edit_user_worktime'><%:ViewResources.SharedStrings.UsersPermisionCallQuests %></label></td>
                            <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("PermissionCallGuests", Model.FoxSecUser.PermissionCallGuests)%></td>
                            <td style='width: 40%; padding: 2px;'></td>
                        </tr>
                    </table>
                    <br />
                    <input type='button' id='button4' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='$.post("/User/CreatePersonalData", $("#editUserPersonalData").serialize()); ShowDialog("Saved", 2000, true);' />
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript" lang="javascript">
    $(function () {
        $(document).tooltip();
    });
    $(document).ready(function () {
        $("div#upload_photo").hide();

        passChecked = false;

        $("#panel_create_user").attr("id", function () {
            $("#panel_create_user").tabs({
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

        $("#panel_create_user").tabs("option", "disabled", [1, 2, 3]);

        $("#create_user_personal_data").attr("id", function () {
            $("#create_user_personal_data").tabs({
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

        $("#create_user_personal_data").tabs("option", "disabled", [1, 2, 3, 4]);
        $("input:button").button();

        $(".select_date").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            gotoCurrent: true,
            onSelect: function (dateText, inst) {
                $(".select_date").datepicker("option", "minDate", dateText);
            }
        });

        $(".date_start").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            gotoCurrent: true,
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
            gotoCurrent: true,
            minDate: $(".date_end").val()
        });

        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });

        $("div#create_dialog_content").fadeIn("300");
        return false;
    });

    function SubmitGenerateUserPassword() {
        $.getJSON('/User/GeneratePassword', function (data) {
            $("input#Password").val(data.passw);
        });
        return false;
    }

    function SubmitGenerateUserPin() {
        $.getJSON('/User/GeneratePin', function (data) {
            $("input#PIN1").val(data.pin1);
            $("input#PIN2").val(data.pin2);
        });
        return false;
    }

    function ActivatePhotoUpload() {
        $.get('/User/UploadImage',
            { userId: -1 },
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
        var re4digit = /^\d{4}$/;
        if (pin.search(re4digit) == -1) {
            return false;
        }
        return true;
    }

    function PassChange() {
        passChecked = false;
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

        if ($("input#FirstName").val() == "" || ($("input#FirstName").val() != "" && !$("input#FirstName").val().match(/^.{1,20}$/))) { $("#FirstName_val").text('!').show().fadeOut(3000); isValid = false; }
        if ($("input#LastName").val() == "" || ($("input#LastName").val() != "" && !$("input#LastName").val().match(/^.{1,20}$/))) { $("#LastName_val").text('!').show().fadeOut(3000); isValid = false; }
        if ($("input#LoginName").val() == "") { $("#UserName_val").text('!').show().fadeOut(3000); isValid = false; }
        if (passChecked == false && ValidatePassword($("input#Password")) == false) {
            $("#Password_val").text('!').show().fadeOut(3000);
            isValid = false;
            ShowDialog('<%=ViewResources.SharedStrings.UsersPasswordErrorMessage %>', 4000);
        }
        if ($("input#Email").val() != "" && !$("input#Email").val().match(/[\w-]+@([\w-]+\.)+[\w-]+/)) { $("#Email_val").text('!').show().fadeOut(3000); isValid = false; }
        if ($("input#PersonalCode").val())
            if ($("input#ExternalPersonalCode").val())
                if ($("input#BirthDayStr").val() != "" && (!$("input#BirthDayStr").val().match(/^(0[1-9]|[12][0-9]|3[01])[.](0[1-9]|1[012])[.](19|20)\d\d$/))) { $("#BirthDayStr_val").text('!').show().fadeOut(3000); isValid = false; }

        if (IsValidPin($("input#PIN1").val()) == false) {
            $("#PIN1_val").text('!').show().fadeOut(3000);
            isValid = false;
            ShowDialog('<%=ViewResources.SharedStrings.UsersIncorrectPinErrorMessage %>', 2000);
        }
        if (IsValidPin($("input#PIN2").val()) == false) {
            $("#PIN2_val").text('!').show().fadeOut(3000);
            isValid = false;
            ShowDialog('<%=ViewResources.SharedStrings.UsersIncorrectPinErrorMessage %>', 2000);
        }

        if (!isValid) {
            ShowDialog('<%=ViewResources.SharedStrings.UsersValidator %>', 5000);
            return false;
        }
        $('input#button_save_edit_user').attr("disabled", true);
        $('input#button_save_edit_user').addClass('ui-state-disabled');

        $.getJSON('/User/CheckLoginName', { login: $("input#LoginName").val() },
            function (data) {
                if (data.isInUse) {
                    ShowDialog('<%=ViewResources.SharedStrings.UsersMessageLoginIsInUse %>', 5000);
                    $('input#button_save_edit_user').removeAttr("disabled");
                    $('input#button_save_edit_user').removeClass('ui-state-disabled');
                    return false;
                }
                else {
                    $.post("/User/CreatePersonalData", $("#editUserPersonalData").serialize(),
                        function (response) {
                            if (response.IsSucceed == false) {
                                ShowDialog('<%=ViewResources.SharedStrings.MaxUserCountLicence %> ' + response.Id, 5000);
                                $('input#button_save_edit_user').removeAttr("disabled");
                                $('input#button_save_edit_user').removeClass('ui-state-disabled');
                            }
                            else {
                                newUserCreated = true;
                                ShowDialog('<%=ViewResources.SharedStrings.UsersMessageUserCreated %>', 2000, true);
                                //$("div#userDepartmentList").html(html);
                                $("div#modal-dialog").dialog("close");

                                var usrnm = $("input#FirstName").val() + " " + $("input#LastName").val();
                                EditUser(response.Id, usrnm);
                                //$('#spanusername').html($("input#FirstName").val() + " " + $("input#LastName").val());
                                //newDialogTitle = "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + $("input#FirstName").val() + ' ' + $("input#LastName").val();
                                //$('#ui-dialog-title-modal-dialog').html(newDialogTitle);
                                //$.ajax({
                                //    type: 'GET',
                                //    url: '/User/Edit',
                                //    cache: false,
                                //    data: {
                                //        id: response.Id
                                //    },
                                //    success: function (html) {
                                //        $("div#modal-dialog").html(html);
                                //    }
                                //});
                                //$("input:button").button();
                            }

                        }, "json");
                }
            });
        return false;
    }


    function EditUser(UserId, Username) {
        debugger;
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: 'GET',
                    url: '/User/Edit',
                    cache: false,
                    data: {
                        id: UserId
                    },
                    success: function (html) {
                        $("div#modal-dialog").html(html);
                    }
                });
                $(this).parents('.ui-dialog-buttonpane button:eq(2)').focus();
            },
            resizable: false,
            width: 1000,
            height: 680,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + Username,
            buttons: {
                '<%= ViewResources.SharedStrings.BtnExport %> PDF': function () {
                    PrintUserDataPDF(UserId);
                },
                '<%= ViewResources.SharedStrings.BtnExport %> XLS': function () {
                    PrintUserDataXLS(UserId);
                },
                '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
                    UpdateUserRow(UserId);
                }
            }
        });
        return false;
    }

    function RemoveUserPhoto() {
        $("div#delete-modal-dialog").dialog({
            open: function () {
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
                        data: { id: 0 },
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

    function FirstNameUp() {
        var f = $("input#LoginName").val();
        if (f == "") { $("input#LoginName").val($("input#FirstName").val() + "."); }
        else
            if (f.indexOf(".") != -1) {
                var s = f.split(".");
                s[0] = $("input#FirstName").val();
                $("input#LoginName").val(s[0] + "." + s[1]);
            }
        return false;
    }

    function LastNameUp() {
        var f = $("input#LoginName").val();
        if (f == "") { $("input#LoginName").val($("input#LastName").val() + "."); }
        else
            if (f.indexOf(".") != -1) {
                var s = f.split(".");
                s[1] = $("input#LastName").val();
                $("input#LoginName").val(s[0] + "." + s[1]);
            }
        return false;
    }

</script>
