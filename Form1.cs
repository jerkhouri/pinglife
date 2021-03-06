using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pinglife
{
    public partial class Form1 : Form
    {
        string url = "8.8.8.8";
        Boolean error = false;
        Boolean highping = false;
        int timeoff = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("start");
            timer1.Start();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            Visible = false; // Hide form window.
            /*
            BeginInvoke(new MethodInvoker(delegate
            {
                Hide();
            }));
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartPing();
        }

        private void StartPing()
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            try
            {        
                if ( error == false) {

                    PingReply reply = pingSender.Send(url, timeout, buffer, options);

                    if (reply.Status == IPStatus.Success)
                    {                        
                        //Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                        if (reply.RoundtripTime <= 50)
                        {
                            ShowText(Convert.ToString(reply.RoundtripTime), new Font("Arial", 10), Color.Green);
                        }
                        else if (reply.RoundtripTime >= 50 && reply.RoundtripTime < 100)
                        {
                            ShowText(Convert.ToString(reply.RoundtripTime), new Font("Arial", 10), Color.Orange);
                        }
                        else if (reply.RoundtripTime >= 100)
                        {
                            if (highping == false)
                            {
                                ShowText("99", new Font("Arial", 10), Color.Red);
                                highping = true;
                            }
                            else
                            {
                                ShowText("!!!", new Font("Arial", 10), Color.Red);
                                highping = false;
                            }
                            
                        }

                        notifyIcon1.Text = "Ping : " + Convert.ToString(reply.RoundtripTime)+ " to \""+url+"\"";
                    }
                    else
                    {
                        ShowText("X", new Font("Arial", 10), Color.Red);
                        notifyIcon1.Text = "Unable to reach \"" + url + "\"";
                    }
                }
                else
                {
                    if(timeoff == 5)
                    {
                        timeoff = 0;
                        error = false;
                    }
                    else
                    {
                        timeoff += 1;
                    }                   
                }

            }
            catch
            {
                error = true;
                ShowText("X", new Font("Arial", 10), Color.Red);
                notifyIcon1.Text = "Host \""+ url + "\" unknow..";
            }
            
        }

        public void ShowText(string text, Font font, Color col)
        {
            Brush brush = new SolidBrush(col);

            // Create a bitmap and draw text on it
            Bitmap bitmap = new Bitmap(16, 16);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawString(text, font, brush, 0, 0);

            // Convert the bitmap with text to an Icon
            Icon icon = Icon.FromHandle(bitmap.GetHicon());

            notifyIcon1.Icon = icon;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            Visible = true; // Hide form window.
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/jerkhouri");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            Visible = false; // Hide form window.
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (tbx_url.Text != "") { 
            url = tbx_url.Text;
            }
            error = false;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            Visible = false; // Hide form window.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            url = "8.8.8.8";
            tbx_url.Text = "";
            error = false;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            Visible = false; // Hide form window.
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



        bool flag = false;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            flag = true;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //Check if Flag is True ??? if so then make form position same
            //as Cursor position
            if (flag == true)
            {
                this.Location = Cursor.Position;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            flag = false;
        }

    }
}
