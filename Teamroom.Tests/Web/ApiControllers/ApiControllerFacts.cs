using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;

namespace HobbyClue.Tests.Web.ApiControllers
{
    public class ApiControllerFacts
    {
        [Theory]
        [InlineData("put")]
        [InlineData("post")]
        public void ValidateAllApiMethodsWithSpecificNameShouldHaveAntiForgeryToken(string methodName)
        {
            var methodsCalledPutWithoutAntiForgeryAttribute = GetApiControllerMethodsWithNoAntiForgeryCheck()
                                                                    .Where(m => String.Equals(m.Name, methodName, StringComparison.CurrentCultureIgnoreCase));

            foreach (var methodInfo in methodsCalledPutWithoutAntiForgeryAttribute)
            {
                Assert.Null(methodInfo);
            }
       }

        [Theory]
        [InlineData(typeof(HttpPutAttribute))]
        [InlineData(typeof(HttpPostAttribute))]
        public void ValidateAllApiMethodsWithHttpPutAttributeHaveAntiForgeryToken(Type httpAttribute)
        {
            var methodsWithHttpPutAttributeButNoAntiForgeryToken = GetApiControllerMethodsWithNoAntiForgeryCheck()
                            .Where(m => m.GetCustomAttributes(httpAttribute, false).Length > 0).ToList();

            foreach (var methodInfo in methodsWithHttpPutAttributeButNoAntiForgeryToken)
            {
                Assert.Null(methodInfo);
            }
        }


        private IEnumerable<MethodInfo> GetApiControllerMethodsWithNoAntiForgeryCheck()
        {
            var type = typeof(ApiController);
            var webAssembly = Assembly.Load("HobbyClue.Web");
            var controllers = webAssembly.GetTypes().Where(type.IsAssignableFrom).ToList();
            return controllers.SelectMany(t => t.GetMethods()).Where(m => !m.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute)).Any());
        }
    }
}
