using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using TAPWin;

namespace TAPWinApp
{
    public partial class Form1 : Form
    {

        private bool once;
        private int chars;
        private System.Windows.Forms.Timer aTimer;

        public Form1()
        {
            this.once = true;
            this.chars = 0;
            InitializeComponent();
            this.aTimer = new System.Windows.Forms.Timer();
            // Hook up the Elapsed event for the timer. 
            this.aTimer.Interval = 4000;
            this.aTimer.Tick += new EventHandler(timer_Tick);
            this.aTimer.Start();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (this.once)
            {
                this.once = false;
                TAPManager.Instance.OnMoused += this.OnMoused;
                TAPManager.Instance.OnTapped += this.OnTapped;
                TAPManager.Instance.OnDualTapped += this.OnDualTapped;
                TAPManager.Instance.OnTapConnected += this.OnTapConnected;
                TAPManager.Instance.OnTapDisconnected += this.OnTapDisconnected;
                TAPManager.Instance.Start();
            }
            
            
        }

        private void OnDualTapped(string id1, string id2, int tapcode)
        {
            string tapCodeBinaryString = Convert.ToString(tapcode, 2).PadLeft(10, '0');

            this.LogLine(String.Format("{0} dualtapped", tapCodeBinaryString));
            this.chars++;
            //this.LogLine(String.Format("{0} tapped by ({1}, {2})", tapCodeBinaryString, id1, id2));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.lblWordsPerMinute.Text = String.Format("{0} WPM", this.chars * 60.0/5/4);
            this.chars = 0;
        }

        private void OnTapped(string identifier, int tapcode)
        {
            this.LogLine(identifier + " tapped " + tapcode.ToString());
        }

        private void OnTapConnected(string identifier, string name, int fw)
        {
            this.LogLine(identifier + " connected. (" + name + ", fw " + fw.ToString() + ")");
        }

        private void OnTapDisconnected(string identifier)
        {
            this.LogLine(identifier + " disconnected.");
        }

        private void OnMoused(string identifier, int vx, int vy, bool isMouse)
        {
            if (isMouse)
            {
                this.LogLine(identifier + " moused (" + vx.ToString() + "," + vy.ToString() + ")");
            }
            
        }

        private void LogLine(string line)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBox1.AppendText(line + Environment.NewLine);

            });
            
        }
    }
}
