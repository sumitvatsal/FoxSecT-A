<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>
<script type="text/javascript">
	var delBtn;
	$(document).ready(function () {
		SetupDeleteItemConfirmationDialog();
	});

	$(window).scroll(function () {
		if ($('#deleteItemConfirmDialog').size() > 0) {
			$('#deleteItemConfirmDialog').dialog('option', 'position', 'center');
		}
	});

	function SetupDeleteItemConfirmationDialog() {
		$('#deleteItemConfirmDialog').dialog({
			autoOpen: false,
			resizable: false,
			modal: true,
			buttons: {
				"<%: ViewResources.SharedStrings.BtnNo %>": function () {
					$(this).dialog('close');
					return false;
				},
				"<%: ViewResources.SharedStrings.BtnYes %>": function () {
					$(this).dialog('close');
					$(delBtn).parents('form').submit(); return false;
				}
			}
		});
	}

	function DeleteItem(btn) {
		delBtn = btn;
		//$('#deleteItemConfirmDialog').dialog('option', 'position', 'center');
		$('#deleteItemConfirmDialog').dialog('open');
	}
</script>

<div id="deleteItemConfirmDialog" title="<%: ViewResources.SharedStrings.Confirm %>">
	<p><%: Model %></p>
</div>



