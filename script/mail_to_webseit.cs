using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;

namespace BücherDB2._0_Online
{
    public partial class mail_to_webseit : Form
    {
        public mail_to_webseit(string type,string inhalt, string imail,string umail)
        {
            InitializeComponent();

            byte[] umail2 = System.Text.Encoding.UTF8.GetBytes(umail);
            string Umail = System.Convert.ToBase64String(umail2);

            byte[] imail2 = System.Text.Encoding.UTF8.GetBytes(imail);
            string Imail = System.Convert.ToBase64String(imail2);

            byte[] inhalt2 = System.Text.Encoding.UTF8.GetBytes(inhalt);
            string Inhalt = System.Convert.ToBase64String(inhalt2);

            Process.Start("https://server.buch-archiv20-software.de/email_tool/?s1="+Umail+"&s2="+Imail+"&s3="+Inhalt+"&typ="+type);
        }
    }
}
