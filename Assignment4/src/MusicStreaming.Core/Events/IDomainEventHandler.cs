using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Core.Events
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
    }
}