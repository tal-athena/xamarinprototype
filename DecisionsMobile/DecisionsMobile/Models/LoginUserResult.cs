namespace DecisionsMobile.Models
{
    public class LoginUserResult
    {
        public int DisplayType { get; set; }
        public string SessionValue { get; set; }
        public bool StudioPortal { get; set; }
    }

    public class LoginUserWrapper
    {
        public LoginUserResult LoginUserResult { get; set; }
    }
}
