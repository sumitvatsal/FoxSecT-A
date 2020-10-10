<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<int>" %>
<% using (Html.BeginForm("TransferImage", "User", new { userId = Model }, FormMethod.Post, new { enctype = "multipart/form-data", target = "upload_target", onsubmit = "StartUpload();" })) {%>
<div id="upload_process" style="display:none"><img alt="" src="../img/loader.gif" /></div>
<div id='upload_content'><input type="file" name="fileUpload" size="55" style='font-size: 8pt' /><br /><input type="submit" value="Upload" /></div>
<iframe id="upload_target" name="upload_target" src="#" style="width:0;height:0;border:0px solid #fff;"></iframe>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {

        $("input[type=file]").filestyle({
            image: "../img/find.png",
            imageheight: 24,
            imagewidth: 28,
            width: 200
        });

        $("input:submit").button();
    });

    function StartUpload() {
        $("div#upload_content").hide();
        $("div#upload_process").show();
        return false;
    }

</script>
<% } %>