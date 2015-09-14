using Umbraco.Core;

namespace Camelonta.ActivityLog
{
    public class RegisterActivityLog : ApplicationEventHandler
    {
        /// <summary>
        /// If we ever need to log something manually, this is the way to attacht to events
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //ContentService.Something += ContentService_Something;

            base.ApplicationInitialized(umbracoApplication, applicationContext);
        }
        //void AddToLog(ActionEnum action, int sourceId, string sourceName, int targetId = -1)
        //{
        //        var userId = -1;
        //        if (HttpContext.Current.User.Identity.IsAuthenticated)
        //        {
        //            var userService = ApplicationContext.Current.Services.UserService;
        //            var user = userService.GetByUsername(HttpContext.Current.User.Identity.Name);
        //            if (user != null)
        //                userId = user.Id;
        //        }
        //}
    }
}