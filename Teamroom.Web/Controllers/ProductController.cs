using System;
using System.Web.Mvc;
using AutoMapper;
using HobbyClue.Business.Providers;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IMappingEngine mappingEngine;
        private readonly IUserProvider userProvider;
        public ProductController(IProductService productService, IMappingEngine mappingEngine, IUserProvider userProvider)
        {
            this.productService = productService;
            this.mappingEngine = mappingEngine;
            this.userProvider = userProvider;
        }

        public ActionResult Index(long id)
        {
            var product = productService.GetById(1);
            var model = mappingEngine.Map<Product, ProductViewModel>(product);
            return View("Index", model);
        }

        public ActionResult List(string name)
        {
            var product = productService.GetByName(name);
            var model = mappingEngine.Map<Product, ProductViewModel>(product);

            ViewBag.ProductLogo = model.LogoUrl;
            ViewBag.ProductDescription = model.Description;
            ViewBag.ProductName = model.Name;

            return View("Index", model);
        }


        [ChildActionOnly]
        [HttpGet]
        public ActionResult ProductsSidebar()
        {
            var products = productService.Get(userProvider.CurrentUserId);
            return View(products);
        }
    }
}
