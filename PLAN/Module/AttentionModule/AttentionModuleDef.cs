using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CLIPSNet;
using InteractiveCLIPS;

using System.Timers;
using System.Threading;
using System.Runtime.InteropServices;

using Sense.Lib.FACELibrary;
using YarpManagerCS;

using System.Threading.Tasks;
using System.Drawing;

namespace AttentionModule
{
    [ModuleDefinition(ClpFileName = "init.clp")]
    public class AttentionModuleDef : Module, IDisposable
    {
        private YarpPort lookAtEyesPort;  //porta per orientare gli occhi rispetto al piano della testa (si aggancia a NetView.cs di ACT_FACE)      ALTILIA

        private YarpPort lookAtPort;  //porta per orientare la testa rispetto al piano della kinect (si aggancia a ROS tramite yarpROSbridge)
        private YarpPort expressionPort;
        private YarpPort SceneReceiverPort;
        private YarpPort speechPort;
        private YarpPort feedBackSpeechPort;
        private YarpPort moodPort;
        private YarpPort posturePort;

        private Scene sceneData;
        private string received ="";
        private string receivedFeedBack = "";

        private Winner WinnerSub;
        private Winner WinnerSubOld;

        private FaceExpression exp;
        private FaceExpression expOld;


        private Thread _worker;

        public static CLIPSNet.UserFunction uf_lookat;
        public static CLIPSNet.UserFunction uf_makeexp;
        public static CLIPSNet.UserFunction uf_speech;
        public static CLIPSNet.UserFunction uf_mood;
        public static CLIPSNet.UserFunction uf_posture;

        delegate CLIPSNet.DataTypes.Integer lookAtDelegate(CLIPSNet.DataTypes.Integer id, CLIPSNet.DataTypes.Double x, CLIPSNet.DataTypes.Double y, CLIPSNet.DataTypes.Double z);        //aggiunta anche zCoord       ALTILIA
        delegate CLIPSNet.DataTypes.Integer makeExpressionDelegate(CLIPSNet.DataTypes.Double v, CLIPSNet.DataTypes.Double a);
        delegate CLIPSNet.DataTypes.Integer speechDelegate(CLIPSNet.DataTypes.Integer id);
        delegate CLIPSNet.DataTypes.Integer moodDelegate(CLIPSNet.DataTypes.Double v, CLIPSNet.DataTypes.Double a);
        delegate CLIPSNet.DataTypes.Integer postureDelegate(CLIPSNet.DataTypes.String posture, CLIPSNet.DataTypes.Integer duration);



        private System.Timers.Timer checkPortTimer = new System.Timers.Timer();
        private bool sceneAnalyzerPortExists = false;
        private bool sceneAnalyzerConnectionExists = false;

        string WinnerXml;
        string expressionXml;

        float Xmax = 0;
        float X = 0;

        float Ymax = 0;
        float Y = 0;

        float Z = 0;              //variabile appoggio Z        ALTILIA

        bool close = false;

        protected override void Init()
        {
            uf_lookat = new CLIPSNet.UserFunction(ClipsEnv, new lookAtDelegate(FunLookAt), "fun_lookat");
            uf_makeexp = new CLIPSNet.UserFunction(ClipsEnv, new makeExpressionDelegate(FunMakeExp), "fun_makeexp");
            uf_speech = new CLIPSNet.UserFunction(ClipsEnv, new speechDelegate(FunSpeech), "fun_speech");
            uf_mood= new CLIPSNet.UserFunction(ClipsEnv, new moodDelegate(MoodSpeech), "fun_mood");
            uf_posture = new CLIPSNet.UserFunction(ClipsEnv, new postureDelegate(FunPosture), "fun_posture");



            InitYarp();   
                
            

            ThreadPool.QueueUserWorkItem(receiveDataTimer_Elapsed);
            ThreadPool.QueueUserWorkItem(receiveFeedBackSpeech_Elapsed);


            WinnerSub = new Winner();
            exp = new FaceExpression();

        }

