using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;
using System.Runtime.InteropServices;

using MetaScene;

namespace YarpReceiver 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private YarpPort port;
        private Scene sceneData;

        private EventWaitHandle _wh = new AutoResetEvent(false);
        private Thread _worker;
        private int counter = 0;
        string received; 

        public MainWindow()
        {
            InitializeComponent();
            
            //System.Timers.Timer t = new System.Timers.Timer();
            //t.Interval = 10000;//10sec
            //t.Elapsed += delegate { InitYarp(); };
            //t.Enabled = true;
            InitYarp();
        }

        private void InitYarp()
        {
            port = new YarpPort();
            port.initReceiver("/YarpSender", "/YarpReceiver"); 
            port.DataReceived += new YarpDataReceivedEvent(DataAvailable);
        }

        private void DataAvailable(YarpPort sender)
        {
            received = port.getData();  
        }


        //private void ReceiveData()
        //{
        //    //string received = port.receiveData();
        //    string received = port.getData();
        //    if (received != null && received != "")
        //    {
        //        sceneData = ComUtils.XmlUtils.Deserialize<Scene>(received);
        //        textBox1.Dispatcher.Invoke(
        //            System.Windows.Threading.DispatcherPriority.Background,
        //            new Action(() => textBox1.Text = received)
        //        );
        //        //System.Diagnostics.Debug.WriteLine("\n YarpReceiver: " + received + "\n");
        //    }
        //    else
        //    {
        //        textBox1.Dispatcher.Invoke(
        //            System.Windows.Threading.DispatcherPriority.Background,
        //            new Action(() => textBox1.Text += "No data received!/n")
        //        );
        //    }
        //}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //ReceiveData();
            _worker = new Thread(Work);
            _worker.Start();
        }


        private void Work()
        {
            while (true)
            {
                if (received != null && received != "")
                {
                    string s = received.Substring(0, received.Length - 2);
                    if(!s.EndsWith(">"))
                        s += ">";
                    s += "\"";

                    counter++;
                    sceneData = ComUtils.XmlUtils.Deserialize<Scene>(s);
                    textBox1.Dispatcher.Invoke(
                        System.Windows.Threading.DispatcherPriority.Background,
                        new Action(() => textBox1.Text = "[" + counter + "] " + s)
                    );
                }
                else
                {
                    // No more tasks - wait for a signal
                    _wh.WaitOne();
                }
            }
        }

        public void EnqueueTask (string task)
        {
            _wh.Set();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Background,
                new Action(() => textBox1.Text = "")
            );
        }
    }
}
