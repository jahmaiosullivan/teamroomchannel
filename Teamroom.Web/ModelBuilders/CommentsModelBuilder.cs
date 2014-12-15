using System.Collections.Generic;
using System.Linq;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ModelBuilders
{
    public class CommentsModelBuilder : ICommentsModelBuilder
    {
        private readonly IUsersModelBuilder usersModelBuilder;

        public CommentsModelBuilder(IUsersModelBuilder usersModelBuilder)
        {
            this.usersModelBuilder = usersModelBuilder;
        }

        
    }
}