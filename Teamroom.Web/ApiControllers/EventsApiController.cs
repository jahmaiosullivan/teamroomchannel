using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.Controllers.Attributes;
using HobbyClue.Web.ModelBuilders;
using HobbyClue.Web.ViewModels;
using Teamroom.Business.Providers;

namespace HobbyClue.Web.ApiControllers
{
    public class EventsApiController : ApiController
    {
        private readonly IEventsModelBuilder eventsModelBuilder;
        private readonly IEventService eventService;
        private readonly IMappingEngine mappingEngine;
        public EventsApiController(IEventsModelBuilder eventsModelBuilder, IEventService eventService, IUserProvider userProvider, IMappingEngine mappingEngine)
        {
            this.eventsModelBuilder = eventsModelBuilder;
            this.eventService = eventService;
            this.mappingEngine = mappingEngine;
        }

        // GET: api/EventsApi
        public IEnumerable<EventViewModel> Get()
        {
            return eventsModelBuilder.Build();
        }

        // GET: api/EventsApi/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [ValidateApiAntiForgeryToken]
        public EventViewModel Create(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                var savedEvent = eventService.Save(eventModel);
                return mappingEngine.Map<Event, EventViewModel>(savedEvent);
            }

            throw new Exception("Cannot save");
        }


        [HttpPost]
        [ValidateApiAntiForgeryToken]
        public EventViewModel Edit(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                if (eventService.Update(eventModel))
                {
                    var eventResult = mappingEngine.Map<Event, EventViewModel>(eventModel);
                    //Update parent if there is one
                    if (eventModel.ParentId > 0)
                    {
                        eventModel.Id = eventModel.ParentId;
                        eventModel.ParentId = 0;
                        eventService.Update(eventModel);
                    }
                    return eventResult;
                }
                return null;
            }

            throw new Exception("Cannot update");
        }


        // POST: api/EventsApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/EventsApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EventsApi/5
        public void Delete(Event eventObj)
        {
            eventService.Delete(eventObj);
        }

        [HttpPost]
        [ValidateApiAntiForgeryToken]
        public PostViewModel CreatePost(EventPostViewModel post)
        {
            var postToSave = mappingEngine.Map<EventPostViewModel, Post>(post);
            var newPost = eventService.CreateEventPost(postToSave, post.EventId);
            return mappingEngine.Map<Post, PostViewModel>(newPost);
        }


        [HttpDelete]
        [ValidateApiAntiForgeryToken]
        public HttpResponseMessage DeletePost(EventPostViewModel post)
        {
            eventService.DeleteEventPost(post.PostId, post.EventId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
