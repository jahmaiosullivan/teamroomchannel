using System;

namespace HobbyClue.Web.ViewModels
{
    public class ViewModelBase
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserViewModel CreatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public UserViewModel LastUpdatedBy { get; set; }
    }
}