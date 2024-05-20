using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer s = new SpeechSynthesizer();
        Choices list = new Choices();
        public Form1()
        {
            SpeechRecognitionEngine rec = new SpeechRecognitionEngine();

            list.Add(new String[]
            {
                "hello",
                "how are you",
                "open file",
                "nice"
            });

            Grammar gr = new Grammar(new GrammarBuilder(list));

            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gr);
                rec.SpeechRecognized += Rec_SpeechRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
            } catch { return; }

            s.SelectVoiceByHints(VoiceGender.Female);

            s.Speak("System boot up complete");

            InitializeComponent();
        }

        public void say(String h) => s.Speak(h);

        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String r = e.Result.Text;

            //if what you say is
            if (r == "hello")
            {
                //the reply
                say("Hi");
            }

            if (r == "how are you")
            {
                say("I'm good. How about you bro?");
            }

            if (r == "open file")
            {
                say("File opened");
            }

            if(r == "nice")
            {
                say("I know right?");
            }

            //throw new NotImplementedException();
        }
    }
}
