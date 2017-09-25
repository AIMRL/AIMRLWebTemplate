MyWebApp.namespace("UI.User");

MyWebApp.UI.User = (function () {
    "use strict";
    var _isInitialized = false;
   
    var usersData;

    function initialisePage() {
        if (_isInitialized == false) {
            _isInitialized = true;
            BindEvents();
            getAllUsers();
        }
    }

    function BindEvents() {
        
        $("#selectType1").change(function (e) {
            e.preventDefault();
            debugger;
            var selected_dropdownindex1 = parseInt($('#selectType1').val());
            var data = {};
            data.UserList = [];

            if (selected_dropdownindex1 == -1) {
                data = usersData;
            }
            else {
                data.UserList = usersData.UserList.filter(p=> p.IsActive == Boolean(selected_dropdownindex1));
            }

            displayAllUsers(data);

        }); //End of Save Click
        
        $("#newuser").click(function (e) {
            e.preventDefault();
            clearFeilds();
            $.bsmodal.show("#modal-form");
        });

        $("#Save").unbind('click').bind('click', function (e) {
            e.preventDefault();
            
            if ($("#username").val() == "" || $('#userdescription').val() == "") {
                MyWebApp.UI.showRoasterMessage("Empty Field(s)", Enums.MessageType.Error, 2000);
            }
            else {
                $.bsmodal.hide("#modal-form");

                MyWebApp.Globals.ShowYesNoPopup({
                    headerText: "Save",
                    bodyText: 'Do you want to Save this record?',
                    dataToPass: { 
                        UserId: $("#hiddenid").val(),
                        Name: $("#username").val(),
                        Description: $('#userdescription').val()
                    },
                    fnYesCallBack: function ($modalObj, dataObj) {
                        saveUser(dataObj);
                        $modalObj.hideMe()
                    }
                });
            }
            return false;
        });

        $("#ModalClose, #Cancel").click(function (e) {
            e.preventDefault();
            hideAll();
            return false;
        });
    }

    function saveUser(dataObj) {
        debugger;
        var userObjToSend = {
            UserId: dataObj.UserId,
            Name: dataObj.Name,
            Description: dataObj.Description
        }

        var dataToSend = JSON.stringify(userObjToSend);
        var url = "Security/SaveUser";
        MyWebApp.Globals.MakeAjaxCall("POST", url, dataToSend, function (result) {

            if (result.success === true) {
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Success, 2000);
                hideAll();

                var userObj = usersData.UserList.find(p => p.UserId == userObjToSend.Id);
                debugger;

                if(userObj){
                    userObj.Name = userObjToSend.Name;
                    userObj.Description = userObjToSend.Description;
                }
                else {
                    userObjToSend.Id = result.data.UserId;
                    userObjToSend.IsActive = true;
                    usersData.UserList.push(userObjToSend);
                }
              
                $("#selectType1").trigger("change");

            } else {
                MyWebApp.UI.showRoasterMessage('some error has occurred', Enums.MessageType.Error);
                hideAll();
            }
        }, function (xhr, ajaxoptions, thrownerror) {
            MyWebApp.UI.showMessage("#spstatus", 'A problem has occurred while saving this User: "' + thrownerror + '". Please try again.', Enums.MessageType.Error);
        });

    }
    function EnableDisableUser(dataObj) {
        var userData = {
            UserId: dataObj.UserId,
            IsActive: !dataObj.IsActive
        }

        var dataToSend = JSON.stringify(userData);
        var url = "Security/EnableDisableUser";

        MyWebApp.Globals.MakeAjaxCall("POST", url, dataToSend, function (result) {

            if (result.success === true) {
                
                hideAll();
                var userObj = usersData.UserList.find(p => p.UserId == userData.UserId);
                userObj.IsActive = userData.IsActive;
               
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Success, 2000);

                $("#selectType1").trigger("change");

            } else {
                MyWebApp.UI.showRoasterMessage('some error has occurred', Enums.MessageType.Error);
                hideAll();
            }
        }, function (xhr, ajaxoptions, thrownerror) {
            MyWebApp.UI.showMessage("#spstatus", 'A problem has occurred while saving this User: "' + thrownerror + '". Please try again.', Enums.MessageType.Error);
        });
    }
    function getAllUsers() {

        var url = "Security/getUsers";
        MyWebApp.Globals.MakeAjaxCall("GET", url, "{}", function (result) {
            if (result.success === true) {
                displayAllUsers(result.data);
                usersData = result.data;

            } else {
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Error);
            }
        }, function (xhr, ajaxOptions, thrownError) {
            MyWebApp.UI.showRoasterMessage('A problem has occurred while getting Users: "' + thrownError + '". Please try again.', Enums.MessageType.Error);
        }, false);

    }
    function displayAllUsers(UserList) {

        $("#simple-table").html("");

        if (!UserList)
            return;

        try {
            var source = $("#UserTemplate").html();
            var template = Handlebars.compile(source);
            var html = template(UserList);
        } catch (e) {
            debugger;
        }

        $("#simple-table").append(html);
        BindGridEvents();
    }
    function BindGridEvents(){
        $("#simple-table .lnkEdit" ).unbind('click').bind('click', function(){
            debugger;
            var id = $(this).closest('tr').attr('id');
            HandleEditUser(id);
            return false;
        });

        $("#simple-table .lnkDelete" ).unbind('click').bind('click', function(){
            debugger;
            var id = $(this).closest('tr').attr('id');
            HandleEnableDisableUser(id);
            return false;
        });
        
    }
 
    function HandleEditUser(UserId){

        if (usersData ) {
            var userObj = usersData.UserList.find(p => p.UserId == UserId);
            if(userObj){
                if(userObj.IsActive == false){
                    MyWebApp.UI.showRoasterMessage("Editing is not allowed as record is disabled", Enums.MessageType.Error);
                    return false;
                }

                $("#hiddenid").val(userObj.UserId);
                $("#Username").val(userObj.Login);
                $("#Name").val(userObj.Name);
                $("#Email").val(userObj.Email);
           
                $.bsmodal.show("#modal-form");
            }//end of userObj
        }//end of usersData
    }

    function HandleEnableDisableUser(UserId){

        if (usersData ) {
            var userObj = usersData.UserList.find(p => p.UserId == UserId);
            if(userObj){
                
                var header = (userObj.IsActive == false ? "Enable" : "Disable");
                
                MyWebApp.Globals.ShowYesNoPopup({
                    headerText: header,
                    bodyText: 'Do you want to ' + header + ' this record?',
                    dataToPass: { "UserId": userObj.UserId, "IsActive" : userObj.IsActive },
                    fnYesCallBack: function ($modalObj,dataObj) {
                        EnableDisableUser(dataObj);
                        $modalObj.hideMe();
                    }
                });
            }//end of userObj
        }//end of usersData
    }

    function hideAll() {
        $.bsmodal.hide("#modal-form");
        clearFeilds();
    }

    function clearFeilds() {
        
        $("#hiddenid").val("");
        //$("#IsActive2").val("-1");
        $("#username").val("");
        $("#userdescription").val("");
    }

    return {

        readyMain: function () {
            debugger;
            initialisePage();

        }
    };
}
());