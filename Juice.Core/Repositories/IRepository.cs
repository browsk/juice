using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Juice.Core.Repositories
{
    /// <summary>
    /// Generic repository pattern
    /// </summary>
    /// <typeparam name="EntityT">The type of the Entity.</typeparam>
    /// <typeparam name="IdT">The type of the Id column.</typeparam>
    public interface IRepository<EntityT, IdT>
    {
        /// <summary>
        /// Gets the entity with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity with the specified id</returns>
        EntityT Get(IdT id);

        ICollection<EntityT> GetAll();

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <returns>The id of the saved entity</returns>
        IdT Save(EntityT entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(EntityT entity);

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="propertyValuePairs">The property value pairs.</param>
        /// <returns></returns>
        ICollection<EntityT> FindAll(IDictionary<string, object> propertyValuePairs);

        /// <summary>
        /// Finds the single.
        /// </summary>
        /// <param name="propertyValuePairs">The property value pairs.</param>
        /// <returns></returns>
        EntityT FindSingle(IDictionary<string, object> propertyValuePairs);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(EntityT entity);
    }
}
