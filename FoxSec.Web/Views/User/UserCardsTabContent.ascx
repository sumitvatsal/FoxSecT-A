<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CardsTabContentViewModel>" %>


<div id='tab_user_card_list'>
    <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;">
        <tr>
            <td style='width: 50px; padding: 2px;'>
                <select id='userCardFilter' onchange='javascript:UserCardFilterChanged();'>
                    <option value="1"><%:ViewResources.SharedStrings.FilterActive %></option>
                    <option value="0"><%:ViewResources.SharedStrings.FilterDeactivated %></option>
                </select>
            </td>
            <td style='width: 50%; padding: 2px; text-align: right'>
                <input type='button' id='activateUserCard' value='<%=ViewResources.SharedStrings.CardsActivateCardBtn %>' onclick="javascript: ActivateCard();" style='font-size: 8pt; display: none' />
                <input type='button' id='deactivateUserCard' value='<%=ViewResources.SharedStrings.BtnDeactivate %>' style='font-size: 8pt; display: none' onclick='javascript: DeactivateUserCard($("input#UserId").val());' />
                <%if (Model.CanAddCard)
                    {%>
                <input type='button' id='addNewCard' value='<%=ViewResources.SharedStrings.BtnAddNewCard %>' <% if (Model.CannotAddUsersAndCard.GetValueOrDefault() == true) {%> disabled <%} %> style='font-size: 8pt' onclick='javascript: AddNewUserCard($("input#UserId").val());' />
                <%} %>
            </td>
        </tr>
        <tr>
            <td colspan="2" style='width: 100%; padding: 2px;'>
                <fieldset>
                    <div id='userCardsList'></div>
                </fieldset>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $("input:button").button();
        //$.get('/Card/UserCardsList', { id: $("input#UserId").val(), filter: 1 }, function (html) { $("div#userCardsList").html(html); });
    });

    function AddNewUserCard(id) {
        debugger;
        $("div#user-modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                debugger;
                $.get('/Card/NewUserCard', { userId: $("input#UserId").val() }, function (html) {
                    $("div#user-modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 480,
            height: 320,
            modal: true,
            title: "<%=string.Format("<span class={1}ui-icon ui-icon-pencil{1} style={1}float:left; margin:1px 5px 0 0{1} ></span>{0}",ViewResources.SharedStrings.DialogTitleNewUserCard, "'") %>",
            buttons: {
                '<%=ViewResources.SharedStrings.BtnAdd %>': function () {
                    button = $(this).parent().find("button:contains('<%=ViewResources.SharedStrings.BtnAdd %>')");
                    button.attr("disabled", true);
                    button.addClass('ui-state-disabled');
                   
                    dlg = $(this);
                    $.ajax({
                        type: "Post",
                        url: "/Card/AddNewUSerCard",
                        dataType: 'json',
                        data: $("#addNewUserCard").serialize(),
                        success: function (data) {
                            debugger;
                            var button1 = $(this).parent().find("button:contains('<%=ViewResources.SharedStrings.BtnAdd %>')");
                            if (data.IsSucceed) {
                              setTimeout(function () { UserCardFilterChanged(); }, 1000);
                                dlg.dialog("close");
                                $('#regarding').val($(this).attr('value')).effect('highlight', 3000);
                            }
                            else if (data.Msg == "NobuildingSelected") {
                                 ShowDialog('<%=ViewResources.SharedStrings.CardBuildingMessage %>', 5000, false)
                                
                               return false;
                            }
                            else {
                                if (data.Msg == "licence error") {
                                    ShowDialog('<%=ViewResources.SharedStrings.MaxUserCountLicence %> ' + data.Count, 5000);
                                    button.attr("disabled", false);
                                    button.removeClass('ui-state-disabled');
                                }
                                else if (data.Msg == "serdkerror") {
                                    $('#Serial').css('border-color', 'darkred');
                                    $('#Dk').css('border-color', 'darkred');
                                    button.attr("disabled", false);
                                    button.removeClass('ui-state-disabled');
                                }
                                else if (data.IsMessageEmpty == false) {
                                    ShowDialog(data.Msg, 5000);
                                    $("div#user-modal-dialog").html(data.viewData);
                                    button.attr("disabled", false);
                                    button.removeClass('ui-state-disabled');

                                }
                                else {
                                    ShowDialog(data.Msg, 5000);
                                    button.attr("disabled", false);
                                    button.removeClass('ui-state-disabled');
                                }

                            }
                        }
                    });
                },
                '<%=ViewResources.SharedStrings.BtnBack %>': function () {
                    $(this).dialog("close");
                }
            },
            close: function () {
                $("div#user-modal-dialog").html("");
            }
        });
        return false;
    }

    function ActivateCard() {
        var cardsIds = new Array();
        $('input[name=user_card_checkbox]').each(function () {
            if (this.checked) {
                var cardId = $(this).attr('id');
                cardsIds.push(cardId);
            }
        });
        $("div#user-modal-dialog").dialog({
            open: function () {
                $.get('/Card/Activate', {}, function (html) {
                    $("div#user-modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 300,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CardsActivating %>',
            buttons: {
          '<%=ViewResources.SharedStrings.BtnActivate %>': function () {
                    if ($('#selectedReasonId').val() == "") {
                        ShowDialog("<%=ViewResources.SharedStrings.CommonNoReasonSelectedMessage %>", 2000);
                  return false;
              }
              ShowDialog("<%=ViewResources.SharedStrings.CardsActivatingMessage %>", 2000, true);
                    $.ajax({
                        type: "Post",
                        url: "/Card/ActivateCards",
                        data: { cardIds: cardsIds, reasonId: $('#selectedReasonId').val() },
                        traditional: true,
                        success: function (result) {
                         setTimeout(function () { UserCardFilterChanged(); }, 1000);
                        }
                    });
                    $(this).dialog("close");
                },
          '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_activate_card").removeClass("Trans"); }
            }
        });
        return false;
    }

    function DeactivateUserCard(id) {
        var cardsIds = new Array();
        $('input[name=user_card_checkbox]').each(function () {
            if (this.checked) {
                var cardId = $(this).attr('id');
                cardsIds.push(cardId);
            }
        });

        //$("#button_deactivate_card").addClass("Trans");

        $("div#user-modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $.get('/User/DeactivateUserCards', {}, function (html) {
                    $("div#user-modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 440,
            height: 180,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.UsersCardsDeactivating %>',
            buttons: {
				'<%=ViewResources.SharedStrings.BtnDeactivate %>': function () {
                    if ($('#selectedReasonId').val() == "") {
                        ShowDialog("<%=ViewResources.SharedStrings.CommonNoReasonSelectedMessage %>", 2000);
                        return false;
                    }
                    ShowDialog("<%=ViewResources.SharedStrings.CardsDeactivatingMessage %>", 2000, true);
                    $.ajax({
                        type: "Post",
                        url: "/Card/DeactivateCards",
                        data: { cardIds: cardsIds, reasonId: $('#selectedReasonId').val(), isMoveToFree: $('#IsMoveToFree').is(':checked') },
                        traditional: true,
                        success: function () {
                            setTimeout(function () { UserCardFilterChanged(); }, 1000);
                        }
                    });
                    $(this).dialog("close");
                },
				'<%=ViewResources.SharedStrings.BtnCancel %>': function () { $("div#user-modal-dialog").html(""); $(this).dialog("close"); }
            }
        });
        return false;
    }

    function UserCardFilterChanged() {
        $("input#activateUserCard").fadeOut();
        $("input#deactivateUserCard").fadeOut();
        $.get('/Card/UserCardsList', { id: $("input#UserId").val(), filter: $("select#userCardFilter").val() }, function (html) { $("div#userCardsList").html(html); });
        return false;
    }

    function ManageUserCardButtons(filter) {
        var any = 0;
        $('input[name=user_card_checkbox]').each(function () { if (this.checked) any++; });

        if (any != 0) {
            if (filter == 1) {
                $("input#activateUserCard").fadeOut();
                $("input#deactivateUserCard").fadeIn();
            }
            else {
                $("input#deactivateUserCard").fadeOut();
                $("input#activateUserCard").fadeIn();
            }
        }
        else {
            $("input#activateUserCard").fadeOut();
            $("input#deactivateUserCard").fadeOut();
        }
    }

</script>