        private void InitYarp()
        {

            lookAtEyesPort = new YarpPort();
            lookAtEyesPort.openSender("/AttentionModule/LookAtEyes:o");


            lookAtPort = new YarpPort();
            lookAtPort.openSender("/AttentionModule/LookAt:o");

            speechPort = new YarpPort();
            speechPort.openSender("/AttentionModule/Speech:o");

            expressionPort = new YarpPort();
            expressionPort.openSender("/AttentionModule/ECS:o");

            moodPort = new YarpPort();
            moodPort.openSender("/AttentionModule/mood:o");

            posturePort = new YarpPort();
            posturePort.openSender("/AttentionModule/Posture:o");

            feedBackSpeechPort = new YarpPort();
            feedBackSpeechPort.openReceiver("/FACESpeech/FeedBackSpeech:o", "/InteractiveCLIPS/FeedBackSpeech:i");


            SceneReceiverPort = new YarpPort();
            SceneReceiverPort.openReceiver("/SceneAnalyzer/MetaSceneXML:o", "/InteractiveCLIPS/MetaSceneXML:i");


            if (SceneReceiverPort.PortExists("/SceneAnalyzer/MetaSceneXML:o"))
            {
                ClipsEnv.PrintRouter("RouterTest", "Connection created!");
                System.Diagnostics.Debug.WriteLine("");
                sceneAnalyzerConnectionExists = true;
                

             
               

            }
            else 
            {

                ClipsEnv.PrintRouter("RouterTest", "Connection NOT created! /SceneAnalyzer/MetaSceneXML:o port does not exist!");

            }

            
         

        }

        //DOBBIAMO FARE IN MODO CHE ANCHE QUESTO MODULO UNA VOLTA CHIUSO CHIAMI LA DISCONNECT SULLA PORTA YARP UTILIZZATA

        [ClipsAction("fun_lookat")]
        public CLIPSNet.DataTypes.Integer FunLookAt(CLIPSNet.DataTypes.Integer id, CLIPSNet.DataTypes.Double xCoord, CLIPSNet.DataTypes.Double yCoord, CLIPSNet.DataTypes.Double zCoord)      //aggiunta anche zCoord       ALTILIA
        {
            //System.Diagnostics.Debug.WriteLine("WINNER: " + (int)id.Value + " x: " + ((float)xCoord.Value) + " - y: " + ((float)yCoord.Value));
             
                        
            if(id.Value!=1)
            {
                foreach (Subject subject in sceneData.Subjects) //calibration of the LookAt point
                {
                    if(id.Value==subject.idKinect)
                    {
                        //The field of view for the color camera is 84.1 degrees horizontally and 53.8 degrees vertically.
                        //For the depth camera it's 70.6 degrees horizontally and 60 degrees vertically.

                        Xmax = subject.head.Z * (float)Math.Tan(42.05 / 180.00 * Math.PI); //alzando l'angolo della tangente aumento l'eccentricità (42.05°)
                        X = subject.head.X / Xmax / 2 + (float)0.5; //aggiustamento per rappresentazione grafica

                        Ymax = subject.head.Z * (float)Math.Tan(26.9 / 180.00 * Math.PI); //alzando l'angolo della tangente aumento l'eccentricità (26.9°)
                        Y = subject.head.Y / Ymax / 2 + (float)0.5; //aggiustamento per rappresentazione grafica
                        //Y_round = Math.Round(Y, 2);

                        Z = subject.head.Z; // coord Z per convergenza oculare          ALTILIA


                        
                        WinnerSub.id = (int)id.Value;
                        WinnerSub.spinX = X;
                        WinnerSub.spinY = Y;
                        WinnerSub.spinZ = Z;     //   aggiunta Z ad xml punto           ALTILIA
                    }
                }

                
            }         
            else
            {
                WinnerSub.id = (int)id.Value;
                WinnerSub.spinX = (float)xCoord.Value;
                WinnerSub.spinY = (float)yCoord.Value;
                WinnerSub.spinZ = (float)zCoord.Value;     //   aggiunta Z ad xml punto           ALTILIA
            }

            if (WinnerSubOld != null)
            {
                if (!(WinnerSub.spinX == WinnerSubOld.spinX && WinnerSub.spinY == WinnerSubOld.spinY && WinnerSub.spinZ == WinnerSubOld.spinZ))      // aggiunta stessa condizione anche su Z      ALTILIA
                {
                    WinnerXml = ComUtils.XmlUtils.Serialize<Winner>(WinnerSub);
                    lookAtPort.sendData(WinnerXml);
                    lookAtEyesPort.sendData(WinnerXml);
                    WinnerSubOld = (Winner)WinnerSub.Clone();
                    WinnerXml = "";
                }
            }
            else
            {
                WinnerSubOld = new Winner();
                WinnerSubOld = (Winner)WinnerSub.Clone();
            }
           
          

            return id;
        }

