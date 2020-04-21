namespace NetBaires.Api.Services
{
    public interface IQueueServices
    {
        void AddMessage<TData>(TData data);
        TData GetMessage<TData>();
        void Clear<TData>();
    }
    public enum QueueNameEnum
    {
        EarnedBadge
    }
}