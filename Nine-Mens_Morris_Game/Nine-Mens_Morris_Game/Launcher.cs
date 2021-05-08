using System;
using System.Windows.Forms;

namespace Nine_Mens_Morris_Game {
	public partial class Launcher : Form
	{

		public Launcher()
		{
			InitializeComponent();
		}

		private void TovábbClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

	}
}
