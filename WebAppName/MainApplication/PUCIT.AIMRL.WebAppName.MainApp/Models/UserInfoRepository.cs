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
using PUCIT.AIMRL.WebAppName.Entities.Security;
using PUCIT.AIMRL.WebAppName.Entities.DBEntities;
using PUCIT.AIMRL.WebAppName.MainApp.Security;
using PUCIT.AIMRL.WebAppName.Entities;
using PUCIT.AIMRL.Common;


namespace PUCIT.AIMRL.WebAppName.MainApp.Models
{
    public class UserInfoRepository
    {
        private PRMDataService _dataService;
        public UserInfoRepository()
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

        public ResponseResult ValidateUser(String login, String pPassword, String pEmail, Boolean pIgnorePassword, Boolean pLoginAsOtherUser)
        {
            //Object dataToReturn = null;
            //Check to see if the user is provided the rights on the application
            try
            {

                var ipAddress = HttpContext.Current.Request.UserHostAddress.ToString();
                var currTime = DateTime.UtcNow;

                if (pLoginAsOtherUser == true && SessionManager.IsUserLoggedIn == true)
                {
                    SessionManager.LogsInAsOtherUser = true;
                    SessionManager.ActualUserUserID = SessionManager.CurrentUser.UserId;
                    SessionManager.ActualUserLoginID = SessionManager.CurrentUser.Login;
                }
                else
                {
                    SessionManager.LogsInAsOtherUser = false;
                    SessionManager.ActualUserUserID = 0;
                    SessionManager.ActualUserLoginID = "";
                }
                if (pEmail != "")
                {
                    pIgnorePassword = true;
                }
                else
                {
                    if (GlobalDataManager.IgnoreHashing == false)
                        pPassword = PasswordSaltedHashingUtility.HashPassword(pPassword);
                }
                var secUserForSession = DataService.ValidateUserSP(login, pPassword,pEmail, currTime, ipAddress, pIgnorePassword, SessionManager.ActualUserLoginID);

                if (secUserForSession != null)
                {
                    if (secUserForSession.IsActive==false)
                    {
                        CustomUtility.LogData("User Is Inactive, can't log in");
                        SessionManager.CurrentUser = null;
                        return ResponseResult.GetErrorObject("Your account is not active, Please Contact Administrator");
                        
                    }
                    else if (secUserForSession.IsDisabledForLogin == true)
                    {
                        SessionManager.CurrentUser = null;
                        return ResponseResult.GetErrorObject("Your account is disabled, Please Contact Administrator");
                    }
                    else
                    {
                        PermissionManager.HandlePermissions(secUserForSession.Permissions);
                        secUserForSession.Permissions = null;


                        SessionManager.CurrentUser = secUserForSession;

                        var RedirectURl = Resources.PAGES_MANAGERS_DEFAULT_HOME_PAGE;
                        RedirectURl = RedirectURl.Replace("~/", "");
                        
                        return ResponseResult.GetSuccessObject(new
                        {
                            redirect = RedirectURl
                        });
                    }
                }

                else
                {
                    //If the user was not detected as an authorized user
                    CustomUtility.LogData("Invalid Login: " + login + " Password: " + pPassword);
                    SessionManager.CurrentUser = null;
                    return ResponseResult.GetErrorObject("Invalid Login/Password");
                }
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                SessionManager.CurrentUser = null;
                return ResponseResult.GetErrorObject();
            }
        }

        public Boolean IsValidResetToken(String pReset_Token)
        {
            return DataService.IsValidResetToken(pReset_Token);
        }

        //public Object UpdateDesign(int aid)
        //{
        //    var returnObj = (new
        //    {
        //        success = false,
        //        error = "Invalid Request"
        //    });

        //    try
        //    {

        //        Boolean flag = false;
        //        var secUserForSession = SessionManager.CurrentUser;

