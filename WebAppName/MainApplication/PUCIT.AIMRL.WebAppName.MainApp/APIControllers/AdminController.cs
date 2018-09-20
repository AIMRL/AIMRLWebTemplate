using PUCIT.AIMRL.WebAppName.Entities;
using PUCIT.AIMRL.WebAppName.Entities.DBEntities;
using PUCIT.AIMRL.WebAppName.MainApp.Models;
using PUCIT.AIMRL.WebAppName.MainApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PUCIT.AIMRL.WebAppName.MainApp.APIControllers
{

    //    ->DB side paging(DB Level):
    //        Pass page index, page size & search criteria to your SP and get specific records only by using FETCH, OFFSET feature

    //    ->Background: In admin panels, there is mostly an option for admins to login by another user(without providing password). It is to check how system is working for a specific user.
    //Task: We need to design a screen where we'll provide
    //Autocomplete box(to search any user by name, login or by email id)
    //User will click on "Login as" button and same login process will work as it works for normal user on lo



    public class AdminController : BaseDataController
    {
        //
        // GET: /Admin/

        private readonly AdminRepository _repository;

        public AdminController()
        {
            _repository = new AdminRepository();
        }
        private AdminRepository Repository
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
                UserInfoRepository userInfoRepo = new UserInfoRepository();
                return userInfoRepo.ValidateUser(pLogin.UserName, "", "", true, true);
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }

        public class Login
        {
            public String UserName { get; set; }
        }

        [HttpGet]
        public ResponseResult SearchUser(string key)
        {

            return Repository.SearchUser(key);
        }
    }
}
