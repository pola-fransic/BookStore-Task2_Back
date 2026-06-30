using System;
using System.Collections.Generic;
using System.Linq;
using BookStoreApp.Models;

namespace BookStoreApp.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly List<T> _data = new List<T>();

        public void Add(T entity)
        {
            _data.Add(entity);
        }

        public void Remove(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _data.Remove(entity);
            }
        }

        public T? GetById(Guid id)
        {
            return _data.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _data;
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _data.Where(predicate);
        }
    }
}