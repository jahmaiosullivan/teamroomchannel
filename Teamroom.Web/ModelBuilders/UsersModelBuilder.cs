using System;
using System.Collections.Generic;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ModelBuilders
{
    public class UsersModelBuilder : IUsersModelBuilder
    {
        public IEnumerable<UserViewModel> Build(int defaultCount = 1)
        {
            var userModels = new List<UserViewModel>();
            for (var i = 0; i < defaultCount; i++)
            {
                userModels.Add(new UserViewModel
                {
                    Id = Guid.NewGuid(),
                    AvatarThumbnailUrl = GetThumbnailUrl(i),
                    DisplayName = "Jahmai " + i,
                    About = "Jahmai is " + i,
                    Photos = new List<string> { "http://html.creaws.com/the8/pic/portfolio/huge-1.jpg" }
                });
            }
            return userModels;
        }

        private string GetThumbnailUrl(int cnt)
        {
            if (cnt % 6 == 0)
                return "http://html.creaws.com/the8/pic/recent-photos/item-6.jpg";
            if (cnt % 5 == 0)
                return "http://html.creaws.com/the8/pic/recent-photos/item-5.jpg";
            if (cnt % 4 == 0)
                return "http://html.creaws.com/the8/pic/recent-photos/item-4.jpg";
            if (cnt % 3 == 0)
                return "http://html.creaws.com/the8/pic/recent-photos/item-3.jpg";
            return cnt % 2 == 0 ? "http://html.creaws.com/the8/pic/recent-photos/item-2.jpg"
                                : "http://html.creaws.com/the8/pic/recent-photos/item-1.jpg";
        }
        
    }
}