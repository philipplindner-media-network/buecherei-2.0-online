using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Caching;
using Microsoft.Win32;

namespace BücherDB2._0_Online
{
    public partial class Form1 : Form
    {
        static string GetMD5Hash(MD5 md5hach, string input)
        {
            byte[] data = md5hach.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sB = new StringBuilder();
            for(int i=0; i<data.Length; i++)
            {
                sB.Append(data[i].ToString("x2"));
            }
            return sB.ToString();
        }
        public Form1()
        {
            InitializeComponent();
            label5.Text = "Programm geladen";
            comboBox1.Text = "--Bitte Auswählen--";
            string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            linkLabel1.Text = "Version: " + Version;


            //Information zur version
            string pfad = Directory.GetCurrentDirectory();
            if (File.Exists(pfad+"\\beta.txt"))
            {
                MessageBox.Show("Dies ist eine \"OPEN BETA\" Version es kann zu Einschreikungen und Fehlern kommen. Bitte diese auf unsere WEB seite Melden Danke", "Versions Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                
            }

            //Prüfung für Programm Typ
            //Prüfung via Regestry
            RegistryKey regtyp = Registry.LocalMachine;
            RegistryKey DK = regtyp.OpenSubKey("Software\\Philipp Lindner Media & Network\\Bücherei 2.0 Online");
            string progtyp=(string)DK.GetValue("typ");
            //Prüfing via XML Config (porg_typ.bxm)
            XmlDocument serverconfigXML = new XmlDocument();
            serverconfigXML.Load(pfad + "\\prog_typ.bxm");
            XmlNode ptyp = serverconfigXML.SelectSingleNode("/buch2online/typ");
            string pxml = ptyp.InnerText;
            if (progtyp == "on" || pxml=="on") { label7.Text = "Onlein Version";}
            if (progtyp == "st" || pxml=="st") { label7.Text = "Stand a lone (offline) Version"; }
            if (progtyp == "po" || pxml=="po") { label7.Text = "Portable Version"; }
            if (progtyp=="st")
            {
                MessageBox.Show("Stand a lone System wird gestertet", "[Stand a lone] Aktivet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                comboBox1.Items.Add(pfad+"/server/st.bsvr");
            }
            if (progtyp == "po" || pxml == "po")
            {
                MessageBox.Show("Prtabele Version wird gestertet", "[Prtabel] Aktivet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                comboBox1.Items.Add(pfad+"/server/portal.bsvr");
            }
            if (progtyp == "on" || pxml == "on")
            {
                MessageBox.Show("keine eigenstandigkeit (Stand a lone System)!", "[Stand a lone] Deaktivirt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (progtyp == "" || pxml =="")
            {
                //Starte programzusatzeinstellung
                progtyp Veditor2 = new progtyp(label7.Text);
                Veditor2.Show();
            }
            

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            string pfad = Directory.GetCurrentDirectory();
            string serverordener = pfad + "/server";
            //MessageBox.Show("ServerOrtner:" + serverordener);
            if (Directory.Exists(serverordener))
            {
                string[] files = Directory.GetFiles(serverordener);
                foreach (string file in files)
                {
                    if (file.ToLower().EndsWith(".bsvr"))
                    {
                        comboBox1.Items.Add(file);

                        label5.Text = "Server Listen Aktualisirt!";

                    }
                }
            }
            else
            {
                Directory.CreateDirectory(serverordener);
            }
            
        }
        
            

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "--Bitte Auswählen--") { MessageBox.Show("ACHTUNG!!! dies ist keine Server Info Datei!!!"); }
            if (textBox1.Text == "" && textBox2.Text == "") { MessageBox.Show("ACHTUNG!! Keine UserDaten Angegben!!!"); }
            else
            {
                using (MD5 md5hach = MD5.Create())
                {

                    string hash2 = GetMD5Hash(md5hach, textBox2.Text);
                    XmlDocument serverconfigXML = new XmlDocument();
                    serverconfigXML.Load(comboBox1.Text);
                    XmlNode MyS_Host    = serverconfigXML.SelectSingleNode("/buch2online/host");
                    XmlNode MyS_User    = serverconfigXML.SelectSingleNode("/buch2online/user");
                    XmlNode MyS_Pass    = serverconfigXML.SelectSingleNode("/buch2online/pass");
                    XmlNode SName       = serverconfigXML.SelectSingleNode("/buch2online/sn");
                    XmlNode MyS_DBNAme  = serverconfigXML.SelectSingleNode("/buch2online/dbname");
                    XmlNode OWNURL = serverconfigXML.SelectSingleNode("/buch2online/cu");
                    XmlNode Cserver = serverconfigXML.SelectSingleNode("buch2online/cs");


                 //password Überfrüfung
                    string connStr = @"SERVER=" + MyS_Host.InnerText + ";" +
                                     "DATABASE=" + MyS_DBNAme.InnerText + ";" +
                                     "UID=" + MyS_User.InnerText + ";" +
                                     "PWD=" + MyS_Pass.InnerText + ";";

                    MySqlConnection connSYS = new MySqlConnection(connStr);
                    MySqlCommand com2 = connSYS.CreateCommand();
                    com2.CommandText = "SELECT `password`,`securityKEY` FROM `user` WHERE `username` LIKE \'" + textBox1.Text + "\' LIMIT 0, 1";
                    MySqlDataReader Reader;
                    connSYS.Open();
                    Reader = com2.ExecuteReader();
                    while (Reader.Read())
                    {
                        string pw = "";
                        string ky = "";
                        pw += Reader.GetValue(0).ToString();
                        ky += Reader.GetValue(1).ToString();

                        //Scherheits Datei einlesen
                        string pfad = Directory.GetCurrentDirectory();
                        string kayData = pfad + "/keypool/usere_server_" + MyS_Host.InnerText + ".keyBO2";
                        StreamReader file = new StreamReader(kayData);
                        
                        string kay = file.ReadLine();

                        if(pw == hash2)
                        {
                            if(ky==kay)
                            {
                                Form2 go = new Form2(MyS_Host.InnerText, MyS_User.InnerText, MyS_Pass.InnerText, textBox1.Text, hash2, SName.InnerText, MyS_DBNAme.InnerText, OWNURL.InnerText, Cserver.InnerText);
                                go.Show();
                            }
                            else
                            {
                                MessageBox.Show("ERROR faler SicherheitsKey!!\n"+kayData, "Error SicherheitsKey", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("Error falsches Password!\nWenn sie nicht Angemedet ins können sie das Über sie Schaltfäche \"Anmeldung\" Machen.","ERROR Passwort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    connSYS.Close();
                    
                    

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "--Bitte Auswählen--") { MessageBox.Show("ACHTUNG!!! dies ist keine Server Info Datei!!!"); }
            else
            {
                XmlDocument mysqlD1 = new XmlDocument();
                mysqlD1.Load(comboBox1.Text);
                XmlNode MyS_Host1 = mysqlD1.SelectSingleNode("/buch2online/host");
                XmlNode MyS_User1 = mysqlD1.SelectSingleNode("/buch2online/user");
                XmlNode MyS_Pass1 = mysqlD1.SelectSingleNode("/buch2online/pass");
                XmlNode SName1 = mysqlD1.SelectSingleNode("/buch2online/sn");
                XmlNode MyS_DBNAme1 = mysqlD1.SelectSingleNode("/buch2online/dbname");

                Form5 reg = new Form5(MyS_Host1.InnerText,MyS_Pass1.InnerText,MyS_User1.InnerText,MyS_DBNAme1.InnerText,SName1.InnerText);
                reg.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            linkLabel1.Text = "Version: " + Version;
            Process.Start("http://www.buch-archiv20-software.de/pages/verdion.php?version=" + Version);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            progtyp Veditor = new progtyp(label7.Text);
            Veditor.Show();
        }
    }
}