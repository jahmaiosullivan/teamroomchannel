using System;
using AutoMapper;
using HobbyClue.Data.Models;
using HobbyClue.Web.AutoMappings.Resolvers;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings
{
    public class PostMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Post, PostViewModel>()
                .ForMember(d => d.PostedByName, opts => opts.Ignore())
                .ForMember(d => d.PostedByAvatar, opts => opts.UseValue("/Images/user.png"))
                .ForMember(d => d.NewCommentMessage, opts => opts.Ignore())
                .ForMember(d => d.PostComments, opts => opts.ResolveUsing<PostCommentsResolver>());

            CreateMap<Post, EventPostViewModel>()
                .ForMember(d => d.PostedByName, opts => opts.Ignore())
                .ForMember(d => d.PostedByAvatar, opts => opts.UseValue("/Images/user.png"))
                .ForMember(d => d.NewCommentMessage, opts => opts.Ignore())
                .ForMember(d => d.EventId, opts => opts.Ignore())
                .ForMember(d => d.PostComments, opts => opts.ResolveUsing<PostCommentsResolver>());

            CreateMap<Post, GroupPostViewModel>()
                .ForMember(d => d.PostedByName, opts => opts.Ignore())
                .ForMember(d => d.PostedByAvatar, opts => opts.UseValue("/Images/user.png"))
                .ForMember(d => d.NewCommentMessage, opts => opts.Ignore())
                .ForMember(d => d.GroupId, opts => opts.Ignore())
                .ForMember(d => d.PostComments, opts => opts.ResolveUsing<PostCommentsResolver>());

            CreateMap<GroupPostViewModel, Post>()
                .ForMember(d => d.PostedBy, opts => opts.MapFrom(s => !string.IsNullOrEmpty(s.PostedBy) ? Guid.Parse(s.PostedBy) : Guid.Empty));
            

            CreateMap<EventPostViewModel, Post>()
                .ForMember(d => d.PostedBy, opts => opts.MapFrom(s => !string.IsNullOrEmpty(s.PostedBy) ? Guid.Parse(s.PostedBy) : Guid.Empty));
               
            CreateMap<PostComment, PostCommentViewModel>()
                .ForMember(d => d.CommentedByName, opts => opts.MapFrom(s => s.UserProfile.UserName))
                .ForMember(d => d.CommentedByAvatar, opts => opts.UseValue("/Images/user.png"));

        }
    }
}