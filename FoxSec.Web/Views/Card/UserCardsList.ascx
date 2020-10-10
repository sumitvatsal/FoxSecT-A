<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserAccessUnitListViewModel>" %>
<table id="userCardsTable" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
    <tr>
        <th style='width: 5%; padding: 2px;'></th>
        <th style='width: 20%; padding: 2px;'><%:ViewResources.SharedStrings.CardsCardType %></th>
        <th style='width: 20%; padding: 2px;'><%:ViewResources.SharedStrings.CardsCardCode %></th>
        <th style='width: 20%; padding: 2px;'><%:ViewResources.SharedStrings.UsersValidation %></th>
        <th style='width: 10%; padding: 2px;'><%:ViewResources.SharedStrings.CommomStatus %></th>
        <th style='width: 10%; padding: 2px;'></th>
    </tr>
    <% foreach (var card in Model.Cards)
        { %>
    <tr>
        <td style='width: 5%; padding: 2px;'>
            <% if (card.Active)
                {%>
            <input type='checkbox' id='<%= card.Id %>' name='user_card_checkbox' onclick='javascript: ManageUserCardButtons(<%= Model.FilterCriteria %>);' />
            <%} %>
        </td>
        <td style='width: 20%; padding: 2px;'>
            <%= Html.Encode(card.TypeName != null && card.TypeName.Length > 30 ? card.TypeName.Substring(0, 27) + "..." : card.TypeName)%>
        </td>
        <td style='width: 20%; padding: 2px;'>
            <%= Html.Encode(card.FullCardCode) %>
        </td>
        <td style='width: 20%; padding: 2px;'>
            <%= Html.Encode(card.ValidFromStr) %> - <%= Html.Encode(card.ValidToStr) %>
        </td>
        <td style='width: 10%; padding: 2px;'>
            <%= Html.Encode(card.CardStatus) %>
        </td>
        <td style='width: 10%; padding: 2px;'>
            <span id='button_card_edit_<%= card.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, card.Name) %>' onclick="javascript:EditCard(<%= card.Id %>);"></span>
        </td>
    </tr>
    <% } %>
</table>
<script type="text/javascript" language="javascript">
    function EditCard(cardId) {
        $("div#modal-dialog-2").dialog({
            open: function () {
                // $("div#modal-dialog").html("");
                // $("div#user-modal-dialog").html("");
                $.get('/Card/Edit', { id: cardId }, function (html) {
                    $("div#modal-dialog-2").html(html);
                });
            },
            resizable: false,
            width: 500,
            height: 500,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CardsEditCard %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    var sr = $('#Serial').val();
                    var dk = $('#Dk').val();
                    var code = $('#Code').val();
                    if ((sr != "" || dk != "") && code != "") {
                        ShowDialog('Serial and DK or Code one should be entered!!', 4000);
                        return false;
                    }

                    if (code == "" && sr.length < 3) {
                        ShowDialog('Serial should contain 3 digit!!', 4000);
                        return false;
                    }
                    if (code == "" && dk.length < 5) {
                        ShowDialog('DK should contain 5 digit!!', 4000);
                        return false;
                    }
                    if (sr == "255" && dk == "65535") {
                        $('#Serial').css('border-color', 'darkred');
                        $('#Dk').css('border-color', 'darkred');
                        return false;
                    }

                    $.post("/Card/EditCard", $("#editCard").serialize(), function (html) {
                        if (html == "True") {
                            ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>', 2000, true);
                            $("div#modal-dialog-2").html("");
                            $("div#modal-dialog-2").dialog("close");
                        }
                        else if (html == null) {
                            ShowDialog('Some error occurred!! Please try again later.', 5000);
                        }
                        else if (html.toLowerCase() == "valid to date error") {
                          ShowDialog('<%=ViewResources.SharedStrings.DateErrorMessage %>' , 5000);
                        }    
                        else if (html.toLowerCase() == "valid from date error") {
                            ShowDialog('<%=ViewResources.SharedStrings.DateErrorMessage %>', 5000);
                        }
                        else {
                            ShowDialog('Card already in use!! Owner name: ' + html, 5000);
                        }
                    });
                },
            '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            },
            close: function () {
                $("div#modal-dialog-2").html("");
                setTimeout(null, 1000);
                $.get('/Card/UserCardsList', { id: $("div#edit_dialog_content").find("input#UserId").val(), filter: 1 }, function (html) { $("div#userCardsList").html(html); });
            }
        });
        $.get('/Card/UserCardsList', { id: $("div#edit_dialog_content").find("input#UserId").val(), filter: 1 }, function (html) { $("div#userCardsList").html(html); });
        return false;
    }
</script>
