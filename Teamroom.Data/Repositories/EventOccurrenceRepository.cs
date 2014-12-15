using System;
using System.Linq;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IEventOccurrenceRepository : IDapperRepository<EventOccurrence>
    {
        EventOccurrence Get(DateTime occurrenceDate);
    }

    public class EventOccurrenceRepository : BaseDapperRepository<EventOccurrence>, IEventOccurrenceRepository
    {
        public EventOccurrenceRepository(IQueryManager queryManager)
            : base(queryManager)
        {

        }

        public EventOccurrence Get(DateTime occurrenceDate)
        {
            var sql = BaseQuery + " WHERE t1.Date = @OccurrenceDate";
            var occurrence = _queryManager.ExecuteSql<EventOccurrence>(sql, new { @OccurrenceDate = occurrenceDate }).FirstOrDefault();
            return occurrence;
        }

    }
}
