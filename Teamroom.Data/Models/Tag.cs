using System;

namespace HobbyClue.Data.Models
{
    public class Tag : ModelBase
    {
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string Description { get; set; }
        public long ParentId { get; set; }
    }
}
