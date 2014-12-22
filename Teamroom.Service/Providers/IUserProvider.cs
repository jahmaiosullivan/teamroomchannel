using System;

namespace Teamroom.Business.Providers
{
    public interface IUserProvider
    {
        Guid CurrentUserId { get; set; }
        bool IsAuthenticated { get; }
    }
}
