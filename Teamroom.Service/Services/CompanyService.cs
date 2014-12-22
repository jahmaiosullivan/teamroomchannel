using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public interface ICompanyService : IBaseService<Company>
    {
    }

    public class CompanyService : BaseDapperService<Company>, ICompanyService
    {
        public CompanyService(ICompanyRepository repository, IUserProvider userProvider) : base(repository, userProvider)
        {
        }
    }
}
