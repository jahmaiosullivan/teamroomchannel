using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HobbyClue.Common.Models;
using HobbyClue.Data;

namespace HobbyClue.Business.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IRepository<T> _repository;
        public IList<string> _errors = new List<string>();

        protected BaseService(IRepository<T> repository)
        {
            this._repository = repository;
        }

        public virtual T Save(T entity)
        {
            BeforeSave(entity);
            if (IsValid(entity))
            {
                _repository.Insert(entity);
                AfterSave(entity);
                return entity;
            }
            throw new Exception("_errors Adding entity");
        }

        public abstract T GetById(object id);

        public async Task<T> SaveAsync(T entity)
        {
            BeforeSave(entity);
            if (IsValid(entity))
            {
                var result = await _repository.InsertAsync(entity);
                AfterSave(result);
                return result;
            }
            throw new Exception("_errors Adding entity");
        }

        public virtual bool Update(T entity)
        {
            return _repository.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            BeforeDelete(entity);
            _repository.Remove(entity);
            AfterDelete(entity);
        }

        public virtual bool IsValid(T item)
        {
            _errors.Clear();
            Validate(item);
            return _errors.Count == 0;
        }

        public abstract void Validate(T item);
        
        public abstract void BeforeSave(T item);
        public abstract void BeforeUpdate(T item);
        public abstract void BeforeDelete(T item);
        public abstract void AfterDelete(T item);
        public abstract void AfterSave(T item);

        public virtual IList<T> FindAll()
        {
            return _repository.Find().EmptyListIfNull();
        }
    }
}
