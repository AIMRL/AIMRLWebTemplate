MyWebApp.namespace("UI.LoginHistoryReport");

MyWebApp.UI.LoginHistoryReport = (function () {
    "use strict";
    var _isInitialized = false;
    var StatusList;
    var ApproverList;
    var StudentList;
    function initialisePage() {
        if (_isInitialized == false) {
            _isInitialized = true;

            viewAllLoginHistory();
        }
    }

    function viewAllLoginHistory() {

        var url = "Reports/getLoginHistory";
        MyWebApp.Globals.MakeAjaxCall("GET", url, "{}", function (result) {
            if (result.success === true) {
                displayAllLoginHistory(result.data);
            } else {
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Error);
            }
        }, function (xhr, ajaxOptions, thrownError) {
            MyWebApp.UI.showRoasterMessage('A problem has occurred while getting Login History: "' + thrownError + '". Please try again.', Enums.MessageType.Error);
        }, false);
    }

    function displayAllLoginHistory(LoginHistoryList) {

        $("#simple-table6").html("");

        if (!LoginHistoryList)
            return;

        try {
            var source = $("#LoginHistoryTemplate").html();
            var template = Handlebars.compile(source);
            var html = template(LoginHistoryList);
        } catch (e) {
            debugger;
        }

        $("#simple-table6").append(html);
    }

    return {

        readyMain: function () {
            initialisePage();
        }
    };
}
());