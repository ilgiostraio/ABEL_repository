//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace SpeechRecognition
{
    using System;
    using System.Collections.Generic;    
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;    
    using System.Windows;    
    using System.Windows.Documents;
    using System.Windows.Media;
    using Microsoft.Kinect;    
    using Microsoft.Speech.AudioFormat;
    using Microsoft.Speech.Recognition;

    using YarpManagerCS;
    //using System.Speech.Recognition;
    //using System.Speech.AudioFormat;


    public partial class MainWindow : Window
    {
        /// <summary>
        /// Resource key for medium-gray-colored brush.
        /// </summary>
        private const string MediumGreyBrushKey = "MediumGreyBrush";

        /// <summary>
        /// Active Kinect sensor.
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Stream for 32b-16b conversion.
        /// </summary>
        private KinectAudioStream convertStream = null;

        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine speechEngine = null;

        /// <summary>
        /// List of all UI span elements used to select recognized text.
        /// </summary>
        private List<Span> recognitionSpans;

        private System.Threading.Thread senderThread = null;
        private YarpPort yarpPortScene;
      

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            var dllDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/lib";
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDirectory);

            this.InitializeComponent();

            yarpPortScene = new YarpPort();
            yarpPortScene.openSender("/SenderSRecognition");

            //senderThread = new System.Threading.Thread(SendData);
            //senderThread.Start();
        }

        

        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to
        /// process audio from Kinect device.
        /// </summary>
        /// <returns>
        /// RecognizerInfo if found, <code>null</code> otherwise.
        /// </returns>
        private static RecognizerInfo TryGetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;
            
            // This is required to catch the case when an expected recognizer is not installed.
            // By default - the x86 Speech Runtime is always expected. 
            try
            {
                recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            }
            catch (COMException)
            {
                return null;
            }

            foreach (RecognizerInfo recognizer in recognizers)
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

        /// <summary>
        /// Execute initialization tasks.
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Only one sensor is supported
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // open the sensor
                this.kinectSensor.Open();

                // grab the audio stream
                IReadOnlyList<AudioBeam> audioBeamList = this.kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // create the convert stream
                this.convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // on failure, set the status text
                //this.statusBarText.Text = Properties.Resources.NoKinectReady;
                return;
            }

            RecognizerInfo ri = TryGetKinectRecognizer();

            if (null != ri)
            {
                

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);

                // Create a grammar from grammar definition XML file.
                using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
                {
                    var g = new Grammar(memoryStream);
                    this.speechEngine.LoadGrammar(g);
                }

                this.speechEngine.SetInputToAudioStream(this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));

                //Each speech recognizer has an algorithm to distinguish between silence and speech.
                //The recognizer classifies as background noise any non-silence input that does not match the initial rule of any of the recognizer's loaded and enabled speech recognition grammars.
                //If the recognizer receives only background noise and silence within the babble timeout interval, then the recognizer finalizes that recognition operation. 
                this.speechEngine.BabbleTimeout = TimeSpan.FromSeconds(0.3);

                //The speech recognition engine uses this timeout interval when the recognition input is unambiguous. 
                //For example, for a speech recognition grammar that supports recognition of either "new game please" or "new game", "new game please" is an unambiguous input, and "new game" is an ambiguous input.
                //This property determines how long the speech recognition engine will wait for additional input before finalizing a recognition operation. The timeout interval can be from 0 seconds to 10 seconds, inclusive. 
                //The default is 150 milliseconds.
                this.speechEngine.EndSilenceTimeout = TimeSpan.FromSeconds(0.3);

                //The speech recognizer uses this timeout interval when the recognition input is ambiguous.
                //For example, for a speech recognition grammar that supports recognition of either "new game please" or "new game", "new game please" is an unambiguous input, and "new game" is an ambiguous input.
                //This property determines how long the speech recognition engine will wait for additional input before finalizing a recognition operation. The timeout interval can be from 0 seconds to 10 seconds, inclusive. 
                //The default is 500 milliseconds.
                this.speechEngine.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(0.3);

                //Each speech recognizer has an algorithm to distinguish between silence and speech. 
                //If the recognizer input is silence during the initial silence timeout period, then the recognizer finalizes that recognition operation.
                this.speechEngine.InitialSilenceTimeout = TimeSpan.FromSeconds(0.3);

                this.speechEngine.AudioLevelUpdated +=new EventHandler<AudioLevelUpdatedEventArgs>(recognizer_AudioLevelUpdated);

                this.speechEngine.AudioSignalProblemOccurred += new EventHandler<AudioSignalProblemOccurredEventArgs>(recognizer_AudioSignalProblemOccurred);

                //To get the audio state at the time of the event, use the AudioState property of the associated AudioStateChangedEventArgs. 
                //To get the current audio state of the input to the recognizer, use the recognizer's AudioState property. For more information about audio state, see the AudioState enumeration.
                this.speechEngine.AudioStateChanged +=new EventHandler<AudioStateChangedEventArgs>(recognizer_AudioStateChanged);

                //The recognizer raises this event if it determines that input does not match with sufficient confidence any of its loaded and enabled Grammar objects. 
                //The Result property of the SpeechRecognitionRejectedEventArgs contains the rejected RecognitionResult object. 
                //You can use the handler for the SpeechRecognitionRejected event to retrieve recognition Alternates that were rejected and their Confidence scores. 
                this.speechEngine.SpeechRecognitionRejected +=new EventHandler<SpeechRecognitionRejectedEventArgs>(recognizer_SpeechRecognitionRejected);

                this.speechEngine.SpeechRecognized += this.SpeechRecognized;
        
                // let the convertStream know speech is going active
                this.convertStream.SpeechActive = true;

                // For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model. 
                // This will prevent recognition accuracy from degrading over time.
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                //this.statusBarText.Text = Properties.Resources.NoSpeechRecognizer;
            }
        }

        /// <summary>
        /// Execute un-initialization tasks.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (null != this.convertStream)
            {
                this.convertStream.SpeechActive = false;
            }

            if (null != this.speechEngine)
            {
                this.speechEngine.SpeechRecognized -= this.SpeechRecognized;
                this.speechEngine.RecognizeAsyncStop();
            }

            if (null != this.kinectSensor)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handler for recognized speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.3;


            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                Console.WriteLine(e.Result.Semantics.Value.ToString() + "--> Confidence: " + e.Result.Confidence.ToString());
                wordReconiser.Text = e.Result.Semantics.Value.ToString() + "--> Confidence: " + e.Result.Confidence.ToString();

                yarpPortScene.sendData(e.Result.Semantics.Value.ToString());
                
                
            }
        }

     
        // Gather information when the AudioSignalProblemOccurred event is raised.
        void recognizer_AudioSignalProblemOccurred(object sender, AudioSignalProblemOccurredEventArgs e)
        {
            StringBuilder details = new StringBuilder();

            details.AppendLine("Audio signal problem information:");
            details.AppendFormat(
              " Audio level:               {0}" + Environment.NewLine +
              " Audio position:            {1}" + Environment.NewLine +
              " Audio signal problem:      {2}" + Environment.NewLine +
              " Recognition engine audio position: {3}" + Environment.NewLine,
              e.AudioLevel, e.AudioPosition, e.AudioSignalProblem,
              e.RecognizerAudioPosition);

            //Console.WriteLine("The problem audio : " + details.ToString());
         //   MessageBox.Show(details.ToString());
        }

        // Handle the AudioStateChanged event.
        void recognizer_AudioStateChanged(object sender, AudioStateChangedEventArgs e)
        {
            Console.WriteLine("The new audio state is: " + e.AudioState);
            statusSystem.Text = e.AudioState.ToString();
        }

        // Handle the SpeechRecognitionRejected event.
        static void recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Speech input was rejected.");
            foreach (RecognizedPhrase phrase in e.Result.Alternates)
            {
                Console.WriteLine("  Rejected phrase: " + phrase.Text);
                Console.WriteLine("  Confidence score: " + phrase.Confidence);
                Console.WriteLine("  Grammar name:  " + phrase.Grammar.Name);
            }
        }

        // Write the audio level to the console when the AudioLevelUpdated event is raised.
        void recognizer_AudioLevelUpdated(object sender,  AudioLevelUpdatedEventArgs e)
        {
            Console.WriteLine("The audio level is now: {0}.", e.AudioLevel);

            //prgLevel.Value = e.AudioLevel;
        }

        //private void SendData()
        //{
           
        //    while (true)
        //    {
        //        //se sto aggiornando i dati (SKeletonRead)non faccio la lettura e non invio nulla
        //        if (!ReadSubject)
        //            continue;
                    
        //        using ( Scene scene = new Scene())
        //        {

        //            scene.Subjects = sceneSubjects.ToList();
        //            scene.Objects = objects;
        //            scene.Environment = environment;
                    
        //            sceneData = ComUtils.XmlUtils.Serialize<Scene>(scene);
        //            //Console.WriteLine(sceneData);

        //            yarpPortScene.sendData(sceneData);
        //            sceneData = null;// se non cancello col tempo mi occupa tutta la memoria
                   
        //        }
        //    }
    }
}