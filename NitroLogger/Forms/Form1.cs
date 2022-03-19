using NitroLogger.Network;
using NitroLogger.Network.Communication;
using NitroLogger.Network.Proxy;
using NitroLogger.Sulakore;
using NitroLogger.Sulakore.Endianness;
using NitroLogger.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NitroLogger.Forms
{
    public partial class Form1 : Form
    {
        private PacketLogger packetLogger;

        public Form1()
        {
            InitializeComponent();

            string sk = "{\"socket.url\":\"wss://nitro.ironhotel.biz\",\"asset.url\":\"https://cdn.ironhotel.biz/auto-nitro\",";

            var test = Regex.Match(sk, "socket.url\":\"(.+?)\",").Groups[1].Value;
            Debug.WriteLine(test);


            InitializeStuffs();
        }

        public static string ToStructure(HMessage packet)
        {
            return
                 !packet.IsOutgoing ?
                     $"[Incoming - {packet.Header}] -> {packet}"
                 :
                     $"[Outgoing - {packet.Header}] -> {packet}";

        }


        public void InitializeStuffs() 
        {
            packetLogger = new PacketLogger();
            lblStatus.Text = Texts.STANDING_BY;

            Server.Start();

            Nitro.OnStarted += (object sender, EventArgs e) =>
                Invoke(new Action(() => this.Started()));

            Nitro.OnStopped += (object sender, EventArgs e) =>
                Invoke(new Action(() => this.Stopped()));



            Nitro.OnMessage += (object sender, HMessage packet) =>
            {
                if (packet.Header == 4000) 
                {
                    packet.ReplaceString("PRODUCTION-201611291003-338511768", 0);
                    packet.ReplaceString("HTML5", 1);
                }

                string structure = ToStructure(packet);

                if (!packet.IsOutgoing)
                    Invoke(new Action(() => packetLogger.LogClient(structure)));
                else
                    Invoke(new Action(() => packetLogger.LogServer(structure)));
            };

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Intercept")
            {
                button1.Text = "Stop";
                lblStatus.Text = Texts.WAITING;
                Handler.Start();
            }
            else 
            {
                button1.Text = "Intercept";
                lblStatus.Text = Texts.STANDING_BY;
                Handler.Stop();
            }
        }

        private void Started() 
        {
            packetLogger.Show();
            Text = "NitroLogger ~ Connected";
            lblStatus.Text = Texts.CONNECTED;
        }

        private void Stopped() 
        {
            try
            {
                packetLogger.Hide();
                Text = "NitroLogger ~ Disconnected";
                lblStatus.Text = Texts.WAITING;
            }
            catch { }
        }

    }
}
