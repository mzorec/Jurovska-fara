using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using log4net;
using NHibernate;
using NHibernate.Impl;

namespace Vortex.Database.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RepositoryFactory));

        private readonly Dictionary<int, ISession> currentSessions = new Dictionary<int, ISession>();

        private readonly ISessionFactory sessionFactory;

        private readonly ITimeProvider timeProvider;

        private bool disposed;

        public RepositoryFactory(ISessionFactory sessionFactory, ITimeProvider timeProvider)
        {
            this.sessionFactory = sessionFactory;
            this.timeProvider = timeProvider;
        }

        public ISession CurrentSession
        {
            get
            {
                lock (this.currentSessions)
                {
                    return this.currentSessions[CurrentThreadId];
                }
            }
        }

        private static int CurrentThreadId
        {
            get { return Thread.CurrentThread.ManagedThreadId; }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ISession OpenSession()
        {
            int threadId = CurrentThreadId;

            ISession currentSession = null;

            lock (this.currentSessions)
            {
                if (this.currentSessions.ContainsKey(threadId))
                {
                    currentSession = this.currentSessions[threadId];
                }
            }

            if (currentSession != null && currentSession.IsOpen)
            {
                throw new InvalidOperationException("You haven't closed the previous session");
            }

            currentSession = this.sessionFactory.OpenSession();
            try
            {
                lock (this.currentSessions)
                {
                    this.currentSessions[threadId] = currentSession;
                }
            }
            catch (NullReferenceException)
            {
                string message = string.Format(
                    "currentSessions='{2}', threadId='{0}', currentSession='{1}'",
                    threadId,
                    currentSession != null,
                    this.currentSessions != null);
                Console.Error.WriteLine(message);
                Log.FatalFormat("XYZ: {0}", message);
                throw;
            }

            var sessionImpl = (SessionImpl)currentSession;
            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat("OpenSession (session Id = {0})", sessionImpl.SessionId);
            }

            return currentSession;
        }

        public IUserRepository CreateUserRepository()
        {
            return new UserRepository(CurrentSession, timeProvider);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (false == this.disposed)
            {
                // clean native resources   
                if (disposing)
                {
                    lock (this.currentSessions)
                    {
                        foreach (var sessionData in this.currentSessions)
                        {
                            if (sessionData.Value != null && sessionData.Value.IsOpen)
                            {
                                Log.WarnFormat(
                                    "You haven't closed the session {0} on thread {1} before disposing RepositoryFactory",
                                    ((SessionImpl)sessionData.Value).SessionId,
                                    sessionData.Key);
                            }
                        }
                    }
                    //// clean managed resources          
                }

                this.disposed = true;
            }
        }
    }
}