<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.ClassificatorEditViewModel>" %>
<div id="content_classificator_form" style='margin: 10px; text-align: center;'>
    <form id="editClassificator" action="">
        <table width="100%">
            <%=Html.Hidden("Classificator.Id", Model.Classificator.Id)%>
            <tr>
                <td style='width: 30%; padding: 0 5px; text-align: right;'>
                    <label for='Edit_classificator_description'><%:ViewResources.SharedStrings.UsersName %></label></td>
                <td style='width: 70%; padding: 0 5px;'>
                    <%=Html.TextBox("Classificator.Description", Model.Classificator.Description, new { @style = "width:80%;text-transform: capitalize;", id = "Edit_classificator_description" })%>
                    <%= Html.ValidationMessage("Classificator.Description", null, new { @class = "error" })%>
                </td>
            </tr>
            <tr>
                <td style='width: 30%; padding: 0 5px; text-align: right; vertical-align: top;'>
                    <label for='Edit_classificator_comments'><%:ViewResources.SharedStrings.CommonComments %></label></td>
                <td style='width: 70%; padding: 0 5px;'>
                    <%=Html.TextArea("Classificator.Comments", Model.Classificator.Comments, new { @style = "width:80%;text-transform: capitalize;", id = "Edit_classificator_comments" })%>
                    <%= Html.ValidationMessage("Classificator.Comments", null, new { @class = "error" })%>
                </td>
            </tr>
        </table>

        <div style='margin: 10px 0px 10px 0px;' id="panel_classificator_values" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
            <ul class="ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
                <li class="ui-state-default ui-corner-top ui-tabs-selected ui-state-active"><a href="#tab_calssificator_values_menu"><%:ViewResources.SharedStrings.ClassifiersClassifierValues %></a></li>
            </ul>

            <div id="ClassificatorValuesList"></div>
            <%if (Model.Classificator.Description == "Licence" || Model.Classificator.Description == "License") { }
                else
                {%>
            <table width="100%" id="tableBtnAddCalssificatorValue" cellpadding="3" cellspacing="3" style="margin: 0; padding: 3px; border-spacing: 3px;">
                <tr>
                    <td style='width: 100%; padding: 2px; vertical-align: top; text-align: right; padding: 3px;'>
                        <input style='font-size: 13px;' type='button' id='update_classificator_value' class="ui-button ui-widget ui-state-default ui-corner-all" value='<%=ViewResources.SharedStrings.BtnSave %>' onclick="javascript: SaveClassificatorValue();" />
                        <input style='font-size: 13px;' type='button' id='add_new_classificator_value' class="ui-button ui-widget ui-state-default ui-corner-all" value='<%=ViewResources.SharedStrings.ClassifiersAddNewClassifierValue %>' onclick="javascript: AddClassificatorValue('<%= Model.Classificator.Id %>');" /></td>
                </tr>
            </table>
            <%} %>

            <%if (Model.Classificator.Description == "Licence" || Model.Classificator.Description == "License")
                { %>
            <table width="100%" id="tableBtnInsertLicence" cellpadding="3" cellspacing="3" style="margin: 0; padding: 3px; border-spacing: 3px;">
                <tr>
                    <td style='width: 100%; padding: 2px; vertical-align: top; text-align: right; padding: 3px;'>
                        <%-- <input style='font-size: 13px;' type='button' id='insert_new_licence' class="ui-button ui-widget ui-state-default ui-corner-all" value='Insert new licence from disk' onclick="javascript: insertnewlicence('<%= Model.Classificator.Id %>');" />--%>
                        <div id='upload_content'>
                            <%--<div style="width: 90px; height: 100px; border: 1px solid #333; text-align: center; padding: 10px 5px; cursor: pointer;" onclick="javascript:ActivatePhotoUpload();"><%:ViewResources.SharedStrings.UsersUploadFoto%></div>
                            <% using (Html.BeginForm("InsertNewLicense", "Classifiers", new { id = Model.Classificator.Id }, FormMethod.Post, new { enctype = "multipart/form-data", target = "upload_target", onsubmit = "StartUpload();" }))
                                {%>
                            <input type="file" name="fileUpload" style='font-size: 8pt' />
                            <input style='font-size: 13px;' type='button' id='insert_new_licence' class="ui-button ui-widget ui-state-default ui-corner-all" value='Insert new licence from disk' onclick="javascript: insertnewlicence('<%= Model.Classificator.Id %>');" />
                            <% } %>--%>
                            <div id='upload_photo' style="display: none"></div>
                            <input style='font-size: 13px;' type='button' id='insert_new_licence' class="ui-button ui-widget ui-state-default ui-corner-all" value='<%=ViewResources.SharedStrings.btnInsertLicence %>' onclick="javascript: ActivatePhotoUpload()" />
                            <%--<div style="border: 1px solid black; text-align: center; padding: 5px 3px; cursor: pointer;" onclick="javascript:ActivatePhotoUpload();">Insert new licence from disk</div>--%>
                        </div>
                    </td>
                </tr>
            </table>
            <%} %>
        </div>
    </form>

    <script type="text/javascript" language="javascript">

        function ActivatePhotoUpload() {
            var txtval = $('#txtlicencepath').val();
            $('#txtlicencepath').attr("disabled","disabled");
            $.get('/Classifiers/UploadImage',
                { userId: "<%= Model.Classificator.Id %>", licencepath:txtval },
                function (html) {
                    $("div#upload_photo").html(html);
                    $("div#upload_photo").toggle("slow");
                });
            return false;
        }


        function AddClassificatorValue(classificatorId) {
        
            $("div#user-modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $.get('/Classifiers/CreateValue', { id: classificatorId }, function (html) {
                        $("div#user-modal-dialog").html(html);
                    });
                },
                resizable: false,
                width: 640,
                height: 280,
                modal: true,
                title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>",
                buttons: {
            '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                        dlg = $(this);
                        $.ajax({
                            type: "Post",
                            url: "/Classifiers/CreateValue",
                            dataType: 'json',
                            traditional: true,
                            data: $("#createNewClassificatorValue").serialize(),
                            success: function (data) {
                                if (data.IsSucceed == false) {
                                    $("div#user-modal-dialog").html(data.viewData);
                                    if (data.DisplayMessage == true) {
                                        ShowDialog(data.Msg, 2000);
                                    }
                                }
                                else {
                                    ShowDialog(data.Msg, 2000, true);
                                    setTimeout(function () {
                                        $.ajax({
                                            type: 'GET',
                                            url: '/Classifiers/ListValues',
                                            cache: false,
                                            data: {
                                                id: <%:Model.Classificator.Id%>
      										},
                                            success: function (html) {
                                                $("div#ClassificatorValuesList").html(html);
                                            }
                                        });
                                    }, 1000);
                                    dlg.dialog("close");
                                }
                            }
                        });
                    },
            '<%=ViewResources.SharedStrings.ClassifiersBackToClassifier %>': function () {
                        $(this).dialog("close");
                    }
                }
            });

            return false;
        }

        $(document).ready(function () {

            $.ajax({
                type: 'GET',
                url: '/Classifiers/ListValues',
                cache: false,
                data: {
                    id: <%:Model.Classificator.Id%>
                  },
                success: function (html) {
                    $("div#ClassificatorValuesList").html(html);
                }
            });
            $("input:button").button();
        });

        function SaveClassificatorValue() {
            $.ajax({
                type: "Post",
                url: "/Classifiers/Edit",
                dataType: 'json',
                traditional: true,
                data: $("#editClassificator").serialize(),
                success: function (data) {
                    if (data.IsSucceed == false) {
                        $("div#modal-dialog").html(data.viewData);
                        if (data.DisplayMessage == true) {
                            ShowDialog(data.Msg, 2000);
                        }
                    }
                    else {
                        ShowDialog(data.Msg, 2000, true);
                        $("div#modal-dialog").html("");
                        $("div#modal-dialog").dialog("close");
                        setTimeout(function () {
                            $.get('/Classifiers/List', function (html) {
                                $("div#ClassifiersList").html(html);
                            });
                        }, 1000);
                    }
                }
            });
        }

        function insertnewlicence(id) {
            alert("hello");
            $.ajax({
                type: 'POST',
                url: '/Classifiers/InsertNewLicense',
                cache: false,
                data: {
                    id: id
                },
                beforeSend: function () {
                    $("#insert_new_licence").fadeOut('fast');
                    //  $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
                },
                success: function () {

                    $.ajax({
                        type: 'GET',
                        url: '/Classifiers/ListValues',
                        cache: false,
                        data: {
                            id: <%:Model.Classificator.Id%>
                  },
                        success: function (html) {
                            $("div#ClassificatorValuesList").html(html);
                            $("#insert_new_licence").fadeIn('fast');
                        }
                    });
                }
            });


        }
    </script>

</div>
