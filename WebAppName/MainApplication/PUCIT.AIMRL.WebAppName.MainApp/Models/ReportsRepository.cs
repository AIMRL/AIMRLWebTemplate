using PUCIT.AIMRL.WebAppName.DAL;
using PUCIT.AIMRL.WebAppName.Entities;
using PUCIT.AIMRL.WebAppName.Entities.DBEntities;
using PUCIT.AIMRL.WebAppName.MainApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
namespace PUCIT.AIMRL.WebAppName.MainApp.APIControllers
{
    class ReportsRepository
    {
        private PRMDataService _dataService;
        public ReportsRepository()
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


        public ResponseResult GetLoginHistory()
        {

            try
            {
                var List = DataService.GetLoginHistory();
                return ResponseResult.GetSuccessObject(new
                {
                    LoginHistoryList = List
                });

                //return (new
                //{
                //    data = new
                //    {
                //        LoginHistoryList = List
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

    }
}