using DecisionsMobile.Constants;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace DecisionsMobile.Models
{
    public class Session
    {
        /// <summary>
        /// Because iOS requires it, and it's a good security consideration,
        /// this is used to normalize protocols for all ServerUri dependent URLs.
        /// </summary>
        public static readonly string PROTOCOL = "https://";

        /// <summary>
        /// The Name of a server configuration.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The URI of the server being logged into
        /// </summary>
        /// <remarks>
        /// This includes the domain but not the protocol. To build a useable url use <see cref="Session.ServerBaseUrl" />
        /// </remarks>
        public string ServerUri { get; set; }

        /// <summary>
        /// Username of last authenticated user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Cached password of last good session, in case
        /// the session ID expires.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Session ID used to authenticate all other requests
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Feedback Message to display, e.g. login screen feedback.
        /// </summary>
        /// TODO refactor this out of here. need double return from some AuthService methods to do so.
        public string FeedbackMessage { get; set; }
        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                return SessionId != null && SessionId.Length > 0;
            }
        }
        [JsonIgnore]
        public bool IsEmpty
        {
            get
            {
                return String.IsNullOrEmpty(ServerBaseUrl) || String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password);
            }
        }

        public bool ServerUriIsEmpty()
        {
            return String.IsNullOrEmpty(ServerUri);
        }

        /// <summary>
        /// provides the configured URI, but with the assumed https protocol added.
        /// 
        /// iOS only allows these communictions to be https, and it's a reasonable
        /// requirement for security reasons.
        /// 
        /// </summary>
        /// <returns>URL string - https://{uri}</returns>
        public string ServerBaseUrl {
            get
            {
                if (string.IsNullOrEmpty(ServerUri)) return $"{PROTOCOL}{RestConstants.DEFAULT_SERVER_URI}";
                var serverUri = ServerUri.TrimEnd('/');
                return $"{PROTOCOL}{serverUri}";
            }
        }
    }
}
