<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FoxSec.Web.ViewModels.LanguageBarViewModel>>" %>

<% foreach(var language in Model) {
         %><span id='language_<%= language.DisplayName %>' class='icon flag_<%= language.Culture %><% if(language.IsCurrentLanguage) {%> Trans<%}%> tipsy_ns' original-title='<%= language.DisplayName %>' onclick="javascript:FlagHighlight('submit_language',this.id,'<%= language.DisplayName %>');"></span>
<% } %>
<script type="text/javascript" language="javascript">
function FlagHighlight(Action, Id, Lang) {

    var lang = { language: Lang, redirectUrl: '<%= this.Request.RawUrl %>' };

    $.ajax({
        type: "POST",
        url: "/Language/SwitchLanguage",
        data: lang,
        cache: false,
        success: function (result) {
            if (result == "" || result == -1) {
            }
            else {
                window.location.href = '<%= this.Request.RawUrl %>';
            }
        }
    });
}
</script>