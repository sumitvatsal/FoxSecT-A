<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% 
    Type tModel = ViewData.ModelMetadata.ContainerType.GetProperty(ViewData.ModelMetadata.PropertyName).PropertyType;
    if(typeof(string).IsAssignableFrom(tModel)) {
        Html.DevExpress().TextBoxFor(m => m).Render();
    }
    else if(typeof(Enum).IsAssignableFrom(tModel)) {
        Html.DevExpress().ComboBoxFor(m => m, s => {
            s.Properties.Items.AddRange(Enum.GetValues(tModel));
            s.SelectedIndex = 0;
            s.Properties.DropDownStyle = DropDownStyle.DropDownList;
        }).Render();
    } 
%>