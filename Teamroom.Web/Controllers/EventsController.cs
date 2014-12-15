using System;
using System.Web.Mvc;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IMappingEngine mappingEngine;
        private readonly IEventService eventService;
        public EventsController(IMappingEngine mappingEngine, IEventService eventService)
        {
            this.mappingEngine = mappingEngine;
            this.eventService = eventService;
        }

        [HttpGet]
        public ActionResult Create(Guid eventId)
        {
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public ActionResult Index(long id)
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult CreateEventModal()
        {
            return PartialView("EventModal", new EventViewModel{Mode="Create", StartDate = DateTime.Now});
        }

        [HttpGet]
        public ActionResult EditEventModal(long id)
        {
            var ev = eventService.GetById(id);
            var eventModel = mappingEngine.Map<Event, EventViewModel>(ev);
            eventModel.Mode = "Edit";
            return PartialView("EventModal", eventModel);
        }
    }
}