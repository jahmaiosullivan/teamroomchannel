using System;
using System.Collections.Generic;
using System.Linq;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;
using ScheduleWidget.Enums;

namespace HobbyClue.Data.Repositories
{
    public interface IEventRepository : IDapperRepository<Event>
    {
        IEnumerable<Event> GetUpcoming(Guid userId);
        IEnumerable<Event> GetUpcomingForGroup(long groupId);
        IEnumerable<Event> GetPastForGroup(long groupId);
        Event GetRecurringInstanceByDate(long eventId, DateTime instanceDate);
        void AssignPostToEvent(long postId, long eventId);
        void UnAssignPostFromEvent(long postId, long eventId);
    }

    public class EventRepository : BaseDapperRepository<Event>, IEventRepository
    {
        public EventRepository(IQueryManager queryManager) : base(queryManager)
        {
        }

        public IEnumerable<Event> GetUpcoming(Guid userId)
        {
            var sql = _queryManager.GetQuery("GetUpcomingEvents");
            var events = _queryManager.ExecuteSql<Event>(sql, new { @userId = userId });
            return events;
        }

        public IEnumerable<Event> GetUpcomingForGroup(long groupId)
        {
            var sql = BaseQuery + " WHERE t1.GroupId = @GroupId and t1.IsActive = 1 AND ((t1.Frequency > @NoRepeat and t1.ParentId=0) " +
                                  " OR t1.StartDateTime > getutcdate()) order by t1.StartDateTime asc";
            var events = _queryManager.ExecuteSql<Event>(sql, new { @GroupId = groupId, @NoRepeat = (int)FrequencyTypeEnum.None });
            return events;
        }

        public IEnumerable<Event> GetPastForGroup(long groupId)
        {
            var sql = BaseQuery + " WHERE t1.GroupId = @GroupId and t1.IsActive = 1 and t1.StartDateTime < getutcdate()  AND ((t1.Frequency > 0 AND t1.ParentId > 0) or t1.Frequency = 0) order by t1.StartDateTime desc";
            var events = _queryManager.ExecuteSql<Event>(sql, new { @GroupId = groupId });
            return events;
        }

        public Event GetRecurringInstanceByDate(long eventId, DateTime instanceDate)
        {
            var sql = BaseQuery + " WHERE t1.ParentId = @EventId and instanceDate = @instanceDate";
            var events = _queryManager.ExecuteSql<Event>(sql, new { @EventId = eventId, @instanceDate = instanceDate }).FirstOrDefault();
            return events;
        }

        public void AssignPostToEvent(long postId, long eventId)
        {
            const string query = "INSERT INTO [EventPosts] ([EventId],[PostId]) VALUES (@EventId, @PostId)";
            _queryManager.ExecuteNonQuery(query, new { @EventId = eventId, @PostId = postId  });
        }

        public void UnAssignPostFromEvent(long postId, long eventId)
        {
            const string query = "DELETE FROM [EventPosts] WHERE [EventId] = @EventId AND [PostId] = @PostId";
            _queryManager.ExecuteNonQuery(query, new { @EventId = eventId, @PostId = postId });
        }
    }
}
