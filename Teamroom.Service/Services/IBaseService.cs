using System.Collections.Generic;
using System.Threading.Tasks;

namespace HobbyClue.Business.Services
{
    public interface IBaseService<T> where T : class
    {
        T Save(T entity);

        T GetById(object id);

        Task<T> SaveAsync(T entity);

        bool Update(T entity);

        IList<T> FindAll();

        void Delete(T entity);

        bool IsValid(T item);

        void Validate(T item);

        void BeforeSave(T item);

        void BeforeUpdate(T item);

        void BeforeDelete(T item);

        void AfterDelete(T item);

        void AfterSave(T item);

    }
}