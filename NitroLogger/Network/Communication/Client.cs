﻿using WebSocketSharp;
using NitroLogger.Sulakore;
using NitroLogger.Forms;
using System.Diagnostics;
using System;

namespace NitroLogger.Network.Communication
{
    public class Client
    {
        public static WebSocket client;

        public static PacketLogger packetLogger;
        public static Form1 form;

        public static void Start(string SocketUrl) 
        {
            client = new WebSocket(SocketUrl);

            client.OnMessage += (object sender, MessageEventArgs e) =>
            {
                HMessage message = new HMessage(e.RawData, false);

                Nitro.OnMessageReceived(message);

                Server.Sessions.Broadcast(message.ToBytes());
            };

        }
    }
}
