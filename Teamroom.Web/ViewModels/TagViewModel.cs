namespace HobbyClue.Web.ViewModels
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateCreated { get; set; }
        public bool IsSelected { get; set; }
        public string IconUrl { get; set; }
    }
}