using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Core.Networking
{
    /// <summary>
    /// Handles connecting to the server
    /// </summary>
    public class GameClient
    {
        public static GameClient? Client { get; set; }

        public string? ClientId { get; private set; }
        public string Connect { get; }
        public int Port { get; }

        public GameClient(string ServerAddress, int port)
        {
            //ClientId = Guid.NewGuid().ToString(); //.Split('-').First();
            Connect = ServerAddress;
            Port = port;
        }

        public string SendMessage(string message)
        {
            using var client = new TcpClient();
            client.Connect(Connect, Port);

            using var stream = client.GetStream();
            using var buffer = new ByteBuffer(message);
            buffer.WriteToStream(stream);
            using var response = new ByteBuffer(stream);
            return response.ReadString();
        }
    }
}
