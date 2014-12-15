using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface ICompanyRepository : IDapperRepository<Company>
    {
    }

    public class CompanyRepository : BaseDapperRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IQueryManager queryManager) 
                    : base(queryManager)
        {
        }
    }
}
