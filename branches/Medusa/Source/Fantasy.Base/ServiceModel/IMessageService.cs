using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace Fantasy.ServiceModel
{
    public interface IMessageService
    {
        DialogResult Show(DialogResult defaultResult, IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);
        DialogResult Show(DialogResult defaultResult, string text);
        DialogResult Show(DialogResult defaultResult, IWin32Window owner, string text);
        DialogResult Show(DialogResult defaultResult, string text, string caption);
        DialogResult Show(DialogResult defaultResult, IWin32Window owner, string text, string caption);
        DialogResult Show(DialogResult defaultResult, string text, string caption, MessageBoxButtons buttons);
        DialogResult Show(DialogResult defaultResult, IWin32Window owner, string text, string caption, MessageBoxButtons buttons);
        DialogResult Show(DialogResult defaultResult, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        DialogResult Show(DialogResult defaultResult, IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        DialogResult Show(DialogResult defaultResult, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        DialogResult Show(DialogResult defaultResult, IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        DialogResult Show(DialogResult defaultResult, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);


        void WriteLine(string text);
    }


    public abstract class AbstractMessageService : IMessageService
    {

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, string text)
        {
            return this.Show(defaultResult, null, text, null, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, string text, string caption)
        {
            return this.Show(defaultResult, null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons)
        {
            return this.Show(defaultResult, null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon)
        {
            return this.Show(defaultResult, null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton)
        {
            return this.Show(defaultResult, null, text, caption, buttons, icon, defaultButton, (MessageBoxOptions)0);
        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
        {
            return this.Show(defaultResult, null, text, caption, buttons, icon, defaultButton, options);
        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text)
        {
            return this.Show(defaultResult, owner, text, null, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption)
        {
            return this.Show(defaultResult, owner, text, null, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons)
        {
            return this.Show(defaultResult, owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon)
        {
            return this.Show(defaultResult, owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

        }

        public System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton)
        {
            return this.Show(defaultResult, owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

        }

        public abstract System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options);

        public abstract void WriteLine(string text);

    }

    public class CompositeMessageService : AbstractMessageService
    {

        private IMessageService[] _services;
        public CompositeMessageService(IMessageService[] services)
        {
            this._services = (IMessageService[])services.Clone();
        }
        public override System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
        {
            DialogResult rs = defaultResult;
            foreach (IMessageService service in this._services)
            {
                DialogResult dr = service.Show(defaultResult, owner, text, caption, buttons, icon, defaultButton, options);
                if ((dr != defaultResult))
                {
                    rs = dr;
                }
            }
            return rs;
        }

        public override void WriteLine(string text)
        {
            foreach (IMessageService service in this._services)
            {
                service.WriteLine(text);
            }
        }
    }

    public class MessageBoxService : AbstractMessageService
    {


        private Control _owner = null;
        public MessageBoxService()
        {
            //_owner = owner;
        }

        private DialogResult _result;
        public override System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
        {

            if ((this._owner != null) && this._owner.InvokeRequired)
            {
                ShowMessageHandler del = new ShowMessageHandler(ShowMessage);
                this._owner.Invoke(del, owner, text, caption, buttons, icon, defaultButton, options);
            }
            else
            {
                this.ShowMessage(owner, text, caption, buttons, icon, defaultButton, options);
            }

            return _result;
        }

        private void ShowMessage(System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
        {
            this._result = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
        }

        private delegate void ShowMessageHandler(System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options);


        public override void WriteLine(string text)
        {
        }
    }

    public class ConsoleMessageService : AbstractMessageService
    {

        public override System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
        {
            Console.WriteLine(text);
            return defaultResult;
        }

        public override void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }

    public class TextFileMessageService : AbstractMessageService
    {


        private string _fileName;
        public TextFileMessageService(string fileName, bool overwrite)
        {
            if ((overwrite))
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }

            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            this._fileName = fileName;

        }


        private void AppendLine(string text)
        {
            StreamWriter w = File.AppendText(this._fileName);
            w.WriteLine(text);
            w.Close();
        }

        public override System.Windows.Forms.DialogResult Show(System.Windows.Forms.DialogResult defaultResult, System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
        {
            AppendLine(text);
            return defaultResult;
        }

        public override void WriteLine(string text)
        {
            AppendLine(text);
        }
    }
}
