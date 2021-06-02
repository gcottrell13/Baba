using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Networking
{
    public class DataReceivedEventArgs : EventArgs
    {
        public NetworkStream Stream { get; private set; }

        public DataReceivedEventArgs(NetworkStream stream)
        {
            Stream = stream;
        }
    }

    public class TcpServer : IDisposable
    {
        private readonly TcpListener _listener;
        private CancellationTokenSource? _tokenSource;
        private CancellationToken _token;

        public event EventHandler<DataReceivedEventArgs>? OnDataReceived;

        public TcpServer(IPAddress address, int port)
        {
            _listener = new TcpListener(address, port);
        }

        public bool Listening { get; private set; }

        public async Task StartAsync(CancellationToken? token = null)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token ?? new CancellationToken());
            _token = _tokenSource.Token;
            _listener.Start();
            Listening = true;

            try
            {
                while (!_token.IsCancellationRequested)
                {
                    await Task.Run(async () =>
                    {
                        var result = await _listener.AcceptTcpClientAsync();
                        OnDataReceived?.Invoke(this, new DataReceivedEventArgs(result.GetStream()));
                    }, _token);
                }
            }
            finally
            {
                _listener.Stop();
                Listening = false;
            }
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
