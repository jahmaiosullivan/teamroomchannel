using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;
using Teamroom.Business.Providers;

namespace HobbyClue.Web.Controllers
{
    public class HomeController : CommonControllerBase
    {
        private readonly ICityService cityService;
        private readonly IMappingEngine mappingEngine;
        private readonly IUserProvider userProvider;
        private readonly IIntroService introService;
        public HomeController(ICityService cityService, 
             IMappingEngine mappingEngine, IUserProvider userProvider, IIntroService introService)
        {
             this.cityService = cityService;
            this.mappingEngine = mappingEngine;
            this.userProvider = userProvider;
            this.introService = introService;
        }

        public ActionResult Index()
        {
            var intros = introService.FindAll();
            var model = new HomePageViewModel
            {
                Intros = mappingEngine.Map<IEnumerable<Intro>, IEnumerable<IntroViewModel>>(intros)
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ComingSoon()
        {
            return View();
        }
        public ActionResult ChooseLocation(bool? overrideRedirect)
        {
            var cities = cityService.FindAll();
            
            var cities2 = new SortedDictionary<string, IList<City>>();
            foreach (var city in cities)
            {
                if(!cities2.ContainsKey(city.Region))
                    cities2.Add(city.Region, new List<City>{city} );
                else
                    cities2[city.Region].Add(city);
            }

            return View(new ChooseLocationViewModel
            {
                Override = overrideRedirect ?? false,
                Regions = cities2
            });
        }
    }
}
