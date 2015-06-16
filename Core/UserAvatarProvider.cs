using System.Web;
using Umbraco.Core.Models.Membership;

namespace Camelonta.ActivityLog.Core
{
    public class UserAvatarProvider
    {
        public static string GetAvatarUrl(IUser user)
        { 
            if (HttpContext.Current.Application["activitylog_user_" + user.Id] == null)
            {
                var value = string.Format("http://www.gravatar.com/avatar/{0}&s=55", GravatarHelper.HashEmailForGravatar(user.Email));
                HttpContext.Current.Application["activitylog_user_" + user.Id] = value;
            }

            return HttpContext.Current.Application["activitylog_user_" + user.Id].ToString();
        }
    }
}