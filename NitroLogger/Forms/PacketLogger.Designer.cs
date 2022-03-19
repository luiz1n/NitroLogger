using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NitroLogger.Forms
{
    partial class PacketLogger
    {
        private readonly IContainer components = null;

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOutgoingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewIncomingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topMostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pktLogTxt = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(694, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewOutgoingToolStripMenuItem,
            this.viewIncomingToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.topMostToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.viewToolStripMenuItem.Text = "Options";
            // 
            // viewOutgoingToolStripMenuItem
            // 
            this.viewOutgoingToolStripMenuItem.Name = "viewOutgoingToolStripMenuItem";
            this.viewOutgoingToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewOutgoingToolStripMenuItem.Text = "View Outgoing";
            this.viewOutgoingToolStripMenuItem.Click += new System.EventHandler(this.viewOutgoingToolStripMenuItem_Click);
            // 
            // viewIncomingToolStripMenuItem
            // 
            this.viewIncomingToolStripMenuItem.Name = "viewIncomingToolStripMenuItem";
            this.viewIncomingToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewIncomingToolStripMenuItem.Text = "View Incoming";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // topMostToolStripMenuItem
            // 
            this.topMostToolStripMenuItem.Name = "topMostToolStripMenuItem";
            this.topMostToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.topMostToolStripMenuItem.Text = "TopMost";
            this.topMostToolStripMenuItem.Click += new System.EventHandler(this.topMostToolStripMenuItem_Click);
            // 
            // pktLogTxt
            // 
            this.pktLogTxt.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pktLogTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pktLogTxt.Location = new System.Drawing.Point(0, 27);
            this.pktLogTxt.Name = "pktLogTxt";
            this.pktLogTxt.Size = new System.Drawing.Size(694, 469);
            this.pktLogTxt.TabIndex = 1;
            this.pktLogTxt.Text = "";
            this.pktLogTxt.TextChanged += new System.EventHandler(this.pktLogTxt_TextChanged);
            // 
            // PacketLogger
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(694, 493);
            this.Controls.Add(this.pktLogTxt);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PacketLogger";
            this.Text = "NitroLogger ~ PacketLogger";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private MenuStrip menuStrip1;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem viewOutgoingToolStripMenuItem;
        private ToolStripMenuItem viewIncomingToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem topMostToolStripMenuItem;
        private RichTextBox pktLogTxt;

    }
}