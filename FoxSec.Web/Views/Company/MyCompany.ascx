<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.MyCompanyViewModel>" %>

<form id="companyInfo" >
 <table   style="margin: 0; padding: 3px; border-spacing: 3px;">
      <%=Html.Hidden("Company.Id", Model.Company.Id)%>
      <tr>
		    <td style='width:30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.UsersName %></label></td>
		    <td style='width:70%; padding:2px;'><%= Html.TextBox("Company.Name", Model.Company.Name, new { @style = "width:80%"})%></td>
      </tr> 
    
      <tr>
		    <td style='width:30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CompaniesAdditionalInfo %></label></td>
		    <td style='width:70%; padding:2px;'><%= Html.TextArea("Company.Comment", Model.Company.Comment, new { @style = "width:80%" })%></td>
      </tr>   
  </table>
  <div style='margin:0 0 10px 0; text-align:right;'><input type='button' id='button_save_company_info' value='Save company info' onclick="javascript:SaveCompanyInfo();" /></div>
</form>

    <div style='margin:0 0 10px 0; text-align:left;'>
        <input type='button' id='button_add_partner' value='Add new partner' onclick="javascript:AddNewPartner(<%=Model.Company.Id %>);" /></div>
    <div id="PartnersList"></div>



<script type="text/javascript" lang="javascript">
    $(document).ready(function () {
        $("input:button").button();

        $.get('/Company/PartnerList', { companyId: <%=Model.Company.Id %> }, function (html) {
            $("#PartnersList").html(html);
        });
    });

    function SaveCompanyInfo()
    {
        ShowDialog('<%=ViewResources.SharedStrings.CompaniesSavingCompanyInfo %>', 2000, true);
      $.post("/Company/SaveCompanyInfo", $("#companyInfo").serialize());
      return false;
  }

  function AddNewPartner(id) 
  {
      $("div#modal-dialog").dialog({
          open: function () {
              $("div#modal-dialog").html("");
              $.get('/Company/CreatePartner', { companyId: id }, function(html) {
                  $("div#modal-dialog").html(html);
              });
          },
          resizable: false,
          width: 540,
          height: 250,
          modal: true,
          title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span><%=Model.Company.Name%>",
            buttons:
            {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    ShowDialog('<%=ViewResources.SharedStrings.CompaniesAddPartner %>', 2000, true);

                    $.post("/Company/CreatePartner", $("#createNewPartner").serialize());

                    setTimeout(function () {
                        $.get('/Company/PartnerList', { companyId: <%=Model.Company.Id %> }, function (html) {
                            $("#PartnersList").html(html);
                        });
                    }, 2000);

                    $(this).dialog("close");
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    function DeletePartner(id, name)
    {
        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CommonDeleting %>' + name,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    ShowDialog('<%=ViewResources.SharedStrings.CompaniesDeletingPartnerMessage %>' + " " + name, 2000, true);
                    $.post("/Company/DeletePartner", { id: id }, function (html) {
                    });

                    setTimeout(function () {
                        $.get('/Company/PartnerList', { companyId: <%=Model.Company.Id %> }, function (html) {
                            $("#PartnersList").html(html);
                        });
                    }, 2000);

                    $(this).dialog("close");
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }

    function EditPartner(id, name) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Company/EditPartner', { id: id }, function(html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 540,
            height: 270,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>name",
            buttons:
            {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>' + name, 2000, true);

                    $.post("/Company/EditPartner", $("#editPartner").serialize());

                    setTimeout(function () {
                        $.get('/Company/PartnerList', { companyId: <%=Model.Company.Id %> }, function (html) {
                            $("#PartnersList").html(html);
                        });
                    }, 2000);

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