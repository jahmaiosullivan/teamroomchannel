using System;

namespace HobbyClue.Web.ViewModels
{
    public abstract class BaseViewModel
    {
        public UserViewModel Creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}