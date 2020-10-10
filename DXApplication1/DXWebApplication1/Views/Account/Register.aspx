<%@ Page Language="CS" MasterPageFile="~/Views/Shared/Light.Master" Inherits="System.Web.Mvc.ViewPage<DXWebApplication1.Models.RegisterModel>" %>

<asp:content id="ClientArea" contentplaceholderid="MainContent" runat="server">
<div class="accountHeader">
    <h2>
        Create a New Account</h2>
    <p>Use the form below to create a new account.</p>
    </div>
<% using (Html.BeginForm()) { %>
        <%: Html.AntiForgeryToken() %>
    
    <% Html.DevExpress().Label(settings => {
        settings.Name = "UserNameLabel";
        settings.Text = "User Name";
        settings.AssociatedControlName = "UserName";
    }).Render();
     %>
    <div class="form-field">
        <%: Html.EditorFor(m => m.UserName)%>
        <%: Html.ValidationMessageFor(m => m.UserName) %>
    </div>
    <% Html.DevExpress().Label(settings => {
        settings.Name = "EmailLabel";
        settings.Text = "Email";
        settings.AssociatedControlName = "Email";
    }).Render();
     %>
    <div class="form-field">
        <%: Html.EditorFor(m => m.Email)%>
        <%: Html.ValidationMessageFor(m => m.Email) %>
    </div>
    <% Html.DevExpress().Label(settings => {
        settings.Name = "PasswordLabel";
        settings.Text = "Password";
        settings.AssociatedControlName = "Password";
    }).Render();
     %>
    <div class="form-field">
        <%: Html.EditorFor(m => m.Password)%>
        <%: Html.ValidationMessageFor(m => m.Password) %>
    </div>
    <% Html.DevExpress().Label(settings => {
        settings.Name = "ConfirmPasswordLabel";
        settings.Text = "Confirm Password";
        settings.AssociatedControlName = "ConfirmPassword";
    }).Render();
     %>
    <div class="form-field">
        <%: Html.EditorFor(m => m.ConfirmPassword)%>
        <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
    </div>
     <% Html.DevExpress().Button(settings => {
        settings.Name = "Button";
        settings.Text = "Register";
        settings.UseSubmitBehavior = true;
    }).Render(); %>
<% } %>
</asp:content>