using System;
using uLearn.Web.Models.PeerAssasmentModels;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository.Exceptions
{
    public class InconsistentStateByAnswerIdException : Exception
    {
        public InconsistentStateByAnswerIdException(AnswerId answerId)
            :base(GetMessage(answerId))
        {
        }

        private static string GetMessage(AnswerId answerId)
        {
            return string.Format("Обнаружено не консистентное состояние: пользователь [userId: {0}] имеет более одного ответа [courseId:{1}, slideId:{2}]",
                answerId.UserId ?? "NULL",
                answerId.CourseId ?? "NULL",
                answerId.SlideId ?? "NULL");
        }
    }
}