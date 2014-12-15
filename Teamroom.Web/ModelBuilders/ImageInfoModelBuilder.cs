using System.Collections.Generic;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Azure;
using HobbyClue.Web.Configuration;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ModelBuilders
{
    public class ImageInfoModelBuilder : IImageInfoModelBuilder
    {
        private readonly IImageService imageService;
        private readonly IMappingEngine mappingEngine;
        private readonly IRouteBuilder routeBuilder;
        public ImageInfoModelBuilder(IImageService imageService, IMappingEngine mappingEngine, IRouteBuilder routeBuilder)
        {
            this.imageService = imageService;
            this.mappingEngine = mappingEngine;
            this.routeBuilder = routeBuilder;
        }

        public ImageGalleryViewModel BuildModel()
        {
            var imageInfoList = imageService.GetListOfAllImages();
            IEnumerable<ImageInfoViewModel> imageModels = mappingEngine.Map<IEnumerable<ImageInfo>, IEnumerable<ImageInfoViewModel>>(imageInfoList);
            var viewModel = new ImageGalleryViewModel
            {
                Images = new JsonNetResult { Data = imageModels },
                DeleteUrl = routeBuilder.GetApiRoute("Delete","Images")
            };
            return viewModel;
        }
    }
}