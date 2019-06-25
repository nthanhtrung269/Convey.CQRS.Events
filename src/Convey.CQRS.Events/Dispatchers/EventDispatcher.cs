using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Convey.CQRS.Events.Dispatchers
{
    internal sealed class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventDispatcher(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;
        
        public Task PublishAsync<T>(T @event) where T : class, IEvent
        {
            var handler = _serviceProvider.GetService<IEventHandler<T>>();
            
            if (handler is null)
            {
                throw new InvalidOperationException($"Event handler for: '{@event}' was not found.");
            }
                
            return handler.HandleAsync(@event);
        }
    }
}