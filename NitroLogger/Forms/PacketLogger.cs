using System;
using System.Drawing;
using System.Windows.Forms;

namespace NitroLogger.Forms
{
    public partial class PacketLogger : Form
    {

        public PacketLogger()
        {
            InitializeComponent();
            pktLogTxt.ReadOnly = true;
        }

        private void viewOutgoingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewOutgoingToolStripMenuItem.Checked = !viewOutgoingToolStripMenuItem.Checked;
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            topMostToolStripMenuItem.Checked = !topMostToolStripMenuItem.Checked;
            TopMost = !TopMost;
        }

        private void pktLogTxt_TextChanged(object sender, EventArgs e)
        {
            pktLogTxt.SelectionStart = pktLogTxt.Text.Length;
            pktLogTxt.ScrollToCaret();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pktLogTxt.Clear();
        }

        #region Log
        public void Log(string msg, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new LogDelegate(Log), msg, color);
            }
            else
            {
                pktLogTxt.SelectionStart = pktLogTxt.TextLength;
                pktLogTxt.SelectionLength = 0;
                pktLogTxt.SelectionColor = color;
                pktLogTxt.AppendText(pktLogTxt.TextLength > 0 ? "\n" + msg : msg);
                pktLogTxt.SelectionColor = pktLogTxt.ForeColor;
            }

        }
        public void LogClient(string msg)
        {
            Log(msg, Color.Red);
        }

        public void LogServer(string msg)
        {
            Log(msg, Color.LimeGreen);
        }

        private delegate void LogDelegate(string msg, Color color);

        #endregion

    }
}