        int n = 0;
        [ClipsAction("fun_makeexp")]
        public CLIPSNet.DataTypes.Integer FunMakeExp(CLIPSNet.DataTypes.Double v, CLIPSNet.DataTypes.Double a)
        {
            //System.Diagnostics.Debug.WriteLine("ECS -> Valence = " + v.ToString() + " - Arousal = " + a.ToString());

            if (!close)
                if (expOld != null)
                {
                    if (!((float)v.Value == expOld.valence && a.Value == expOld.arousal))
                    {
                        exp.valence = (float)v.Value;
                        exp.arousal = (float)a.Value;

                        expressionXml = ComUtils.XmlUtils.Serialize<FaceExpression>(exp);
                        expressionPort.sendData(expressionXml);
                        expOld = (FaceExpression)exp.Clone();
                        expressionXml = "";
                    }
                }
                else
                {
                    expOld = new FaceExpression();
                    expOld = (FaceExpression)exp.Clone();
                }

            

            return new CLIPSNet.DataTypes.Integer(0);
        }

        [ClipsAction("fun_speech")]
        public CLIPSNet.DataTypes.Integer FunSpeech(CLIPSNet.DataTypes.Integer id)
        {
            if (!close)
                speechPort.sendData(id.ToString());

            return id;
        }

        [ClipsAction("fun_mood")]
        public CLIPSNet.DataTypes.Integer MoodSpeech(CLIPSNet.DataTypes.Double v, CLIPSNet.DataTypes.Double a)
        {
            if(!close)
                moodPort.sendData(new List<float>() { (float)v.Value, (float)a.Value });

            return new CLIPSNet.DataTypes.Integer(0);
        }

        [ClipsAction("fun_posture")]
        public CLIPSNet.DataTypes.Integer FunPosture(CLIPSNet.DataTypes.String posture, CLIPSNet.DataTypes.Integer duration)
        {
            if (!close)
                posturePort.sendData(Convert.ToString(posture));   //modificato perchè prima c'era moodPort

            return new CLIPSNet.DataTypes.Integer(1000);
        }


        #region Yarp


        /* Asserts managed by a timer */
        void receiveDataTimer_Elapsed(object sender)
        {
            while (!close)
            {
                
                SceneReceiverPort.receivedData(out received);
                if (received != null && received != "")
                {

                    sceneData = ComUtils.XmlUtils.Deserialize<Scene>(received);

                    Parallel.ForEach(sceneData.Subjects, (subject) =>
                    {
                        StringBuilder sObj = convertedObject(typeof(Subject), subject, (subject.idKinect).ToString());
                        AssertTemplate(sObj.ToString(), (subject.idKinect).ToString());
                        //AssertTemplate(subject.ToStringClips(), (subject.idKinect).ToString());

                    });



                    //StringBuilder s = convertedObject(typeof(Surroundings), sceneData.Environment, "0");
                    //System.Diagnostics.Debug.WriteLine(s.ToString());
                    AssertTemplate(convertedObject(typeof(Surroundings), sceneData.Environment, "0").ToString(), "0");
                    //AssertTemplate(typeof(Surroundings), sceneData.Environment, "0");
                    //AssertFact("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    //sceneData = null;


                }
            }
        }

        void receiveFeedBackSpeech_Elapsed(object sender)
        {
            while (!close)
            {
                feedBackSpeechPort.receivedData(out receivedFeedBack);
                if (receivedFeedBack != null && receivedFeedBack != "")
                {

                    AssertFact("speak", receivedFeedBack.Replace("\"", ""));

                }
            }
        }
        #endregion


