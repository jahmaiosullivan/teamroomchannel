using System.Collections.Generic;
using System.ComponentModel;
using HobbyClue.Web.Configuration.TypeConverters;

namespace HobbyClue.Web.Models
{
    [TypeConverter(typeof(CommaSeparatedListTypeConverter))]
    public class CommaSeparatedList : List<string>
    {

    }
}