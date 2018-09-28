using PUCIT.AIMRL.NotificationEngine.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PUCIT.AIMRL.NotificationServer
{
    public static class GlobalDataManager
    {
        public static int DBWriteTimerInSec = 15; //Seconds
        public static int DBReadTimerEmpListPreDefListInHour = 1;
        public static int DBReadTimerEmpHierarchyInHour = 6;

        // Connections

        public readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        public readonly static ConcurrentDictionary<string, string> _userIdentity = new ConcurrentDictionary<string, string>();

        private static object syncObj = new object();

        public static void LoadAllDataFromDB()
        {

        }
    }
}