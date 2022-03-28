using NitroLogger.Network;
using NitroLogger.Network.Communication;
using NitroLogger.Network.Proxy;
using NitroLogger.Sulakore;
using NitroLogger.Utils;

using System;
using System.Windows.Forms;

namespace NitroLogger.Forms
{
    public partial class Form1 : Form
    {
        private PacketLogger packetLogger;

        public Form1()
        {
            InitializeComponent();
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
                Invoke(new Action(() => Started()));

            Nitro.OnStopped += (object sender, EventArgs e) =>
                Invoke(new Action(() => Stopped()));

            Nitro.OnMessage += (object sender, HMessage packet) =>
            {
                string structure = ToStructure(packet);

                if (!packet.IsOutgoing)
                {
                    Invoke(new Action(() => packetLogger.LogClient(structure)));
                }
                else
                {
                    Invoke(new Action(() => packetLogger.LogServer(structure)));
                }
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


        #region Send To Server & Client
        private void btnSendToServer_Click(object sender, EventArgs e)
        {
            HMessage packet = new HMessage(textBox1.Text);
            Nitro.SendToServer(packet.ToBytes());
        }

        private void btnSendToClient_Click(object sender, EventArgs e)
        {
            HMessage packet = new HMessage(textBox1.Text);
            Nitro.SendToServer(packet.ToBytes());
        }
        #endregion
    }
}
