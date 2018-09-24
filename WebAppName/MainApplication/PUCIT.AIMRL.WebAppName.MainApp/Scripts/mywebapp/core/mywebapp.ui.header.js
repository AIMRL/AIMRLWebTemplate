MyWebApp.namespace("UI.Header");

MyWebApp.UI.Header = (function () {
    "use strict";
    var _isInitialized = false;

    function initialisePage() {
        if (_isInitialized == false) {
            _isInitialized = true;
            BindEvents();
        }
    }
    function BindEvents() {
        
        $("#lnkLoginAs").unbind('click').bind('click', function (e) {
            e.preventDefault();
            $('#divLoginAs').modal('show');

            return false;
        });

        $("#lnkProfileModal").unbind('click').bind('click', function (e) {
            e.preventDefault();

            $.bsmodal.show("#profileModal", { top: "5%", left: "25%", closeid: "#btnCloseProfileModal,#btnCloseProfileModal2" });

            return false;
        });

        
        $("#SearchButton").click(function (e) {
            debugger;
            var number = $("#SearchDiary").val();
            window.location = window.MyWebAppBasePath + "Home/ApplicationView/" + number;
        });

        $(".user-menu .designation a").click(function (e) {
            e.preventDefault();

            if ($(this).hasClass("selected"))
                return false;

            var aid = $(this).attr("aid");

            var url = "UserInfoData/ChangeDesig?aid=" + aid;

            MyWebApp.Globals.MakeAjaxCall("GET", url, {}, function (result) {
                
                if (result.success == true) {
                    MyWebApp.UI.ShowLastMsgAndRefresh("Changed successfully.");
                } else {
                    MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Error);
                }
            }, function (xhr, ajaxoptions, thrownerror) {
                MyWebApp.UI.showRoasterMessage('A problem has occurred."' + thrownerror + '". please try again.', Enums.MessageType.Error);
            });

            return false;
        });
    }

    return {

        readyMain: function () {
            initialisePage();
        }
    };
}
    ());