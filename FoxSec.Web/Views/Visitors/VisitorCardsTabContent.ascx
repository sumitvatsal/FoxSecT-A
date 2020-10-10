<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CardsTabContentViewModel>" %>
<div id='tab_user_card_list'>
    <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;">

        <tr>
            <td colspan="2" style='width: 100%; padding: 2px;'>
                <fieldset>
                    <div id='VisitorCardsList'></div>
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
        $("div#user-modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
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
                    dlg = $(this);
                    $.ajax({
                        type: "Post",
                        url: "/Card/AddNewUSerCard",
                        dataType: 'json',
                        data: $("#addNewUserCard").serialize(),
                        success: function (data) {
                            if (data.IsSucceed) {
                                setTimeout(function () { UserCardFilterChanged(); }, 1000);
                                dlg.dialog("close");
                                ShowDialog("<%=ViewResources.SharedStrings.CommonDataSavedMessage %>", 2000, true);
                            }
                            else {
                                if (data.IsMessageEmpty == false) {
                                    ShowDialog(data.Msg, 2000);
                                }
                                $("div#user-modal-dialog").html(data.viewData);
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
        $.get('/Visitors/VisitorCardsList', { vid: $('#tab_edit_user_personal_data').find('#Id').attr('value'), filter: $("select#userCardFilter").val() }, function (html) { $("div#VisitorCardsList").html(html); });

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
