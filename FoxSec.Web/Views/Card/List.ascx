<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserAccessUnitListViewModel>" %>
<%= Html.Hidden("CardsCount", Model.Cards.Count()) %>
<%= Html.Hidden("CardExists", Model.CardExists) %>
<%= Html.Hidden("IsInList", Model.IsInList) %>
<% if (Model.Cards.Count() == 1)
    { %>
<%= Html.Hidden("FoundCardId", Model.Cards.First().Id) %>
<%= Html.Hidden("IsFreeCard", Model.Cards.First().Free) %>
<% } %>
<table id="searchedTableCard" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <thead>
        <tr style="background-color: black">
            <th style='width: 2%; padding: 2px; color: white'>
                <input id='check_all' style='display: <%= (Model.FilterCriteria == 3) ? "none" : "inline" %>;' name='check_all' type='checkbox' class='tipsy_we' title='<%=ViewResources.SharedStrings.CommonSelectAll %>' onclick="javascript: CheckAll();" />
            </th>
            <th style='width: 9%; padding: 2px; color: white'>
                <%=ViewResources.SharedStrings.CardsCardStatus %>
                <input type="checkbox" id="cbx_expCardList_1" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(1,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(1,1);'></span></li>
            </th>
            <th style='width: 10%; padding: 2px; color: white'>
                <%=ViewResources.SharedStrings.CardsCardReason %>
                <input type="checkbox" id="Checkbox1" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(8,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(8,1);'></span></li>
            </th>
            <th style='width: 9%; padding: 2px; color: white'>
                <%=ViewResources.SharedStrings.CardsCardType %>
                <input type="checkbox" id="cbx_expCardList_7" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(7,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(7,1);'></span></li>
            </th>
            <th style='width: 20%; padding: 2px; color: white'>
                <%=ViewResources.SharedStrings.CardCode %>
                <input type="checkbox" id="cbx_expCardList_2" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(2,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(2,1);'></span></li>
            </th>
            <th style='width: 12%; padding: 2px; color: white'>
                <%=ViewResources.SharedStrings.Name %>
                <input type="checkbox" id="cbx_expCardList_3" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(3,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(3,1);'></span></li>
            </th>
            <th style='width: 12%; padding: 2px; color: white'>
                <%=ViewResources.SharedStrings.CommonBuilding %>
                <input type="checkbox" id="cbx_expCardList_4" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(4,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(4,1);'></span></li>
            </th>
            <% if (!Model.User.IsCompanyManager)
                { %><th style='width: 12%; padding: 2px; color: white'><% }
                                                                           else
                                                                           { %>
            <th style='width: 12%; padding: 2px; display: none'><% } %>
                <%=ViewResources.SharedStrings.UsersCompany %>
                <input type="checkbox" id="cbx_expCardList_5" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(5,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(5,1);'></span></li>
            </th>

            <th style='width: 10%; padding: 2px; color: white'>
                <%=ViewResources.SharedStrings.CardsValidTo %>
                <input type="checkbox" id="cbx_expCardList_6" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(6,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(6,1);'></span></li>
            </th>
            <th style='width: 10%; padding: 2px; color: white'>
                <%=ViewResources.SharedStrings.UsersDeactivationDate %>
                <input type="checkbox" id="cbx_expCardList_9" checked="checked" class="tipsy_we" title="Export this column" style="display: none" />
              <%--  <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CardSort(9,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CardSort(9,1);'></span></li>--%>
            </th>
            <th align="right" style='width: 4%; color: white'>
                <li class='ui-state-default ui-corner-all ui-icon-custom' style="height: 18px; width: 18px;"><span class='ui-icon ui-icon-print' style='cursor: pointer; background-position: -161px -96px;' onclick="javascript:TogglePrintCard();"></span></li>
            </th>
        </tr>
    </thead>
    <tbody>
        <% var i = 1; foreach (var card in Model.Cards)
            {
                var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr id="userCardRow" <%= bg %>>
            <td style='width: 2%; padding: 2px;'>
                <input type='checkbox' id='<%= card.Id %>' name='card_checkbox' onclick='javascript: CheckCard(this);' />
                <%:Html.Hidden("userCardStatusNumber", card.CardStatusNumber) %>
                <%:Html.Hidden("userCardId", card.Id) %>
            </td>
            <td id="userCardStatus" style='width: 9%; padding: 2px;'>
                <%= Html.Encode(card.CardStatus)%>
            </td>
            <td id="userCardDeactivationReason" style='width: 10%; padding: 2px;'>
                <%= Html.Encode((!card.Free) ? card.DeactivationReason : "")%>
                <%--<%= Html.Encode(card.DeactivationReason)%>--%>
            </td>
            <td id="userCardType" style='width: 9%; padding: 2px;'>
                <%= Html.Encode(card.TypeName != null && card.TypeName.Length > 30 ? card.TypeName.Substring(0, 27) + "..." : card.TypeName)%>
            </td>
            <td style='width: 20%; padding: 2px;'>
                <%= Html.Encode(card.FullCardCode)%>
            </td>
            <td style='width: 12%; padding: 2px;'>
                <%= Html.Encode(card.Name != null && card.Name.Length > 30 ? card.Name.Substring(0, 27) + "..." : card.Name)%>
            </td>
            <td style='width: 12%; padding: 2px;'>
                <%= Html.Encode(card.Building) %>
            </td>
            <% if (!Model.User.IsCompanyManager)
                { %><td style='width: 12%; padding: 2px;'><% }
                        else
                        { %>
            <td style='width: 12%; padding: 2px; display: none'><% } %>
                <%= Html.Encode(card.CompanyName != null && card.CompanyName.Length > 30 ? card.CompanyName.Substring(0, 27) + "..." : card.CompanyName)%>
            </td>

            <td style='width: 10%; padding: 2px;'>
                <%= Html.Encode(card.ValidToStr) %>
            </td>
            <td style='width: 10%; padding: 2px;'>
                <%= Html.Encode((!card.Active  && !card.Free) ? card.DeactivationDateTime : "")%>
            </td>
            <td style='width: 4%; padding: 2px; text-align: right;'>
                <%--<% if (!card.Free) { %>--%>
                <span id='button_card_edit_<%= card.Id %>' class='icon icon_green_go tipsy_we' title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, card.Name) %>' onclick="javascript:EditCard(<%= card.Id %>);"></span>
                <%--<% } %>--%>
            </td>
        </tr>
        <% } %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="5">
                <% Html.RenderPartial("Paginator", Model.Paginator); %>
            </td>
        </tr>
    </tfoot>
</table>

<script type="text/javascript" lang="javascript">

    $(document).ready(function () {
        if ($("div#cardPrintControlButtons").is(":visible")) {
            $("input[id^=cbx_expCardList_][type='checkbox']").each(function () {
                $(this).show();
            });
        }
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
        $('input#button_delete_card').fadeOut();
        $('input#button_activate_card').fadeOut();
        $('input#button_give_card_back').fadeOut();
        $('input#button_deactivate_card').fadeOut();
        $('input#button_move_to_company').fadeOut();
        $('input#button_add_card').fadeOut();
    });

    function CheckAll() {
        $('input[name=card_checkbox]').each(function () {
            this.checked = !this.checked;
        });
        ManageCardButtons(<%= Model.FilterCriteria %>);
        return false;
    }

    function CheckCard(cntr) {
        parent_td = $(cntr).parent();
        statusNum = parent_td.find('#userCardStatusNumber').attr('value');
        var any = 0;

        $('input[name=card_checkbox]').each(function () {
            if (this.checked) any++;
        });
        if (any == 0) {
            $('input[name=card_checkbox]').each(function () {
                $(this).fadeIn();
            });
        }
        else {
            $('input[name=card_checkbox]').each(function () {
                tmp_parent = $(this).parent();
                tmp_num = tmp_parent.find('#userCardStatusNumber').attr('value');
                if (tmp_num != statusNum) {
                    $(this).fadeOut();
                }
            });
        }
        ManageCardButtons(statusNum);
    }

    function ManageCardButtons(filter) {
        var any = 0;

        $('input[name=card_checkbox]').each(function () {
            if (this.checked) any++;
        });
        if (any != 0) {
            switch (filter) {
                case 0: case "0":

                    $('input#button_delete_card').fadeIn();
                    $('input#button_Free_card').fadeIn();

                    $('input#button_activate_card').fadeIn();
                    $('input#button_give_card_back').fadeIn();
                    if (any == 1) {
                        $('input#button_add_card').fadeIn();
                    }
                    else {
                        $('input#button_add_card').fadeOut();
                    }
                    $('input#button_deactivate_card').fadeOut();
                    $('input#button_move_to_company').fadeOut();
                    break;
                case 1: case "1":

                    $('input#button_delete_card').fadeOut();
                    $('input#button_Free_card').fadeOut();
                    $('input#button_activate_card').fadeOut();
                    $('input#button_give_card_back').fadeOut();
                    $('input#button_add_card').fadeOut();
                    $('input#button_deactivate_card').fadeIn();
                    $('input#button_move_to_company').fadeOut();
                    break;
                case 2: case "2":

                    $('input#button_delete_card').fadeIn();
                    $('input#button_Free_card').fadeOut();
                    $('input#button_activate_card').fadeOut();
                    $('input#button_give_card_back').fadeOut();
                    $('input#button_deactivate_card').fadeOut();
                    $('input#button_add_card').fadeIn();
                    if (any == 1) {
                        $('input#button_add_card').fadeIn();
                    }
                    else {
                        $('input#button_add_card').fadeOut();
                    }
                        <% if (!Model.User.IsCompanyManager)
    { %> $('input#button_move_to_company').fadeIn();
                   <% } %>
                    break;
                case 3: case "3":

                    $('input#button_delete_card').fadeIn();
                    $('input#button_activate_card').fadeIn();
                    $('input#button_give_card_back').fadeIn();
                    $('input#button_deactivate_card').fadeIn();
                        <% if (!Model.User.IsCompanyManager)
    { %> $('input#button_move_to_company').fadeIn();
                   <% } %>
                    break;
            }
        }
        else {

            $('input#button_add_card').fadeOut();
            $('input#button_Free_card').fadeOut();
            $('input#button_delete_card').fadeOut();
            $('input#button_activate_card').fadeOut();
        $('input#button_give_card_back').fadeOut();
            $('input#button_deactivate_card').fadeOut();
            $('input#button_move_to_company').fadeOut();
            $('input#check_all').attr('checked', false);
        }
        return false;
    }

    function ActivateCard() {
        var cardsIds = new Array();
        $('input[name=card_checkbox]').each(function () {
            if (this.checked) {
                var cardId = $(this).attr('id');
                cardsIds.push(cardId);
            }
        });
        $("#button_activate_card").addClass("Trans");
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $("div#user-modal-dialog").html("");
                $.get('/Card/Activate', {}, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 400,
            height: 200,
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
                        success: function () {
                            $("#button_activate_card").removeClass("Trans");
                            setTimeout(function () { SubmitCardSearch(); }, 1000);
                        }
                    });
                    $(this).dialog("close");
                },
          '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $("div#user-modal-dialog").html(""); $(this).dialog("close"); $("#button_activate_card").removeClass("Trans"); }
            },
            close: function () {
                $("div#modal-dialog").html("");
            }
        });
        return false;
    }

    function DeactivateCard() {
        var cardsIds = new Array();
        $('input[name=card_checkbox]').each(function () {
            if (this.checked) {
                var cardId = $(this).attr('id');
                cardsIds.push(cardId);
            }
        });
        $("#button_deactivate_card").addClass("Trans");
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $("div#user-modal-dialog").html("");
                $.get('/Card/Deactivate', {}, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 440,
            height: 180,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CardsDeactivating %>',
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
                            $("#button_deactivate_card").removeClass("Trans");
                            setTimeout(function () { SubmitCardSearch(); }, 1000);
                        }
                    });
                    $(this).dialog("close");
                },
            '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $("div#user-modal-dialog").html(""); $(this).dialog("close"); $("#button_deactivate_card").removeClass("Trans"); }
            },
            close: function () {
                $("div#modal-dialog").html("");
            }
        });
        return false;
    }

    function DeleteCard() {
        var cardsIds = new Array();
        $('input[name=card_checkbox]').each(function () {
            if (this.checked) {
                var cardId = $(this).attr('id');
                cardsIds.push(cardId);
            }
        });

        $("#button_delete_card").addClass("Trans");
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessageCard %> ');

            },
            resizable: false,
            width: 400,
            height: 200,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CardsDeletingCards %>',
            buttons: {
          '<%=ViewResources.SharedStrings.BtnDelete %>': function () {
                    ShowDialog('<%=ViewResources.SharedStrings.CardsDeletingCards %>' + '...', 2000, true);
                    $.ajax({
                        type: "Post",
                        url: "/Card/DeleteCards",
                        data: { cardIds: cardsIds },
                        traditional: true,
                        success: function (result) {
                            $("#button_delete_card").removeClass("Trans");
                            setTimeout(function () { SubmitCardSearch(); }, 1000);
                        }
                    });
                    $(this).dialog("close");
                },
          '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_delete_card").removeClass("Trans"); }
            }
        });
        return false;
    }

    function FreeCard() {
        var cardsIds = new Array();
        $('input[name=card_checkbox]').each(function () {
            if (this.checked) {
                var cardId = $(this).attr('id');
                cardsIds.push(cardId);
            }
        });

        $("#button_Free_card").addClass("Trans");
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CardsbackConfirmMessage %> ');

            },
            resizable: false,
            width: 400,
            height: 200,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CardsBackToFree %>',
            buttons: {
          '<%=ViewResources.SharedStrings.CardsBtnAddToFreeCards %>': function () {
                    ShowDialog('<%=ViewResources.SharedStrings.CardsBackToFree %>' + '...', 2000, true);
                    $.ajax({
                        type: "Post",
                        url: "/Card/FreeCards",
                        data: { cardIds: cardsIds },
                        traditional: true,
                        success: function (result) {
                            $("#button_Free_card").removeClass("Trans");
                            setTimeout(function () { SubmitCardSearch(); }, 1000);
                        }
                    });
                    $(this).dialog("close");
                },
          '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_Free_card").removeClass("Trans"); }
            }
        });
        return false;
    }

    function MoveCard() {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $("div#user-modal-dialog").html("");
                $.get('/Card/MoveToCompany', {}, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 400,
            height: 150,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CardsMoving %>',
            buttons: {
              '<%=ViewResources.SharedStrings.BtnMove %>': function () {
                    dlg = $(this);
                    var cardsIds = new Array();
                    $('input[name=card_checkbox]').each(function () {
                        if (this.checked) {
                            var cardId = $(this).attr('id');
                            cardsIds.push(cardId);
                        }
                    });
                    $('input#button_move_to_company').fadeOut();

                    $.ajax({
                        type: "Post",
                        url: "/Card/DoMoveToCompany",
                        dataType: "json",
                        data: { companyId: $("select#MoveToCompanyId").val(), cardIds: cardsIds },
                        traditional: true,
                        success: function (result) {
                            if (result.IsSucceed == true) {
                                dlg.dialog("close");
                                ShowDialog(result.Msg, 2000, true);
                                setTimeout(function () { SubmitCardSearch(); }, 1000);
                            }
                            else {
                                ShowDialog(result.Msg, 2000);
                            }
                        }
                    });
                },
              '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); }
            },
            close: function () {
                $("div#modal-dialog").html("");
            }
        });
        return false;
    }

    function EditCard(cardId) {
       
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $("div#user-modal-dialog").html("");
                $.get('/Card/Edit', { id: cardId }, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 500,
            height: 500,
            modal: true,
            stack: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CardsEditCard %>',
            buttons: {
            '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    var cid = $("#CompanyId").val();
                    var Hdn_cid = $("#Comp_Id").val();
                    if (cid != "-1" && cid != Hdn_cid) {
                        //alert("Are you sure you want to change the company??");
                        $('<div></div>').appendTo('body')
                            .html('<div><h4>Are you sure you want to change the company?</h4></div>')
                            .dialog({
                                modal: true, title: 'Confirm', zIndex: 10000, autoOpen: true,
                                width: 'auto', resizable: false,
                                buttons: {
                                    Yes: function () {
                                        var sr = $('#Serial').val();
                                        var dk = $('#Dk').val();
                                        if (sr == "255" && dk == "65535") {
                                            $('#Serial').css('border-color', 'darkred');
                                            $('#Dk').css('border-color', 'darkred');
                                        }
                                        else {
                                            $.post("/Card/EditCard", $("#editCard").serialize(), function (a) {
                                               
                                                if (a == "True") {
                                                    ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>', 2000, true);
                                                }
                                                else if (a == null) {
                                                    ShowDialog('<%=ViewResources.SharedStrings.Error %>', 2000, false);
                                                }
                                                else {
                                                    ShowDialog('Card already in use!! Owner name: ' + a, 5000);
                                                }
                                            });
                                            setTimeout(function () { SubmitCardSearch(); }, 1000);
                                            $(this).dialog("close");
                                            $('button:contains("<%=ViewResources.SharedStrings.BtnCancel %>")').click();
                                        }
                                    },
                                    No: function () {
                                        setTimeout(function () { SubmitCardSearch(); }, 1000);
                                        $(this).dialog("close");
                                    }
                                },
                                close: function (event, ui) {
                                    $(this).remove();
                                }
                            });
                        //$(this).dialog("close");
                    }
                    else {
                        var sr = $('#Serial').val();
                        var dk = $('#Dk').val();
                        if (sr == "255" && dk == "65535") {
                            $('#Serial').css('border-color', 'darkred');
                            $('#Dk').css('border-color', 'darkred');
                        }
                        else {
                            $.post("/Card/EditCard", $("#editCard").serialize(), function (a) {

                                if (a == "True") {
                                    ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>', 2000, true);
                                }
                                else if (a == null) {
                                    ShowDialog('<%=ViewResources.SharedStrings.Error %>', 2000, false);
                                }
                                else {
                                    ShowDialog('Card already in use!! Owner name: ' + a, 5000);
                                }
                            });
                            setTimeout(function () { SubmitCardSearch(); }, 1000);
                            $(this).dialog("close");
                        }
                    }

                },
            '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            },
            close: function () {
                $("div#modal-dialog").html("");
            }
        });
        return false;
    }


    function GiveCardBack() {

        var cardsIds = new Array();
        $('input[name=card_checkbox]').each(function () {
            if (this.checked) {
                var cardId = $(this).attr('id');
                cardsIds.push(cardId);
            }
        });
        $('input#button_give_card_back').fadeOut();

        $.ajax({
            type: "Post",
            url: "/Card/GiveCardBack",
            dataType: "json",
            data: { cardIds: cardsIds },
            traditional: true,
            success: function (result) {
                if (result.IsSucceed == true) {
                    ShowDialog('<%=ViewResources.SharedStrings.CommonDataSavedMessage %>', 2000, true);
                    setTimeout(function () { SubmitCardSearch(); }, 1000);
                }
                else {
                    ShowDialog( '<%=ViewResources.SharedStrings.Error %>', 2000);
                }
            }
        });
        return false;
    }
</script>
