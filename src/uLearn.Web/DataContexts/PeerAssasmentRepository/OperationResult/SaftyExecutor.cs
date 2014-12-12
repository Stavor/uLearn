using System;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult
{
    public static class SaftyExecutor
    {
        public static Result<T2> TryMake<T1, T2>(T1 arg, Func<T1, T2> func, string failMessage = null) //todo добавить шаблонный метод для catch ?
        {
            var result = default(T2);
            try
            {
                result = func(arg);
            }
            catch (Exception e)
            {
                var exc = string.IsNullOrEmpty(failMessage)
                    ? e
                    : new Exception(failMessage, e);
                return result.MarkAsFail(exc);
            }
            return result.MarkAsSuccess();
        }
    }
}