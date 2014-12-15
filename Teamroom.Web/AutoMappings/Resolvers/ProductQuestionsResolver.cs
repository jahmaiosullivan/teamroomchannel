using System;
using System.Collections.Generic;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class ProductQuestionsResolver : ValueResolver<Product, IEnumerable<QuestionViewModel>>
    {
        private readonly IQuestionService questionService;
        private readonly IMappingEngine mappingEngine;
        public ProductQuestionsResolver(IMappingEngine mappingEngine, IQuestionService questionService)
        {
            this.mappingEngine = mappingEngine;
            this.questionService = questionService;
        }

        protected override IEnumerable<QuestionViewModel> ResolveCore(Product source)
        {
            var questions = questionService.Get(source.Id);
            return mappingEngine.Map<IEnumerable<Question>, IEnumerable<QuestionViewModel>>(questions);
        }
    }
}