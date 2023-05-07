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
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;


namespace BücherDB2._0_Online
{
    public partial class Form2 : Form
    {
        //ping 
        public string IsConnectedToServer(string serverHost)
        {
            System.Text.StringBuilder SB2 = new System.Text.StringBuilder();

            Ping pingSender = new Ping();
            PingOptions option = new PingOptions();

            option.DontFragment = true;

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            PingReply reply = pingSender.Send(serverHost, timeout, buffer, option);
            if(reply.Status == IPStatus.Success)
            {
                SB2.AppendFormat("Ladezeit: {0} ms / Time to Live: {1} ms", reply.RoundtripTime, reply.Options.Ttl);
            }
            return SB2.ToString();
        }
               
        private string DBIDG(int Länge)
        {
            string ret = string.Empty;
            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            string Content = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvw";
            Random rnd = new Random();
            for (int i = 0; i < Länge; i++)
                SB.Append(Content[rnd.Next(Content.Length)]);
            return SB.ToString();
        }
        private void SCS()
        {
            Application.Run(new scs());
        }
        public Form2(string host, string user, string pas, string username, string userpass, string servername , string dbname, string CloudURL ,string Cserver)
        {

            int sleep = 20000;
            Thread t = new Thread(new ThreadStart(SCS));
            t.Start();
            Thread.Sleep(sleep);
            t.Abort();


            InitializeComponent();
            label2.Text = host;
            label5.Text = user;
            label6.Text = pas;
            label8.Text = username;
            label10.Text = userpass;
            textBox3.Text = username;
            label34.Text = servername;
            label36.Text = dbname;

            this.Text = "Bücherei 2.0 ™ [Verbunden mit: " + servername + "]";

            //Versin auslesen und weiter leiten an webseite:
            string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            webBrowser1.Navigate("https://www.buch-archiv20-software.de/pages/programm-infobox.php?version=" + Version);

            ping.Text = IsConnectedToServer(host);

            if (Cserver == "yes") { label37.Text = "https://" + host + "/owncloud"; } else { label37.Text = "https://de.share-your-photo.com/"; }

            string pfad = Directory.GetCurrentDirectory();
            textBox14.Text = pfad + "/PlagIn/";

        }   
        private void Form2_Load(object sender, EventArgs e)
        {

            string connStr = @"SERVER="+label2.Text+";" +
                 "DATABASE="+label36.Text+";" +
                 "UID=" + label5.Text + ";" +
                 "PWD=" + label6.Text + ";";

            MySqlConnection connSYS = new MySqlConnection(connStr);

            connSYS.Open();

            MySqlCommand cmd = null;
            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM server_info";

                cmd = new MySqlCommand(sql, connSYS);

                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(dataTable);
                }

