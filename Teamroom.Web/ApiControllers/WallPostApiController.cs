using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.Controllers.Attributes;
using HobbyClue.Web.ViewModels;
using Teamroom.Business.Providers;

namespace HobbyClue.Web.ApiControllers
{
    public class WallPostApiController : ApiController
    {
        private readonly IUserProvider userProvider;
        private readonly IPostService postService;
        private readonly IMappingEngine mappingEngine;
        public WallPostApiController(IUserProvider userProvider, IPostService postService, IMappingEngine mappingEngine)
        {
            this.userProvider = userProvider;
            this.postService = postService;
            this.mappingEngine = mappingEngine;
        }

        [HttpGet]
        public IList<PostViewModel> Get(long id)
        {
            var allPosts = postService.GetPostsForGroup(id);
            var postModels = mappingEngine.Map<IEnumerable<Post>, IList<PostViewModel>>(allPosts);
            return postModels;
        }


        [HttpPost]
        [ValidateApiAntiForgeryToken]
        public Post Get(int id)
        {
            Post post = postService.GetById(id);
            if (post == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return post;
        }


        [HttpPut]
        [ValidateApiAntiForgeryToken]
        public HttpResponseMessage Put(int id, Post post)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != post.PostId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                post.PostId = id;
                postService.Update(post);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPost]
        [ValidateApiAntiForgeryToken]
        public PostViewModel Post(Post post)
        {
            post.PostedBy = userProvider.CurrentUserId;
            post.PostedDate = DateTime.UtcNow;
            ModelState.Remove("post.PostedBy");
            ModelState.Remove("post.PostedDate");

            if (ModelState.IsValid)
            {
                postService.Save(post);
                var model = mappingEngine.Map<Post, PostViewModel>(post);
                return model;
            }
            throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request");
        }


        [HttpDelete]
        [ValidateApiAntiForgeryToken]
        public HttpResponseMessage Delete(int id)
        {
            Post post = postService.GetById(id);
            if (post == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            //db.Posts.Remove(post);

            try
            {
                postService.Delete(post);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, post);
        }

    }
}
