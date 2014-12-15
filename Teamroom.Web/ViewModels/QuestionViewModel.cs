using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class QuestionViewModel : ViewModelBase
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string BodySummary { get; set; }
        public int Views { get; set; }
        public string State { get; set; }
        public long ProductId { get; set; }
        public IEnumerable<MessageViewModel> Replies { get; set; }
    }
}