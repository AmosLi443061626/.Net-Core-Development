using CoreCommon.RedisHelper;
using System;
using System.Threading;

namespace CoreCommon.Lockings
{
    public class DistributedLockManager
    {
        /// <summary>
        /// 阻塞式调用，事情最终会被调用（等待时间内）
        /// </summary>
        /// <param name="resource">锁定资源的标识</param>
        /// <param name="expiryTime">锁过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <param name="work"></param>
        public static bool BlockingWork(string resource, TimeSpan expiryTime, TimeSpan waitTime, Action work)
        {
            var redLock = StackExchangeRedisHelper.Instance().GetRedlock();
            Lock lockObject;
            bool locked;
            GetLockObjetc(resource, expiryTime, waitTime, redLock, out lockObject, out locked);
            LockExec(work, redLock, lockObject, locked);
            return locked;
        }


        /// <summary>
        /// 跳过式调用，如果事情正在被调用，直接跳过
        /// </summary>
        /// <param name="resource">锁定资源的标识</param>
        /// <param name="expiryTime">锁过期时间</param>
        /// <param name="work"></param>
        public static bool OverlappingWork(string resource, TimeSpan expiryTime, Action work)
        {
            var redLock = StackExchangeRedisHelper.Instance().GetRedlock();
            Lock lockObject;
            bool locked;
            GetLockObjetc(resource, expiryTime, new TimeSpan(0), redLock, out lockObject, out locked);
            LockExec(work, redLock, lockObject, locked);
            return locked;
        }

        #region 私有方法
        private static void LockExec(Action work, Redlock redLock, Lock lockObject, bool locked)
        {
            try
            {
                if (locked)
                    work();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (locked)
                    redLock.Unlock(lockObject);
            }
        }

        private static void GetLockObjetc(string resource, TimeSpan expiryTime, TimeSpan waitTime, Redlock redLock, out Lock lockObject, out bool locked)
        {
            locked = redLock.Lock(resource, expiryTime, out lockObject);
            if (waitTime.Seconds == 0)
                return;
            int waitSecond = waitTime.Seconds;
            while (!locked && waitSecond > 1)
            {
                locked = redLock.Lock(resource, expiryTime, out lockObject);
                if (locked)
                    break;
                #region 超时
                Thread.Sleep(1000);
                waitSecond--;
                #endregion
            }
        }
        #endregion
    }
}
