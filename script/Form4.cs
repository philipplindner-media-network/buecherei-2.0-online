using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Drawing.Printing;
using System.IO;
using System.Diagnostics;

namespace BücherDB2._0_Online
{
    public partial class Form4 : Form
    {
       

        public Form4(string DBID, string MYhost, string MYuser, string MYpass, string MYdbname)
        {
            
            InitializeComponent();

            // Datenbank Verbindungs Aufbau
            string connStr = @"SERVER=" + MYhost + ";" +
                 "DATABASE=" + MYdbname + ";" +
                 "UID=" + MYuser + ";" +
                 "PWD=" + MYpass + ";";

            MySqlConnection connSYS = new MySqlConnection(connStr);
            MySqlCommand com = connSYS.CreateCommand();

            com.CommandText = "SELECT * FROM `media` WHERE `dbid` LIKE \'" + DBID + "%\'";

            MySqlDataReader Reader;
            connSYS.Open();
            Reader = com.ExecuteReader();
            while (Reader.Read())
            {
                string m01 = "";
                string m02 = "";
                string m03 = "";
                string m04 = "";
                string m05 = "";
                string m06 = "";
                string m07 = "";
                string m08 = "";
                string m09 = "";
                string m10 = "";
                string m11 = "";
                string m12 = "";
                string m13 = "";
                string m14 = "";

                m01 += Reader.GetValue(0).ToString();   //<<DB ID
                m02 += Reader.GetValue(1).ToString();   //<<Inhaber ID
                m03 += Reader.GetValue(2).ToString();   //<<Inhaber
                m04 += Reader.GetValue(3).ToString();   //<<Name
                m05 += Reader.GetValue(4).ToString();   //<<Band
                m06 += Reader.GetValue(5).ToString();   //<<Doppel Bannd
                m07 += Reader.GetValue(6).ToString();   //<<ISBN 10
                m08 += Reader.GetValue(7).ToString();   //<<ISBN 13
                m09 += Reader.GetValue(8).ToString();   //<<Preis
                m10 += Reader.GetValue(9).ToString();   //<<Typ
                m11 += Reader.GetValue(10).ToString();  //<<Verlag
                m12 += Reader.GetValue(11).ToString();  //<<BildURL
                m13 += Reader.GetValue(12).ToString();  //<<Standort
                m14 += Reader.GetValue(13).ToString();  //<<Zusand

                label2.Text = m01;
                label3.Text = m02;
                label5.Text = m03;
                label8.Text = m04;
                label10.Text = m05 + " (Doppelband?: " + m06 + ")";
                label12.Text = m07;
                label14.Text = m08;
                label16.Text = m09;
                label18.Text = m10;
                label20.Text = m11;
                //Abfrage bei Keine Bilder --> Shere Untenb
                label22.Text = m13;
                label23.Text = m14;

                if (m12 == "") { pictureBox1.Load("https://arisatheotaku.files.wordpress.com/2012/04/lesen.jpg"); }
                else { pictureBox1.Load(m12); }

            }
            connSYS.Close();

            string connStr2 = @"SERVER=" + MYhost + ";" +
                 "DATABASE=" + MYdbname + ";" +
                 "UID=" + MYuser + ";" +
                 "PWD=" + MYpass + ";";

            MySqlConnection connSYS2 = new MySqlConnection(connStr2);
            MySqlCommand com2 = connSYS2.CreateCommand();

            com2.CommandText = "SELECT `username`,`email` FROM `user` WHERE `username` LIKE \'" + label5.Text + "%\'";

            MySqlDataReader R1;
            connSYS2.Open();
            R1 = com2.ExecuteReader();
            while (R1.Read())
            {
                string u1 = "";
                string u2 = "";

                u1 += R1.GetValue(0).ToString();
                u2 += R1.GetValue(1).ToString();

                label26.Text = u2;
            }
            

            //zusand
            if (label23.Text == "Seher Gut") { progressBar1.Value = 100; }
            if (label23.Text == "Gut") { progressBar1.Value = 75; }
            if (label23.Text == "Mittel") { progressBar1.Value = 50; }
            if (label23.Text == "Schelcht") { progressBar1.Value = 25; }
            if (label23.Text == "Seher Schlecht") { progressBar1.Value = 0; }

            //PlugIn Tool
            if (label18.Text == "Anime") { linkLabel1.Text = "weiter infos zur Anime"; } else { linkLabel1.Text = ""; }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //User mit eine E-Mail Informiren
            string testinhalt = "Hallo \n Ich möchte gere Das Medium "+ label8.Text + " Band "+ label10.Text + " (DBID: "+ label2.Text + " Ausleihen bitte um kontaktaufnahme";

            mail_to_webseit mail = new mail_to_webseit("ausl",testinhalt,label26.Text,textBox1.Text);
            mail.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://plugins.buch-archiv20-software.de/AnimeFanSubSaystem/afss_red.php?xml=" + label2.Text + ".xml");
        }
    }
}
