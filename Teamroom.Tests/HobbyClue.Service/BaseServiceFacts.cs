using System;
using HobbyClue.Business.Services;
using HobbyClue.Common.Attributes;
using HobbyClue.Data;
using Moq;
using Xunit;

namespace HobbyClue.Tests.HobbyClue.Service
{
    public class BaseServiceFacts
    {
        public class Add
        {
            private readonly TestableBaseService baseService = TestableBaseService.Create();

        }

        public class Update
        {
            private readonly TestableBaseService baseService = TestableBaseService.Create();


            [Fact(Skip = "")]
            public void CallsRepoAdd()
            {
                var obj = new TestObject();
                //baseService.RepoMock.Setup(x => x.Find(obj, "Id")).Returns(obj);

                baseService.Update(obj);

                baseService.RepoMock.Verify(x => x.Update(obj), Times.Once);
            }
        }

        public class FindAll
        {
            private readonly TestableBaseService baseService = TestableBaseService.Create();

            [Fact(Skip = "")]
            public void CallsFindAllApiOnRepositoryWithCorrectObject()
            {
                baseService.FindAll();

                baseService.RepoMock.Verify(x => x.Find(It.IsAny<string>()), Times.Once);
            }
        }


        #region Setup
        public class TestableBaseService : TestableBaseServiceBase<TestObject>
        {
            public Mock<IRepository<TestObject>> RepoMock;
            public TestableBaseService(Mock<IRepository<TestObject>> repoMock) : base(repoMock.Object)
            {
                RepoMock = repoMock;
            }

            public static TestableBaseService Create()
            {
                var service = new TestableBaseService(new Mock<IRepository<TestObject>>());
                return service;
            }

            public override TestObject GetById(object id)
            {
                throw new NotImplementedException();
            }

            public override bool Update(TestObject entity)
            {
                return true;
            }
        }

        public class TestableBaseServiceWithPrimaryKey : TestableBaseServiceBase<TestObjectWithPrimaryKey>
        {
            public Mock<IRepository<TestObjectWithPrimaryKey>> RepoMock;
             public TestableBaseServiceWithPrimaryKey(Mock<IRepository<TestObjectWithPrimaryKey>> repoMock)
                 : base(repoMock.Object)
            {
                RepoMock = repoMock;
            }

             public static TestableBaseServiceWithPrimaryKey Create()
            {
                var service = new TestableBaseServiceWithPrimaryKey(new Mock<IRepository<TestObjectWithPrimaryKey>>());
                return service;
            }

            public override TestObjectWithPrimaryKey GetById(object id)
            {
                throw new NotImplementedException();
            }

            public override bool Update(TestObjectWithPrimaryKey entity)
            {
                return true;
            }
        }

        public abstract class TestableBaseServiceBase<T> : BaseService<T> where T : class
        {
            protected TestableBaseServiceBase(IRepository<T> repository)
                : base(repository)
            {
            }

            public override void Validate(T item)
            {
            }

            public override void BeforeSave(T item)
            {
            }

            public override void BeforeUpdate(T item)
            {
            }

            public override void BeforeDelete(T item)
            {
            }

            public override void AfterDelete(T item)
            {
            }

            public override void AfterSave(T item)
            {
            }
        }


        public class TestObject { }

        public class TestObjectWithPrimaryKey
        {
            [PrimaryKey]
            public int Key { get; set; }
        }

        #endregion 
    }
}
