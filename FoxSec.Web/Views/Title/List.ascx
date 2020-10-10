<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TitleListViewModel>" %>

<table cellpadding="1" cellspacing="0" style="margin:0; width:100%; padding:1px; border-spacing:0;">
<tr>
	<th style='width:25%; padding:2px;'><%:ViewResources.SharedStrings.UsersName %></th>
    <th style='width:25%; padding:2px;'><%:ViewResources.SharedStrings.UsersCompany%></th>
    <th style='width:40%; padding:2px;'><%:ViewResources.SharedStrings.CommonDescription %></th>
    <th style='width:5%; padding:2px;'></th>
    <th style='width:5%; padding:2px;'></th>
</tr>

<% var i = 1; foreach (var title in Model.Titles) { var bg = (i++ % 2 == 1) ? " background-color:#CCC;" : ""; %>

<tr>
<td style='width:40%; padding:2px;<%= bg %>'><%= Html.Encode(title.Name)%> </td>
<td style='width:40%; padding:2px;<%= bg %>'><%= Html.Encode(title.Company.Name)%> </td>
<td style='width:50%; padding:2px;<%= bg %>'><%= Html.Encode(title.Description != null && title.Description.Length > 50 ? title.Description.Substring(0, 47) + "..." : title.Description)%></td>
<td style='width:5%; padding:2px; text-align:right;<%= bg %>'><span id='button_title_edit_<%= title.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(title.Name)) %>' onclick='<%=string.Format("javascript:EditTitle(\"submit_edit_title\", {0}, \"{1}\")", title.Id, Html.Encode(title.Name)) %>' ></span></td>
<td style='width:5%; padding:2px; text-align:right;<%= bg %>'><span id='button_title_delete_<%= title.Id %>' class="ui-icon ui-icon-closethick tipsy_we" style="cursor:pointer" original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnDelete, Html.Encode(title.Name)) %>' onclick='<%=string.Format("javascript:DeleteTitle({0}, \"{1}\")", title.Id, Html.Encode(title.Name)) %>'></span></td>
</tr>
<% } %>

</table>

<script type="text/javascript" language="javascript">

	$(document).ready(function () {
		$(".tipsy_we").attr("class", function () {
			$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
		});
	});

	function EditTitle(Action, TitleId, TitleTitle) {
	    $("div#modal-dialog").dialog({
	        open: function () {
	            $("div#modal-dialog").html("");
	            $.get('/Title/Edit', { id: TitleId }, function(html) {
	                $("div#modal-dialog").html(html);
	            });
	        },
	        resizable: false,
	        width: 480,
	        height: 340,
	        modal: true,
	        title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + TitleTitle,
	        buttons: {
	            '<%=ViewResources.SharedStrings.BtnOk %>': function () {
	                dlg = $(this);
	                $.ajax({
	                    type: "Post",
	                    url: "/Title/Edit",
	                    dataType: 'json',
	                    traditional: true,
	                    data: $("#editTitleTable").serialize(),
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
	                                $.get('/Title/List', function (html) {
	                                    $("div#TitlesList").html(html);
	                                });
	                            }, 1000);
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

    function DeleteTitle(TitleId, TitleTitle) {
        $("div#modal-dialog").dialog({
            open: function (event, ui) {
            	$("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span> Deleting " + TitleTitle,
            buttons: {
            	'<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $.post("/Title/Delete", { id: TitleId });
                    setTimeout(function () {
                        $.get('/Title/List', function (html) {
                            $("div#TitlesList").html(html);
                        });
                    }, 1000);

                          ShowDialog('<%=ViewResources.SharedStrings.TitlesDeletingMessage %>' + TitleTitle + "...", 2000, true);
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