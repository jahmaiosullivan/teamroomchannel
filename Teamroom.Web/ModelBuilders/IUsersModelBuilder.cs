using System.Collections.Generic;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ModelBuilders
{
    public interface IUsersModelBuilder
    {
        IEnumerable<UserViewModel> Build(int defaultCount = 1);
    }
}