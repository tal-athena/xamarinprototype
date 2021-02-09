namespace DecisionsMobile.Services
{
    /// <summary>
    /// Service for handling user feedback messages, etc.
    /// </summary>
    public class UserFeedbackService
    {
        public static string GetLoginResultMessage(string error)
        {
            error = error.ToUpper();
            if (
                error.Contains("SSL") ||
                error.Contains("CERTIFICATION") ||
                error.Contains("CERTPATHVALIDATOREXCEPTION"))
            {
                return "There is a connection problem. Please contact your administrator.";
            }
            return "Problem communicating with server.";
        }
    }
}
