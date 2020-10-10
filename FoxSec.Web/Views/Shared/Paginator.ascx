<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PaginatorViewModel>" %>
<div class="footer">
<table align="left" cellpadding="0" cellspacing="0" style="margin: 0; padding: 0; border-spacing: 0">
<tr>
    <td>     <span id="defaultValue" style="display:none"><%:Model.RowsPerPage%></span>
        <div style="margin: 3px 2px 0px 2px">
            <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthickstop-1-w first' onclick='javascript:<%: Model.Prefix %>ChangePage(0);'></span></li>
        </div>
    </td>
    <td>
        <div style="margin: 3px 2px 0px 2px;">
            <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-w prev' onclick='javascript:<%: Model.Prefix %>ChangePage(<%: Model.CurrentPage-1 %>);'></span></li>
        </div>
    </td>
    <td>
        <input type="text" class="pagedisplay" style="width: 65px; font-size:11px; height:14px" value="<%: Model.CurrentPage+1 %> / <%: Model.TotalPages %>" />
    </td>
    <td>
        <div style="margin: 3px 2px 0px 2px;">
            <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-e next' onclick='javascript:<%: Model.Prefix %>ChangePage(<%: Model.CurrentPage+1 %>);'></span></li>
        </div>
    </td>
    <td>
        <div style="margin: 3px 2px 0px 2px;">
            <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthickstop-1-e last' onclick='javascript:<%: Model.Prefix %>ChangePage(<%: Model.TotalPages-1 %>);'></span></li>
        </div>
    </td>
    <td>
		<%=Html.DropDownList(string.Format("{0}Rows", Model.Prefix),
			new SelectList(Model.RowsPerPageItems, "Value", "Text", Model.RowsPerPage),
						new { @class = "pagesize", style = "font-size:11px; height:20p", onchange = string.Format("javascript:{0}ChangePage(0)", Model.Prefix) })%>
		
       </td>
	<td>
        &nbsp; <%:string.Format("{0} {1}", Model.TotalRows, ViewResources.SharedStrings.CommonRecordsFound ) %>
	</td>
</tr>
</table>
<script type="text/javascript">

    function <%: Model.Prefix %>ChangePage(page) {
       


        Handle<%: Model.Prefix %>Paging(page, $("select#<%: Model.Prefix %>Rows").val());
       
        
        return false;
    }

    function <%: Model.Prefix %>Sort(field, direction) {
        
        Handle<%: Model.Prefix %>Soring(<%: Model.CurrentPage %>, $("select#<%: Model.Prefix %>Rows").val(), field, direction);
        return false;
    }

</script>
</div>