using HobbyClue.Business.Providers;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;

namespace HobbyClue.Business.Services
{
    public interface IUserService : IBaseService<User>
    {
    }

    public class UserService : BaseDapperService<User>, IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository repository, IUserProvider userProvider)
            : base(repository, userProvider)
        {
            userRepository = repository;
        }

    }
}