        //        if (secUserForSession.ApproverDesignations.Count > 0 && secUserForSession.IsContributor)
        //        {
        //            var desig = secUserForSession.ApproverDesignations.Where(p => p.ApproverID == aid).FirstOrDefault();
        //            if (desig != null)
        //            {
        //                var rolesList = new List<String>();
        //                var permList = new List<String>();

        //                permList = DataService.GetRolePermissionById(aid, out rolesList);

        //                PermissionManager.HandlePermissions(permList);

        //                secUserForSession.Roles = rolesList;

                        
        //                flag = true;
        //            }
        //        }

        //        if (flag)
        //        {
        //            SessionManager.CurrentUser = secUserForSession;
        //            return (new
        //            {
        //                success = true,
        //                error = ""
        //            });
        //        }
        //        else
        //        {
        //            return returnObj;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return returnObj;
        //    }
        //}

        public ResponseResult SendEmail(string email_login)
        {
            if (PUCIT.AIMRL.WebAppName.UI.Common.SessionManager.LogsInAsOtherUser == true)
            {
                return ResponseResult.GetErrorObject("You are not allowed");
            }
            try
            {
                var ipAddress = CustomUtility.GetUserIPAddress();
                var currTime = DateTime.UtcNow;

                String token = Guid.NewGuid().ToString();

                String url = PUCIT.AIMRL.WebAppName.UI.Common.Resources.GetCompletePath("~/Login/ResetPassword1");
                url = String.Format("{0}?rt={1}", url, token);
                String userEmail = DataService.UpdateResetToken(email_login, token, ipAddress, currTime, url);

                if (!String.IsNullOrEmpty(userEmail))
                {
                    String subject = "Reset Password";
                    String msg = String.Format("Dear User, <br> Open the following link or copy and open in browser to reset your password <br><br> <a href='{0}' target='_blank'>{0}</a> <br><br> If you hadn't generated this request, Kindly ignore it.", url);

                    var flag = GlobalDataManager._emailhandler.SendEmail(new EmailMessageParam()
                    {
                        ToIDs = userEmail,
                        Subject = subject,
                        Body = msg
                    });
                    return ResponseResult.GetSuccessObject(new
                    {
                        Id = email_login
                    });
                    
                }
                else
                {
                    return ResponseResult.GetErrorObject("Unable to recognize provided Login/Email ID");
                }
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject("Email not correct");
            }
        }

        //public Object SignOut(Boolean pManualEclockLogout)
        //{
        //    try
        //    {
        //        SessionManager.CurrentUser = null;
        //        SessionManager.AbandonSession();
        //        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
        //        HttpContext.Current.Response.Cache.SetNoStore();

        //        if (HttpContext.Current.Request.Cookies["breadcrumbs"] != null)
        //        {
        //            HttpCookie myCookie = new HttpCookie("breadcrumbs");
        //            myCookie.Expires = DateTime.UtcNow.AddDays(-1d);
        //            HttpContext.Current.Response.Cookies.Add(myCookie);
        //        }

        //        return ResponseResult.GetSuccessObject();

        //        //var result = new
        //        //{
        //        //    success = true,
        //        //    error = ""
        //        //};

        //        //return (result);

        //    }
        //    catch (Exception ex)
        //    {
        //        CustomUtility.HandleException(ex);
        //        return ResponseResult.GetErrorObject("Email not correct");
        //    }
        //}

        public ResponseResult ResetPassword(PasswordEntity pass)
        {
            if (PUCIT.AIMRL.WebAppName.UI.Common.SessionManager.LogsInAsOtherUser == true)
            {
                return ResponseResult.GetErrorObject("You Are Not Allowed");
            }
            try
            {
                var password = pass.NewPassword;
                if (GlobalDataManager.IgnoreHashing == false)
                {
                    password = PasswordSaltedHashingUtility.HashPassword(pass.NewPassword);
                }

                var flag = DataService.UpdatePassword(pass.Token, "", password, 0, DateTime.UtcNow, false);

                if (flag)
                {
                    return ResponseResult.GetSuccessObject(null, "Password is reset");
                }
                else
                {
                    return ResponseResult.GetErrorObject("Reset is failed");
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