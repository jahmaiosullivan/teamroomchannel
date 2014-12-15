using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IIntroRepository : IDapperRepository<Intro>
    {
    }

    public class IntroRepository : BaseDapperRepository<Intro>, IIntroRepository
    {
        public IntroRepository(IQueryManager queryManager) 
                    : base(queryManager)
        {
        }
    }
}
