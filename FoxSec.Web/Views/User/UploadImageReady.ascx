<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>
     <script type="text/javascript" src="../../Scripts/jquery.js"></script>
<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        if ("<%= Model %>" == "../Uploads/") {
            $("div#upload_photo", window.parent.document).hide();
            $("div#upload_process", window.parent.document).hide();
        } else {
            setTimeout(function () {
                $("div#photo_content", window.parent.document).html('');
                $("div#upload_photo", window.parent.document).hide();
                $("div#upload_process", window.parent.document).hide("slow", function () {
                    $("div#photo_content", window.parent.document).html('<table><tr><td align="right"><span class="ui-icon ui-icon-circle-close" style="cursor:pointer;" onclick="javascript:RemoveUserPhoto();"></span></td></tr><tr><td><img alt="" border="0" src="<%= Model %>" style="cursor:pointer;" onclick="javascript:ActivatePhotoUpload();" /></td></tr></table>');
                });
            }, 3000);
        }
    });

</script>