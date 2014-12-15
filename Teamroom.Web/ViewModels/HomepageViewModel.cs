using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
            Intros = new List<IntroViewModel>();
        }

        public IEnumerable<IntroViewModel> Intros { get; set; }
    }
}