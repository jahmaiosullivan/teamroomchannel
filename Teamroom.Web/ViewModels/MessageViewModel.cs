using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        public MessageViewModel()
        {
            Children = new List<MessageViewModel>();
        }
        public string Body { get; set; }
        public long QuestionId { get; set; }
        public long ParentMessageId { get; set; }
        public IEnumerable<MessageViewModel> Children { get; set; }
    }
}