using System;
using System.Collections.Generic;
using System.Linq;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public class VoteService : BaseDapperService<Vote>, IVoteService
    {
        private readonly IVoteRepository _repository;
        public VoteService(IVoteRepository repository, IUserProvider userProvider)
            : base(repository, userProvider)
        {
            _repository = repository;
        }
        
        public IDictionary<long, IList<Vote>> Get(IEnumerable<long> cardIds = null)
        {
            var votes = _repository.Get(cardIds);
            return votes;
        }

        public Vote GetForUser(long cardId, Guid userId)
        {
            var cardvotes = Get(new List<long>{cardId}).FirstOrDefault();
            return (cardvotes.Value != null && cardvotes.Value.Any())
                ? cardvotes.Value.FirstOrDefault(x => x.CreatedBy == userId)
                : null;
        }

        public override Vote Save(Vote entity)
        {
            var vote = GetForUser(entity.CardId, entity.CreatedBy.Value);
            if (vote != null)
            {
                vote.Value = entity.Value;
                Update(vote);
                return vote;
            }
            base.Save(entity);
            return entity;
        }

    }
}
