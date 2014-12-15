using System;
using System.Collections.Generic;
using HobbyClue.Data.Models;

namespace HobbyClue.Business.Services
{
    public interface IEventService : IBaseService<Event>
    {
        IEnumerable<Event> GetUpcoming(Guid userId);
        IEnumerable<Event> GetUpcomingForGroup(long groupId);
        Event CreateRecurringInstance(Event ev, DateTime instanceDate);
        Event GetRecurringInstanceByDate(long eventId, DateTime instanceDate);
        Post CreateEventPost(Post post, long eventId);
        void DeleteEventPost(Post post, long eventId);
        void DeleteEventPost(long postId, long eventId);
        IEnumerable<Event> GetPastForGroup(long groupId);
    }
}