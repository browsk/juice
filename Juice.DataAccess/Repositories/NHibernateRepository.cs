using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juice.Core.Repositories;
using NHibernate;
using NHibernate.Criterion;
using Spring.Transaction.Interceptor;

namespace Juice.DataAccess.Repositories
{
    public class NHibernateRepository<EntityT, IdT> : IRepository<EntityT, IdT>
    {
        public ISessionFactory SessionFactory
        {
            get; set;
        }

        /// <summary>
        /// Gets the entity with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity with the specified id</returns>
        public virtual EntityT Get(IdT id)
        {
            return SessionFactory.GetCurrentSession().Get<EntityT>(id);
        }

        public virtual ICollection<EntityT> GetAll()
        {
            return SessionFactory.GetCurrentSession().CreateCriteria(typeof(EntityT)).List<EntityT>();
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <returns>The saved entity</returns>
        /// 
        [Transaction(ReadOnly = false)]
        public virtual IdT Save(EntityT entity)
        {
            // TODO: Catch NHibernate exception and throw our own
            return (IdT)SessionFactory.GetCurrentSession().Save(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// 
        [Transaction(ReadOnly = false)]
        public virtual void Update(EntityT entity)
        {
            SessionFactory.GetCurrentSession().Update(entity);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="propertyValuePairs">The property value pairs.</param>
        /// <returns></returns>
        public virtual ICollection<EntityT> FindAll(IDictionary<string, object> propertyValuePairs)
        {
            ICriteria criteria = SessionFactory.GetCurrentSession().CreateCriteria(typeof(EntityT));

            foreach (var pair in propertyValuePairs)
            {
                criteria.Add(Expression.Eq(pair.Key, pair.Value));
            }

            return criteria.List<EntityT>();
        }

        /// <summary>
        /// Finds the single.
        /// </summary>
        /// <param name="propertyValuePairs">The property value pairs.</param>
        /// <returns></returns>
        public virtual EntityT FindSingle(IDictionary<string, object> propertyValuePairs)
        {
            ICriteria criteria = SessionFactory.GetCurrentSession().CreateCriteria(typeof(EntityT));

            foreach (var pair in propertyValuePairs)
            {
                criteria.Add(Expression.Eq(pair.Key, pair.Value));
            }
            return (EntityT)criteria.UniqueResult();
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// 
        [Transaction(ReadOnly = false)]
        public virtual void Delete(EntityT entity)
        {
            SessionFactory.GetCurrentSession().Delete(entity);
        }
    }
}
