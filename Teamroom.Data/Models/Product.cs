namespace HobbyClue.Data.Models
{
    public class Product : ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long CompanyId { get; set; }
        public string LogoUrl { get; set; }
    }
}
