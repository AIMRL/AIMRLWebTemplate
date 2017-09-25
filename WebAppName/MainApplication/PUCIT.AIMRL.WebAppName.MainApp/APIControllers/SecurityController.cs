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
        public Object getRoles()
        {
            return Repository.getRoles();
        }
        [HttpPost]
        public Object SaveRole(Roles r)
        {
            return Repository.SaveRole(r);
        }
        [HttpPost]
        public Object EnableDisableRole(Roles r)
        {
            return Repository.EnableDisableRole(r);
        }
        [HttpGet]
        public Object getMappings()
        {
            return Repository.getMappings();
        }
        [HttpGet]
        public Object getPermissions()
        {
            return Repository.getPermissions();
        }
        [HttpPost]
        public Object UpdateMappings(customUpdateMappings m)
        {
            return Repository.UpdateMappings(m);
        }
        [HttpPost]
        public Object DeleteMappings(int roleid)
        {
            return Repository.DeleteMappings(roleid);
        }
        [HttpPost]
        public Object SavePermission(PermissionsWithRoleID r)
        {
            return Repository.SavePermission(r);
        }
        [HttpPost]
        public Object EnableDisablePermission(PermissionsWithRoleID r)
        {
            return Repository.EnableDisablePermission(r);
        }


        [HttpGet]
        public Object getUsers()
        {
            return Repository.getUsers();
        }

        [HttpPost]
        public object SaveUsers(User u)
        {
            return Repository.SaveUsers(u);
        }

        [HttpPost]
        public object EnableDisableUser(User u)
        {
            return Repository.EnableDisableUser(u);
        }
    }
}
