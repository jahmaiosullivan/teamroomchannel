using HobbyClue.Web.Configuration;

namespace HobbyClue.Web.ViewModels
{
    public class ImageGalleryViewModel
    {
        public JsonNetResult Images { get; set; }

        public string DeleteUrl { get; set; }
    }
}