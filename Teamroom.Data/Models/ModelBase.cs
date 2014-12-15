using System;
using HobbyClue.Common.Attributes;

namespace HobbyClue.Data.Models
{
    public class ModelBase
    {
        protected ModelBase()
        {
            IsActive = true;
        }

        [AutoSuppliedFromDatabase]
        [PrimaryKey]
        public virtual long Id { get; set; }

        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
