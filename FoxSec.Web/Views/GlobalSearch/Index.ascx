<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.GlobalSearchViewModel>" %>
<div id="GlobalSearch">
<table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
	<tr>
		<td style='width: 30%; vertical-align: top'>
		</td>
		<td style='width: 35%; vertical-align: top'>
			<%:Html.TextBoxFor(m=>m.SearchCriteria) %>
		</td>
		<td style='width: 5%; vertical-align: top'>
			<span id='button_submit_global_search' class='icon icon_find tipsy_we' original-title="Global search" onclick="javascript:HandleSystemSearch();"></span>
		</td>
		<td style='width: 30%; vertical-align: top'>
		</td>
	</tr>
</table>
<div id="GlobalSearchResult">
	<% Html.RenderPartial("List", Model); %>
</div>
</div>

<script type="text/javascript" language="javascript">

	var system_search_page = 0;
	var system_search_rows = 10;
	var system_search_field = 1;
	var system_search_direction = 1;
	
	$(document).ready(function () {
		$(".tipsy_we").attr("class", function () {
			$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
		});
		
	});

	function HandleSystemSearch() {
		criteria = $('#SearchCriteria').attr('value');
		$.get('/GlobalSearch/List', { searchCriteria: criteria, nav_page: system_search_page,
		 rows:system_search_rows, sort_field: system_search_field, sort_direction: system_search_direction  }, function (html) {
			$("#GlobalSearchResult").html(html);
		})	
	}
</script>
