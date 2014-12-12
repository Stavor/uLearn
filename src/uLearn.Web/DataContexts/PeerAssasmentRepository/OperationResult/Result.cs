using System;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult
{
    public class Result<T>
    {
        public static Result<T> GetSuccess(T value)
        {
            return new Result<T>(value, ResultState.Success, null);
        }

        public static Result<T> GetFail(T value, Exception exc)
        {
            return new Result<T>(value, ResultState.Fail, exc);
        }

        private Result(T value, ResultState state, Exception failMessage)
        {
            FailMessage = failMessage;
            Value = value;
            State = state;
        }

        public bool IsFail
        {
            get { return State == ResultState.Fail; }
        }

        public T Value { get; private set; }
        public Exception FailMessage { get; private set; }
        private ResultState State { get; set; }
        
        private enum ResultState
        {
            Success,
            Fail
        }
    }
}