<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>"  %>

<% if( Request.IsAuthenticated ) { %>
<input id="systemSearchField" type='text' style='margin-right:50px; width:150px; display:none;' value='System search' onclick="javascript:this.value = '';" onblur="javascript:this.value = 'System search';" onkeypress="javascript:if(event.keyCode == 13 && this.value.length > 0) { SubmitSystemSearch(); };" />
<%: Model %> 
<span id='button_submit_logout' class='icon icon_logout tipsy_ns' title='Logout!' onclick="javascript:SubmitLogout('submit_logout','<%:ViewResources.SharedStrings.Logout %>');"></span>
<script type="text/javascript" lang="javascript">
function SubmitLogout(Action,Message)
{
    $(document).ready(function () {
        if ($("div#top-dialog").is(":visible")) $("div#top-dialog").parent().hide();
        if ($("div#modal-dialog").is(":visible")) $("div#modal-dialog").parent().hide();
        if ($("div#modal-dialog-2").is(":visible")) $("div#modal-dialog-2").parent().hide();
        if ($("div#user-modal-dialog").is(":visible")) $("div#user-modal-dialog").parent().hide();
        if ($("div#delete-modal-dialog").is(":visible")) $("div#delete-modal-dialog").parent().hide();
        $("div#user-logon-modal-dialog").dialog({
			open: function(event, ui) {
			},
			resizable: false,
			width: 300,
			height: 140,
			modal: true,
			title: "<span class='ui-icon ui-icon-power' style='float:left; margin:1px 5px 0 0;'></span>" + Message,
			buttons: {
				'<%:ViewResources.SharedStrings.Ok %>': function() {
					$(this).dialog("close");
					$.ajax({
						type: "POST",
						url: "/Account/LogOff",
						beforeSend: function() {
							$("#button_submit_logout").addClass("Trans");
						},
						success: function(msg) {
						    location.href = '/Home/Index';
						}
					});
				},
				'<%:ViewResources.SharedStrings.BtnCancel %>': function() {
					$(this).dialog("close");
				}
			}
		});

		return false;
	});
}
</script>
<% } %>