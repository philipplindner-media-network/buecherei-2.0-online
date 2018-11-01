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

namespace BücherDB2._0_Online
{
    public partial class progtyp : Form
    {
        public progtyp(string typ)
        {
            InitializeComponent();
            comboBox1.Text = typ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string keyData = @"prog_typ.bxm";
            string typ = "<buch2online>\n\t<typ>" + comboBox1.Text + "</typ>\n</buch2online>";
            File.WriteAllText(keyData, typ);
        }
    }
}
