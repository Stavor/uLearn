using System;
using uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository.Storage
{
    public interface IPeerAssasmentStorage
    {
        Result<T> TryUpdate<T>(T obj)
            where T : class;

        Result<T> TryInsert<T>(T obj)
            where T : class;

        Result<T[]> TryRead<T>(Func<T, bool> condition)
            where T : class;
    }
}