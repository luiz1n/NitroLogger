using NitroLogger.Network.Proxy;
using NitroLogger.Utils;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeStuffs();
        }

        public void InitializeStuffs() 
        {
            lblStatus.Text = Texts.STANDING_BY;
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
    }
}
