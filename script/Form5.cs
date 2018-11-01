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
using MySql.Data;
using System.Security.Cryptography;
using System.Net.Mail;
using System.IO;

namespace BücherDB2._0_Online
{
    public partial class Form5 : Form
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
        private string xC(int Länge)
        {
            string ret = string.Empty;
            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            string Content = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvw-_#!?%$";
            Random rnd = new Random();
            for (int i = 0; i < Länge; i++)
                SB.Append(Content[rnd.Next(Content.Length)]);
            return SB.ToString();
        }

        private string inhaberIDG(int Länge)
        {
            string ret = string.Empty;
            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            string Content = "1234567890";
            Random rnd = new Random();
            for (int i = 0; i < Länge; i++)
                SB.Append(Content[rnd.Next(Content.Length)]);
            return SB.ToString();
        }
        public Form5(string myhost, string mypass, string myuser, string mydbname, string server)
        {
            InitializeComponent();
            textBox4.Text = inhaberIDG(9) + "-" + inhaberIDG(10) + "-" + inhaberIDG(8);
            label6.Text = "Anmelduen für: " + server + " (" + myhost + ")";

            label7.Text = myhost;
            label10.Text = mypass;
            label8.Text = myuser;
            label9.Text = mydbname;
            label11.Text = server;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (MD5 md5hach = MD5.Create())
                {
                    string pHachMD5 = GetMD5Hash(md5hach, textBox3.Text);
                    //secorety
                    string KaySysMD5 = GetMD5Hash(md5hach, textBox4.Text);

                    string KeySecy=xC(20)+"-Y-"+KaySysMD5+"|"+xC(30)+"_"+label7.Text+"-BO-DE";

                    string connStrSV = @"SERVER=" + label7.Text + ";" +
                         "DATABASE=" + label9.Text + ";" +
                         "UID=" + label8.Text + ";" +
                         "PWD=" + label10.Text + ";";

                    MySqlConnection myConnection = new MySqlConnection(connStrSV);
                    string gomedia = "INSERT INTO user (UserID, inhaberID, username, Name, password, email, securityKEY) VALUES ('', '" + textBox4.Text + "', '" + textBox2.Text + "', '" + textBox1.Text + "', '" + pHachMD5 + "', '" + textBox5.Text + "', '" + KeySecy + "');";
                    MySqlCommand myCommand = new MySqlCommand(gomedia);
                    myCommand.Connection = myConnection;
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();

                    //Sicherhets Datei anlegen
                    string keyData = @"keypool/usere_server_" + label7.Text + ".keyBO2";
                    File.WriteAllText(keyData, KeySecy);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ein Feheler ist Aufgetreten:\n {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //if (myCommand != null) myCommand.Clone();
                MessageBox.Show("Iher Daten sine nun gespeicher!!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


                //User mit eine E-Mail Informiren

               string textinhalt ="Hallo "+textBox1.Text+"\nSie haben sich beim Server: "+label11.Text+" Angemedet. Hier Naochmal Ihre Daten.\nInhaberID: "+textBox4.Text+"\nUsername: "+textBox2.Text+"\nName: "+textBox1.Text+"\npassword: "+textBox3.Text+"\n\nMfG Der ServerAdmin\n\n--\nDiese E-Mal ist Automatich erstelt, bitte nicht Darauf Antworten Danke!";

                mail_to_webseit mail = new mail_to_webseit("reg", textinhalt, "", textBox5.Text);
                mail.Show();
            }
        }
    }
}
