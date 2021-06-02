using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Networking
{
    /// <summary>
    /// A dedicated server
    /// </summary>
    public class GameServer
    {
        public GameServer(int port)
        {
            Port = port;
        }

        public int Port { get; }

        TcpServer? Server;
        private CancellationTokenSource? _tokenSource;

        public async Task Start(CancellationToken? token = null)
        {
            Server = new TcpServer(IPAddress.Any, Port);
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token ?? new CancellationToken());
            await Server.StartAsync(token);
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
            Server?.Stop();
            Server?.Dispose();
        }
    }
}
