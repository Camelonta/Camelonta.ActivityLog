using System;
using System.Collections.Generic;
using System.Linq;
using umbraco.BusinessLogic;
using umbraco.DataLayer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Camelonta.ActivityLog.Data
{
    public class UmbracoRepository
    {
        private readonly static LogTypes[] LogTypeList = { LogTypes.Publish, LogTypes.Save, LogTypes.Delete, LogTypes.UnPublish, LogTypes.Move, LogTypes.Copy, LogTypes.RollBack };
        private readonly static string LogTypesAsString = "'" + string.Join("','", LogTypeList) + "'";
        // private readonly string _logTypes = "'Publish', 'Save','Delete','Unpublish','Move','Copy','RollBack'";
        private readonly DateTime _getLogSince = DateTime.Now.AddDays(-183);


        //public IEnumerable<LogItem> GetLatestLogItems(int take, int skip)
        //{
        //    var logItems = new List<LogItem>();

        //    foreach (var logtype in _logTypes)
        //    {
        //        logItems.AddRange(Log.Instance.GetLogItems(logtype, _getLogSince));
        //    }

        //    // "Delete media" is when the item is deleted from recycle bin.
        //    return logItems.OrderByDescending(x => x.Timestamp).Where(x => x.NodeId != -1 &&
        //        !x.Comment.StartsWith("Move Content") &&
        //        !x.Comment.StartsWith("Delete Media") &&
        //        !x.Comment.StartsWith("Version rolled back"))
        //        .Skip(skip).Take(take);
        //}

        private static ISqlHelper SqlHelper
        {
            get { return Application.SqlHelper; }
        }
        public IEnumerable<LogItem> GetLatestLogItems(int take, int skip)
        {
            return LogItem.ConvertIRecordsReader(SqlHelper.ExecuteReader(
                          "SELECT userId, NodeId, DateStamp, logHeader, logComment " +
                           "FROM umbracoLog " +
                            "WHERE logHeader IN (" + LogTypesAsString + ") " +
                                "AND DateStamp >= @dateStamp " +
                            "AND logComment NOT LIKE 'Move Content%'" + //Exclude rows where comment starts with "Move Content" to prevent duplicated messages
                            "AND logComment NOT LIKE 'Delete Media%'" +//Exclude rows where comment starts with "Delete media". For now. This occurs when item is removed from media library.
                            "AND logComment NOT LIKE 'Version rolled back%'" +//Exclude rows where comment starts with "Version rolled back" to prevent two messages
                            "ORDER BY dateStamp DESC " +
                            "OFFSET (@skip) ROWS FETCH NEXT (@take) ROWS ONLY",
                          SqlHelper.CreateParameter("@dateStamp", _getLogSince),
                          SqlHelper.CreateParameter("@skip", skip),
                          SqlHelper.CreateParameter("@take", take)));
        }




        public int CountLogItems()
        {
            return ApplicationContext.Current.DatabaseContext.Database.ExecuteScalar<int>("SELECT COUNT(*) FROM umbracoLog WHERE logHeader IN (" + LogTypesAsString + ")");
            //return SqlHelper.ExecuteReader(
            //    "SELECT COUNT(*) FROM umbracoLog WHERE logHeader IN (" + LogTypesAsString + ")");
            //return 500;
        }

        public IEnumerable<IContent> GetRecycleBinNodes()
        {
            return UmbracoContext.Current.Application.Services.ContentService.GetContentInRecycleBin();
        }

    }
}