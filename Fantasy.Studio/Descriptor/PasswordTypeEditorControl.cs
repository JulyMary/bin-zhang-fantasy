using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Fantasy.RichClient.Framework.Descriptor
{
	internal partial class PasswordTypeEditorControl : Form
	{
		public PasswordTypeEditorControl()
		{
			InitializeComponent();
			
			this.textBox1.PasswordChar = this.textBox2.PasswordChar = '\u25cf';
		}

		private void TextBoxTextChanged(object sender, EventArgs e)
		{
			this.m_accept.Enabled = this.textBox1.Text == this.textBox2.Text;
		}


		public string Value
		{
			get
			{
				return this.textBox1.Text;
			}
			set
			{
				this.textBox1.Text = this.textBox2.Text = value;
			}
		}
	}
}