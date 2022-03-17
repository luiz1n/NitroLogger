using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace NitroLogger.Network.Communication
{
    public class ServerL : WebSocketBehavior
    {
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
