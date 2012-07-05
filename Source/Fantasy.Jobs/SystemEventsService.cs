using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    public class SystemEventsService : AbstractService
    {

        private Thread _hiddenFormThread;
        
        public override void InitializeService()
        {
            if (Application.OpenForms.Count == 0)
            {
                _waitHandler = new ManualResetEvent(false);
                _hiddenFormThread = ThreadFactory.CreateThread(this.StartApplication).WithStart();
               
                _waitHandler.WaitOne();
                _waitHandler.Dispose();
            }

            base.InitializeService();

           
        }

        private ManualResetEvent _waitHandler;

        private void StartApplication()
        {
            HiddenForm form = new HiddenForm() { };
            form.HandleCreated += new EventHandler(form_HandleCreated);
            Application.Run(form); 
        }

        void form_HandleCreated(object sender, EventArgs e)
        {
            _waitHandler.Set();
        }

        public override void UninitializeService()
        {
           
            base.UninitializeService();
            if (this._hiddenFormThread != null)
            {
                Application.Exit();
            }
        }

        private class HiddenForm : Form
        {
           
        }
    }


}
