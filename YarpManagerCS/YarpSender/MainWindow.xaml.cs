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

using System.Runtime.InteropServices;


using YarpManagerCS;

namespace YarpSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private YarpPort port;
        private System.Threading.Thread senderThread;
        private string serializedScene;

        public MainWindow()
        {
            InitializeComponent();

            var dllDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "";
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDirectory);

            //serializedScene = LoadScene();

            InitYarp();            
        }

        private void InitYarp()
        {
            port = new YarpPort();
            port.openSender("/YarpSender");
        }

        private void SendData()
        {
            //string sceneData = ComUtils.XmlUtils.Serialize<Scene>(scene);            
            ///System.Diagnostics.Debug.WriteLine(sceneData);
            //port.sendData(sceneData);
            //textBox1.Dispatcher.Invoke(
            //    System.Windows.Threading.DispatcherPriority.Background,
            //    new Action(() => textBox1.Text += sceneData)
            //);

            //statusLabel.Dispatcher.Invoke(
            //    System.Windows.Threading.DispatcherPriority.Background,
            //    new Action(() => statusLabel.Content = "Sending...")
            //);

            //for (int i = 0; i < 10000; i++)
            //{
            //    //numLabel.Dispatcher.Invoke(
            //    //    System.Windows.Threading.DispatcherPriority.Background,
            //    //    new Action(() => numLabel.Content = i + 1)
            //    //);
            //    System.Diagnostics.Debug.WriteLine(serializedScene);
            //    port.sendData(serializedScene);                
            //}
            int i = 0;
            while (true)
            {
                System.Diagnostics.Debug.WriteLine(serializedScene);
                port.sendData(i.ToString());

                i++;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (senderThread == null || !senderThread.IsAlive)
            {
                senderThread = new System.Threading.Thread(SendData);
                senderThread.Start();
            }
            //SendData();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (senderThread == null || !senderThread.IsAlive)
            {
                senderThread.Abort();
                port.Close();
            }
        }

        private string LoadScene()
        {
            string content = "<<?xml version=\"1.0\" encoding=\"utf-8\"?><Scene xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Subjects><Subject><id>1</id><age>0</age><speak_prob>0.606206536</speak_prob><gesture /><uptime>0</uptime><angle>2.45</angle><happiness_ratio>0</happiness_ratio><anger_ratio>0</anger_ratio><sadness_ratio>0</sadness_ratio><surprise_ratio>0</surprise_ratio><head_xyz><float>0.21159327</float><float>0.507474</float><float>1.9168222</float></head_xyz><spincenter_xyz><float>0.0789103061</float><float>-0.0355466157</float><float>1.84327412</float></spincenter_xyz><righthand_xyz><float>0.3884985</float><float>-0.166170061</float><float>1.66761076</float></righthand_xyz><lefthand_xyz><float>-0.21978493</float><float>-0.0200313367</float><float>1.85819626</float></lefthand_xyz></Subject></Subjects><Objects><ObjectScene><id>0</id></ObjectScene></Objects><Environment><soundAngle>-19.9962273</soundAngle><soundEstimatedX>-0.363895655</soundEstimatedX><recognizedWord /></Environment></Scene>>";
            System.IO.FileStream filestream = null;
            try
            {
                //filestream = new System.IO.FileStream("MetaScene.xml", System.IO.FileMode.Open);
                //System.IO.StreamReader sr = new System.IO.StreamReader(filestream);
                //string content = sr.ReadToEnd();
                //sceneData = ComUtils.XmlUtils.Deserialize<Scene>(content);
                //filestream.Close();
            }
            catch (Exception ex)
            {
                filestream.Close();
            }

            return content;
        }

    }
}
