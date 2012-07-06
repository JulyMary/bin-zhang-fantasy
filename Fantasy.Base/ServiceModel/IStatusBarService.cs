using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace Fantasy.ServiceModel
{
    public interface IStatusBarService
    {
        void SetStatus(string status);
    }

    public interface IStatusBarServiceContainer : IStatusBarService
    {
        void Add(IStatusBarService service);
        void Remove(IStatusBarService service);
    }

    public class StatusBarServiceContainer : IStatusBarServiceContainer
    {

        private ArrayList _services = new ArrayList();
        public void SetStatus(string status)
        {
            foreach (IStatusBarService service in this._services)
            {
                service.SetStatus(status);
            }
        }

        public void Add(IStatusBarService service)
        {
            _services.Add(service);
        }

        public void Remove(IStatusBarService service)
        {
            _services.Remove(service);
        }
    }


    public class ToolStripStatusBarService : IStatusBarService
    {

        private ToolStripStatusLabel _label;
        public ToolStripStatusBarService(ToolStripStatusLabel label)
        {
            if ((label == null))
            {
                throw new ArgumentNullException("label");
            }
            this._label = label;
        }
        #region IStatusBarService Members

        public void SetStatus(string status)
        {
            if (_label.Owner.InvokeRequired)
            {
                MethodInvoker<string> del = new MethodInvoker<string>(SetStatus);
                _label.Owner.Invoke(del, status);
            }
            else
            {
                this._label.Text = status;
              
            }
        }

        #endregion
    }
 

    public class LabelStatusBarService : IStatusBarService
    {

        private Label _label;
        private string _status;
        public LabelStatusBarService(Label label)
        {
            if ((label == null))
            {
                throw new ArgumentNullException("label");
            }
            this._label = label;
            if (!this._label.IsHandleCreated)
            {
                this._label.HandleCreated += new EventHandler(Label_HandleCreated);
            }

        }

        void Label_HandleCreated(object sender, EventArgs e)
        {
            this._label.HandleCreated -= new EventHandler(Label_HandleCreated);
            this._label.Invoke(new MethodInvoker(()=>{
                this._label.Text = this._status;
            }));
        }


        public virtual void SetStatus(string status)
        {
            this._status = status;
            if (_label.IsHandleCreated)
            {
                if (_label.InvokeRequired)
                {
                    MethodInvoker<string> del = new MethodInvoker<string>(SetStatus);
                    _label.Invoke(del, status);
                }
                else
                {
                    this._label.Text = status;

                }
            }

        }
    }


    public static class StatusBarExtensions
    {
        public static void SetStatus(this IStatusBarService bar, string format, params object[] args)
        {
            bar.SetStatus(string.Format(format, args));
        }

        public static void SafeSetStatus(this IStatusBarService bar, string format, params object[] args)
        {
            if (bar != null)
            {
                bar.SetStatus(string.Format(format, args));
            }
        }
    }

    public class ActionStatusBarService : IStatusBarService
    {
        Action<string> _action;
        public ActionStatusBarService(Action<string> action)
        {
            this._action = action;
        }

        #region IStatusBarService Members

        public void SetStatus(string status)
        {
            this._action(status);
        }

        #endregion
    }

}