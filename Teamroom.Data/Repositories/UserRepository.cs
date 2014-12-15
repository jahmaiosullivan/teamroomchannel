using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IUserRepository : IDapperRepository<User>
    {
    }

    public class UserRepository : BaseDapperRepository<User>, IUserRepository
    {
        public UserRepository(IQueryManager queryManager) : base(queryManager) { }

        public override string TableName
        {
            get { return "AspNetUsers"; }
        }
    }
}
