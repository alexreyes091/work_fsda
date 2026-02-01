namespace app.webapi.backoffice_viajes_altairis.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Asynchronously retrieves all entities of type T from the data source.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all
        /// entities of type T. The collection will be empty if no entities are found.</returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Returns a queryable collection of entities of type T that can be further filtered, sorted, and projected
        /// using LINQ operations.
        /// </summary>
        /// <remarks>The returned query supports deferred execution. Additional LINQ operators can be
        /// applied to refine the query before execution. Changes to the underlying data source after obtaining the
        /// query may affect the results when the query is executed.</remarks>
        /// <returns>An <see cref="IQueryable{T}"/> representing the collection of entities. The query is not executed until the
        /// results are enumerated.</returns>
        IQueryable<T> Query();
        /// <summary>
        /// Asynchronously retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity of type T if found;
        /// otherwise, null.</returns>
        Task<T> GetByIdAsync(int id);
        /// <summary>
        /// Asynchronously creates a new entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the entity
        /// was created successfully; otherwise, <see langword="false"/>.</returns>
        Task<bool> CreateAsync(T entity);
        /// <summary>
        /// Asynchronously updates the specified entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update
        /// was successful; otherwise, <see langword="false"/>.</returns>
        Task<bool> UpdateAsync(T entity);
        /// <summary>
        /// Asynchronously deletes the specified entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to delete. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the
        /// entity was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteAsync(T entity);
        /// <summary>
        /// Asynchronously determines whether an entity with the specified identifier exists.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// entity exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> ExistAsync(int id);
        /// <summary>
        /// Asynchronously saves the current changes to the underlying data store.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. The task result is <see langword="true"/> if the
        /// changes were saved successfully; otherwise, <see langword="false"/>.</returns>
        Task<bool> SaveAsync();
    }
}
