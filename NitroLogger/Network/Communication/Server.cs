using WebSocketSharp;
using WebSocketSharp.Server;

using NitroLogger.Sulakore;

namespace NitroLogger.Network.Communication
{
    public class ServerL : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Client.client.SslConfiguration = Context.WebSocket.SslConfiguration;
            Client.client.Connect();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                HMessage message = new HMessage(e.RawData, true);
                Client.client.Send(message.ToBytes());
            }
        }
    }

    public class Server
    {
        public static WebSocketServer server;
        public static WebSocketSessionManager Sessions => server.WebSocketServices["/"].Sessions;

        public static void Start() 
        {
            server = new WebSocketServer(9092);
            server.AddWebSocketService<ServerL>("/");
            server.Start();
        }
    }
}
