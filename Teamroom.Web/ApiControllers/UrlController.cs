using System.Web.Http;
using HobbyClue.Business.Services;

namespace HobbyClue.Web.ApiControllers
{
    public class UrlController : ApiController
    {
        private readonly IVideoUrlService videoUrlService;

        public UrlController(IVideoUrlService videoUrlService)
        {
            this.videoUrlService = videoUrlService;
        }

        [HttpGet]
        public string VideoTitle(string id)
        {
            return videoUrlService.GetTitleFromUrl("https://www.youtube.com/watch?v=" + id);
        }

        // GET api/url/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/url
        public void Post([FromBody]string value)
        {
        }

        // PUT api/url/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/url/5
        public void Delete(int id)
        {
        }
    }
}
