using System;
using System.Collections.Generic;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public class TagService : BaseDapperService<Tag>, ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository, IUserProvider userProvider)
            : base(tagRepository, userProvider)
        {
            _tagRepository = tagRepository;
        }

        public IDictionary<long, IList<Tag>> Get(IEnumerable<long> cardIds = null)
        {
            return _tagRepository.Get(cardIds);
        }

        public IEnumerable<Tag> GetForUser(Guid userId)
        {
            return _tagRepository.GetForUser(userId);
        }

        public IEnumerable<Tag> GetForCity(long cityId)
        {
            return _tagRepository.GetCityDefaultTags(cityId);
        }

        public IEnumerable<Tag> GetTopLevelTags()
        {
            return _tagRepository.GetTopLevelTags();
        }

        public IEnumerable<Tag> GetChildTags(long tagId)
        {
            return _tagRepository.GetChildTags(tagId);
        }

        public IEnumerable<Tag> GetForHobby(long hobbyId)
        {
            return _tagRepository.GetForHobby(hobbyId);
        }

        public IEnumerable<Tag> GetLastUsed(int number, int pageNum = 1)
        {
            return _tagRepository.GetLastUsed(number, pageNum);
        }

        public Tag GetForUser(Guid userId, long tagId)
        {
            return _tagRepository.GetForUser(userId, tagId);
        }

        public virtual IEnumerable<Tag> FindAllInUse()
        {
            return _tagRepository.GetUsedByCards();
        }

        public void AddUserTag(Guid userId, long tagId)
        {
            _tagRepository.AddUserTag(userId, tagId);
        }

        public void RemoveUserTag(Guid userId, long tagId)
        {
            _tagRepository.DeleteUserTag(userId, tagId);
        }
    }
}
