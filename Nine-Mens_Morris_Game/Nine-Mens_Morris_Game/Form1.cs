using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nine_Mens_Morris_Game
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void EnterNyomHehe(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                button1.PerformClick();
            }
        }
    }
}
