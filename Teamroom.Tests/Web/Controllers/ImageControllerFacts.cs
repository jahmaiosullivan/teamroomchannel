using HobbyClue.Business.Services;
using HobbyClue.Data.Azure;
using HobbyClue.Tests.Helpers;
using HobbyClue.Web.ApiControllers;
using Moq;
using Xunit;

namespace HobbyClue.Tests.Web.Controllers
{
    public class ImageControllerFacts
    {
        public class Delete
        {
            private readonly TestableImagesController imagesController = TestableImagesController.Create();

            [Fact]
            public void ReturnsOkResponseMessageIfSuccessful()
            {
                const string filename = "file1.png";

                var result = imagesController.ClassUnderTest.Delete(filename);

                Assert.NotNull(result);
                Assert.Equal(200,(int)result.StatusCode);
            }

            [Fact]
            public void ReturnsNotFoundIfImageInfoIsNull()
            {
                var result = imagesController.ClassUnderTest.Delete(null);

                Assert.NotNull(result);
                Assert.Equal(404, (int)result.StatusCode);
            }

            [Fact]
            public void ReturnsNotFoundIfUnableToDeleteImageInfo()
            {
                const string filename = "file1.png";
                var info = new ImageInfo {Name = "ImageInfo 1"};
                imagesController.Mock<IImageService>().Setup(x => x.Get(filename)).Returns(info);
                imagesController.Mock<IImageService>().Setup(x => x.Delete(info)).Returns(false);

                var result = imagesController.ClassUnderTest.Delete(filename);

                Assert.NotNull(result);
                Assert.Equal(404, (int)result.StatusCode);
            }
        }
        public class TestableImagesController : Facts<ImageApiController>
        {
            public static TestableImagesController Create()
            {
                var controller = new TestableImagesController();
                controller.Mock<IImageService>().Setup(x => x.Delete(It.IsAny<ImageInfo>())).Returns(true);
                return controller;
            }
        }
    }
}