        protected StringBuilder convertedObject(Type t, object obj, string search_key)
        {
            var convertedObject = Convert.ChangeType(obj, t);
            StringBuilder s = new StringBuilder();

            s.Append("(" + t.Name.ToString().ToLower() + " \n");

            if (t.Name == "Subject")
            {
                #region sub
                foreach (System.Reflection.PropertyInfo prop in t.GetProperties())
                {
                    object val = typeof(Subject).GetProperty(prop.Name).GetValue(convertedObject, null);
                    if (val != null)
                    {
                        StringBuilder sbGeneric = new StringBuilder();
                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {

                            System.Collections.IList l = (System.Collections.IList)val;
                            sbGeneric.AppendFormat("({0}", prop.Name);
                            foreach (object elem in l)
                            {
                                if (elem.ToString() != null)
                                    sbGeneric.AppendFormat(" {0}", elem.ToString());
                            }
                            sbGeneric.AppendFormat(")\n");

                        }
                        else if (prop.PropertyType.IsClass && prop.PropertyType.Name == "Limb")
                        {


                            Limb li = (Limb)val;

                            sbGeneric.AppendFormat(" ({0}Left", prop.Name);
                            sbGeneric.AppendFormat(" ");
                            sbGeneric.AppendFormat(" {0}", li.left.X.ToString("F3"));
                            sbGeneric.AppendFormat(" {0}", li.left.Y.ToString("F3"));
                            sbGeneric.AppendFormat(" {0}", li.left.Z.ToString("F3"));
                            sbGeneric.AppendFormat(")\n");

                            sbGeneric.AppendFormat(" ({0}Right", prop.Name);
                            sbGeneric.AppendFormat(" {0}", li.right.X.ToString("F3"));
                            sbGeneric.AppendFormat(" {0}", li.right.Y.ToString("F3"));
                            sbGeneric.AppendFormat(" {0}", li.right.Z.ToString("F3"));
                            sbGeneric.AppendFormat(")\n");

                        }
                        else if (prop.PropertyType.IsClass && prop.PropertyType.Name == "Position")
                        {
                            Position pos = (Position)val;
                            sbGeneric.AppendFormat("({0}", prop.Name);

                            sbGeneric.AppendFormat(" {0}", pos.X.ToString("F3"));
                            sbGeneric.AppendFormat(" {0}", pos.Y.ToString("F3"));
                            sbGeneric.AppendFormat(" {0}", pos.Z.ToString("F3"));
                            sbGeneric.AppendFormat(")\n");
                        }
                        else
                        {
                            sbGeneric.AppendFormat("({0} {1})\n", prop.Name, val.ToString());
                        }
                        s.Append(sbGeneric.ToString());
                    }

                }
                #endregion
            }
            else if (t.Name == "Surroundings")
            {
                #region surroundings
                foreach (System.Reflection.PropertyInfo prop in t.GetProperties())
                {
                    object val = typeof(Surroundings).GetProperty(prop.Name).GetValue(convertedObject, null);
                    if (val != null)
                    {
                        StringBuilder sbGen = new StringBuilder();
                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            System.Collections.IList l = (System.Collections.IList)val;
                            sbGen.AppendFormat(" ({0} ", prop.Name);
                            foreach (object elem in l)
                            {
                                if (elem.ToString() != null)
                                {
                                    if (elem.ToString().Length > 4)
                                        sbGen.AppendFormat(" {0}", elem.ToString().Substring(0, 4));
                                    else
                                        sbGen.AppendFormat(" {0}", elem.ToString());
                                }
                            }
                            sbGen.AppendFormat(")\n");
                        }
                        else if (prop.PropertyType.IsClass && prop.PropertyType.Name == "SOME")
                        {
                            SOME some = (SOME)val;

                            sbGen.AppendFormat(" ({0}Mic", prop.Name);
                            sbGen.AppendFormat(" {0})\n", some.mic.ToString());
                            sbGen.AppendFormat(" ({0}Lux", prop.Name);
                            sbGen.AppendFormat(" {0})\n", some.lux.ToString());
                            sbGen.AppendFormat(" ({0}Temp", prop.Name);
                            sbGen.AppendFormat(" {0})\n", some.temp.ToString());
                            sbGen.AppendFormat(" ({0}IR", prop.Name);
                            sbGen.AppendFormat(" {0})\n", some.IR.ToString());
                            sbGen.AppendFormat(" ({0}Touch", prop.Name);
                            sbGen.AppendFormat(" {0})\n", some.touch.ToString());


                        }
                        else if (prop.PropertyType.IsClass && prop.PropertyType.Name == "Saliency")
                        {
                            Saliency sal = (Saliency)val;
                            sbGen.AppendFormat(" ({0}", prop.Name);
                            if (sal.position.Count != 0)
                            {
                                sbGen.AppendFormat(" {0}", sal.position[0].ToString("F2"));
                                sbGen.AppendFormat(" {0}", sal.position[1].ToString("F2"));
                                sbGen.AppendFormat(" {0}", sal.saliencyWeight.ToString("F1"));
                            }
                            sbGen.AppendFormat(")\n ");
                        }
                        else if (prop.PropertyType.IsClass && prop.PropertyType.Name == "Ambience")
                        {
                            sbGen.AppendFormat(" ({0}", prop.Name);
                            sbGen.AppendFormat(")\n");
                        }
                        else if (prop.PropertyType.IsClass && prop.PropertyType.Name == "Resolution")
                        {
                            Resolution res = (Resolution)val;
                            sbGen.AppendFormat(" ({0}", prop.Name);
                            sbGen.AppendFormat(" {0}", res.Width.ToString("F1"));
                            sbGen.AppendFormat(" {0}", res.Height.ToString("F1"));
                            sbGen.AppendFormat(")\n ");
                        }
                        else
                        {
                            sbGen.AppendFormat(" ({0} {1})\n", prop.Name, val.ToString());
                        }
                        s.Append(sbGen.ToString());


                    }
                }
                #endregion
            }
            else
            {
                s.AppendFormat("({0} (", t.Name);
                foreach (var f in t.GetFields())
                {
                    if (f.FieldType.IsSubclassOf(typeof(CLIPSNet.DataType)))
                    {
                        var typename = "";
                        if (f.FieldType == typeof(CLIPSNet.DataTypes.String)) typename = "STRING";
                        else if (f.FieldType == typeof(CLIPSNet.DataTypes.Integer)) typename = "INTEGER";
                        else if (f.FieldType == typeof(CLIPSNet.DataTypes.Symbol)) typename = "SYMBOL";
                        s.AppendFormat(" (slot {0} (type {1}))", f.Name, typename);
                    }
                }
                s.AppendFormat("))");
            }


