MyWebApp.namespace("UI.User");

MyWebApp.UI.User = (function () {
    "use strict";
    var _isInitialized = false;
   
    var usersData;
    var temp_roleData;
    var current_page = 0;

    function initialisePage() {
        if (_isInitialized == false) {
            _isInitialized = true;
            BindEvents();
            SearchUsers();
            getActiveRoles();
        }
    }

    function BindEvents() {
        
        $("#cmbPageSizeSearch").change(function(e){
            e.preventDefault();
            SearchUsers();
            return false;
        });

        $("#newuser").click(function (e) {
            e.preventDefault();
            clearFeilds();
            $('#modal-form').modal('show');
        });

        $("#Save").unbind('click').bind('click', function (e) {
            e.preventDefault();
            
            if ($("#txtName").val().trim() == "" 
                || $('#txtEmail').val().trim() == ""
                || $('#txtLogin').val().trim() == ""
                ) {
                MyWebApp.UI.showRoasterMessage("Empty Field(s)", Enums.MessageType.Error, 2000);
                return;
            }
            else {
                $('#modal-form').modal('hide');

                MyWebApp.Globals.ShowYesNoPopup({
                    headerText: "Save",
                    bodyText: 'Do you want to Save this record?',
                    dataToPass: { 
                        UserId: $("#hiddenid").val(),
                        Name: $("#txtName").val().trim(),
                        Email: $('#txtEmail').val().trim(),
                        Login: $('#txtLogin').val().trim()
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
            $('#modal-form').modal('hide');
            return false;
        });

        $("#SaveMappings").unbind('click').bind('click',function(){
            SaveRoleMapping();
            return false;
        });

        $("#search").unbind("click").bind("click", function (e) {
            e.preventDefault();
            current_page = 0;
            SearchUsers();
            return false;
        });
    }
    
    //-----------------User Management + Grid Event Handling
    function SearchUsers(){
        if(current_page == 0)
            current_page = 1;

        var searchObject = {
            TextToSearch: $("#txtTextToSearch").val().trim(),
            IsActive:  $("#cmbIsActiveSearch").val(),
            PageSize: $("#cmbPageSizeSearch").val(),
            PageIndex: current_page - 1
        };

        var url = "Security/SearchUsers";
        var Data = JSON.stringify(searchObject);

        MyWebApp.Globals.MakeAjaxCall("POST", url, Data, function (result) {
            if (result.success === true) {
                usersData = result.data;
                $("#spResultsFound").text("(" + result.data.Count + " Users are found)");
                CreatePages(result.data.Count);
                displayAllUsers(result.data);
            } else {
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Error);
            }
        }, function (xhr, ajaxOptions, thrownError) {
            MyWebApp.UI.showRoasterMessage('A problem has occurred while getting applications: "' + thrownError + '". Please try again.', Enums.MessageType.Error);
        });
    }
    function CreatePages(recordCount){
        var pageSize = Number($("#cmbPageSizeSearch").val());

        if (isNaN(pageSize) || pageSize <= 0) {
            alert('Invalid Page Size');
            return;
        }
        var pages = Math.ceil(recordCount / pageSize);
        CreateNavButtonForPage(pages);
    }
    function CreateNavButtonForPage(pageNo) {
        $(".pagination").empty();
        for (var i = 1; i <= pageNo; i++) {
            var li = $("<li>").attr("id", "pageNo").attr("pageNo1", i).attr("value", i).attr("style", "color:white");
            if(i == current_page)
                li.addClass("active");
            var i1 = $("<a>").text(i);
            li.append(i1);
            $(".pagination").append(li);
        }

        $(".pagination #pageNo").unbind("click").bind("click", function (e) {
            var pgNo = $(this).closest("li").attr("pageNo1");
            if(current_page != pgNo){
                current_page = pgNo;
                SearchUsers();
            }
                
            return false;
        });
    }
    function saveUser(dataObj) {
        debugger;
        var userObjToSend = {
            UserId: dataObj.UserId,
            Name: dataObj.Name,
            Email: dataObj.Email,
            Login: dataObj.Login
        }

        var dataToSend = JSON.stringify(userObjToSend);
        var url = "Security/SaveUsers";
        MyWebApp.Globals.MakeAjaxCall("POST", url, dataToSend, function (result) {

            if (result.success === true) {
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Success, 2000);

                var userObj = usersData.UserList.find(p => p.UserId == userObjToSend.UserId);
                
                if(userObj){
                    userObj.Name = userObjToSend.Name;
                    userObj.Description = userObjToSend.Description;
                }
                //else {
                //    userObjToSend.UserId = result.data.UserId;
                //    userObjToSend.IsActive = true;
                //    usersData.UserList.push(userObjToSend);
                //}
              
                displayAllUsers(usersData);

            } else {
                MyWebApp.UI.showRoasterMessage('some error has occurred', Enums.MessageType.Error);
            }
            $('#modal-form').modal('hide');
        }, function (xhr, ajaxoptions, thrownerror) {
            MyWebApp.UI.showMessage("#spstatus", 'A problem has occurred while saving this User: "' + thrownerror + '". Please try again.', Enums.MessageType.Error);
        });

    }
    function clearFeilds() {
        
        $("#hiddenid").val("");
        $("#txtName").val("");
        $("#txtEmail").val("");
        $("#txtLogin").val("");
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
                
                var userObj = usersData.UserList.find(p => p.UserId == userData.UserId);
                userObj.IsActive = userData.IsActive;
               
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Success, 2000);

                displayAllUsers(usersData);

            } else {
                MyWebApp.UI.showRoasterMessage('some error has occurred', Enums.MessageType.Error);                
            }
        }, function (xhr, ajaxoptions, thrownerror) {
            MyWebApp.UI.showMessage("#spstatus", 'A problem has occurred while saving this User: "' + thrownerror + '". Please try again.', Enums.MessageType.Error);
        });
    }
    
    function displayAllUsers(UserList) {

        MyWebApp.UI.Common.setHandlebarTemplate("#UserTemplate","#simple-table",UserList,false,BindGridEvents);
    }
    

    function BindGridEvents(){
        $("#simple-table .lnkEdit" ).unbind('click').bind('click', function(){
            var id = $(this).closest('tr').attr('id');
            debugger;
            HandleEditUser(id);
            return false;
        });

        $("#simple-table .lnkDelete" ).unbind('click').bind('click', function(){
            debugger;
            var id = $(this).closest('tr').attr('id');
            HandleEnableDisableUser(id);
            return false;
        });

        $("#simple-table .lnkEditMapping" ).unbind('click').bind('click', function(){
            var id = $(this).closest('tr').attr('id');
            GenerateRolesHTML(temp_roleData);
            GetRolesByUserID(id);
            
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
                $("#txtName").val(userObj.Name);
                $("#txtEmail").val(userObj.Email);
                $("#txtLogin").val(userObj.Login);
           
                $('#modal-form').modal('show');

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

    //-------------------------- Roles/Mappings Code

    function getActiveRoles() {

        var url = "Security/getActiveRoles";
        MyWebApp.Globals.MakeAjaxCall("GET", url, "{}", function (result) {
            if (result.success === true) {
                temp_roleData = result.data;
                GenerateRolesHTML(temp_roleData);
            } else {
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Error);
            }
        }, function (xhr, ajaxOptions, thrownError) {
            MyWebApp.UI.showRoasterMessage('A problem has occurred while getting Roles: "' + thrownError + '". Please try again.', Enums.MessageType.Error);
        }, false);
    }
    function GenerateRolesHTML(Roles) {
        MyWebApp.UI.Common.setHandlebarTemplate("#RoleTemplate","#sortable1",Roles);
    }
    function GetRolesByUserID(pUserID){

        var url = "Security/GetRolesByUserID?pUserID=" + pUserID;
        MyWebApp.Globals.MakeAjaxCall("GET", url, "{}", function (result) {
            if (result.success === true) {
                SelectRolesInPopupForCurrentUser(result.data.Roles,pUserID);
            } else {
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Error);
            }
        }, function (xhr, ajaxOptions, thrownError) {
            MyWebApp.UI.showRoasterMessage('A problem has occurred while getting Roles: "' + thrownError + '". Please try again.', Enums.MessageType.Error);
        }, false);
    }
    function SelectRolesInPopupForCurrentUser(permissionIds,pUserID){

        $("#sortable1 li").each(function(){
            var id = parseInt($(this).attr("id"));
            if( permissionIds.indexOf(id) >= 0)
                $(this).find("input[type=checkbox]").attr("checked",true);
        });

        $("#EditRolesModal").data('UserID',pUserID);

        $('#EditRolesModal').modal('show');
    }

    function SaveRoleMapping(){
        var permList = [];
        $("#sortable1 li :checked").each(function(){
            var permId = $(this).closest('li').attr("id");
            permList.push(parseInt(permId));
        });

        var userID = $("#EditRolesModal").data('UserID');
        var dataToSend = {};
        dataToSend.UserID = userID;
        dataToSend.Roles = permList;

        dataToSend = JSON.stringify(dataToSend);
        var url = "Security/SaveUserRoleMapping";
        MyWebApp.Globals.MakeAjaxCall("POST", url, dataToSend, function (result) {

            if (result.success === true) {
                MyWebApp.UI.showRoasterMessage(result.error, Enums.MessageType.Success, 2000);
                debugger;
                $('#EditRolesModal').modal('hide');
            } else {
                MyWebApp.UI.showRoasterMessage('some error has occurred', Enums.MessageType.Error);
            }
        }, function (xhr, ajaxoptions, thrownerror) {
            MyWebApp.UI.showMessage("#spstatus", 'A problem has occurred while saving: "' + thrownerror + '". Please try again.', Enums.MessageType.Error);
        });
        
    }

    return {

        readyMain: function () {
            initialisePage();

        }
    };
}
());