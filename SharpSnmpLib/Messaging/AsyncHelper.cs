using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    internal static class AsyncHelper
    {
        private static readonly TaskFactory MyTaskFactory = new
            (CancellationToken.None,
             TaskCreationOptions.None,
             TaskContinuationOptions.None,
             TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return MyTaskFactory
              .StartNew(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            MyTaskFactory
              .StartNew(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }
    }
}
