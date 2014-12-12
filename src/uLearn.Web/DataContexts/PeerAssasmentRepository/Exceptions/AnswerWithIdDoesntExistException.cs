using System;
using uLearn.Web.Models.PeerAssasmentModels;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository.Exceptions
{
    public class AnswerWithIdDoesntExistException : Exception
    {
        public AnswerWithIdDoesntExistException(AnswerId answerId)
            : base(GetMessage(answerId))
        {
        }

        private static string GetMessage(AnswerId answerId)
        {
            return string.Format("База не содержит Answer c answerId:  [UserId: {0}, CourseId: {1}, SlideId: {2}]",
                answerId.UserId ?? "NULL",
                answerId.CourseId ?? "NULL",
                answerId.SlideId ?? "NULL");
        }
    }
}