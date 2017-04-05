using IdentityServer4.Events;

namespace IdServ4.Quickstart
{
    public class UserLoginSuccessEvent : Event<object>
        {
        private string username1;

        public UserLoginSuccessEvent(string username, string subjectId, string user)
                : base(EventConstants.Categories.Authentication,
                        "User Login Success",
                        EventTypes.Success,
                        EventConstants.Ids.UserLogin)
            {
                Username = username;
            }

        public UserLoginSuccessEvent(string username, string subjectId, string user, string username1) : this(username, subjectId, user)
        {            
        }

        public string Username { get; set; }
        }
    
}
