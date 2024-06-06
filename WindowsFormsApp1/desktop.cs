using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace WindowsFormsApp1
{
    public partial class desktop : Form
    {
        bool sidebarExpand;
        notepad form2 = new notepad();
        SpeechSynthesizer s = new SpeechSynthesizer();
        Choices list = new Choices();
        bool form2Show = false;
        public desktop()
        {

            SpeechRecognitionEngine rec = new SpeechRecognitionEngine();

            list.Add(new String[]
            {
                "hello",
                "how are you",
                "open file please",
                "nice",
                "add new file please",
                "save file please",
                "save new file please",
                "close note pad please",
                "open note pad please",
                "shutdown"
            });

            Grammar gr = new Grammar(new GrammarBuilder(list));
            //Grammar gr = new DictationGrammar();

            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gr);
                rec.SpeechRecognized += Rec_SpeechRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch { return; }

            s.SelectVoiceByHints(VoiceGender.Female);

            s.Speak("System boot up complete");
            

            InitializeComponent();
            s.Speak("Welcome to HoneyOs");
        }

        public void say(String h) => s.Speak(h);

        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String r = e.Result.Text;
            Console.WriteLine(r);

            //if what you say is
            if (r == "hello")
            {
                say("Hi");
            }
            else if (r == "how are you")
            {
                say("I'm good. How about you bro?");
            }
            else if (r == "open file please")
            {
                say("Opening file dialog");
                form2.button2_Click(null, null); // Call the method to open a file dialog
            }
            else if (r == "nice")
            {
                say("I know right?");
            }
            else if (r == "add new file please" && form2Show)
            {
                say("Opening a new tab");
                form2.button1_Click(null, null); // Call the method to add a new tab
            }
            else if (r == "save file please" && form2Show)
            {
                say("Saving File");
                form2.button3_Click(null, null); // Call the method to add a new tab
            }
            else if (r == "save new file please" && form2Show)
            {
                say("Saving New File");
                form2.button4_Click(null, null); // Call the method to add a new tab
            }
            else if(r == "open note pad please" && !form2Show)
            {
                say("Opening Notepad");
                form2Show = true;
                form2.Show(); 
            }
            else if (r == "close note pad please" && form2Show)
            {
                say("Closing Notepad");
                form2Show = false;
                form2.Hide();
            }
            else if (r == "shutdown" && !form2Show)
            {
                say("Shutting Down HoneyOs");
                this.Close();
                Application.Exit();
            }
            else
            {
                say("Command not recognized");
            }

            //throw new NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void sidebar(object sender, EventArgs e)
        {

        }

        private void sidebarTimer_Tick(object sender, EventArgs e)
        {
            if(sidebarExpand)
            {
                sidebarContainer.Width -= 10;
                if(sidebarContainer.Width == sidebarContainer.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebarContainer.Width += 10;
                if(sidebarContainer.Width == sidebarContainer.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sidebar(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            // Create an instance of Form2


            // Show Form2
            if (!form2Show)
            {
                form2.Show();
            }
        }

            private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
