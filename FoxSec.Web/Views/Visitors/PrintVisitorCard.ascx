<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.VisitorEditViewModel>" %>
<meta content="text/html; charset=ISO-8859-1"
    http-equiv="content-type">
<style>
    @page {
        size: auto; /* auto is the initial value */
        margin: 0mm; /* this affects the margin in the printer settings */
    }
</style>
<div id='create_dialog_content' style="display: none">
    <div id='panel_create_user'>
        <div id='tab_user_default'>
            <div id='create_user_personal_data'>
                <div id='tab_create_user_personal_data'>
                    <div id="printdiv" align="center">
                        <embed src="data:application/pdf;base64, <%: ViewBag.ImageData1 %>" type="application/pdf" width="100%" height="350" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script>
    $(document).ready(function () {
        $("#panel_create_user").attr("id", function () {
            $("#panel_create_user").tabs({
                beforeLoad: function (event, ui) {
                    ui.ajaxSettings.async = false,
                        ui.ajaxSettings.error = function (xhr, status, index, anchor) {
                            $(anchor.hash).html("Couldn't load this tab!");
                        }
                },
                fx: {
                    opacity: "toggle",
                    duration: "fast"
                },
                active: 0
            });
        });

        $("input:button").button();


        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });

        $("div#create_dialog_content").fadeIn("300");

        return false;
    });

    function PrintElem(elem) {
        Popup($(elem).html());
    }

    function Popup(data) {
        var mywindow = window.open('', '', 'height=400,width=600');
        mywindow.document.write('</head><body>');
        mywindow.document.write(data);
        mywindow.document.write('</body></html>');
        mywindow.print();
        mywindow.close();
        return true;
    }
</script>