            s.AppendFormat(")");
            //System.Diagnostics.Debug.WriteLine(s);
            return s;

        }

        public override void YarpClose() 
        {
            close = true;

            if (lookAtEyesPort != null)
                lookAtEyesPort.Close();


            if (lookAtPort != null)
                lookAtPort.Close();
            if (expressionPort != null)
                expressionPort.Close();
            if (SceneReceiverPort != null)
                SceneReceiverPort.Close();
            if (speechPort != null)
                speechPort.Close();
            if (feedBackSpeechPort != null)
                feedBackSpeechPort.Close();

            if (moodPort != null)
                moodPort.Close();
            

        }

        


    }
}




/*
 //(assert-string "(primary color is red)")
        //private void assertTemplate(Type t)
        private List<string> assertTemplate(Scene t)
        {
            List<string> assertSubjects = new List<string>();
            for (int i = 0; i < t.Subjects.Count; i++)
            {
                Subject currSubject = t.Subjects[i];
                StringBuilder s = new StringBuilder();
                s.Append("(subj ");
                foreach (System.Reflection.PropertyInfo prop in typeof(Subject).GetProperties())
                {
                    //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(t, null));
                    //if (prop.GetType() == typeof(String))
                    //    s.AppendFormat("({0} \"{1}\")", prop.Name, currSubject.GetType().GetProperty(prop.Name).GetValue(currSubject, null));
                    //else
                    object val = currSubject.GetType().GetProperty(prop.Name).GetValue(currSubject, null);
                    if ( val.ToString() == "")                        
                        s.AppendFormat("({0} {1})", prop.Name, "-");
                    else
                        s.AppendFormat("({0} {1})", prop.Name, val);
                }
                s.AppendFormat(")");
                assertSubjects.Add(s.ToString());
            }
            return assertSubjects;
        }
 */


//if (xCoord is CLIPSNet.DataTypes.GenericDataType<float> && yCoord is CLIPSNet.DataTypes.GenericDataType<float>)
//{
//    var x = (CLIPSNet.DataTypes.GenericDataType<float>)xCoord;
//    var y = (CLIPSNet.DataTypes.GenericDataType<float>)yCoord;
//    try
//    {
//        System.Diagnostics.Debug.WriteLine("x: " + x.ToString() + " - y: " + y.ToString()); 
//    }
//    catch (Exception e) { Console.WriteLine(e.ToString()); }
//}
//else 
//{ 
//    System.Diagnostics.Debug.WriteLine("x: " + xCoord.ToString() + " - y: " + yCoord.ToString()); 
//}