using System;
using Moq;
using StructureMap.AutoMocking;

namespace HobbyClue.Tests.Helpers
{
    public class Facts<TClassUnderTest> where TClassUnderTest : class
    {
        protected MoqAutoMocker<TClassUnderTest> AutoMocker { get; private set; }

        public TClassUnderTest ClassUnderTest
        {
            get
            {
                return AutoMocker.ClassUnderTest;
            }
        }

        public Facts()
        {
            AutoMocker = new MoqAutoMocker<TClassUnderTest>();
        }

        public Mock<T> Mock<T>(params Object[] args) where T : class
        {
            var autoMock = AutoMocker.Get<T>();
            return Moq.Mock.Get(autoMock);
        }
        
        public TDependency Get<TDependency>() where TDependency : class
        {
            return AutoMocker.Get<TDependency>();
        }
    }
}
