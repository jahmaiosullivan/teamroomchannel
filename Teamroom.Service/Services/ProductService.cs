using System;
using System.Collections.Generic;
using HobbyClue.Business.Providers;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;

namespace HobbyClue.Business.Services
{
    public interface IProductService : IBaseService<Product>
    {
        IEnumerable<Product> Get(Guid userId);
        Product GetByName(string name);
    }

    public class ProductService : BaseDapperService<Product>, IProductService
    {
        private readonly IProductRepository productRepository;
        public ProductService(IProductRepository repository, IUserProvider userProvider)
            : base(repository, userProvider)
        {
            productRepository = repository;
        }

        public IEnumerable<Product> Get(Guid userId)
        {
            return productRepository.GetProducts(userId);
        }

        public Product GetByName(string name)
        {
            return productRepository.GetByName(name);
        }
    }
}
