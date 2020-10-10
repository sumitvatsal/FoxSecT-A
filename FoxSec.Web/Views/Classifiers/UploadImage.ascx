<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<int>" %>
<style>
    #upload_process {
        width: 100%;
        height: 10vh;
        background: #fff url('../../img/loader.gif') no-repeat center center;
        z-index: 9999;
    }
</style>
<form action="<%=Url.Action("SaveLicenceDetails", "Classifiers")%>?Id=<%=Model%>" enctype="multipart/form-data" method="post">
    <div id="upload_process" style="display: none"></div>

    <div id='upload_content1'>
        <input type="file" name="fileUpload" style="font-size: 8pt;" />
        <label id="divupload"></label>
    </div>
    <iframe id="upload_target" name="upload_target" src="#" style="width: 0; height: 0; border: 0px solid #fff;"></iframe>
    <hr />
    <script>
        $(document).ready(function () {
            //$("div#upload_process").hide();
            //$("input[type=file]").filestyle({
            //    image: "../img/find.png",
            //    imageheight: 24,
            //    imagewidth: 28,
            //    width: 200
            //});

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
                        $("input#insert_new_licence").hide();
                        $("div#upload_process").show();
                        $("#txtlicencepath").attr("disabled", "disabled");
                    },
                    success: function (result) {
                        var recv = result;
                        if (recv == <%=Model%>) {
                            ShowDialog('<%=ViewResources.SharedStrings.LicenceInsert %>', 2000, true);
                            $.get('/Classifiers/ListValues', { id: <%=Model%> }, function (html) {
                                $('div#ClassificatorValuesList').html("");
                                $('div#ClassificatorValuesList').append(html);
                                $("div#upload_photo").html("");
                                $("div#upload_content").show();
                                $("div#upload_content1").hide();
                                $("div#upload_process").hide();
                                $("input#insert_new_licence").show();
                            });
                        }
                        else if (recv == "-1") {
                            ShowDialog('<%=ViewResources.SharedStrings.InvalidLicence %>', 5000);
                            $("div#modal-dialog").dialog("close");
                        }
                        else if (recv == "-2") {
                            ShowDialog('<%=ViewResources.SharedStrings.InvalidLicencePath %>', 5000);
                            $("div#modal-dialog").dialog("close");
                        }
                        else if (recv == "-3") {
                            ShowDialog('<%=ViewResources.SharedStrings.ChooseLicense %>', 5000);
                            $("div#modal-dialog").dialog("close");
                        }
                        else if (recv == "-4") {
                            ShowDialog('<%=ViewResources.SharedStrings.LicenseLessCountAlert %>', 5000, true);
                            $("div#modal-dialog").dialog("close");
                        }
                        else {
                            ShowDialog('<%=ViewResources.SharedStrings.LicenceError %>' + ":" + result, 5000);
                            $("div#modal-dialog").dialog("close");
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

