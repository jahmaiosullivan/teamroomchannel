using System;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class IdToUserViewModelResolver : ValueResolver<Guid, UserViewModel>
    {
        private readonly IUserService userService;
        private readonly IMappingEngine mappingEngine;
        public IdToUserViewModelResolver(IUserService userService, IMappingEngine mappingEngine)
        {
            this.userService = userService;
            this.mappingEngine = mappingEngine;
        }

        protected override UserViewModel ResolveCore(Guid source)
        {
            var user = userService.GetById(source);
            return mappingEngine.Map<User, UserViewModel>(user);
        }
    }
}