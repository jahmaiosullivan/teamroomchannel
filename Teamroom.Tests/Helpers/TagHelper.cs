using System;
using System.Collections.Generic;
using HobbyClue.Data.Models;

namespace HobbyClue.Tests.Helpers
{
    public static class TagHelper
    {
        public static List<Tag> Create(int num)
        {
            var results = new List<Tag>();
            for (var i = 0; i < num; i++)
            {
                results.Add(new Tag{Name = "tag" + DateTime.Now.Ticks});
            }
            return results;
        } 
    }
}
