using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Impl;
using Vortex.Database.Models;

namespace Vortex.Database.Repository
{
    public abstract class RepositoryBase<TEntity> where TEntity : EntityBase<TEntity>
    {
        protected RepositoryBase(ISession session, ITimeProvider timeProvider)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            this.session = session;
            this.TimeProvider = timeProvider;
        }

        protected ITimeProvider TimeProvider { get; private set; }

        protected ISession Session
        {
            get
            {
                if (false == session.IsOpen)
                {
                    SessionImpl sessionImpl = (SessionImpl)session;
                    string message = string.Format(
                        CultureInfo.InvariantCulture,
                        "Session '{0}' is closed",
                        sessionImpl.SessionId);
                    throw new SessionClosedException(message);
                }

                return session;
            }
        }

        protected virtual TEntity Add(TEntity entity)
        {
            var time = TimeProvider.Now;
            entity.CreatedTime = time;
            entity.ModifiedTime = time;
            Session.Save(entity);
            return entity;
        }

        protected virtual TEntity Update(TEntity entity)
        {
            entity.ModifiedTime = TimeProvider.Now;
            Session.Update(entity);
            return entity;
        }

        protected virtual TEntity Get(long id)
        {
            TEntity entity = Session.Get<TEntity>(id);
            if (entity == null)
            {
                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} with ID {1} does not exist",
                    typeof(TEntity).Name,
                    id);
                throw new KeyNotFoundException(message);
            }

            return entity;
        }

        protected virtual void Delete(long id)
        {
            session.Delete(this.Get(id));
        }

        private ISession session;
    }
}