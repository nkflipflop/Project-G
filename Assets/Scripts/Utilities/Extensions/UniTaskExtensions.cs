using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Utilities
{
    public static partial class Extensions
    {
        public static async UniTask PeriodicAsync(Func<UniTask> action, float interval, float initialDelay = 0f,
            CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(initialDelay), cancellationToken: cancellationToken);
            while (true)
            {
                UniTask delayTask = UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: cancellationToken);
                await action();
                await delayTask;
            }
        }
    }
}