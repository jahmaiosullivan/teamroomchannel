using System;

namespace HobbyClue.Business.Providers
{
    public interface IUserProvider
    {
        Guid CurrentUserId { get; set; }
        bool IsAuthenticated { get; }
    }
}
