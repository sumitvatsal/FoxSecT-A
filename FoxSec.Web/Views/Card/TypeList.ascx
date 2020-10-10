<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserAccessUnitTypeListViewModel>" %>
<table width="100%" style="margin:0; width:100%; padding:2px; border-spacing:0;">
<tr>
    <th style='width:20%; padding:2px;'><%:ViewResources.SharedStrings.UsersName %></th>
    <th style='width:40%; padding:2px;'><%:ViewResources.SharedStrings.CommonDescription %></th>
    <th style='width:10%; padding:2px;'><%:ViewResources.SharedStrings.CardsCardCode %></th>
    <th style='width:10%; padding:2px;'><%:ViewResources.SharedStrings.CardsSerDk %></th>
    <th style='width:5%; padding:2px;'></th>
    <th style='width:5%; padding:2px;'></th>
</tr>

<% var i = 1; foreach (var type in Model.CardTypes) { var bg = (i++ % 2 == 1) ? " background-color:#CCC;" : ""; %>
<tr>
    <td style='width:20%; padding:2px;<%= bg %>'><%= Html.Encode(type.Name != null && type.Name.Length > 30 ? type.Name.Substring(0, 27) + "..." : type.Name)%></td>
    <td style='width:40%; padding:2px;<%= bg %>'><%= Html.Encode(type.Description != null && type.Description.Length > 50 ? type.Description.Substring(0, 47) + "..." : type.Description)%></td>
    <td style='width:10%; padding:2px;<%= bg %>'><% if (type.IsCardCode) {%><span class='ui-icon ui-icon-check' style='float:left; margin:1px 5px 0 0'></span><%}%></td>
    <td style='width:10%; padding:2px;<%= bg %>'><% if (type.IsSerDK) {%><span class='ui-icon ui-icon-check' style='float:left; margin:1px 5px 0 0'></span><%}%></td>
    <td style='width:5%; padding:2px; text-align:right;<%= bg %>'><!--<span id='button_card_type_edit_<%= type.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(type.Name)) %>' onclick='<%=string.Format("javascript:EditCardType(\"submit_edit_card_type\", {0}, \"{1}\")", type.Id, Html.Encode(type.Name)) %>' ></span>--></td>
	<td style='width:5%; padding:2px; text-align:right;<%= bg %>'><!--<span id='button_card_type_delete_<%= type.Id %>' class="ui-icon ui-icon-closethick tipsy_we" style="cursor:pointer" original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnDelete, Html.Encode(type.Name)) %>' onclick='<%=string.Format("javascript:DeleteCardType({0}, \"{1}\")", type.Id, Html.Encode(type.Name)) %>'></span>--></td>
</tr>
<% } %>

</table>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    });

    function EditCardType(Action, Id, Title) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Card/EditType', { id: Id }, function(html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 640,
            height: 300,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + Title,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    dlg = $(this);
                    $.ajax({
                        type: "Post",
                        url: "/Card/UpdateType",
                        dataType: 'json',
                        traditional: true,
                        data: $("#editCardType").serialize(),
                        success: function (data) {
                            if (data.IsSucceed == false) {
                                $("div#modal-dialog").html(data.viewData);
                                if (data.DisplayMessage == true) {
                                    ShowDialog(data.Msg, 2000);
                                }
                            }
                            else {
                                ShowDialog(data.Msg, 2000, true);
                                setTimeout(function () { $.get('/Card/TypeList', function (html) { $("div#CardTypesList").html(html); }); }, 1000);
                                dlg.dialog("close");
                            }
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

    function DeleteCardType(Id, Name) {
        $("div#modal-dialog").dialog({
            open: function (event, ui) {
            	$("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CommonDeleting %>' + " "  + Name,
            buttons: {
            	'<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $.post("/Card/DeleteType", { id: Id }, null);
                    ShowDialog("Card Type " + Name + " deleting..", 2000, true);

                    setTimeout(function () { $.get('/Card/TypeList', function (html) { $("div#CardTypesList").html(html); }); }, 1000);
                    $(this).dialog("close");
                },
                   '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }

</script>