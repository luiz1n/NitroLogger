using NitroLogger.Network.Communication;
using NitroLogger.Sulakore;

namespace NitroLogger.Network
{
    public class Nitro
    {
        public static void SendToServer(short header, params object[] values)
            => Server.Sessions.Broadcast(HMessage.Construct(header, values));
        public static void SendToClient(short header, params object[] values)
            => Client.client.Send(HMessage.Construct(header, values));

    }
}
