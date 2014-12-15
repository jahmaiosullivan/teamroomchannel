using System;
using System.Globalization;
using System.Web.Http;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.Providers;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ApiControllers
{
    public class VotesController : ApiController
    {
        private readonly IVoteService voteService;
        private readonly IMappingEngine mappingEngine;
        public VotesController(IVoteService voteService, IMappingEngine mappingEngine)
        {
            this.voteService = voteService;
            this.mappingEngine = mappingEngine;
        }
        
        public Vote Get(long id)
        {
            return voteService.GetById(id.ToString(CultureInfo.InvariantCulture));
        }

        // POST: api/Votes
        public void Post([FromBody] VoteModel vote)
        {
            var voteObj = mappingEngine.Map<VoteModel, Vote>(vote);
            voteObj.CreatedDate = DateTime.UtcNow;
            voteService.Save(voteObj);
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
