using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Net;


namespace BücherDB2._0_Online
{
    public partial class scs : Form
    {
        private string dowDirkay(int Länge)
        {
            string ret = string.Empty;
            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            string Content = "1234567890abcdef";
            Random rnd = new Random();
            for (int i = 0; i < Länge; i++)
                SB.Append(Content[rnd.Next(Content.Length)]);
            return SB.ToString();
        }

        public scs()
        {
            InitializeComponent();
            label2.Text = "0%";
            label3.Text = "-";
            timer1.Tick += new EventHandler(timer_Tick);
            timer1.Interval = 2000;
            timer1.Start();
        }
               public void timer_Tick(object sender, EventArgs e)
        {
            progressBar1.Value += 10;

            progressBar1.Value = Math.Max(Math.Min(progressBar1.Value, 100), 0);

            if(progressBar1.Value >= 100)
            {
                timer1.Stop();
            }
            label2.Text = progressBar1.Value + "%";

            //Test
            

            if (progressBar1.Value == 10) { label3.Text = "Programm wierd Gestartet"; }
            if (progressBar1.Value == 20) { label3.Text = "Suche nach Updats"; 
                timer1.Stop();

                //Update Sequens
               
                string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                XmlDocument UpSysXML = new XmlDocument();
                UpSysXML.Load("http://server.buch-archiv20-software.de/update_system/?v="+Version);
                XmlNode up = UpSysXML.SelectSingleNode("/update/up"); //<-- anfage ob update / Patch gemacht werden muss (y = ja , n = nein)
                XmlNode onversion = UpSysXML.SelectSingleNode("/update/onver"); //<-- Aktuelle version
                XmlNode dowVersion = UpSysXML.SelectSingleNode("/update/dow"); //<-- der aktuelle Uploade link
                
                if (up.InnerText == "n") { MessageBox.Show("Alles IO newuste version Verhanden."); } 
                if (up.InnerText == "y")
                {
                    //uploade
                    string ordnerKay = dowDirkay(10);
                    string pfad = Directory.GetCurrentDirectory();
                    string dow = dowVersion.InnerText;
                    string loc = pfad + "/uploade_patch/"+ordnerKay+"/patch.zip";

                    DirectoryInfo dowdir = Directory.CreateDirectory(pfad + "/uploade_patch/" + ordnerKay);
                
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(dow, loc);

                    MessageBox.Show("Patch Datei Herunter geladen bitte Empacken und in drüberligen Ordnerstuktor einfügen");

                    Process.Start(pfad+ "/uploade_patch");


                }                

                timer1.Start();
            }
            if (progressBar1.Value == 30) { label3.Text = "Suche verbung zum Server"; }
            if (progressBar1.Value == 40) { label3.Text = "Verbindung zum Server wird Hergestelt"; }
            if (progressBar1.Value == 50) { label3.Text = "Lade Benutzer Profilr"; }
            if (progressBar1.Value == 60) { label3.Text = "Lade Programm Profirl"; }
            if (progressBar1.Value == 70) { label3.Text = "Progamm Vorbreiten"; }
            if (progressBar1.Value == 80) { label3.Text = "Programm wierd Gestartet"; }
            if (progressBar1.Value == 90) { label3.Text = "Programm wierd Gestartet"; }
            if (progressBar1.Value == 100) { label3.Text = "Programm wierd Gestartet"; }

            
        }
    }
}
