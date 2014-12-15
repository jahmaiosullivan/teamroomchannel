using System;
using System.Web.Mvc;

namespace HobbyClue.Web.Controllers.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class LogExceptionAttribute : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Gets or sets the policy.
        /// </summary>
        /// <value>The policy.</value>
        public string Policy
        {
            get;
            set;
        }

        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            
        }
    }
}
