
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PUCIT.AIMRL.WebAppName.MainApp.Models;
using PUCIT.AIMRL.WebAppName.MainApp.Security;
using PUCIT.AIMRL.WebAppName.MainApp.Utils.HttpFilters;
using PUCIT.AIMRL.WebAppName.Entities;
using PUCIT.AIMRL.WebAppName.MainApp.Util;

namespace PUCIT.AIMRL.WebAppName.MainApp.APIControllers
{
    public class UserInfoDataController : ApiController
    {
        public class Login
        {
            public String UserName { get; set; }
            public String Password { get; set; }
        }

        private readonly UserInfoRepository _repository;
        public UserInfoDataController()
        {
            _repository = new UserInfoRepository();
        }

        private UserInfoRepository Repository
        {
            get
            {
                return _repository;
            }
        }

        [HttpPost]
        public ResponseResult ValidateUser(Login pLogin)
        {
            try
            {
                Util.CustomUtility.LogData("Going to validate Login:" + pLogin.UserName);
                return Repository.ValidateUser(pLogin.UserName, pLogin.Password,false,false);
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
       
        [HttpGet]
        public ResponseResult sendEmail(string emailAddress)
        {
            return Repository.SendEmail(emailAddress);
        }
        //[AuthorizedForWebAPI]
        public ResponseResult resetPassword(PasswordEntity pass)
        {
            return Repository.ResetPassword(pass);
        }

        [AuthorizedForWebAPI]
        [HttpGet]
        public ResponseResult ChangeDesig(int aid)
        {
            return null;
            //return Repository.UpdateDesign(aid);
        }
        
        
    }
}