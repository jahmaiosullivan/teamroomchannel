using System.Web;

namespace HobbyClue.Web.Models
{
    public class ModalViewModel
    {
        public ModalViewModel()
        {
            ShowSubmitButton = ShowFooter = ShowHeader = true;
            SubmitButtonText = "Post";
            SubmitButtonId = "sbmtButton";
        }

        public string Id { get; set; }
        public string Class { get; set; }
        public string Title { get; set; }
        public string SubmitButtonText { get; set; }
        public bool ShowSubmitButton { get; set; }
        public bool ShowHeader { get; set; }
        public bool ShowFooter { get; set; }
        public IHtmlString Content { get; set; }
        public string SubmitButtonId { get; set; }
    }
}