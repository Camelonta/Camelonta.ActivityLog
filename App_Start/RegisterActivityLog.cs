using Umbraco.Core;

namespace Camelonta.ActivityLog
{
    public class RegisterActivityLog : ApplicationEventHandler
    {
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //ContentService.Something += ContentService_Something;

            base.ApplicationInitialized(umbracoApplication, applicationContext);
        }
        //void AddToLog(ActionEnum action, int sourceId, string sourceName, int targetId = -1)
        //{
        //    try
        //    {
        //        var userId = -1;
        //        if (HttpContext.Current.User.Identity.IsAuthenticated)
        //        {
        //            var userService = ApplicationContext.Current.Services.UserService;
        //            var user = userService.GetByUsername(HttpContext.Current.User.Identity.Name);
        //            if (user != null)
        //                userId = user.Id;
        //        }

        //        var db = ApplicationContext.Current.DatabaseContext.Database;
        //        db.Insert(new ActivityLogItem()
        //        {
        //            Date = DateTime.Now,
        //            Action = action.ToString(),
        //            SourceId = sourceId,
        //            SourceName = sourceName,
        //            TargetId = targetId,
        //            UserId = userId
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(this.GetType(), "Error logging to ActivityLogItems table", ex);
        //    }
        //    //LogHelper.Info(this.GetType(), page.Name + " saved by " + UmbracoContext.Current.Security.CurrentUser.Name);
        //}
    }
}