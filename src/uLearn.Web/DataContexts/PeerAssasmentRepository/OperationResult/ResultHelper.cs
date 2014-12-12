using System;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult
{
    public static class ResultHelper
    {
        public static Result<T> MarkAsFail<T>(this T obj, Exception failMessage)
        {
            if (failMessage == null)
                throw new ArgumentNullException("failMessage");

            return Result<T>.GetFail(obj, failMessage);
        }

        public static Result<T> MarkAsSuccess<T>(this T obj)
        {
            return Result<T>.GetSuccess(obj);
        }

        public static Result<T2> SucceedsWith<T1, T2>(this Result<T1> curRes, Func<T1, Result<T2>> next)
        {
            if (curRes.IsFail)
                throw curRes.FailMessage;

            return next(curRes.Value);
        }

        public static T2 SucceedsWith<T1, T2>(this Result<T1> curRes, Func<T1, T2> next)
        {
            if (curRes.IsFail)
                throw curRes.FailMessage;

            return next(curRes.Value);
        }

        public static Result<T2> IfSuccess<T1, T2>(this Result<T1> curRes, Func<T1, Result<T2>> next)
        {
            return curRes.IsFail
                ? Result<T2>.GetFail(default(T2), curRes.FailMessage)
                : next(curRes.Value);
        }
    }
}