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


        public ResponseResult SearchLoginHistory(LoginHistorySearchParam pSearchParam)
        {
            try
            {
                if (pSearchParam.SDate <= DateTime.MinValue)
                    pSearchParam.SDate = new DateTime(1900, 1, 1);

                if (pSearchParam.EDate <= DateTime.MinValue)
                    pSearchParam.EDate = DateTime.MaxValue;

                var result = DataService.SearchLoginHistory(pSearchParam);

                return ResponseResult.GetSuccessObject(new
                {
                    Count = result.ResultCount,
                    LoginHistoryList = result.Result
                });
                
            }
            catch (Exception ex)
            {
                CustomUtility.HandleException(ex);
                return ResponseResult.GetErrorObject();
            }
        }

        public ResponseResult SearchForgotPasswordLog(ForgotPasswordSearchParam pSearchParam)
        {
            try
            {
                if (pSearchParam.SDate <= DateTime.MinValue)
                    pSearchParam.SDate = new DateTime(1900, 1, 1);

                if (pSearchParam.EDate <= DateTime.MinValue)
                    pSearchParam.EDate = DateTime.MaxValue;

                var result = DataService.SearchForgotPasswordLog(pSearchParam);

                return ResponseResult.GetSuccessObject(new
                {
                    Count = result.ResultCount,
                    ForgotPasswordLogList = result.Result
                });

                //return (new
                //{
                //    data = new
                //    {
                //        Count = result.ResultCount,
                //        ForgotPasswordLogList = result.Result
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