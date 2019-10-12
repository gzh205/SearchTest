using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Crawler
{
    class ThreadQueue
    {
        public ThreadQueue(int num)
        {
            queue = new Queue<Thread>[num];
            for (int i = 0; i < queue.Length; i++)
                queue[i] = new Queue<Thread>();
        }
        public void addTask(Thread t)
        {
            int minNum = 0;
            for (int i = 0; i < queue.Length; i++)
                if (queue[i].Count < minNum)
                    minNum = queue[i].Count;
            queue[minNum].Enqueue(t);
        }

        private Queue<Thread>[] queue;
    }
}
