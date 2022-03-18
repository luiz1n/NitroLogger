using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private void Log(bool isOutgoing, string packet) 
        {
            pktLogTxt.SelectionStart = pktLogTxt.TextLength;
            pktLogTxt.SelectionLength = 0;
            pktLogTxt.SelectionColor = isOutgoing ? Color.LimeGreen : Color.Red;
            pktLogTxt.AppendText(pktLogTxt.TextLength > 0 ? "\n" + packet : packet);
        }

        public void LogClient(string packet) => Log(false, packet);
        public void LogServer(string packet) => Log(true, packet);
        #endregion

    }
}
