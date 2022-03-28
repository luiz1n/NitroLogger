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
