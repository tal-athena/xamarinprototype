namespace DecisionsMobile.Models
{

    public class LoginBody
    {

        public LoginBody(string userId, string password)
        {
            this.userid = userId;
            this.password = password;
        }


#pragma warning disable IDE1006 // Naming Styles - name convention is based on REST end-point
        public string userid { get; set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles - name convention is based on REST end-point
        public string password { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
