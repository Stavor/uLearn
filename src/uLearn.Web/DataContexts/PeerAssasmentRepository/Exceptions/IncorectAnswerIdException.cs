using System;
using uLearn.Web.Models.PeerAssasmentModels;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository.Exceptions
{
    public class IncorectAnswerIdException : Exception
    {
        public IncorectAnswerIdException(AnswerId answerId)
            :base(GetMessage(answerId))
        {
        }

        private static string GetMessage(AnswerId answerId)
        {
            if (answerId == null)
                return "AnswerId не может быть NULL.";

            return string.Format("Не коррестный answerId [UserId: {0}, CourseId: {1}, SlideId: {2}]",
                answerId.UserId ?? "NULL",
                answerId.CourseId ?? "NULL",
                answerId.SlideId ?? "NULL");
        }
    }
}