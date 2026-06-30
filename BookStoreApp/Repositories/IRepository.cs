using System;
using System.Collections.Generic;
using BookStoreApp.Models;

namespace BookStoreApp.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        void Remove(Guid id);
        T? GetById(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
    }
}