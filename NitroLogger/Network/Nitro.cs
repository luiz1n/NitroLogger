using NitroLogger.Network.Communication;
using NitroLogger.Sulakore;
using System;

namespace NitroLogger.Network
{
    public class Nitro
    {
        public static void SendToServer(short header, params object[] values)
            => Server.Sessions.Broadcast(HMessage.Construct(header, values));
        public static void SendToClient(short header, params object[] values)
            => Client.client.Send(HMessage.Construct(header, values));

        public static void Disconnect() 
        {
            Server.server.Stop();
            Client.client.Close();
        }


        public static event EventHandler OnStarted;
        public static event EventHandler OnStopped;
        public static event EventHandler<HMessage> OnMessage;


        public static void OnStartedConnection()
           => OnStarted?.Invoke(null, EventArgs.Empty);

        public static void OnStoppedConnection() 
            => OnStopped?.Invoke(null, EventArgs.Empty);

        public static void OnMessageReceived(HMessage hmessage)
            => OnMessage?.Invoke(null, hmessage);

    }
}
