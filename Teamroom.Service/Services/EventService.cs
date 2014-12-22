using System;
using System.Collections.Generic;
using System.Globalization;
using HobbyClue.Common.Extensions;
using HobbyClue.Common.Helpers;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public class EventService : BaseDapperService<Event>, IEventService
    {
        private readonly IEventRepository eventRepository;
        private readonly IBodyTextSanitizer bodyTextSanitizer;
        private readonly IPostService postService;
        public EventService(IEventRepository repository, IBodyTextSanitizer bodyTextSanitizer, IUserProvider userProvider, IPostService postService)
            : base(repository, userProvider)
        {
            eventRepository = repository;
            this.bodyTextSanitizer = bodyTextSanitizer;
            this.postService = postService;
        }

        public override void BeforeSave(Event item)
        {
           base.BeforeSave(item);
           PrepareItem(ref item);
        }

        public override void BeforeUpdate(Event item)
        {
            base.BeforeUpdate(item);
            PrepareItem(ref item);
        }
        
        /// <summary>
        /// Gets the first event from each group the user is a member of
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetUpcoming(Guid userId)
        {
            return eventRepository.GetUpcoming(userId);
        }

        public IEnumerable<Event> GetUpcomingForGroup(long groupId)
        {
            return eventRepository.GetUpcomingForGroup(groupId);
        }

        private void PrepareItem(ref Event item)
        {
            if (item.StartDateTime.HasValue) item.StartDateTime = item.StartDateTime.Value.ConvertToUTC();
            if (item.EndDateTime.HasValue) item.EndDateTime = item.EndDateTime.Value.ConvertToUTC();
            item.Description = bodyTextSanitizer.CleanHtml(item.Description, new CultureInfo("en-us"));
        }

        public Event CreateRecurringInstance(Event ev, DateTime instanceDate)
        {
            ev.ParentId = ev.Id;
            ev.StartDateTime = instanceDate;
            return Save(ev);
        }

        public Event GetRecurringInstanceByDate(long eventId, DateTime instanceDate)
        {
            return eventRepository.GetRecurringInstanceByDate(eventId, instanceDate);
        }

        public Post CreateEventPost(Post post, long eventId)
        {
            if(post.PostedBy == Guid.Empty) post.PostedBy = userProvider.CurrentUserId;
            if(!post.PostedDate.HasValue) post.PostedDate = DateTime.UtcNow;
            var savedPost = postService.Save(post);
            
            eventRepository.AssignPostToEvent(savedPost.PostId, eventId);
            
            return savedPost;
        }

        public void DeleteEventPost(long postId, long eventId)
        {
            DeleteEventPost(new Post { PostId = postId }, eventId);
        }

        public IEnumerable<Event> GetPastForGroup(long groupId)
        {
            return eventRepository.GetPastForGroup(groupId);
        }

        public void DeleteEventPost(Post post, long eventId)
        {
            eventRepository.UnAssignPostFromEvent(post.PostId, eventId);
            postService.Delete(post);
        }
    }
}
