using System;
using System.Collections.Concurrent;

namespace Modulos.Messaging.Configuration
{
    public class ConnectionMarkProvider : IConnectionMarkProvider
    {
        private readonly ConcurrentDictionary<EndpointConfigMark, ConnectionMark> marks =
            new ConcurrentDictionary<EndpointConfigMark, ConnectionMark>();

        public ConnectionMark Get(EndpointConfigMark endpointConfigMark)
        {
            if (endpointConfigMark == null) throw new ArgumentNullException(nameof(endpointConfigMark));
            if (marks.TryGetValue(endpointConfigMark, out var connectionMark))
                return connectionMark;

            connectionMark = new ConnectionMark(Guid.NewGuid());
            marks.TryAdd(endpointConfigMark, connectionMark);
            return connectionMark;
        }

        public void Refresh(EndpointConfigMark endpointConfigMark)
        {
            if (endpointConfigMark == null) throw new ArgumentNullException(nameof(endpointConfigMark));
            marks.TryRemove(endpointConfigMark, out _);
        }
    }
}