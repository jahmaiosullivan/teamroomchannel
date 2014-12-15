using System;
using System.Collections.Generic;
using System.Linq;
using HobbyClue.Common.Models;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IProductRepository : IDapperRepository<Product>
    {
        IEnumerable<Product> GetProducts(Guid userId);
        Product GetByName(string name);
    }

    public class ProductRepository : BaseDapperRepository<Product>, IProductRepository
    {
        public ProductRepository(IQueryManager queryManager) 
                    : base(queryManager)
        {
        }

        public IEnumerable<Product> GetProducts(Guid userId)
        {
            var products = _queryManager.ExecuteQuery<Product>("GetUserProducts", new {@UserId = userId});
            return products;
        }

        public Product GetByName(string name)
        {
            var sql = BaseQuery + " where t1.Name = @name";
            var cities = _queryManager.ExecuteSql<Product>(sql, new { @name = name });
            return cities.EmptyListIfNull().FirstOrDefault();
        }

    }
}
