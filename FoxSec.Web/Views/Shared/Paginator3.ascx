 
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PaginatorViewModel>" %>

 <div id="loader" style="display:none">
      <%:Html.Hidden("lodrflag", Session["Loadingstatus"]) %>
 </div>
<div class="footer">
<table align="left" cellpadding="0" cellspacing="0" style="margin: 0; padding: 0; border-spacing: 0">
<tr>
    <td>
        <span id="defaultValue" style="display:none"><%:Model.RowsPerPage%></span>
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
						new { @class = "pagesize", style = "font-size:11px; height:20p", onchange = string.Format("javascript:{0}ChangePage(0,this.value)", Model.Prefix) })%> 
      
		
       </td>
	
       <td>
        &nbsp; <%:string.Format("{0} {1}", Model.TotalRows, ViewResources.SharedStrings.CommonRecordsFound ) %>
	</td>
	
</tr>
</table>
     
    <style type="text/css">
    #loader {  
    position: fixed;  
    left: 0px;  
    top: 0px;  
    width: 100%;  
    height: 100%;  
    z-index: 99;
     opacity: 0.8;
     filter: alpha(opacity=80);
      -moz-opacity: 0.8;
     background: url('../../img/loader7.gif') 50% 50% no-repeat rgb(0, 0, 0);
    
  /* background: url('../../img/loader1.gif') 50% 50% no-repeat rgb(249,249,249);  */
}  
</style>
   
    <script>

        function <%: Model.Prefix %>ChangePage(page) {
        Handle<%: Model.Prefix %>Paging(page, $("select#<%: Model.Prefix %>Rows").val());
        var cc=$("select#<%: Model.Prefix %>Rows").val();
            var flag=$("#lodrflag").val();
            if(flag=="y")
            {
            
                $("#loader").fadeOut();
                $("#loader").hide();
                //sd.hide();
            }
            
            if(cc==1000 || cc >1000)
            {
                $("#loader").fadeIn();
           
            }
        
        return false;
    }

    function <%: Model.Prefix %>Sort(field, direction) {
        Handle<%: Model.Prefix %>Soring(<%: Model.CurrentPage %>, $("select#<%: Model.Prefix %>Rows").val(), field, direction);
        return false;
    }
    </script>
 
</div>