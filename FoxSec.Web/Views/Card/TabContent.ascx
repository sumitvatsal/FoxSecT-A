<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<%= Html.Hidden("IsHideCardNotFoundDialog", 0) %>
<%= Html.Hidden("IsAddFreeCard", 0) %>
<div id='panel_owner_tab_cards'>
    <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
        <tr>
            <td style='vertical-align: top;'>
                <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
                    <tr>
                        <td style='width: 10%;'>
                            <label><%:ViewResources.SharedStrings.CardsCardStatus %></label>
                            <br />
                            <select id='CardFilter' style='width: 90%;'>
                                <option value="3"><%:ViewResources.SharedStrings.DefaultDropDownValue %></option>
                                <option value="1"><%:ViewResources.SharedStrings.FilterActive %></option>
                                <option value="0"><%:ViewResources.SharedStrings.FilterDeactivated %></option>
                                <option value="2"><%:ViewResources.SharedStrings.CardsFreeCards %></option>
                            </select>
                        </td>
                        <td style='width: 10%;'>
                            <label><%:ViewResources.SharedStrings.CardsCardReason%></label>
                            <br />
                            <select id='DeactivationReason' style='width: 90%;'>
                                <option value="0"><%:ViewResources.SharedStrings.DefaultDropDownValue %></option>
                                <% foreach (FoxSec.DomainModel.DomainObjects.ClassificatorValue i in Model.ClassificatorValues)
                                    { %>
                                <option value='<%:i.Id %>'><%:i.Value %></option>
                                <% } %>
                            </select>
                        </td>
                        <td style='width: 10%;'>
                            <label><%:ViewResources.SharedStrings.CardsCardType %></label>
                            <br />
                            <select id='CardType' style='width: 90%;'>
                                <option value="0"><%:ViewResources.SharedStrings.DefaultDropDownValue %></option>
                                <% foreach (FoxSec.DomainModel.DomainObjects.UserAccessUnitType i in Model.CardTypes)
                                    { %>
                                <option value='<%:i.Id %>'><%:i.Name %></option>
                                <% } %>
                            </select>
                        </td>
                        <td style='width: 20%; vertical-align: top;'>
                            <label for='Search_by_card_no'><%:ViewResources.SharedStrings.CardsCardCode %></label>
                            <table cellpadding='0' cellspacing='0' style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
                                <tr>
                                    <td style='width: 25%'><%:ViewResources.SharedStrings.CardsSerDk %>:</td>
                                    <td style='width: 60%'>
                                        <input type='text' id='Search_by_card_ser' style='width: 30px;' value='' onkeyup="javascript:CardSerValidation();" maxlength='3' />+<input type='text' id='Search_by_card_dk' maxlength='5' style='width: 47px;' value='' onkeyup="javascript:CardDkValidation();" />
                                    </td>
                                    <td style='width: 15%'>
                                        <span class='ui-icon ui-icon-refresh' class='icon icon_find tipsy_we' original-title='<%=ViewResources.SharedStrings.CardsClearSerDk %>' style='margin: 0 3px 0 5px; cursor: pointer' onclick="javascript:ClearCardFields();"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='width: 25%'><%:ViewResources.SharedStrings.CardsCode %>:</td>
                                    <td style='width: 75%' colspan="2">
                                        <input type='text' id='Search_by_card_no' style='width: 80%;' value='' onkeyup="javascript:CardCodeValidation();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style='width: 12%; vertical-align: top;'>
                            <label for='Search_cardname'><%:ViewResources.SharedStrings.UsersName %></label><br />
                            <input type='text' id='Search_by_cardname' style='width: 90%;' value='' onkeypress="javascript:onPressSubmitCardSearch(event);" />
                        </td>
                        <td style='width: 12%; vertical-align: top;'>
                            <label for='Search_building'><%:ViewResources.SharedStrings.CommonBuilding %></label><br />
                            <input type='text' id='Search_by_building' style='width: 90%;' value='' onkeypress="javascript:onPressSubmitCardSearch(event);" />
                        </td>
                        <% if (!Model.User.IsCompanyManager)
                            { %>
                        <td style='width: 12%; vertical-align: top;'>
                            <label for='Search_card_company'><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                            <input type='text' id='Search_by_card_company' style='width: 90%;' value='' onkeypress="javascript:onPressSubmitCardSearch(event);" />
                        </td>
                        <% } %>
                        <td style='width: 10%; vertical-align: top;'>
                            <label for='Search_validation'><%:ViewResources.SharedStrings.UsersDeactivationDate%></label><br />
                            <input type='text' id='Search_by_validation' style='width: 90%' class="select_date" value='' onkeypress="javascript:onPressSubmitCardSearch(event);" />
                        </td>

                        <td style='width: 4%; vertical-align: top; padding-top: 20px;'>
                            <span id='button_submit_card_search' class='icon icon_find tipsy_we' original-title='<%=ViewResources.SharedStrings.CardsSearch %>' onclick="javascript:SubmitCardSearch();"></span>
                        </td>
                    </tr>
                    <!--By manoranjan-->
                    <tr>
                        <td colspan="3">
                            <input type="button" onclick="commentedCardList();" name="commentedCards" id="commentedCards" value="<%=ViewResources.SharedStrings.CommentedCardsName %>" />
                        </td>
                    </tr>
                    <!---->
                </table>
                <div style='margin: 10px 0 0 0; text-align: right;'>
                    <table cellpadding='0' cellspacing='0' style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
                        <tr>
                            <td colspan="2" style='width: 60%; text-align: right'>
                                <input type='button' id='button_give_card_back' value='<%=ViewResources.SharedStrings.GiveCardBack %>' onclick="javascript: GiveCardBack();" style='display: none' />
                                <input type='button' id='button_move_to_company' value='<%=ViewResources.SharedStrings.CardsMoveCardBtn %>' onclick="javascript: MoveCard();" style='display: none' />
                                <input type='button' id='button_delete_card' value='<%=ViewResources.SharedStrings.CardsDeleteCardBtn %>' onclick="javascript: DeleteCard();" style='display: none' />
                                <input type='button' id='button_Free_card' value='<%=ViewResources.SharedStrings.CardsBtnAddToFreeCards %>' onclick="javascript: FreeCard();" style='display: none' />
                                <input type='button' id='button_activate_card' value='<%=ViewResources.SharedStrings.CardsActivateCardBtn %>' onclick="javascript: ActivateCard();" style='display: none' />
                                <input type='button' id='button_deactivate_card' value='<%=ViewResources.SharedStrings.CardsDeactivateCardBtn %>' onclick="javascript: DeactivateCard();" style='display: none' />
                                <input type='button' id='button_add_card' value='<%=ViewResources.SharedStrings.CardsAddCardToNewUser %>' onclick="javascript: AddCard();" style='display: none' />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id='cardPrintControlButtons' style="display: none; text-align: right">
                    <a style="cursor: pointer;" onclick="javascript:SetCardExportLink(this,'/Print/CardListPDF')">PDF</a> / <a style="cursor: pointer;" onclick="javascript:SetCardExportLink(this,'/Print/CardListExcel')">XLS</a>
                </div>
                <div id='AreaTabCardSearchResultsWait' style='display: none; width: 100%; height: 378px; text-align: center'><span style='position: relative; top: 35%' class='icon loader'></span></div>
                <div id='CardsList' style='display: none; margin: 15px 0;'></div>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        var card_page = 0;
        var card_rows = 10;
        var card_field = 1;
        var card_direction = 0;
        var CardSer;
        var CardDk;
        var CardNo;
        var BuildingId;
        var Reason;
        var ValidFrom;
        var ValidTo;

        $(document).ready(function () {
            var i = $('#panel_owner li').index($('#cardsTab'));
            var opened_tab = '<%: Session["tabIndex"] %>';

            if (opened_tab != '') {
                i1 = new Number(opened_tab);
                if (i1 != i) {
                    SetOpenedTab(i);
                }
            }
            else {
                SetOpenedTab(i);
            }

            $(".tipsy_we").attr("class", function () {
                $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
            });
            $("input:button").button();
            $(".select_date").datepicker({
                dateFormat: "dd.mm.yy",
                firstDay: 1,
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
            $('div#Work').fadeIn("slow");
        });

        function onPressSubmitCardSearch(e) {
            if (e.keyCode == 13) { SubmitCardSearch(); }
            return false;
        }

        function SubmitCardSearch() {
            Reason = $("select#DeactivationReason").val();
            CardSer = $("input#Search_by_card_ser").val();
            CardDk = $("input#Search_by_card_dk").val();

            if ((CardSer.length > 0) && (CardSer.length < 3)) {
                while (CardSer.length < 3) {
                    CardSer = '0' + CardSer;
                }
                $("input#Search_by_card_ser").val(CardSer);
            }
            if ((CardDk.length > 0) && (CardDk.length < 5)) {
                while (CardDk.length < 5) {
                    CardDk = '0' + CardDk;
                }
                $("input#Search_by_card_dk").val(CardDk);
            }

            CardNo = $("input#Search_by_card_no").val();
            CardName = $("input#Search_by_cardname").val();
            Company = $("input#Search_by_card_company").val();
            if ($("input#Search_by_card_company").size() == 0) {
                Company = "";
            }
            Building = $("input#Search_by_building").val();
            Validation = $("input#Search_by_validation").val();
            Filter = $("select#CardFilter").val();
            Type = $("select#CardType").val();
            flagc = "f";
            $('input#button_delete_card').fadeOut();
            $('input#button_activate_card').fadeOut();
            $('input#button_give_card_back').fadeOut();
            $('input#button_deactivate_card').fadeOut();
            $('input#button_add_card').fadeOut();
            $('input#button_Free_card').fadeOut();
            $.ajax({
                type: "Post",
                url: "/Card/Search",
                cache: false,
                data: { reasonId: Reason, cardSer: CardSer, cardDk: CardDk, cardNo: CardNo, cardName: CardName, company: Company, building: Building, validation: Validation, filter: Filter, type: Type, nav_page: card_page, rows: card_rows, sort_field: card_field, sort_direction: card_direction, flagc: flagc },
                beforeSend: function () {
                    $("#button_submit_card_search").addClass("Trans");
                    //$("div#CardsList").fadeOut('fast', function () { $("div#AreaTabCardSearchResultsWait").fadeIn('slow'); });
                },
                success: function (result) {
                    $("div#CardsList").html(result);
                    $("div#CardsList").fadeIn('fast');
                    $("#button_submit_card_search").removeClass("Trans");

                    if (CardSer != "" || CardDk != "" || CardNo != "") {
                        if ($("#CardsCount").val() == 0) {
                            if ($("#IsInList").val() == 1) {
                                ShowDialog('<%=ViewResources.SharedStrings.CardDetailsExist %>', 5000);
                            }
                            else if ($("#CardExists").val() == "False") {
                                $.get('/Card/IsCompanyCanUseOwnCards', function (result) {
                                    if (result == 'True') { CardNotFound(); }
                                    else { ShowDialog('<%=ViewResources.SharedStrings.CardsCardNotFoundErrorMesage %>', 2000); }
                                });
                            } else { ShowDialog("<%=ViewResources.SharedStrings.CardsCardFoundWithAnotherStatus %>", 5000); }
                        }
                        if ($("#CardsCount").val() == 1) {
                            if ($("#IsFreeCard").val() == "False") {
                                ShowDialog('<%=ViewResources.SharedStrings.CardsCardNotFree %>', 2000);
                            }
                        }
                    }
                }
            });
            return false;
        }

        function IsHideCardNotFoundDialog() {
            if ($("#IsHideThisDialog").is(':checked') == true) {
                $("#IsHideCardNotFoundDialog").val('1');
            }
            return false;
        }

        function AddCardToNewUser() {
            AddCard();
            IsHideCardNotFoundDialog();
            $("#IsAddFreeCard").val('0');
            return false;
        }

        function CardNotFound() {
            $("div#delete-modal-dialog").html("");
            if ($("#IsHideCardNotFoundDialog").val() == 0) {
                $("div#delete-modal-dialog").dialog({
                    open: function () {
                        $.get('/Card/CardNotFound', function (html) {
                            $("div#delete-modal-dialog").html(html);
                        });
                    },
                    resizable: false,
                    width: 430,
                    height: 300,
                    modal: true,
                    title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CardsCardNotFoundTitle %>',
                    buttons: {
                    '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                            $("div#delete-modal-dialog").html("");
                            $(this).dialog("close");
                        }
                    }
                });
            }
            if ($("#IsHideCardNotFoundDialog").val() == 1) {
                if ($("#IsAddFreeCard").val() == 1) { AddFreeCard(); }
                else { AddCardToNewUser(); }
            }
            return false;
        }

        function AddCard() {
            CardSer = $("input#Search_by_card_ser").val() != null ? $("input#Search_by_card_ser").val() : CardSer;
            CardDk = $("input#Search_by_card_dk").val() != null ? $("input#Search_by_card_dk").val() : CardDk;
            CardNo = $("input#Search_by_card_no").val() != null ? $("input#Search_by_card_no").val() : CardNo;
            BuildingId = $("#cardBuildingRow").find("#BuildingId").val() != null ? $("#cardBuildingRow").find("#BuildingId").val() : BuildingId;
            ValidFrom = $("#cardValidFromRow").find("#validFrom").val();
            ValidTo = $("#cardValidToRow").find("#validTo").val();
            cardid = "";
            $('input[name=card_checkbox]').each(function () {
                if (this.checked) {
                    tmp_parent = $(this).parent();
                    tmp_num = tmp_parent.find('#userCardId').val();
                    cardid = tmp_num;
                }
            });
            $("div#modal-dialog").dialog({
                open: function () {
                    $.get('/Card/Create', { cardId: cardid }, function (html) {
                        $("div#modal-dialog").html(html);
                    });
                },
                resizable: false,
                width: 640,
                height: 420,
                modal: true,
                title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.BtnAddNewCard %>',
                buttons: {
         		'<%=ViewResources.SharedStrings.BtnSave %>': function () {
                        dlg = $(this);
                        $.ajax({
                            type: "Post",
                            url: "/Card/CreateCard",
                            dataType: 'json',
                            traditional: true,
                            data: $("#createNewCard").serialize(),
                            success: function (data) {
                                if (data.IsSucceed == false) {
                                    if (data.Msg == "licence error") {
                                        ShowDialog('<%=ViewResources.SharedStrings.MaxUserCountLicence %> ' + data.Count, 5000);
                                        return false;
                                    }
                                    else {
                                        $("div#modal-dialog").html(data.viewData);
                                        if (data.Msg != "") {
                                            ShowDialog(data.Msg, 3000);
                                        }
                                    }
                                }
                                else {
                                    ShowDialog('<%=ViewResources.SharedStrings.CardsNewUserCreating %>', 3000, true);
                                    dlg.dialog("close");
                                    $("div#delete-modal-dialog").each(function () {
                                        $(this).dialog('close');
                                    });
                                    setTimeout(function () { SubmitCardSearch(); }, 1000);
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

        function AddFreeCard() {
            $("#IsAddFreeCard").val('1');
            IsHideCardNotFoundDialog();
            CardSer = $("input#Search_by_card_ser").val() != null ? $("input#Search_by_card_ser").val() : CardSer;
            CardDk = $("input#Search_by_card_dk").val() != null ? $("input#Search_by_card_dk").val() : CardDk;
            CardNo = $("input#Search_by_card_no").val() != null ? $("input#Search_by_card_no").val() : CardNo;
            BuildingId = $("#cardBuildingRow").find("#BuildingId").val() != null ? $("#cardBuildingRow").find("#BuildingId").val() : BuildingId;
            CardTypeId = $("#cardCardTypeRow").find("#TypeId").val() != null ? $("#cardCardTypeRow").find("#TypeId").val() : CardTypeId;
            ValidFrom = $("#cardValidFromRow").find("#validFrom").val();
            ValidTo = $("#cardValidToRow").find("#validTo").val();

            $.ajax({
                type: "Post",
                url: "/Card/CreateFreeCard",
                dataType: 'json',
                data: { serial: CardSer, dk: CardDk, code: CardNo, buildingId: BuildingId, cardTypeId: CardTypeId, validFrom: ValidFrom, validTo: ValidTo },
                beforeSend: function () {
                    $("#button_add_to_free_cards").fadeOut("fast");
                    $("div#CardsList").fadeOut('fast', function () {
                        // $("div#AreaTabCardSearchResultsWait").fadeIn('slow');
                    });
                },
                success: function (response) {
                    if (response.IsSucceed == true) {
                        $("div#delete-modal-dialog").dialog("close");


                        ShowDialog(response.Msg, 2000, true);
                        Filter = "2";
                        flagc = "n";
                        SubmitCardSearch();
                    }
                    else {
                        ShowDialog(response.Msg, 2000);
                    }
                }
            });/*
        
        $.post("/Card/CreateFreeCard",
			 { serial: CardSer, dk: CardDk, code: CardNo, buildingId: BuildingId, cardTypeId: CardTypeId, validFrom: ValidFrom, validTo: ValidTo },
			 function (response) {
			 	if (response.IsSucceed == true) {
			 		$("div#delete-modal-dialog").dialog("close");
			 		ShowDialog(response.Msg, 2000, true);
			 	}
			 	else {
			 		ShowDialog(response.Msg, 2000);
			 	}
			 },
        "json");
        
        */
            return false;
        }

        function SearchCardValidation() {
            digitsValidate('Search_by_card_ser');
            if ($('#Search_by_card_ser').val() > 255) {
                $('#Search_by_card_ser').val("255");
            }
            SearchCardRelationSERDK();
            return false;
        }

        function SearchCardRelationSERDK() {
            digitsValidate("Search_by_card_dk");
            if ($('#Search_by_card_dk').val() > 65535) {
                $('#Search_by_card_dk').val("65535");
            }
            if ($("input#Search_by_card_ser").val().length > 0 || $("input#Search_card_dk").val().length > 0) {
                $("input#Search_by_card_no").val("");
            }
            return false;
        }

        function SearchCardRelationCODE() {

            // digitsValidate("Search_by_card_no");
            if ($("input#Search_by_card_no").val().length > 0) {
                $("input#Search_by_card_ser").val("");
                $("input#Search_by_card_dk").val("");
            }
            return false;
        }

        function ClearCardFields() {
            $("input#Search_by_card_dk").val("");
            $("input#Search_by_card_ser").val("");
            $("input#Search_by_card_ser").focus();
            return false;
        }

        function TogglePrintCard() {
            $("div#cardPrintControlButtons").toggle(500, function () {
                $("input[id^=cbx_expCardList_][type='checkbox']").each(function () {
                    $(this).toggle();
                });
            });
            return false;
        }

        function HandleCardPaging(page, rows) {
            card_page = page;
            card_rows = rows;
            SubmitCardSearch();
            return false;
        }

        function HandleCardSoring(page, rows, field, direction) {
            card_page = page;
            card_rows = rows;
            card_field = field;
            card_direction = direction;
            SubmitCardSearch();
            return false;
        }

        function SetCardExportLink(link, base) {
            CardSer = $("input#Search_by_card_ser").val();
            CardDk = $("input#Search_by_card_dk").val();
            CardNo = $("input#Search_by_card_no").val();
            CardName = $("input#Search_by_cardname").val();
            Company = $("input#Search_by_card_company").val();
            if ($("input#Search_by_card_company").size() == 0) {
                Company = "";
            }
            Building = $("input#Search_by_building").val();
            Validation = $("input#Search_by_validation").val();
            Filter = $("select#CardFilter").val();
            if ($("input#cbx_expCardList_1").is(':checked') == true) { Display1 = true; } else { Display1 = false; }
            if ($("input#cbx_expCardList_2").is(':checked') == true) { Display2 = true; } else { Display2 = false; }
            if ($("input#cbx_expCardList_3").is(':checked') == true) { Display3 = true; } else { Display3 = false; }
            if ($("input#cbx_expCardList_4").is(':checked') == true) { Display4 = true; } else { Display4 = false; }
            if ($("input#cbx_expCardList_5").is(':checked') == true) { Display5 = true; } else { Display5 = false; }
            if ($("input#cbx_expCardList_6").is(':checked') == true) { Display6 = true; } else { Display6 = false; }
            if ($("input#cbx_expCardList_7").is(':checked') == true) { Display7 = true; } else { Display7 = false; }
            if ($("input#cbx_expCardList_8").is(':checked') == true) { Display8 = true; } else { Display8 = false; }
            if ($("input#cbx_expCardList_9").is(':checked') == true) { Display9 = true; } else { Display9 = false; }
            link.href = base +
                '?cardSer=' + CardSer +
                '&cardDk=' + CardDk +
                '&cardNo=' + CardNo +
                '&cardName=' + CardName +
                '&company=' + Company +
                '&building=' + Building +
                '&validation=' + Validation +
                '&filter=' + Filter +
                '&sort_field=' + card_field +
                '&sort_direction=' + card_direction +
                '&display1=' + Display1 +
                '&display2=' + Display2 +
                '&display3=' + Display3 +
                '&display4=' + Display4 +
                '&display5=' + Display5 +
                '&display6=' + Display6 +
                '&display7=' + Display7 +
                '&display8=' + Display8 +
                '&display9=' + Display9;
            return true;
        }

        function CardSerValidation() {

            digitsValidate('Search_by_card_ser');
            var val = $('#Search_by_card_ser').val();
            if (val > 255) {
                $('#Search_by_card_ser').val("255");
            }
            if ($('#Search_by_card_ser').val() != null) {
                if ($('#Search_by_card_ser').val().length == 3) {
                    $("#Search_by_card_dk").focus();
                }
                if ($('#Search_by_card_ser').val().length != 0) {
                    $("#Search_by_card_no").val("");
                }
            }
            return false;
        }

        function CardDkValidation() {
            digitsValidate('Search_by_card_dk');
            if ($('#Search_by_card_dk').val() > 65535) {
                $('#Search_by_card_dk').val("65535");
            }
            if ($('#Search_by_card_dk').val() != null) {
                if ($('#Search_by_card_dk').val().length != 0) {
                    $("#Search_by_card_no").val("");
                }
            }
            return false;
        }

        function CardCodeValidation() {
            //digitsValidate('Code');
            if ($('#Search_by_card_no').val().length != 0) {
                $("#Search_by_card_ser").val("");
                $("#Search_by_card_dk").val("");
            }
            return false;
        }


        //By manoranjan

        function commentedCardList() {
           $("div#modal-dialog").dialog({
                open: function () {

                    var buildingid = $("#BuildingId").val();
                    if (buildingid == "" || buildingid == null || buildingid == undefined) {
                        buildingid = "0";
                    }
                    $('body').css('overflow', 'auto');
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/Card/CommentedCardsList',
                        cache: false,
  
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: false,
               width: '95%',
               draggable: false,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + "Commented Card",
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    $('body').css('overflow', 'hidden');
                }
            });
        }

        //
    </script>
</div>
