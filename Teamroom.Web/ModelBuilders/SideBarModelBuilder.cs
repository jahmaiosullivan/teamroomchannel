using System;
using System.Collections.Generic;
using System.Web;
using HobbyClue.Business.Services;
using HobbyClue.Common.Models;
using HobbyClue.Data.Models;
using HobbyClue.Web.Configuration;
using HobbyClue.Web.Providers;
using HobbyClue.Web.ViewModels;
using AutoMapper;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;

namespace HobbyClue.Web.ModelBuilders
{
    public class SideBarModelBuilder : ISideBarModelBuilder
    {
        private readonly IRouteBuilder routeBuilder;
        private readonly HttpContextBase httpContext;
        private readonly ITagService tagService;
        private readonly IMappingEngine _mappingEngine;

        public SideBarModelBuilder(IRouteBuilder routeBuilder, HttpContextBase httpContext, ITagService tagService, IMappingEngine mappingEngine)
        {
            this.routeBuilder = routeBuilder;
            this.httpContext = httpContext;
            this.tagService = tagService;
            this._mappingEngine = mappingEngine;
        }

        public SideBarViewModel BuildModel()
        {
            var model = new SideBarViewModel
            {

            };

            var aEvent1 = new RecurringEvent
            {
                ID = 1,
                Title = "Critical Mass Bicycle Ride",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                MonthlyIntervalOptions = MonthlyIntervalEnum.Last,
                DaysOfWeekOptions = DayOfWeekEnum.Fri
            };

            var aEvent = new RecurringEvent
            {
                ID = 1,
                Title = "Every Mon and Wed",
                FrequencyTypeOptions = FrequencyTypeEnum.Weekly,
                DaysOfWeekOptions = DayOfWeekEnum.Mon | DayOfWeekEnum.Wed
            };

            var schedule = new Schedule(aEvent);

            // give me all the upcoming dates for the next year
            var range = new DateRange()
            {
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddYears(1)
            };

            var occurrences = schedule.Occurrences(range);


            return model;
        }


        private List<TagViewModel> GetSystemTags(long cityId)
        {
           var systemTags = new List<TagViewModel>{
                new TagViewModel{IsSelected = true,Name="Overview",Id=0, Description="General overview of city"}
           };
           IList<Tag> storedSystemTags = tagService.GetForCity(cityId).EmptyListIfNull();
           systemTags.AddRange(_mappingEngine.Map<IList<Tag>, IList<TagViewModel>>(storedSystemTags));
           return systemTags;
        }


        #region "Admin Sidebar"

        public AdminSidebarViewModel BuildAdminSideBar()
        {
            var adminSidebarViewModel = new AdminSidebarViewModel
            {
                SidebarItems = GetAdminSideBarItems()
            };
            return adminSidebarViewModel;
        }


        private IEnumerable<AdminSidebarViewModelItem> GetAdminSideBarItems()
        {
            var usersSideBarItem = CreateTopLevelSidebarItemViewModel("Users", "fa fa-user");
            var categoriesSideBarItem = CreateTopLevelSidebarItemViewModel("Categories", "fa fa-cogs");
            categoriesSideBarItem.Children = new List<AdminSidebarViewModelItem>
            {
                CreateSidebarItemViewModel("All Categories", routeBuilder.GetRoute("Index", "Category")),
                CreateSidebarItemViewModel("Add New Category", routeBuilder.GetRoute("Create", "Category"))
            };

            var galllerySideBarItem = CreateTopLevelSidebarItemViewModel("Gallery", "fa fa-cogs");
            galllerySideBarItem.Children = new List<AdminSidebarViewModelItem>
            {
                   CreateSidebarItemViewModel("Images Gallery", routeBuilder.GetRoute("Images", "Gallery"))
            };

            var activitiesSideBarItem = CreateTopLevelSidebarItemViewModel("Activities", "fa fa-cogs");
            activitiesSideBarItem.Children = new List<AdminSidebarViewModelItem>
            {
                CreateSidebarItemViewModel("All Activities", routeBuilder.GetRoute("Index", "Activity")),
                CreateSidebarItemViewModel("Add New Activity", routeBuilder.GetRoute("Create", "Activity"))
            };

            var venuesSideBarItem = CreateTopLevelSidebarItemViewModel("Venues", "fa fa-cogs");
            venuesSideBarItem.Children = new List<AdminSidebarViewModelItem>
            {
                CreateSidebarItemViewModel("Manage Venues", routeBuilder.GetRoute("Index", "Venue"))
            };

            var healthSideBarItem = CreateTopLevelSidebarItemViewModel("Health", "fa fa-cogs");
            healthSideBarItem.Children = new List<AdminSidebarViewModelItem>
            {
                   CreateSidebarItemViewModel("External Services", routeBuilder.GetRoute("ExternalServices", "Admin")),
                   CreateSidebarItemViewModel("System Exceptions", routeBuilder.GetRoute("SystemExceptions", "Admin"))
            };
            
            var sidebarItems = new List<AdminSidebarViewModelItem>
            {
                CreateSidebarItemViewModel("Dashboard", routeBuilder.GetRoute("Index", "Admin"), "fa fa-home", true),
                categoriesSideBarItem,
                activitiesSideBarItem,
                venuesSideBarItem,
                galllerySideBarItem,
                usersSideBarItem,
                healthSideBarItem
            };

            return sidebarItems;
        }

        private AdminSidebarViewModelItem CreateTopLevelSidebarItemViewModel(string title, string iconclass = "", bool isFirst = false)
        {
            return CreateSidebarItemViewModel(title, string.Empty, iconclass, isFirst);
        }
        
        private AdminSidebarViewModelItem CreateSidebarItemViewModel(string title, string url, string iconclass = "", bool isFirst = false)
        {
            var isActive = url == httpContext.Request.Url.ToString();
            return new AdminSidebarViewModelItem
            {
                Title = title,
                IconClass = iconclass,
                IsFirst = isFirst,
                Url = url,
                IsActive = isActive
            };
        }

        #endregion
    }
}