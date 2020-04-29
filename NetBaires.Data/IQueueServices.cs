using System.Collections.Generic;

namespace NetBaires.Data
{
    public interface IQueueServices
    {
        void AddMessage<TData>(TData data);
        TData GetMessage<TData>();
        List<TData> GetMessages<TData>(int count = 10);
        void Clear<TData>();
    }
    public enum QueueNameEnum
    {
        EarnedBadge
    }
}