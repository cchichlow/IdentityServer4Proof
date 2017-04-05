using IdentityServer4.Events;

namespace IdServ4.Quickstart
{
    public class UserLogoutSuccessEvent : Event<object>
    {
        public UserLogoutSuccessEvent(string subjectId, string user)
                : base(EventConstants.Categories.Authentication,
                        "User Login Success",
                        EventTypes.Success,
                        EventConstants.Ids.UserLogout)
        {

        }
    }
 }

