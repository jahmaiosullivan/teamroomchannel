using System;

namespace HobbyClue.Web.ViewModels
{
    public enum VoteType
    {
        Down = 0,
        Up = 1
    }

    public class VoteModel
    {
        public long Id { get; set; }
        public long CardId { get; set; }
        public Guid UserId { get; set; }
        public VoteType Vote { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}