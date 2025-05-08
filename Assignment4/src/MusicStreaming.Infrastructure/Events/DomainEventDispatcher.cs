using Microsoft.Extensions.DependencyInjection;
using MusicStreaming.Core.Events;
using MusicStreaming.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Infrastructure.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        
        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            
            dynamic? handler = _serviceProvider.GetService(handlerType);
            if (handler != null)
            {
                await handler.HandleAsync((dynamic)domainEvent, cancellationToken);
            }
        }
    }
}