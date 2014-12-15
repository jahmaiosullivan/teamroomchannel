using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using HobbyClue.Business.Providers;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ApiControllers
{
    public class CommentApiController : ApiController
    {
        private readonly IPostCommentService postCommentService;
        private readonly IMappingEngine mappingEngine;
        private readonly IUserProvider userProvider;

        public CommentApiController(IUserProvider userProvider, IPostCommentService postCommentService, IMappingEngine mappingEngine)
        {
            this.userProvider = userProvider;
            this.postCommentService = postCommentService;
            this.mappingEngine = mappingEngine;
        }

        // GET api/Comment
        public IEnumerable<PostComment> Get()
        {
            var postcomments = postCommentService.FindAll(); //db.PostComments.Include(p => p.Post);
            return postcomments.AsEnumerable();
        }

        // GET api/Comment/5
        public PostComment Get(int id)
        {
            PostComment postcomment = postCommentService.GetById(id); //db.PostComments.Find(id);
            if (postcomment == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return postcomment;
        }

        // PUT api/Comment/5
        public HttpResponseMessage Put(int id, PostComment postcomment)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != postcomment.CommentId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                postCommentService.Save(postcomment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public PostCommentViewModel Post(PostComment postcomment)
        {
            postcomment.CommentedBy = userProvider.CurrentUserId;
            postcomment.CommentedDate = DateTime.UtcNow;
            ModelState.Remove("postcomment.CommentedBy");
            ModelState.Remove("postcomment.CommentedDate");
            if (ModelState.IsValid)
            {
                postCommentService.Save(postcomment);
                var commentModel = mappingEngine.Map<PostComment, PostCommentViewModel>(postcomment);
                commentModel.CommentedByName = "Jahmai OSully";
            
                return commentModel;
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Bad Request");
            }
        }

        // DELETE api/Comment/5
        public HttpResponseMessage Delete(int id)
        {
            var postcomment = postCommentService.GetById(id);
            if (postcomment == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                postCommentService.Delete(postcomment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, postcomment);
        }
    }
}
