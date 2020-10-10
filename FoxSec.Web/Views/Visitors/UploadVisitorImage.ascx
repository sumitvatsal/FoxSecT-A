<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<int>" %>
<style>
    #upload_process {
        width: 100%;
        height: 10vh;
        background: #fff url('../../img/loader.gif') no-repeat center center;
        z-index: 9999;
    }
</style>
<form action="<%=Url.Action("SaveVisitorImage", "Visitors")%>?Id=<%=Model%>" enctype="multipart/form-data" method="post">
    <div id="upload_process" style="display: none"></div>

    <div id='upload_content1'>
        <input type="file" name="fileUpload" style="font-size: 8pt;" />
        <label id="divupload"></label>
    </div>
    <iframe id="upload_target" name="upload_target" src="#" style="width: 0; height: 0; border: 0px solid #fff;"></iframe>
    <hr />
    <script>
        $(document).ready(function () {

            $("#divupload").html('');
            $("#divupload").append('<input type="submit" value="Upload" id="btnupload" disabled="disabled" style="font-size: 9pt;" />');
            $("input:submit").button();
        });

        $("input[type=file]").change(function () {
            $("#divupload").html('<input type="submit" value="Upload" id="btnupload" style="font-size: 9pt;" />');
            $("input:submit").button();
        });

        $('form').submit(function (event) {
            event.preventDefault();

            if ($(this).valid()) {
                var formdata = new FormData($(this).get(0));
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: formdata,
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        $("div#upload_content1").hide();
                        $("input#btnuploadpic").hide();
                        $("div#upload_process").show();
                    },
                    success: function (result) {
                        var recv = result;
                        if (recv == "0") {
                            ShowDialog('<%=ViewResources.SharedStrings.SelectPhoto %>', 5000);
                            $("div#upload_photo").html("");
                            $("div#upload_content").show();
                            $("div#upload_content1").hide();
                            $("div#upload_process").hide();
                            $("input#btnuploadpic").show();
                        }
                        else if (recv == "1") {
                            ShowDialog('<%=ViewResources.SharedStrings.PhotoUploaded %>', 2000, true);
                            context.clearRect(0, 0, 192, 192);
                            var isIE = /*@cc_on!@*/false || !!document.documentMode;
                            var isEdge = !isIE && !!window.StyleMedia;
                            if (isEdge == true) {
                                context.drawImage(video, 100, 0, 480, 360, 0, 0, 192, 192);
                            }
                            else {
                                context.drawImage(video, 100, 0, 460, 478, 0, 0, 192, 192);
                            }
                            var image = document.getElementById("imgCapture").toDataURL("image/png");
                            image = image.replace('data:image/png;base64,', '');

                            $.ajax({
                                type: 'GET',
                                url: '/Visitors/Edit',
                                cache: false,
                                data: {
                                    id: <%=Model%>
                                    },
                                success: function (html) {
                                    $("div#modal-dialog").html(html);
                                    $("div#upload_photo").html("");
                                    $("div#upload_content").show();
                                    $("div#upload_content1").hide();
                                    $("div#upload_process").hide();
                                    $("input#btnuploadpic").show();
                                }
                            });
                        }
                        else {
                            ShowDialog('<%=ViewResources.SharedStrings.Error %>' + ":" + result, 5000);
                            $("div#upload_photo").html("");
                            $("div#upload_content").show();
                            $("div#upload_content1").hide();
                            $("div#upload_process").hide();
                            $("input#btnuploadpic").show();
                        }
                    },
                    complete: function () {
                    }
                })
            }
            return false;
        });
    </script>
</form>

