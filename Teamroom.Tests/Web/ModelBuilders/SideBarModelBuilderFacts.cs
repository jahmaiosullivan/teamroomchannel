using System;
using System.Linq;
using System.Web;
using HobbyClue.Tests.Helpers;
using HobbyClue.Web.Configuration;
using HobbyClue.Web.ModelBuilders;
using Moq;
using Xunit;

namespace HobbyClue.Tests.Web.ModelBuilders
{
    public class SideBarModelBuilderFacts
    {
        public class BuildAdminSideBar
        {
            private readonly TestableSideBarModelBuilder builder = TestableSideBarModelBuilder.Create();

            [Fact]
            public void ReturnsViewModel()
            {
                var model = builder.ClassUnderTest.BuildAdminSideBar();

                Assert.NotNull(model);
                Assert.NotNull(model.SidebarItems);
                Assert.True(model.SidebarItems.Any());           
            }

            [Fact]
            public void AddsDashboardAsFirstItem()
            {
                const string adminRoute = "/Admin";
                builder.Mock<IRouteBuilder>().Setup(x => x.GetRoute("Index", "Admin")).Returns(adminRoute);
                
                var model = builder.ClassUnderTest.BuildAdminSideBar();
                var firstSideBarItem = model.SidebarItems.First();

                Assert.True(firstSideBarItem.Title == "Dashboard" && firstSideBarItem.Url == adminRoute && firstSideBarItem.IconClass == "fa fa-home" && firstSideBarItem.IsFirst);
            }


            [Fact]
            public void AddsCategories()
            {
                var model = builder.ClassUnderTest.BuildAdminSideBar();
                var categoriesSidebarItem = model.SidebarItems.First(x => x.Title == "Categories");

                Assert.True(categoriesSidebarItem.IconClass == "fa fa-cogs" && !categoriesSidebarItem.IsFirst);
            }

            [Fact]
            public void AddsGallery()
            {
                const string imagesGalleryRoute = "/Admin/Gallery/Images";
                builder.Mock<IRouteBuilder>().Setup(x => x.GetRoute("Images", "Gallery")).Returns(imagesGalleryRoute);

                var model = builder.ClassUnderTest.BuildAdminSideBar();

                var gallerySidebarItem = model.SidebarItems.First(x => x.Title == "Gallery");
                var sidebarChildren = gallerySidebarItem.Children.ToList();

                Assert.True(!gallerySidebarItem.IsFirst);
                Assert.Equal(1, sidebarChildren.Count());
                Assert.True(sidebarChildren[0].Title == "Images Gallery" && sidebarChildren[0].Url == imagesGalleryRoute && !sidebarChildren[0].IsFirst);
            }

            [Fact]
            public void AddsHealthItem()
            {
                const string externalServicesRoute = "/Admin/ExternalServices";
                builder.Mock<IRouteBuilder>().Setup(x => x.GetRoute("ExternalServices", "Admin")).Returns(externalServicesRoute);
                const string systemExceptionsRoute = "/Admin/SystemExceptions";
                builder.Mock<IRouteBuilder>().Setup(x => x.GetRoute("SystemExceptions", "Admin")).Returns(systemExceptionsRoute);

                var model = builder.ClassUnderTest.BuildAdminSideBar();
                var healthSidebarItem = model.SidebarItems.First(x => x.Title == "Health");
                var children = healthSidebarItem.Children.ToList();

                Assert.True(healthSidebarItem.IconClass == "fa fa-cogs" && !healthSidebarItem.IsFirst);
                Assert.Equal(2, children.Count());
                Assert.True(children[0].Title == "External Services" && children[0].Url == externalServicesRoute && children[0].IconClass == string.Empty && !children[0].IsFirst);
                Assert.True(children[1].Title == "System Exceptions" && children[1].Url == systemExceptionsRoute && children[1].IconClass == string.Empty && !children[1].IsFirst);
            }

            [Fact]
            public void AddsActivitiesMenuItem()
            {
                const string activitiesRoute = "/Activities";
                builder.Mock<IRouteBuilder>().Setup(x => x.GetRoute("Index", "Activity")).Returns(activitiesRoute);
                const string createActivityRoute = "/Activities/Create";
                builder.Mock<IRouteBuilder>().Setup(x => x.GetRoute("Create", "Activity")).Returns(createActivityRoute);

                var model = builder.ClassUnderTest.BuildAdminSideBar();
                var activitiesSidebarItem = model.SidebarItems.First(x => x.Title == "Activities");
                var sidebarChildren = activitiesSidebarItem.Children.ToList();

                Assert.True(sidebarChildren[0].Title == "All Activities" && sidebarChildren[0].Url == activitiesRoute);
                Assert.True(sidebarChildren[1].Title == "Add New Activity" && sidebarChildren[1].Url == createActivityRoute);
                Assert.True(activitiesSidebarItem.IconClass == "fa fa-cogs" && !activitiesSidebarItem.IsFirst);
            }

            [Fact]
            public void AddsVenuesMenuItem()
            {
                const string venuesRoute = "/Venues";
                builder.Mock<IRouteBuilder>().Setup(x => x.GetRoute("Index", "Venue")).Returns(venuesRoute);
                
                var model = builder.ClassUnderTest.BuildAdminSideBar();
                var activitiesSidebarItem = model.SidebarItems.First(x => x.Title == "Venues");
                var sidebarChildren = activitiesSidebarItem.Children.ToList();

                Assert.True(sidebarChildren[0].Title == "Manage Venues" && sidebarChildren[0].Url == venuesRoute);
                Assert.True(activitiesSidebarItem.IconClass == "fa fa-cogs" && !activitiesSidebarItem.IsFirst);
            }

            [Fact]
            public void SetsActiveToTrueIfCurrentUrlIsSameAsItemUrl()
            {
                 const string categoryCreateRoute = "http://hobbyclue.com/Admin/Categories";
                 builder.Mock<IRouteBuilder>().Setup(x => x.GetRoute("Create", "Category")).Returns(categoryCreateRoute);
                builder.HttpRequestMock.Setup(x => x.Url).Returns(new Uri(categoryCreateRoute));

                var model = builder.ClassUnderTest.BuildAdminSideBar();
                var activitiesSidebarItem = model.SidebarItems.First(x => x.Title == "Categories");
                var sidebarChildren = activitiesSidebarItem.Children.ToList();

                Assert.True(sidebarChildren.First(x => x.Url == categoryCreateRoute).IsActive);
                Assert.Equal(1, sidebarChildren.Count(x => x.IsActive));
            }
        }

        public class TestableSideBarModelBuilder : Facts<SideBarModelBuilder>
        {
            public Mock<HttpRequestBase> HttpRequestMock = new Mock<HttpRequestBase>();
            public string CurrentUrl = "http://hobbyclue.com/";
            public static TestableSideBarModelBuilder Create()
            {
                var builder = new TestableSideBarModelBuilder();
                builder.HttpRequestMock.Setup(x => x.Url).Returns(new Uri(builder.CurrentUrl));
                builder.Mock<HttpContextBase>().Setup(x => x.Request).Returns(builder.HttpRequestMock.Object);
                return builder;
            }
        }
    }
}
