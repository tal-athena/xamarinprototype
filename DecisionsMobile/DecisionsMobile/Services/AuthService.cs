using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DecisionsMobile.Services
{
    /// <summary>
    /// Singleton service for managing session information.
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        static readonly string SESSION_STORAGE_KEY = "saved_session";
        static readonly string LOGGEDIN_FLAG_STORAGE_KEY = "logged_in_flag";
        HttpClient httpClient;
        public Dictionary<string, Session> Sessions;
        public Session CurrentSession;
        public bool InitialLogin { get; set; }
        private UserContext userContext;
        private static AuthService instance = null;

        static AuthService() { }

        private AuthService()
        {
            httpClient = new HttpClient();
            InitialLogin = true;
            CurrentSession = new Session();
            Sessions = new Dictionary<string, Session>();
        }

        public static AuthService Instance
        {
            get
            {
                if (instance == null)
                    instance = new AuthService();
                return instance;
            }
        }

        /// <summary>
        /// Ping a host to ensure it is reachable.
        /// </summary>
        /// TODO this probably belongs in a class related to connectivity generally.
        /// <param name="serverUrl"></param>
        /// <returns>boolean task</returns>
        public static async Task<bool> IsHostReachable(string serverUrl)
        {
            try
            {
                var connectivity = CrossConnectivity.Current;
                if (!connectivity.IsConnected)
                    return false;
                int port = 80;

                if (serverUrl.Contains(":"))
                {
                    int.TryParse(serverUrl.Split(':')[1], out port);                 
                    serverUrl = serverUrl.Split(':')[0];
                }
                    
                var reachable = await connectivity.IsRemoteReachable(serverUrl, port, 3000);

                return reachable;
            }
            catch (Exception)
            {
                return false;
            }

        }

        static HttpContent CreateLoginContent(string userId, string password)
        {
            var body = new LoginBody(userId, password);
            var json = JsonConvert.SerializeObject(body);
            return new StringContent(json, Encoding.UTF8, RestConstants.JSON_HEADER);
        }

        private bool IsValidUri(string uriName)
        {
            return Uri.TryCreate(uriName, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public async Task<Session> CreateSession(string server, string username, string password)
        {
            return await CreateSession(new Session
            {
                ServerUri = server,
                UserName = username,
                Password = password
            });
        }

        public async Task<Session> CreateSession(Session session)
        {
            if (session.IsEmpty || !IsValidUri(session.ServerBaseUrl)) { return session; }

            httpClient = new HttpClient();

            var uri = new Uri($"{session.ServerBaseUrl}{RestConstants.REST_ROOT}/AccountService/LoginUser");
            // var uriDebugString = uri.ToString();
            try
            {
                var loginContent = CreateLoginContent(session.UserName, session.Password);
                var response = await httpClient.PostAsync(uri, loginContent);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<LoginUserWrapper>(body);
                    var result = resultWrapper.LoginUserResult;
                    InitialLogin = false;

                    // defensively copy Session
                    SetCurrentServerSession(new Session
                    {
                        Password = session.Password,
                        ServerName = session.ServerName,
                        ServerUri = session.ServerUri,
                        UserName = session.UserName,
                        SessionId = result.SessionValue
                    });

                    Preferences.Set(LOGGEDIN_FLAG_STORAGE_KEY, true);
                    return CurrentSession;
                }
                else
                {
                    // TODO get better information from server as to why... 500 ain't good enough
                    // TODO i18n?
                    session.FeedbackMessage = "Invalid Username or Password.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);

                session.FeedbackMessage = UserFeedbackService.GetLoginResultMessage(ex.Message);
            }

            return session;
        }

        private String GetLoginErrorMessage(Exception ex)
        {
            ex.Message.ToUpper().Contains("PASSWORD");
            return ex.Message;
        }

        public Task<Session> RefreshSession(Session session)
        {
            return CreateSession(session);
        }

        public async Task<Session> CheckSessionAsync()
        {
            if (CurrentSession.IsValid)
            {
                // we already have a session in memory. App is live.
                // TODO for this to be robust, it should make a network request to validate the sessionId
                return CurrentSession;
            }
            // App is being launched or re-launched.
            // Attempt to get up-to-date sessionId from stored credentials.
            await RestoreCurrentSession();
            return await RefreshSession(CurrentSession);
        }

        public String AppendSessionId(string uri)
        {
            if (uri.EndsWith("?"))
                uri = uri.Substring(0, uri.Length - 1);
            if (!uri.Contains("?"))
                return $"{uri}?sessionId={CurrentSession.SessionId}";
            else return $"{uri}&sessionId={CurrentSession.SessionId}";
        }


        public void AddServerNamedSession(Session session)
        {
            if (Sessions.ContainsKey(session.ServerName))
            {
                Sessions.Remove(session.ServerName);
            }
            Sessions.Add(session.ServerName, session);
        }

        public Session SetCurrentServerByName(string serverName)
        {
            if (!Sessions.ContainsKey(serverName))
            {
                return CurrentSession; // log error? handle another way?
            }

            this.Sessions.TryGetValue(serverName, out var session);
            return session;
        }

        public void SetCurrentServerSession(Session session)
        {
            if (!Sessions.ContainsKey(session.ServerName))
            {
                AddServerNamedSession(session);
            }
            CurrentSession = session;
            SaveCurrentSession();

            this.userContext = new UserContext(CurrentSession.SessionId);
        }

        public async Task<UserContext> GetUserContextAsync()
        {
            await CheckSessionAsync();
            return userContext;
        }

        private async void SaveCurrentSession()
        {
            try
            {
                string SessionJsonString = JsonConvert.SerializeObject(CurrentSession, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                Debug.WriteLine("SessionJsonString", SessionJsonString);
                await SecureStorage.SetAsync(SESSION_STORAGE_KEY, SessionJsonString);
                await RestoreCurrentSession();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem saving session");
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
        }

        private async Task<Session> RestoreCurrentSession()
        {
            try
            {
                string SessionJsonString = await SecureStorage.GetAsync(SESSION_STORAGE_KEY);

                if (SessionJsonString != null)
                {
                    CurrentSession = JsonConvert.DeserializeObject<Session>(SessionJsonString, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    userContext = new UserContext(CurrentSession.SessionId);
                }

                return CurrentSession;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem restoring session");
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return CurrentSession;
        }
        public bool HaveEverLoggedIn()
        {
            try
            {
                if (Preferences.Get(LOGGEDIN_FLAG_STORAGE_KEY, false))
                    return true;

            } catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return false;
        }

        public Session LogOut()
        {
            // defensive copies... immutability, etc.
            Session newSession = new Session
            {
                ServerName = CurrentSession.ServerName,
                UserName = CurrentSession.UserName,
                ServerUri = CurrentSession.ServerUri,
            };
            SetCurrentServerSession(newSession);
            SecureStorage.Remove(SESSION_STORAGE_KEY);
            return newSession;
        }
    }
}
