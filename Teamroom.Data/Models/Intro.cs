namespace HobbyClue.Data.Models
{
    public class Intro : ModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string Images { get; set; }
        public long CompanyId { get; set; }
    }
}
