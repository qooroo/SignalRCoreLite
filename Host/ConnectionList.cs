using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Connections;

namespace Host
{
    internal class ConnectionList : IReadOnlyCollection<ConnectionContext>
    {
        private readonly ConcurrentDictionary<string, ConnectionContext> _connections = new ConcurrentDictionary<string, ConnectionContext>(StringComparer.Ordinal);

        public ConnectionContext this[string connectionId] => _connections.TryGetValue(connectionId, out var connection) ? connection : null;

        public int Count => _connections.Count;

        public void Add(ConnectionContext connection) => _connections.TryAdd(connection.ConnectionId, connection);

        public void Remove(ConnectionContext connection) => _connections.TryRemove(connection.ConnectionId, out var dummy);

        public IEnumerator<ConnectionContext> GetEnumerator() => _connections.Select(item => item.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
