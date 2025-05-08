using MusicStreaming.Core.Events;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}