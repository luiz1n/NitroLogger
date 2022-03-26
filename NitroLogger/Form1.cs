using NitroLogger.Proxy;
using Sulakore.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace NitroLogger
{

    class Client {

        public static WebSocket client;

        public static void Start( string ws ) {
            client = new WebSocket(ws);
            client.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;

            client.OnMessage += (object sender, MessageEventArgs e) =>
            {
                HMessage message = new HMessage(e.RawData);
                Form1.server.WebSocketServices["/"].Sessions.Broadcast(message.ToBytes());
            };

        }
    }

    class Servidor : WebSocketBehavior {
        protected override void OnOpen()
        {
            if (!Client.client.IsAlive)
            {
                Client.client.Connect();
            }
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var packet = new HMessage(e.RawData);
            MessageBox.Show(packet.Header.ToString());
        }

    }

    public partial class Form1 : Form
    {

        public static WebSocketServer server;

        public Form1()
        {
            InitializeComponent();

            server = new WebSocketServer( port:9092 );
            server.AddWebSocketService<Servidor>("/");
            server.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Intercept")
            {


                button1.Text = "Stop";
                ProxyHandler.Start();
            }
            else {
                button1.Text = "Intercept";
            }
        }
    }
}
