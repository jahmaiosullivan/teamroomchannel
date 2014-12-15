using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HobbyClue.Business.Services;
using HobbyClue.Web.Configuration;

namespace HobbyClue.Web.ApiControllers
{
    public class ImageApiController : ApiController
    {
        private readonly IImageService imageService;

        public ImageApiController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        // GET api/image
        public JsonNetResult Get()
        {
            return new JsonNetResult
            {
                Data = imageService.GetListOfAllImages()
            };
        }

        // GET api/image/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/image
        public void Post([FromBody]string value)
        {
        }

        // PUT api/image/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/image/5
        public HttpResponseMessage Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var imageInfo = imageService.Get(id);
            var isdeleted = imageService.Delete(imageInfo);

            return !isdeleted ?
                new HttpResponseMessage(HttpStatusCode.NotFound) :
                new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