                dataGridView1.DataSource = dataTable;
                dataGridView1.DataMember = dataTable.TableName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ein Feheler ist Aufgetreten:\n {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connSYS != null) connSYS.Close();
            }
            MySqlCommand com2 = connSYS.CreateCommand();
            com2.CommandText = "SELECT `inhaberID` FROM `user` WHERE `username` LIKE \'"+textBox3.Text+"\' LIMIT 0, 1";
            MySqlDataReader Reader;
            connSYS.Open();
            Reader = com2.ExecuteReader();
            while (Reader.Read())
            {
                string row = "";
                row += Reader.GetValue(0).ToString();
                textBox2.Text = row;
            }
            connSYS.Close();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = DBIDG(15);
            textBox1.Text = id;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //BuchID kontrolle
            if (textBox1.Text == "") { textBox1.Text= DBIDG(15); }
            if (comboBox2.Text== "Anime") { MessageBox.Show("Sie Konnen unser PlugIn \"AnimeFanSubSystem\" Verwenden um Nochmer daten zu erfassen");}
            try
            {
                string connStrSV = @"SERVER=" + label2.Text + ";" +
                     "DATABASE=" + label36.Text + ";" +
                     "UID=" + label5.Text + ";" +
                     "PWD=" + label6.Text + ";";

                MySqlConnection myConnection = new MySqlConnection(connStrSV);
                string gomedia = "INSERT INTO media (dbid, inhaberid, inhaber, name, band, doppelband, isbn10, isbn13, preis, typ, verlag, bildurl, standort, zusand) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + comboBox1.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "', '" + textBox8.Text + "', '" + comboBox2.Text + "', '" + textBox9.Text + "', '" + textBox11.Text + "', '" + textBox10.Text + "', '" + comboBox3.Text + "');";
                MySqlCommand myCommand = new MySqlCommand(gomedia);
                myCommand.Connection = myConnection;
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ein Feheler ist Aufgetreten:\n {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //if (myCommand != null) myCommand.Clone();
                MessageBox.Show(string.Format("Ihr Medium ist unter der BuchID {0} gespeichert.", textBox1.Text), "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(label37.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 deti = new Form4(comboBox5.Text, label2.Text,label5.Text,label6.Text,label36.Text);
            deti.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connStr = @"SERVER=" + label2.Text + ";" +
                 "DATABASE=" + label36.Text + ";" +
                 "UID=" + label5.Text + ";" +
                 "PWD=" + label6.Text + ";";

            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = null;
            DataTable dataTable = new DataTable();

            try
            {
                if (comboBox4.Text == "Name") { label32.Text = "SELECT * FROM `media` WHERE `name` LIKE \'" + textBox12.Text + "%\'"; }
                if (comboBox4.Text == "ISBN (10)") { label32.Text = "SELECT * FROM `media` WHERE `isbn10` LIKE \'" + textBox12.Text + "%\'"; }
                if (comboBox4.Text == "ISBN (13)") { label32.Text = "SELECT * FROM `media` WHERE `isbn13` LIKE \'" + textBox12.Text + "%\'"; }
                if (comboBox4.Text == "Typ") { label32.Text = "SELECT * FROM `typ` WHERE `name` LIKE \'" + textBox12.Text + "%\'"; }
                if (comboBox4.Text == "Verlag") { label32.Text = "SELECT * FROM `media` WHERE `verlag` LIKE \'" + textBox12.Text + "%\'"; }

                string sql = label32.Text;

                cmd = new MySqlCommand(sql, conn);

                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(dataTable);
                }

                dataGridView2.DataSource = dataTable;
                dataGridView2.DataMember = dataTable.TableName;

            
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ein Feheler ist Aufgetreten:\n {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            comboBox5.Items.Clear();
            MySqlConnection connSYS = new MySqlConnection(connStr);
            MySqlCommand com = connSYS.CreateCommand();

            com.CommandText = label32.Text;

            MySqlDataReader Reader;
            connSYS.Open();
            Reader = com.ExecuteReader();
            while (Reader.Read())
            {
                string m01 = "";
                
                m01 += Reader.GetValue(0).ToString();   //<<DB ID

                comboBox5.Items.Add(m01);

            }
            connSYS.Close();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            infobox infoB = new infobox();
            infoB.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String param_1 = textBox13.Text;
            String param_2 = "";

            if (param_1 == "") { param_2 = "x"; }else { param_2 = param_1; }

            string time1 = System.DateTime.Now.ToShortDateString();
            string id = DBIDG(5);
            string datei = "startup" + time1 + ""+id+".bat";
            string keyData = @"temp/"+datei;
            string inputbat = "start "+textBox14.Text + "AnimeFanSubSystem.exe " + param_2 +"\n start " + textBox14.Text + "nanweb.exe -p=2377 -c";
            File.WriteAllText(keyData, inputbat);
            string pfad = Directory.GetCurrentDirectory();

            string o1 = textBox14.Text;
            string o2 = pfad + "/temp";

            try
            {
                string[] textlist = Directory.GetFiles(o1, "*.txt");
                foreach (string f in textlist)
                {
                    string fName = f.Substring(o1.Length);
                    File.Copy(Path.Combine(o1, fName), Path.Combine(o2, fName), true);
                }
            }
            catch (DirectoryNotFoundException dirNoFound)
            {
                MessageBox.Show(string.Format("Ein Feheler ist Aufgetreten!\nOrdner Nich Gefunden {0}\nACHTUNG: settings.txt Pfade Anpassen anpassen!", dirNoFound), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Process.Start(pfad+"/temp/"+datei);
        }

        private void TextBox14_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
