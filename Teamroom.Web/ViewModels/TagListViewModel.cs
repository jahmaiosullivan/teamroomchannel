using System.Collections.Generic;
using System.ComponentModel;
using HobbyClue.Data.Models;
using HobbyClue.Web.Configuration.TypeConverters;

namespace HobbyClue.Web.ViewModels
{
    [TypeConverter(typeof(TagListTypeConverter))]
    public class TagListViewModel : List<Tag>
    {
    }
}