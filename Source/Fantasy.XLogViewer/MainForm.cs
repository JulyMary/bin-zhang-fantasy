using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Fantasy.XLogViewer.Properties;
using System.Xml.Xsl;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace Fantasy.XLogViewer
{
    public partial class MainForm : Form
    {

        private string _fileName;
       
        public MainForm()
        {
           
            InitializeComponent();
          
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Open(this.openFileDialog.FileName); 
            }
        }

        private void Open(string path)
        {

            try
            {
                this._fileName = path;
                this.toolStripStatusLabel.Image = null; 
                this.toolStripStatusLabel.Text = "Loading...";
                Application.DoEvents();

               
                List<XElement> nodes = new List<XElement>();
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(fs);
                try
                {
                    string line;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();

                        try
                        {
                            XElement node = XElement.Parse(line);
                            nodes.Add(node);
                        }
                        catch
                        {
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }

                
                nodes.Reverse();
                XElement root = new XElement("xlog");
                foreach (XElement node in nodes)
                {
                    root.Add(node);
                }
                

                XmlDocument xsltDoc = new XmlDocument();
                xsltDoc.LoadXml(Resources.XLogXslt);

                XsltSettings settings = new XsltSettings(false, true);



                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xsltDoc.CreateNavigator(),settings, new XmlUrlResolver() );


                MemoryStream output = new MemoryStream();

                StreamWriter writer = new StreamWriter(output);
                XsltArgumentList args = new XsltArgumentList();
                Uri location = new Uri(Path.GetDirectoryName(Application.ExecutablePath));
                args.AddParam("executeLocation", "", location.ToString() + "/");
                xslt.Transform(root.CreateReader(), args, output);

                output.Position = 0;

                reader = new StreamReader(output);
                string html = reader.ReadToEnd();

                string tempFile = Path.GetTempFileName() + ".htm";
                File.WriteAllText(tempFile, html);

                webBrowser.Navigated += new WebBrowserNavigatedEventHandler(XLogFile_Navigated);

                webBrowser.Navigate(tempFile); 

                this.Text = "Fantasy XLog Viewer - " + Path.GetFileName(path);
            }
            catch(Exception ex)
            {
                _errorMessage = path + "\n" + ex.ToString();
                this.toolStripStatusLabel.Image = Resources.ErrorIcon.ToBitmap(); 
                this.toolStripStatusLabel.Text = "Error";
            }

        }

        void XLogFile_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.toolStripStatusLabel.Text = "Ready";
        }

        private string _errorMessage;

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.webBrowser.Navigated += new WebBrowserNavigatedEventHandler(webBrowser_Navigated);
            string blankfile = new Uri(Path.GetDirectoryName(Application.ExecutablePath) + "\\welcome.htm").ToString();
            this.webBrowser.Navigate(blankfile); 
        }

        void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            
 
            this.webBrowser.Navigated -= new WebBrowserNavigatedEventHandler(webBrowser_Navigated);
            string[] activationData = null;
            if(AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null)
            {
                activationData = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
            }
            if (activationData != null && activationData.Length > 0)
            {
                Uri uri = new Uri(activationData[0]);

                this.Open(uri.LocalPath);
            }
            else if(Environment.GetCommandLineArgs().Length > 1) 
            {
                this.Open(Environment.GetCommandLineArgs()[1]);

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._errorMessage))
            {
                MessageBox.Show(this._errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

      

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            if (Settings.Default.StartupBounds != Rectangle.Empty)
            {
                this.Bounds = Settings.Default.StartupBounds;
            }
            this.WindowState = Settings.Default.StartupWindowsState;

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.StartupBounds = this.WindowState == FormWindowState.Normal ? this.Bounds : this.RestoreBounds;
            Settings.Default.StartupWindowsState = this.WindowState != FormWindowState.Minimized ? this.WindowState : FormWindowState.Normal;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this._fileName))
            {
                this.Open(this._fileName); 
            }
        }

        private void webBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                if (!string.IsNullOrWhiteSpace(this._fileName))
                {
                    this.Open(this._fileName);
                }
                
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.FileName = Path.GetFileName(this._fileName);
            if (this.saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {

                    File.Copy(this._fileName, this.saveFileDialog.FileName, true);
                    this._fileName = this.saveFileDialog.FileName;

                    this.Text = "Fantasy XLog Viewer - " + Path.GetFileName(_fileName);
                }
                catch (Exception ex)
                {
                    _errorMessage = this.saveFileDialog.FileName + "\n" + ex.ToString();
                    this.toolStripStatusLabel.Image = Resources.ErrorIcon.ToBitmap();
                    this.toolStripStatusLabel.Text = "Error";
                }
                
            }
                  
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.saveAsToolStripMenuItem.Enabled = !string.IsNullOrEmpty(this._fileName);
        }

       
    }
}
