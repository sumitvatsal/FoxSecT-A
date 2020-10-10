<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.ClassificatorValueListViewModel>" %>
<%if (Model.ClassificatorValues.Count() > 0)
    { %>
<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <tr>
        <%if (ViewBag.ClassifierName == "Licence" || ViewBag.ClassifierName == "License") %>
        <%   { %>
        <th style='width: 30%; padding: 2px;'><%:ViewResources.SharedStrings.CommonValue %></th>
        <th style='width: 20%; padding: 2px;'>Legal</th>
        <th style='width: 20%; padding: 2px;'>Remaining</th>
        <th style='width: 20%; padding: 2px;'><%:ViewResources.SharedStrings.CardsValidTo %></th>
        <%} %>
        <%else%>
        <%{ %>
        <th style='width: 50%; padding: 2px;'><%:ViewResources.SharedStrings.CommonValue %></th>
        <th style='width: 5%; padding: 2px;'></th>
        <th style='width: 5%; padding: 2px;'></th>
        <%} %>
    </tr>

    <% var i = 1; foreach (var cv in Model.ClassificatorValues)
        {
            var bg = (i++ % 2 == 1) ? " background-color:#CCC;" : ""; %>
    <tr>
        <%if (ViewBag.ClassifierName == "Licence" || ViewBag.ClassifierName == "License") %>
        <% { %>

        <%if (cv.Value == "Licence Path" || cv.Value == "Licence Path") %>
        <% { %>
        <td style='width: 30%; padding: 2px; <%= bg %>'><%= Html.Encode(cv.Value)%></td>
        <td style='width: 20%; padding: 2px; <%= bg %>' colspan="3">
            <%=Html.TextBox("cv.Comments", Html.Encode(cv.Comments), new {@style = "width:95%" ,id = "txtlicencepath", disabled = "true"})%>
        </td>
        <%} %>
        <%else%>
        <%{ %>
        <td style='width: 30%; padding: 2px; <%= bg %>'><%= Html.Encode(cv.Value)%></td>
        <td style='width: 20%; padding: 2px; <%= bg %>'><%= Html.Encode(cv.Legal)%></td>
        <td style='width: 20%; padding: 2px; <%= bg %>'><%= Html.Encode(cv.Remaining)%></td>
        <td style='width: 20%; padding: 2px; <%= bg %>'><%= Html.Encode(cv.ToDateTime)%></td>
        <%} %>
        <%} %>
        <%else%>
        <%{ %>
        <td style='width: 50%; padding: 2px; <%= bg %>'><%= Html.Encode(cv.Value)%></td>
        <td style='width: 5%; padding: 2px; text-align: right; <%= bg %>'><span id='button_classificator_value_edit_<%= cv.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(cv.Value)) %>' onclick='<%=string.Format("javascript:EditClassificatorValue({0}, {1}, \"{2}\")", cv.Id, cv.ClassificatorId, Html.Encode(cv.Value)) %>'></span></td>
        <td style='width: 5%; padding: 2px; text-align: right; <%= bg %>'><span id='button_classificator_value_delete_<%= cv.Id %>' class="ui-icon ui-icon-closethick tipsy_we" style="cursor: pointer" original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnDelete, Html.Encode(cv.Value)) %>' onclick='<%=string.Format("javascript:DeleteClassificatorValue({0}, {1}, \"{2}\")", cv.Id, cv.ClassificatorId, Html.Encode(cv.Value)) %>'></span></td>
        <%} %>
    </tr>
    <% } %>
</table>
<% } %>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    });

    function DeleteClassificatorValue(valueId, classificatorId, value) {
        $("div#delete-modal-dialog").dialog({
            open: function (event, ui) {
                $("div#delete-modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessageClassifierValue %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CommonDeleting %>' + " " + value,
            buttons: {
           	'<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $.post("/Classifiers/DeleteValue", { id: valueId, classifierId: classificatorId }, null);
                    ShowDialog('<%=ViewResources.SharedStrings.ClassifiersClassificatorValueDeleting %>' + value + "...", 2000, true);

                    setTimeout(function () {
                        $.ajax({
                            type: 'GET',
                            url: '/Classifiers/ListValues',
                            cache: false,
                            data: {
                                id: classificatorId
                            },
                            success: function (html) {
                                $("div#ClassificatorValuesList").html(html);
                            }
                        });
                    }, 1000);

                    $(this).dialog("close");
                },
              '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }
    function EditClassificatorValue(valueId, classificatorId, value) {
     
        $("div#user-modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $.get('/Classifiers/EditValue', { id: valueId }, function (html) {
                    $("div#user-modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 640,
            height: 300,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + value,
            buttons: {
                   '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    dlg = $(this);
                    $.ajax({
                        type: "Post",
                        url: "/Classifiers/EditValue",
                        dataType: 'json',
                        traditional: true,
                        data: $("#editClassificatorValue").serialize(),
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
                                            id: classificatorId
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
                   '<%=ViewResources.SharedStrings.ClassifiersBackToClassificatorValues %>': function () {
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }
</script>

