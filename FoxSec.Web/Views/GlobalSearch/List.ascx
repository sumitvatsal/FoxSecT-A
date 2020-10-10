<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.GlobalSearchViewModel>" %>
<table id="searchedTableGlobalSearch" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <thead>
        <tr>
            <th style='width: 30%; padding: 2px;'>
                <label><%:ViewResources.SharedStrings.UsersName %></label>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:GlobalSearchSort(1,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:GlobalSearchSort(1,1);'></span></li>
            </th>
			<th style='width: 75%; padding: 2px;'>
                <label>Entity type</label>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:GlobalSearchSort(2,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:GlobalSearchSort(2,1);'></span></li>
            </th>
            <th align="right" style='width: 5%'>
            </th>
        </tr>
    </thead>
    <tbody>
        <% var i = 1; foreach (var item in Model.Items) { var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr <%= bg %>>
            <td style='width: 30%; padding: 2px;'>
				<%=Html.Encode(item.Name) %>
            </td>
            <td id="userListDataName" style='width:70%; padding:2px;'>
                <%= Html.Encode(item.TypeDescription)%>
            </td>
			<td style='width: 5%; padding: 2px; text-align: right;'>
                <span id='button_item_view<%= item.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=item.ToolTipName  %>' onclick='<%=item.Function %>'></span>
            </td>
        </tr>
        <% } %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="2">
                <% Html.RenderPartial("Paginator", Model.Paginator); %>
            </td>
        </tr>
    </tfoot>
</table>

<script type="text/javascript" language="javascript">

	$(document).ready(function () {
		$(".tipsy_we").attr("class", function () {
			$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
		});
	});

	function HandleGlobalSearchPaging(page, rows) {
		system_search_page = page;
		system_search_rows = rows;
		HandleSystemSearch();
		return false;
	}
																  
	function HandleGlobalSearchSoring(page, rows, field, direction){
		system_search_page = page;
		system_search_rows = rows;
		system_search_field = field;
		system_search_direction = direction;
		HandleSystemSearch();
		return false;
	}

</script>

