using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Collections;

using PUCIT.AIMRL.WebAppName.DAL;
using PUCIT.AIMRL.WebAppName.MainApp.Util;
using PUCIT.AIMRL.WebAppName.UI.Common;
using PUCIT.AIMRL.WebAppName.Entities;
using PUCIT.AIMRL.WebAppName.Entities.DBEntities;
using PUCIT.AIMRL.WebAppName.Entities.Enum;
using PUCIT.AIMRL.Common;

namespace PUCIT.AIMRL.WebAppName.MainApp.Models
{
    public class DashboardRepository
    {
        private PRMDataService _dataService;
        public DashboardRepository()
        {

        }

        private PRMDataService DataService
        {
            get
            {
                if (_dataService == null)
                    _dataService = new PRMDataService();

                return _dataService;
            }
        }

        public ResponseResult ChangePassword(PasswordEntity pass)
        {
            
            try
            {
                if (PUCIT.AIMRL.WebAppName.UI.Common.SessionManager.LogsInAsOtherUser == true)
                {
                    return ResponseResult.GetErrorObject("You Are Not Allowed");
                }
                if (GlobalDataManager.IgnoreHashing == false)
                {
                    pass.CurrentPassword = PasswordSaltedHashingUtility.HashPassword(pass.CurrentPassword);
                    pass.NewPassword = PasswordSaltedHashingUtility.HashPassword(pass.NewPassword);
                }

                var userObj = SessionManager.CurrentUser;

                var isSuccess = DataService.UpdatePassword(userObj.Login, pass.CurrentPassword, pass.NewPassword, userObj.UserId, DateTime.UtcNow, true);

                if (isSuccess == false)
                {

                    return ResponseResult.GetErrorObject("Password Change Failure.");
                }
                else
                {
                    return ResponseResult.GetSuccessObject(new
                    {
                        Id = isSuccess
                    }, "Password is changed");
                }
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
    }
}