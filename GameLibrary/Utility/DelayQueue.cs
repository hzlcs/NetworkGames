using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Utility
{
    public class DelayQueue<T>
    {
        private readonly PriorityQueue<T, DateTime> queue = new();
        private readonly PeriodicTimer timer = new(Timeout.InfiniteTimeSpan);

        public void Enqueue(T item, TimeSpan delay)
        {
            DateTime time = DateTime.Now + delay;
            queue.Enqueue(item, time);
            if (queue.TryPeek(out _, out DateTime t) && t == time)
                timer.Period = delay;
        }

        public async Task<T> DequeueAsync()
        {
            if (queue.TryPeek(out _, out DateTime t))
            {
                if (t < DateTime.Now)
                    return queue.Dequeue();
                timer.Period = t - DateTime.Now;
                await timer.WaitForNextTickAsync();
                return queue.Dequeue();
            }
            timer.Period = Timeout.InfiniteTimeSpan;
            await timer.WaitForNextTickAsync();
            return queue.Dequeue();
        }

    }


}
