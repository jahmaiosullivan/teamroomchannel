namespace HobbyClue.Data.Models
{
    public class Message : ModelBase
    {
        public string Body { get; set; }
        public long QuestionId { get; set; }
        public long ParentMessageId { get; set; }
    }
}
