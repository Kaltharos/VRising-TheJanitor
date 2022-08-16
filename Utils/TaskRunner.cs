//-- https://github.com/adainrivers/randomencounters/blob/2e0f4357cba5b32bfe9bcdc6c5324ef4d78d3677/src/Components/TaskRunner.cs

using System;
using System.Collections.Concurrent;
using TheJanitor.Hooks;
using Unity.Entities;

namespace TheJanitor.Utils
{
    public class TaskRunner
    {
        private static readonly ConcurrentQueue<RisingTask> TaskQueue = new();

        public static void Initialize()
        {
            ServerEvents.OnUpdate += Update;
        }

        public static Guid Start(Func<World, object> func, bool runNow = true, TimeSpan startAfter = default)
        {
            var risingTask = new RisingTask
            {
                ResultFunction = func,
                RunNow = runNow,
                StartAfter = DateTime.UtcNow.Add(startAfter),
                TaskId = Guid.NewGuid()
            };
            TaskQueue.Enqueue(risingTask);
            return risingTask.TaskId;
        }

        private static void Update(World world)
        {
            if (!TaskQueue.TryDequeue(out var task))
            {
                return;
            }

            if (!task.RunNow)
            {
                if (task.StartAfter > DateTime.UtcNow)
                {
                    TaskQueue.Enqueue(task);
                    return;
                }
            }

            object result;
            try
            {
                result = task.ResultFunction.Invoke(world);
            }
            catch { }
        }

        public static void Destroy()
        {
            ServerEvents.OnUpdate -= Update;
            TaskQueue.Clear();
        }

        private class RisingTask
        {
            public Guid TaskId { get; set; } = Guid.NewGuid();
            public bool RunNow { get; set; }
            public DateTime StartAfter { get; set; }
            public Func<World, object> ResultFunction { get; set; }
        }
    }
}