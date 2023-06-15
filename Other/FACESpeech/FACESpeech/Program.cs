using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.ComponentModel;
using YarpManagerCS;    // requires version 1.0.0.0 of both YarpCS and YarpManagerCS



using System.Timers;
using System.Threading;


namespace FACESpeech
{
    class Program
    {
        static SoundPlayer player;
        static String fileAudio = "";
        static YarpPort yarpPortSpeech;
        static YarpPort yarpPortFeedBackSpeech;
        static YarpPort yarpPortLipAnimator;

//        static System.Timers.Timer yarpReceiverSpeech;
        static string receiveLookAtData = "";


        static void Main(string[] args)
        {
            InitializeSound();

            InitYarp();

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            while (keyInfo.Key != ConsoleKey.Escape)
                keyInfo = Console.ReadKey();
        }

        // Sets up the SoundPlayer object.
        static void InitializeSound()
        {
            // Create an instance of the SoundPlayer class.
            player = new SoundPlayer();

            // Listen for the LoadCompleted event.
            player.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);

            // Listen for the SoundLocationChanged event.
            player.SoundLocationChanged += new EventHandler(player_LocationChanged);
        }

        private static void InitYarp()
        {


            yarpPortSpeech = new YarpPort();
            yarpPortSpeech.openReceiver("/AttentionModule/Speech:o", "/FACESpeech/Speech:i");

            yarpPortFeedBackSpeech = new YarpPort();
            yarpPortFeedBackSpeech.openSender("/FACESpeech/FeedBackSpeech:o");

            yarpPortLipAnimator = new YarpPort();
            yarpPortLipAnimator.openSender("/FACESpeech/LipAnimation:o");

            ThreadPool.QueueUserWorkItem(ReceiveDataSpeech);
            //yarpReceiverSpeech = new System.Timers.Timer();
            //yarpReceiverSpeech.Interval = 200;
            //yarpReceiverSpeech.Elapsed += new ElapsedEventHandler(ReceiveDataSpeech);
            //yarpReceiverSpeech.Start();

        }

        static void ReceiveDataSpeech(object sender)
        {
            while (true)
            {
                yarpPortSpeech.receivedData(out receiveLookAtData);

                if (receiveLookAtData != null && receiveLookAtData != "")
                {
                    try
                    {
                        fileAudio = receiveLookAtData;
                        LipAnimation();
                        System.Threading.Thread.Sleep(500);
                        loadSync();
                        playOnceSync();
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Error XML Winner: " + exc.Message);
                    }

                }
            }
        }

        static void LipAnimation()
        {
            float max = 0;
            int reSample = 12;
            List<float> wave = new List<float>();

            using (var reader = new AudioFileReader("audio/"+fileAudio+".wav"))
            {
                float timeSample = 1.0f / reSample;
                wave.Add(timeSample*1000);

                var mono = new StereoToMonoSampleProvider(reader);
                mono.LeftVolume = 0.0f; // discard the left channel
                mono.RightVolume = 1.0f; // keep the right channel


                // find the max peak
                float[] buffer = new float[mono.WaveFormat.SampleRate / reSample];
                int read;
                do
                {
                    read = mono.Read(buffer, 0, buffer.Length);
                    for (int n = 0; n < read; n++)
                    {
                        var abs = Math.Abs(buffer[n]);
                        if (abs > max) max = abs;
                      

                    }
                    wave.Add(max);
                    max = 0;
                } while (read > 0);

                Console.WriteLine("Durata file {0}", Convert.ToDecimal(reader.Length) / Convert.ToDecimal(reader.WaveFormat.AverageBytesPerSecond));
                Console.WriteLine("Durata wave {0}", Convert.ToDecimal(wave.Count-1 * timeSample));


                yarpPortLipAnimator.sendData(wave);

            }
        }

        #region player
        static void player_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string message = String.Format("LoadCompleted: {0}", fileAudio);
            ReportStatus(message);
        }

        // Handler for the SoundLocationChanged event.
        static void player_LocationChanged(object sender, EventArgs e)
        {
            string message = String.Format("SoundLocationChanged: {0}", player.SoundLocation);
            ReportStatus(message);
        }
        static void loadSync()
        {
          
            try
            {
                // Assign the selected file's path to 
                // the SoundPlayer object.  
                player.SoundLocation = "audio/" + fileAudio + ".wav";

                // Load the .wav file.
                player.Load();
            }
            catch (Exception ex)
            {
                ReportStatus(ex.Message);
            }
        }

        static void loadAsync()
        {
           

            try
            {
                // Assign the selected file's path to 
                // the SoundPlayer object.  
                player.SoundLocation = "audio/" + fileAudio + ".wav";

                // Load the .wav file.
                player.LoadAsync();
            }
            catch (Exception ex)
            {
                ReportStatus(ex.Message);
            }
        }

        // Synchronously plays the selected .wav file once.
        // If the file is large, UI response will be visibly 
        // affected.
        static void playOnceSync()
        {
            ReportStatus("Playing .wav file synchronously.");

            yarpPortFeedBackSpeech.sendData(ComUtils.XmlUtils.Serialize<string>("speak start"));
            player.PlaySync();
            yarpPortFeedBackSpeech.sendData(ComUtils.XmlUtils.Serialize<string>("speak end"));

            ReportStatus("Finished playing .wav file synchronously.");
        }

        // Asynchronously plays the selected .wav file once.
        static void playOnceAsync()
        {
            ReportStatus("Playing .wav file asynchronously.");
            player.Play();
        }



        // Stops the currently playing .wav file, if any.
        static void stopButton()
        {
            player.Stop();
            ReportStatus("Stopped by user.");
        }
        #endregion

        static void ReportStatus(string statusMessage)
        {
            // If the caller passed in a message...
            if (!string.IsNullOrEmpty(statusMessage))
            {
                // ...post the caller's message to the status bar.
                Console.WriteLine(statusMessage);
            }
        }
    }
}
