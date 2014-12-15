using System;
using AutoMapper;
using HobbyClue.Common.Extensions;
using HobbyClue.Data.Models;
using HobbyClue.Web.AutoMappings.Resolvers;
using HobbyClue.Web.ViewModels;
using ScheduleWidget.Enums;

namespace HobbyClue.Web.AutoMappings
{
    public class EventMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Event, EventViewModel>()
                 .ForMember(d => d.Attendees, opts => opts.Ignore())
                 .ForMember(d => d.Address, opts => opts.MapFrom(s => s.Location))
                 .ForMember(d => d.Description, opts => opts.MapFrom(s => s.Description))
                 .ForMember(d => d.OccurrencesFormatted, opts => opts.Ignore())
                 .ForMember(d => d.Posts, opts => opts.ResolveUsing<EventPostsResolver>())
                 .ForMember(d => d.Occurrences, opts => opts.ResolveUsing<EventOccurencesResolver>())
                 .ForMember(d => d.Mode, opts => opts.Ignore())
                 .ForMember(d => d.FirstDayOfWeek, opts => opts.Ignore())
                 .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDateTime.Value.ConvertFromUtcToTimeZone()))
                 .ForMember(d => d.MonthlyWeekNum, opts => opts.MapFrom(s => s.MonthlyInterval))
                 .ForMember(d => d.Creator, opts => opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.CreatedBy ?? Guid.Empty))
                 .ForMember(d => d.StartDateDisplayText, opts => opts.Ignore())
                 .ForMember(d => d.StartTime, opts => opts.Ignore())
                 .ForMember(d => d.EndTime, opts => opts.MapFrom(s => s.EndDateTime.HasValue ? s.EndDateTime.Value.ConvertFromUtcToTimeZone().FormatTimeOnly() : string.Empty))
                 .ForMember(d => d.Hosts, opts => opts.Ignore())
                 .ForMember(d => d.Frequency, opts => opts.MapFrom(s => (FrequencyTypeEnum) s.Frequency))
                 .ForMember(d => d.NewComment, opts => opts.Ignore())
                 .ForMember(d => d.LocationName, opts => opts.MapFrom(s => s.Location))
                 .ForMember(d => d.MaxAttendees, opts => opts.MapFrom(s => s.MaxAttendees ?? 0))
                 .ForMember(d => d.Image, opts => opts.MapFrom(s => s.Images))
                 .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Name))
                 .ForMember(d => d.DaysOfWeek, opts => opts.ResolveUsing<DaysOfWeekResolver>().FromMember(s => s.DaysOfWeek));

            
            CreateMap<EventViewModel, Event>()
                 .ForMember(d => d.Location, opts => opts.MapFrom(s => s.Address))
                 .ForMember(d => d.Images, opts => opts.MapFrom(s => s.Image))
                 .ForMember(d => d.AnniversaryMonth, opts => opts.Ignore())
                 .ForMember(d => d.AnniversaryDay, opts => opts.Ignore())
                 .ForMember(d => d.DaysOfWeek, opts => opts.Ignore())
                 .ForMember(d => d.EndDateTime, opts => opts.Ignore())
                 .ForMember(d => d.Frequency, opts => opts.Ignore())
                 .ForMember(d => d.MonthlyInterval, opts => opts.Ignore())
                 .ForMember(d => d.NumberOfOccurrencesThatWasLastSet, opts => opts.Ignore())
                 .ForMember(d => d.OneTimeOnlyEventDate, opts => opts.Ignore())
                 .ForMember(d => d.QuarterInterval, opts => opts.Ignore())
                 .ForMember(d => d.QuarterlyInterval, opts => opts.Ignore())
                 .ForMember(d => d.MonthRangeStartMonth, opts => opts.Ignore())
                 .ForMember(d => d.MonthRangeStartDayMonth, opts => opts.Ignore())
                 .ForMember(d => d.MonthRangeEndMonth, opts => opts.Ignore())
                 .ForMember(d => d.MonthRangeEndDayMonth, opts => opts.Ignore())
                 .ForMember(d => d.RepeatInterval, opts => opts.Ignore())
                 .ForMember(d => d.StartDateTime, opts => opts.Ignore())
                 .ForMember(d => d.EndDateTime, opts => opts.Ignore())
                 .ForMember(d => d.GroupId, opts => opts.Ignore())
                 .ForMember(d => d.HostPerson, opts => opts.Ignore())
                 .ForMember(d => d.LastUpdatedBy, opts => opts.Ignore())
                 .ForMember(d => d.LastUpdatedDate, opts => opts.Ignore())
                 .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.Creator.Id))
                 .ForMember(d => d.Location, opts => opts.MapFrom(s => s.LocationName))
                 .ForMember(d => d.MaxAttendees, opts => opts.MapFrom(s => s.MaxAttendees))
                 .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Title));

        }
    }
}