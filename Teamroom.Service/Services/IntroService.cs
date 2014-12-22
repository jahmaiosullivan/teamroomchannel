using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public interface IIntroService : IBaseService<Intro>
    {
    }

    public class IntroService : BaseDapperService<Intro>, IIntroService
    {
        public IntroService(IIntroRepository repository, IUserProvider userProvider)
            : base(repository, userProvider)
        {
        }
    }
}
