using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge.Video;
using AForge.Video.DirectShow;

namespace nForgeTool
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoCaptureDevice;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in _videoDevices)
            {
                comboBox1.Items.Add(device.Name);
            }

            _videoCaptureDevice = new VideoCaptureDevice();


        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            if (_videoCaptureDevice.IsRunning)
            {
                _videoCaptureDevice.Stop();
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
            }
            else
            {
                _videoCaptureDevice = new VideoCaptureDevice(_videoDevices[comboBox1.SelectedIndex].MonikerString);
                // set NewFrame event handler
                _videoCaptureDevice.NewFrame += _videoCaptureDevice_NewFrame;
                _videoCaptureDevice.Start();
            }
        }

        private void _videoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_videoCaptureDevice.IsRunning)
                _videoCaptureDevice.Stop();
        }
    }
}
