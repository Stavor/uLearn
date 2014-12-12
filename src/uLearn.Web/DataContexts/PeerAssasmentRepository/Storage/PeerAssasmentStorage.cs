using System;
using System.Data.Entity;
using System.Linq;
using uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository.Storage
{
    public class PeerAssasmentStorage : IPeerAssasmentStorage
    {
        private readonly ULearnDb db;

        public PeerAssasmentStorage()
            : this(new ULearnDb())
        {
        }

        private PeerAssasmentStorage(ULearnDb db)
        {
            this.db = db;
        }

        public Result<T> TryUpdate<T>(T obj)
            where T : class
        {
            if (obj == null)
                return default(T).MarkAsFail(new ArgumentNullException("obj"));

            return TryExecuteQuery(
                d =>
                {
                    d.Entry(obj).State = EntityState.Modified;
                    d.SaveChanges();
                    return obj;
                });
        }

        public Result<T> TryInsert<T>(T obj)
            where T : class
        {
            if (obj == null)
                return default(T).MarkAsFail(new ArgumentNullException("obj"));

            return TryExecuteQuery(
                d =>
                {
                    var t = d.Set<T>().Add(obj);
                    d.SaveChanges();
                    return t;
                });
        }

        public Result<T[]> TryRead<T>(Func<T, bool> condition)
            where T : class
        {
            if (condition == null)
                return default(T[]).MarkAsFail(new ArgumentNullException("condition"));

            return TryExecuteQuery(d =>
                d.Set<T>()
                    .Where(condition)
                    .ToArray());
        }

        private Result<T> TryExecuteQuery<T>(Func<ULearnDb, T> func)
        {
            return SaftyExecutor.TryMake(db, func);
        }
    }
}