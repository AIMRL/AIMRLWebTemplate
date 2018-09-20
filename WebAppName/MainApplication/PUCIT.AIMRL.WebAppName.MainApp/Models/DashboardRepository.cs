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
            if (PUCIT.AIMRL.WebAppName.UI.Common.SessionManager.LogsInAsOtherUser == true)
            {
                return ResponseResult.GetErrorObject("You Are Not Allowed");
                //return (new
                //{
                //    success = false,
                //    error = "You Are Not Allowed"
                //});
            }
            try
            {
                var id = DataService.changePassword(pass);
                if (id == 0)
                {

                    return ResponseResult.GetErrorObject("Password Change Failure.");
                    //return (new
                    //{
                    //    data = new
                    //    {
                    //        Id = id
                    //    },
                    //    success = false,
                    //    error = "Wrong Password"
                    //});
                }
                else
                {
                    return ResponseResult.GetSuccessObject(new
                    {
                        Id = id
                    }, "Password is changed");

                    //return (new
                    //{
                    //    data = new
                    //    {
                    //        Id = id
                    //    },
                    //    success = true,
                    //    error = "Password Changed"
                    //});
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