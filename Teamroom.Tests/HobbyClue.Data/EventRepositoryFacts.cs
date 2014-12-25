using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HobbyClue.Data.Repositories;
using HobbyClue.Tests.Helpers;
using Xunit;

namespace Teamroom.Tests.HobbyClue.Data
{
    public class EventRepositoryFacts
    {
        private readonly TestableEventRepository repository = TestableEventRepository.Create();

        [Fact]
        public void TestTest()
        {
            repository.ClassUnderTest.AssignPostToEvent(10, 10);
        }

        public class TestableEventRepository : Facts<EventRepository>
        {
            public static TestableEventRepository Create()
            {
                var repo = new TestableEventRepository();
                return repo;
            }
        }
    }
}
