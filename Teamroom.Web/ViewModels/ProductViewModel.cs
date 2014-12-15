using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public CompanyViewModel Company { get; set; }
        public IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}