using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace B_M.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Get all entities
        IEnumerable<T> GetAll();
        
        // Get entity by ID
        T GetById(int id);
        
        // Find entities by condition
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        
        // Get single entity by condition
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        
        // Add entity
        void Add(T entity);
        
        // Add multiple entities
        void AddRange(IEnumerable<T> entities);
        
        // Update entity
        void Update(T entity);
        
        // Remove entity
        void Remove(T entity);
        
        // Remove multiple entities
        void RemoveRange(IEnumerable<T> entities);
        
        // Count entities
        int Count();
        
        // Count entities by condition
        int Count(Expression<Func<T, bool>> predicate);
    }
}
