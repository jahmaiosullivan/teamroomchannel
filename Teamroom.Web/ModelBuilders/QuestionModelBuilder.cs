using System.Collections.Generic;
using System.Linq;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ModelBuilders
{
    public interface IQuestionModelBuilder
    {
        List<MessageViewModel> ThreadReplies(IEnumerable<MessageViewModel> replies);
    }

    public class QuestionModelBuilder : IQuestionModelBuilder
    {
        public List<MessageViewModel> ThreadReplies(IEnumerable<MessageViewModel> replies)
        {
            var threadedComments = new List<MessageViewModel>();
            var skipList = new List<long>();
            foreach (var pMessage in replies)
            {
                if (!skipList.Contains(pMessage.Id))
                {
                    pMessage.Children = FindChildren(replies, pMessage.Id);
                    GenerateListOfIds(skipList, pMessage);
                    threadedComments.Add(pMessage);
                }
            }

            return threadedComments;
        }


        private static void GenerateListOfIds(List<long> ids, MessageViewModel message)
        {
            if (!ids.Contains(message.Id)) ids.Add(message.Id);
            foreach (var cMessage in message.Children)
            {
                GenerateListOfIds(ids, cMessage);
            }
        }




        private static IEnumerable<MessageViewModel> FindChildren(IEnumerable<MessageViewModel> replies, long parentMessageId)
        {
            foreach (var msg in replies.Where(msg => msg.ParentMessageId == parentMessageId))
            {
                msg.Children = FindChildren(replies, msg.Id);
                yield return msg;
            }
        }
    }
}