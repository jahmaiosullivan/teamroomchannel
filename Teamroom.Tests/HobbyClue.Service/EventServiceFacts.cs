using HobbyClue.Business.Services;
using HobbyClue.Tests.Helpers;

namespace HobbyClue.Tests.HobbyClue.Service
{
    public class EventServiceFacts
    {
        
    }

    public class TestableEventService : Facts<EventService>
    {
        public static TestableEventService Create()
        {
            var service = new TestableEventService();
            return service;
        }
    }
}
