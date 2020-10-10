<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.ClassificatorListViewModel>" %>
<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <tr>
        <th style='width: 40%; padding: 2px;'><%:ViewResources.SharedStrings.UsersName %></th>
        <th style='width: 50%; padding: 2px;'><%:ViewResources.SharedStrings.CommonDescription %></th>
        <th style='width: 5%; padding: 2px;'></th>
        <th style='width: 5%; padding: 2px;'></th>
    </tr>

    <% var i = 1; foreach (var calssificator in Model.Classifiers)
        {
            var bg = (i++ % 2 == 1) ? " background-color:#CCC;" : ""; %>
    <tr>
        <td style='width: 30%; padding: 2px; <%= bg %>'><%= Html.Encode(calssificator.Description != null && calssificator.Description.Length > 50 ? calssificator.Description.Substring(0, 47) + "..." : calssificator.Description)%> </td>
        <td style='width: 60%; padding: 2px; <%= bg %>'><%= Html.Encode(calssificator.Comments != null && calssificator.Comments.Length > 50 ? calssificator.Comments.Substring(0, 47) + "..." : calssificator.Comments)%></td>
        <td style='width: 5%; padding: 2px; text-align: right; <%= bg %>'><span id='button_classificator_edit_<%= calssificator.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(calssificator.Description)) %>' onclick='<%=string.Format("javascript:EditClassificator({0}, \"{1}\")", calssificator.Id, Html.Encode(calssificator.Description)) %>'></span></td>
        <td style='width: 5%; padding: 2px; text-align: right; <%= bg %>'>
            <%if (calssificator.Description == "Licence" || calssificator.Description == "License") { }
                else
                {%>
            <span id='button_classificator_delete_<%= calssificator.Id %>' class="ui-icon ui-icon-closethick tipsy_we" style="cursor: pointer" original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnDelete, Html.Encode(calssificator.Description)) %>' onclick='<%=string.Format("javascript:DeleteClassificator({0}, \"{1}\")", calssificator.Id, Html.Encode(calssificator.Description)) %>'></span>
            <%}%>
        </td>
    </tr>
    <% } %>
</table>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    });


    function DeleteClassificator(calssificatorId, calssificatorTitle) {
        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessageClassifier %>');
            },
            resizable: false,
            width: 400,
            height: 160,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CommonDeleting %>' + ' ' + calssificatorTitle,
            buttons: {
				'<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $.post("/Classifiers/Delete", { id: calssificatorId }, function (html) {
                        if (html == "False") {
                            ShowDialog("<%=ViewResources.SharedStrings.ClassifiersDelClassErrorMessage %>", 2000);
                        } else {
                            ShowDialog('<%=ViewResources.SharedStrings.ClassifiersDeleting %>' + calssificatorTitle + '...', 2000, true);
                            $.get('/Classifiers/List', function (html) {
                                $("div#ClassifiersList").html(html);
                            });
                        }
                    });

                    setTimeout(function () {

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

    function EditClassificator(calssificatorId, calssificatorTitle) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Classifiers/Edit', { id: calssificatorId }, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 800,
            height: 600,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + calssificatorTitle,
            buttons: {
              <%--  '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    dlg = $(this);
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
                                dlg.dialog("close");
                                setTimeout(function () {
                                    $.get('/Classifiers/List', function (html) {
                                        $("div#ClassifiersList").html(html);
                                    });
                                }, 1000);
                            }
                        }
                    });
                },--%>
                '<%=ViewResources.SharedStrings.ClassifiersBackToList %>': function () {
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }

</script>
