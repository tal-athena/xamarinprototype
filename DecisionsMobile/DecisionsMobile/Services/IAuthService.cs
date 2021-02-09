using DecisionsMobile.Models;
using System;
using System.Threading.Tasks;

namespace DecisionsMobile.Services
{
    interface IAuthService
    {
        /// <summary>
        /// Check validity of cached session.
        /// </summary>
        /// <returns>existing session or new session</returns>
        Task<Session> CheckSessionAsync();

        Task<Session> CreateSession(string server, string username, string password);

        /// <summary>
        /// Restore a session using last cached credentials
        /// 
        /// presumes that SessionId is expired, but Username/Password are still valid
        /// </summary>
        /// <returns></returns>
        Task<Session> RefreshSession(Session session);

        /// <summary>
        /// Append the current session Id to the provided URI.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>string with session ID query param appended</returns>
        String AppendSessionId(string uri);

        /// <summary>
        /// Add a session object to the service's dictionary of named server sessions configs.
        /// 
        /// If a session with the same name already exists, this is no
        /// </summary>
        /// <param name="session"></param>
        void AddServerNamedSession(Session session);

        /// <summary>
        /// Sets current server to log into based on server name.
        /// Assumes server config already exists in the local dictionary
        /// </summary>
        /// <param name="serverName"></param>
        Session SetCurrentServerByName(string serverName);

        /// <summary>
        /// Sets current server to log into based on session object.
        /// Presumes session is valid.
        /// Adds session to dictionary if it doesn't exist.
        /// </summary>
        /// <param name="serverName"></param>
        void SetCurrentServerSession(Session session);

        /// <summary>
        /// Clears key part(s) of current session object so that it is no longer valid,
        /// Cheifly the SessionId and the Password.
        /// 
        /// If the concrete implementation is also managing the persistence of the session,
        /// it should also clear the stored session information.
        /// </summary>
        /// <returns>A copy of current sesssion, without key part(s)</returns>
        Session LogOut();

    }
}
