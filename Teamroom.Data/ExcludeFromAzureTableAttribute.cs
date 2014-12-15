using System;

namespace HobbyClue.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromAzureTableAttribute : Attribute
    {
    }
}
