using System;
using System.Collections.Generic;
using umbraco.BusinessLogic;
using umbraco.DataLayer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Camelonta.ActivityLog.Data
{
    public class UmbracoRepository
    {
        // For the log we retrieve these types
        private readonly static LogTypes[] LogTypeList = { LogTypes.Publish, LogTypes.Save, LogTypes.Delete, LogTypes.UnPublish, LogTypes.Move, LogTypes.Copy, LogTypes.RollBack };

        private readonly static string LogTypesAsString = "'" + string.Join("','", LogTypeList) + "'";
        private readonly DateTime _getLogSince = DateTime.Now.AddDays(-183);


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

        // Total count of log items for the pagination
        public int CountLogItems()
        {
            return ApplicationContext.Current.DatabaseContext.Database.ExecuteScalar<int>("SELECT COUNT(*) FROM umbracoLog WHERE logHeader IN (" + LogTypesAsString + ")");
        }

        public IEnumerable<IContent> GetRecycleBinNodes()
        {
            return UmbracoContext.Current.Application.Services.ContentService.GetContentInRecycleBin();
        }

    }
}