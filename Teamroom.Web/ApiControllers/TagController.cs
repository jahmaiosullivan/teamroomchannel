using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.Models;
using HobbyClue.Web.Providers;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ApiControllers
{
    public class TagController : ApiController
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        public IEnumerable<Tag> Get()
        {
            return tagService.FindAll();
        }

        // GET api/category/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/category
        public void Post([FromBody]string value)
        {
        }

        // PUT api/category/5
        public void Put(Tag newCategory)
        {
            tagService.Save(newCategory);
        }

    
    }
}
