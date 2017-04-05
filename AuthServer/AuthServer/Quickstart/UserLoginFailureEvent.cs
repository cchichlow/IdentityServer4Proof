using IdentityServer4.Events;

namespace IdServ4.Quickstart
{
    public class UserLoginFailureEvent : Event<object>
    {
        public UserLoginFailureEvent(string username, string error)
            : base(IdentityServer4.Events.EventConstants.Categories.Authentication,
                    "User Login Failure",
                    EventTypes.Failure,
                    IdentityServer4.Events.EventConstants.Ids.UserLogin,
                    error)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
