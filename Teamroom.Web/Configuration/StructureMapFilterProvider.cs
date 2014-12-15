using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Teamroom.Web.Configuration;
using WebGrease.Css.Extensions;

namespace HobbyClue.Web.Configuration
{
    public class StructureMapFilterProvider : FilterAttributeFilterProvider
    {
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor).ToList();
            filters.Select(x => x.Instance).ForEach(WebContainer.Current.BuildUp);
            return filters;
        }
    }
}