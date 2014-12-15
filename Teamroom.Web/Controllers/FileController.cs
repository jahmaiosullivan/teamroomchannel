using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobbyClue.Business.Services;
using HobbyClue.Common.Helpers;
using HobbyClue.Web.Configuration;

namespace HobbyClue.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly IImageService imageService;

        public FileController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpPost]
        public JsonResult AjaxUpload(HttpPostedFileBase[] files, Guid tempid)
        {
            if (files != null && files.Any())
            {
                // Get a reference to the file that our jQuery sent.  Even with multiple files, they will all be their own request and be the 0 index
                var file = files[0];

                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        FileHelper.CreateDirectoryIfNotExists(Server.MapPath("~//temp"));
                        FileHelper.CreateDirectoryIfNotExists(Server.MapPath("~//temp/uploads"));

                        var path = Path.Combine(Server.MapPath("~/temp/uploads"), fileName);
                        file.SaveAs(path);

                        string thumbNailUrl;
                        var uploadedFile = imageService.UploadImage(path, "CategoryImages", out thumbNailUrl);
                        // Now we need to wire up a response so that the calling script understands what happened
                        System.Web.HttpContext.Current.Response.StatusCode = 200;
                        return new JsonNetResult
                        {
                            Data = new {files = new[] { uploadedFile } }
                        };
                    }
                }
            }
            return new JsonResult();
        }

        [HttpPost]
        public JsonResult SingleFileUpload()
        {
            var file = Request.Files.Get("img");;
            if (file != null)
            {
                var path = Path.Combine(Server.MapPath("~/temp/uploads"), file.FileName);
                file.SaveAs(path);

                string thumbnailUrl;
                var uploadedFileUrl = imageService.UploadImage(path, "GroupImages", out thumbnailUrl);
                // Now we need to wire up a response so that the calling script understands what happened
                System.Web.HttpContext.Current.Response.StatusCode = 200;
                return new JsonNetResult
                {
                    Data = new { code = 0, url = uploadedFileUrl, thumbnailUrl = thumbnailUrl }
                };

                //"{'code': 0, "url": "img.jpg"}"
            }
            return new JsonResult();
        }
    }
}