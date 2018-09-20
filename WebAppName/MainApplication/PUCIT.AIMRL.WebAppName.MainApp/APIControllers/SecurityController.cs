using PUCIT.AIMRL.WebAppName.Entities;
using PUCIT.AIMRL.WebAppName.Entities.DBEntities;
using PUCIT.AIMRL.WebAppName.MainApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace PUCIT.AIMRL.WebAppName.MainApp.APIControllers
{
    public class SecurityController : BaseDataController
    {
        //
        // GET: /Admin/

        private readonly SecurityRepository _repository;

        public SecurityController()
        {
            _repository = new SecurityRepository();
        }
        private SecurityRepository Repository
        {
            get
            {
                return _repository;
            }
        }
        [HttpGet]
        public ResponseResult getRoles()
        {
            return Repository.GetRoles();
        }
        [HttpGet]
        public ResponseResult getActiveRoles()
        {
            return Repository.getActiveRoles();
        }

        [HttpPost]
        public ResponseResult SaveRole(Roles r)
        {
            return Repository.SaveRole(r);
        }
        [HttpPost]
        public ResponseResult EnableDisableRole(Roles r)
        {
            return Repository.EnableDisableRole(r);
        }
        
        [HttpGet]
        public ResponseResult getPermissions()
        {
            return Repository.GetPermissions();
        }

        [HttpGet]
        public ResponseResult getActivePermissions()
        {
            return Repository.GetActivePermissions();
        }
        [HttpGet]
        public ResponseResult GetPermissionsByRoleID(int pRoleID)
        {
            return Repository.GetPermissionsByRoleID(pRoleID);
        }
        [HttpPost]
        public ResponseResult SaveRolePermissionMapping(TempRolePermMapping r)
        {
            return Repository.SaveRolePermissionMapping(r.RoleID, r.Permissions);
        }

        [HttpPost]
        public ResponseResult SavePermission(PermissionsWithRoleID r)
        {
            return Repository.SavePermission(r);
        }
        [HttpPost]
        public ResponseResult EnableDisablePermission(PermissionsWithRoleID r)
        {
            return Repository.EnableDisablePermission(r);
        }


        [HttpPost]
        public ResponseResult SaveUsers(User u)
        {
            return Repository.SaveUsers(u);
        }

        [HttpPost]
        public ResponseResult SearchUsers(UserSearchParam u)
        {
            return Repository.SearchUsers(u);
        }
        [HttpPost]
        public ResponseResult EnableDisableUser(User u)
        {
            return Repository.EnableDisableUser(u);
        }

        [HttpGet]
        public ResponseResult GetRolesByUserID(int pUserID)
        {
            return Repository.GetRolesByUserID(pUserID);
        }
        [HttpPost]
        public ResponseResult SaveUserRoleMapping(TempUserRoleMapping r)
        {
            return Repository.SaveUserRoleMapping(r.UserID, r.Roles);
        }
    }
    public class TempRolePermMapping
    {
        public int RoleID { get; set; }
        public List<int> Permissions { get; set; }
    }

    public class TempUserRoleMapping
    {
        public int UserID { get; set; }
        public List<int> Roles { get; set; }
    }
}
