using PUCIT.AIMRL.Common;
using PUCIT.AIMRL.WebAppName.DAL;
using PUCIT.AIMRL.WebAppName.Entities;
using PUCIT.AIMRL.WebAppName.Entities.DBEntities;
using PUCIT.AIMRL.WebAppName.MainApp.Util;
using PUCIT.AIMRL.WebAppName.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
namespace PUCIT.AIMRL.WebAppName.MainApp.APIControllers
{
    class SecurityRepository
    {
        private PRMDataService _dataService;
        public SecurityRepository()
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

        #region Permissions

        public ResponseResult GetPermissions()
        {
            try
            {
                var List = DataService.GetAllPermissions();
                return ResponseResult.GetSuccessObject(new
                {
                    PermissionList = List
                });

                //return (new
                //{
                //    data = new
                //    {
                //        PermissionList = List
                //    },
                //    success = true,
                //    error = ""
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        public ResponseResult GetActivePermissions()
        {
            try
            {
                var List = DataService.GetAllPermissions().Where(p => p.IsActive == true).ToList();

                return ResponseResult.GetSuccessObject(new
                {
                    PermissionList = List
                });

                //return (new
                //{
                //    data = new
                //    {
                //        PermissionList = List
                //    },
                //    success = true,
                //    error = ""
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        public ResponseResult EnableDisablePermission(PermissionsWithRoleID r)
        {
            String msg = " ";
            try
            {

                bool rowdeleted = DataService.EnableDisablePermission(r.Id, r.IsActive, DateTime.UtcNow, SessionManager.CurrentUser.UserId);

                if (rowdeleted == true)
                {
                    var param = (r.IsActive == false ? "disabled" : "enabled");
                    msg = String.Format("Permission is {0} successfully", param);
                }
                else
                {
                    msg = " ";
                }

                return ResponseResult.GetSuccessObject(new
                {
                    PermssionId = r.Id
                }, msg);

                //return (new
                //{

                //    success = true,
                //    error = msg
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        public ResponseResult SavePermission(PermissionsWithRoleID p)
        {
            String msg = " ";
            try
            {
                var permId = DataService.SavePermission(p, DateTime.UtcNow, SessionManager.CurrentUser.UserId);
                if (p.Id > 0)
                {
                    msg = "Permission Updated Successfully";
                }
                if (p.Id == 0)
                {
                    msg = "Permission Added Successfully";
                }

                return ResponseResult.GetSuccessObject(new
                {
                    PermssionId = permId
                }, msg);

                //return (new
                //{
                //    data = new
                //    {
                //        PermssionId = permId
                //    },
                //    success = true,
                //    error = msg
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        public ResponseResult GetPermissionsByRoleID(int pRoleID)
        {
            try
            {
                var List = DataService.GetPermissionsByRoleID(pRoleID);
                return ResponseResult.GetSuccessObject(new
                {
                    Permissions = List
                });

                //return (new
                //{
                //    data = new
                //    {
                //        Permissions = List
                //    },
                //    success = true,
                //    error = ""
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }

        public ResponseResult SaveRolePermissionMapping(int pRoleID, List<int> pPermissionsList)
        {
            try
            {
                var List = DataService.SaveRolePermissionMapping(pRoleID, pPermissionsList);

                return ResponseResult.GetSuccessObject(new
                {
                    Permissions = List
                }, "Mappings are saved");

                //return (new
                //{
                //    data = new
                //    {
                //        Permissions = List
                //    },
                //    success = true,
                //    error = "Mappings are saved"
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }

        #endregion

        #region Roles
        public ResponseResult GetRoles()
        {
            try
            {
                var List = DataService.GetAllRoles();

                return ResponseResult.GetSuccessObject(new
                {
                    RoleList = List
                });

                //return (new
                //{
                //    data = new
                //    {
                //        RoleList = List
                //    },
                //    success = true,
                //    error = ""
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        public ResponseResult EnableDisableRole(Roles pRoleObj)
        {
            String msg = " ";
            try
            {
                bool rowdeleted = DataService.EnableDisableRole(pRoleObj.Id, pRoleObj.IsActive, DateTime.UtcNow, SessionManager.CurrentUser.UserId);

                if (rowdeleted == true)
                {
                    var param = (pRoleObj.IsActive == false ? "disabled" : "enabled");
                    msg = String.Format("Role is {0} successfully", param);
                }
                else
                {
                    msg = " ";
                }
                return ResponseResult.GetSuccessObject(new
                {
                    RoleId = pRoleObj.Id
                }, msg);

                //return (new
                //{

                //    success = true,
                //    error = msg
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        public ResponseResult SaveRole(Roles r)
        {
            String msg = " ";
            try
            {
                var roleId = DataService.SaveRole(r, DateTime.UtcNow, SessionManager.CurrentUser.UserId);
                if (r.Id > 0)
                {
                    msg = "Role Updated Successfully";
                }
                if (r.Id == 0)
                {
                    msg = "Role added Successfully";
                }

                return ResponseResult.GetSuccessObject(new
                {
                    RoleId = roleId
                }, msg);

                //return (new
                //{
                //    data = new
                //    {
                //        RoleId = roleId
                //    },
                //    success = true,
                //    error = msg
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        public ResponseResult getActiveRoles()
        {
            try
            {
                var List = DataService.GetAllRoles().Where(p => p.IsActive == true).ToList();

                return ResponseResult.GetSuccessObject(new
                {
                    RoleList = List
                });

                //return (new
                //{
                //    data = new
                //    {
                //        RoleList = List
                //    },
                //    success = true,
                //    error = ""
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }

        public ResponseResult GetRolesByUserID(int pUserID)
        {
            try
            {
                var List = DataService.GetRolesByUserID(pUserID);
                return ResponseResult.GetSuccessObject(new
                {
                    Roles = List
                });
                //return (new
                //{
                //    data = new
                //    {
                //        Roles = List
                //    },
                //    success = true,
                //    error = ""
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }

        public ResponseResult SaveUserRoleMapping(int pUserID, List<int> pRoles)
        {
            try
            {
                var List = DataService.SaveUserRoleMapping(pUserID, pRoles);
                return ResponseResult.GetSuccessObject(new
                {
                    Roles = List
                }, "Mappings are saved");

                //return (new
                //{
                //    data = new
                //    {
                //        Roles = List
                //    },
                //    success = true,
                //    error = "Mappings are saved"
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        #endregion


        #region Users

        public ResponseResult EnableDisableUser(User pUserObj)
        {
            try
            {
                String msg = "";
                var result = DataService.EnableDisableUser(pUserObj.UserId, pUserObj.IsActive, DateTime.UtcNow, SessionManager.CurrentUser.UserId);
                if (result == true)
                {
                    var param = (pUserObj.IsActive == false ? "disabled" : "enabled");
                    msg = String.Format("User is {0} successfully", param);
                }
                else
                {
                    msg = "";
                }


                if (result == true)
                {
                    return ResponseResult.GetSuccessObject(new
                    {
                        UserList = result
                    }, msg);

                    //return (new
                    //{
                    //    data = new
                    //    {
                    //        UserList = result
                    //    },
                    //    success = true,
                    //    error = msg
                    //});

                }
                else
                {
                    return ResponseResult.GetErrorObject();
                }

            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }

        }

        public ResponseResult SaveUsers(User u)
        {
            try
            {
                String msg;
                u.Password = PasswordSaltedHashingUtility.HashPassword("123");           
                var result = DataService.SaveUsers(u, DateTime.UtcNow, SessionManager.CurrentUser.UserId);
                if (result > 0)
                {
                    if (u.UserId > 0)
                    {
                        msg = "User Updated Successfully";
                    }
                    else
                    {
                        msg = "User Added Successfully";
                    }
                    return ResponseResult.GetSuccessObject(new
                    {
                        UserId = result,
                    }, msg);
                }
                else
                {
                    return ResponseResult.GetErrorObject();
                }
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }

        public ResponseResult getUsers()
        {
            try
            {
                var List = DataService.GetAllUsers();
                return ResponseResult.GetSuccessObject(new
                {
                    UserList = List
                });
                //return (new
                //{
                //    data = new
                //    {
                //        UserList = List
                //    },
                //    success = true,
                //    error = ""
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }

        public ResponseResult SearchUsers(UserSearchParam pSearchParam)
        {
            try
            {
                var result = DataService.SearchUsers(pSearchParam);
                return ResponseResult.GetSuccessObject(new
                {
                    Count = result.ResultCount,
                    UserList = result.Result
                });
                //return (new
                //{
                //    data = new
                //    {
                //        Count = result.ResultCount,
                //        UserList = result.Result
                //    },
                //    success = true,
                //    error = ""
                //});
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }
        #endregion
    }
}