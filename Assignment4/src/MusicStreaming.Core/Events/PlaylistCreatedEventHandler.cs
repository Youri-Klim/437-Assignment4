using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Core.Events
{
    public class PlaylistCreatedEventHandler : IDomainEventHandler<PlaylistCreatedEvent>
    {
        public Task HandleAsync(PlaylistCreatedEvent domainEvent, CancellationToken cancellationToken = default)
        {
            // Use Console.WriteLine instead of _logger
            Console.WriteLine("====== PLAYLIST CREATED EVENT HANDLED ======");
            Console.WriteLine($"🎉 New playlist '{domainEvent.PlaylistTitle}' created by User {domainEvent.UserId}");
            return Task.CompletedTask;
        }
    }
}