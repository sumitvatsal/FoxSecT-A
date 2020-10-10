<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PartnerListViewModel>" %>
<%if(Model.Partners != null){%>
<form id="partnerTable">
  <table id="partner_table_sort" class="tablesorter" width="100%" style="margin:0; width:100%; padding:2px; border-spacing:0;">
  <thead>
  <tr>
    <th style='width:25%; padding:2px; vertical-align:middle'>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s'></span></li>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n'></span></li>
       <%:ViewResources.SharedStrings.UsersName %>
    </th>
	<th style='width:40%; padding:2px;'>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s'></span></li>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n'></span></li>
       <%:ViewResources.SharedStrings.CompaniesAdditionalInfo %>
    </th>
    <th style='width:20%; padding:2px;'>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s'></span></li>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n'></span></li>       
       <%:ViewResources.SharedStrings.DepartmentsManager %>
    </th>
    <th style='width:5%; padding:2px;'>
      <%-- <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s'></span></li>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n'></span></li>--%>
       <%:ViewResources.SharedStrings.FilterActive %>
    </th>
    <th style='width:5%; padding:2px;'></th>
    <th style='width:5%; padding:2px;'></th>
</tr>
</thead>
  <tbody>
    <%foreach (var partner in Model.Partners){%>
      <tr>
        <td style='width:25%; padding:2px; vertical-align:middle'>
           <%= Html.Encode(partner.Name)%>
        </td>
	    <td style='width:40%; padding:2px;'>
           <%= Html.Encode(partner.Comment)%>
        </td>
        <td style='width:20%; padding:2px;'>       
           <%= Html.Encode(partner.ManagerName)%>
        </td>
        <td style='width:5%; padding:2px;'>
            <%= Html.CheckBox("Company.Partners[" + partner.Id + "].Active", partner.Active, new { @disabled = "disabled" })%>
        </td>
        <td style='width:5%; padding:2px;'>
          <span class="icon icon_green_go tipsy_we" style="cursor:pointer" original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.CommonUpdate, partner.Name) %>' onclick="javascript:EditPartner(<%= partner.Id %>, '<%= partner.Name %>');"></span>
        </td>
        <td style='width:5%; padding:2px;'>
          <span class="ui-icon ui-icon-closethick" style="cursor:pointer" original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnDelete, partner.Name) %>' onclick="javascript:DeletePartner(<%= partner.Id %>, '<%= partner.Name %>');"></span>        
        </td>
    </tr>
    <%}%>
    </tbody>
        <tfoot>
        <tr>
            <td colspan="5">
                <div id="partnerPagerSort" class="footer">
                    <table align="left" cellpadding="0" cellspacing="0" style="margin: 0; padding: 0; border-spacing: 0"><tr>
                    <th><div style="margin: 3px 2px 0px 2px"><li id="Li1" class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthickstop-1-w first'></span></li></div></th>
                    <th><div style="margin: 3px 2px 0px 2px;"><li id="Li2" class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-w prev'></span></li></div></th>
                    <th><input type="text" class="pagedisplay" style="width: 35px; font-size:11px; height:14px" /></th>
                    <th><div style="margin: 3px 2px 0px 2px;"><li id="Li3" class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-e next'></span></li></div></th>
                    <th><div style="margin: 3px 2px 0px 2px;"><li id="Li4" class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthickstop-1-e last'></span></li></div></th>
                    <th>
                    <select class="pagesize" style="font-size:11px; height:20px">
                        <option value="10">10 &nbsp;<%:ViewResources.SharedStrings.CommonPerPage %></option>
                        <option value="15">15 &nbsp;<%:ViewResources.SharedStrings.CommonPerPage %></option>
                        <option value="999999"><%:ViewResources.SharedStrings.CommonAll %></option>
                    </select>
                    </th>
                    </tr>
                    </table>
                </div>
            </td>
        </tr>
    </tfoot>
    </table>
  </form>
<%}%>

<script type="text/javascript" language="javascript">
  $(document).ready(function () {
  	$(".tipsy_we").attr("class", function () {
  		$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
  	});
	//$("#partner_table_sort").tablesorter({
 //     widgets: ['zebra'],
 //     headers: {
 //       0: { sorter: 'text' },
 //       1: { sorter: 'text' },
 //       2: { sorter: 'text' },
 //       3: { sorter: 'text' },
 //       4: { sorter: false },
 //       5: { sorter: false }
 //     }
 //   }).tablesorterPager({ container: $("#partnerPagerSort") });
  });

</script>