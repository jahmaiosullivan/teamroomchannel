using System;
using HobbyClue.Data.Dapper;
using HobbyClue.Common.Helpers;
using HobbyClue.Data.Models;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public interface IBaseDapperService
    {
    }

    public abstract class BaseDapperService<T> : BaseService<T>, IBaseDapperService where T : class
    {
        private readonly IDapperRepository<T> _repository;
        protected readonly IUserProvider userProvider;
        protected BaseDapperService(IDapperRepository<T> repository, IUserProvider userProvider) : base(repository)
        {
            _repository = repository;
            this.userProvider = userProvider;
        }

        public override bool Update(T entity)
        {
            if (!IsValid(entity)) return false;
            BeforeUpdate(entity);
            var result = _repository.ItemExists(entity) && _repository.Update(entity);
            return result;
        }

        public override T GetById(object id)
        {
            return _repository.GetById(id);
        }

        public override void Validate(T item)
        {

        }

        public override void BeforeSave(T item)
        {
            Guid primaryKeyValue;
            if (Guid.TryParse(item.GetPrimaryKeyField().Value.ToString(), out primaryKeyValue))
            {
                if(primaryKeyValue == Guid.Empty)
                    item.SetPrimaryKey(Guid.NewGuid());
            }

            var modelItem = item as ModelBase;
            if (modelItem == null) return;

            modelItem.CreatedDate = DateTime.UtcNow;
            if((!modelItem.CreatedBy.HasValue || modelItem.CreatedBy.Value == Guid.Empty) && userProvider.IsAuthenticated)
                modelItem.CreatedBy = userProvider.CurrentUserId;
        }

        public override bool IsValid(T item)
        {
            return true;
        }

        public override void BeforeUpdate(T item)
        {
            var modelItem = item as ModelBase;
            if (modelItem == null) return;

            modelItem.LastUpdatedDate = DateTime.UtcNow;
            if ((!modelItem.LastUpdatedBy.HasValue || modelItem.LastUpdatedBy.Value == Guid.Empty) && userProvider.IsAuthenticated)
                modelItem.LastUpdatedBy = userProvider.CurrentUserId;
        }

        public override void BeforeDelete(T item)
        {

        }

        public override void AfterDelete(T item)
        {

        }

        public override void AfterSave(T item)
        {

        }

    }
}
