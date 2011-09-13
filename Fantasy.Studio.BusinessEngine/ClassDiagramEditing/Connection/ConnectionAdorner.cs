using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Syncfusion.Windows.Diagram;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class ConnectionAdorner : Adorner, IObjectWithSite
    {
        public ConnectionAdorner(DiagramView view, IServiceProvider services)
            : base(view.Page)
        {
            this.Site = services;
            this.View = view;
            this.View.ForceCursor = true;
            this._oldCursor = this.View.Cursor;
            this._pen = new Pen(new SolidColorBrush(Colors.DarkGray), 2);
            _pen.DashStyle = DashStyles.Dash; 

         

            this.View.AddHandler(Control.MouseEnterEvent, new MouseEventHandler(diagramView_MouseEnter), true);
            this.View.AddHandler(Control.MouseLeaveEvent, new MouseEventHandler(diagramView_MouseLeave), true);
            this.View.AddHandler(Control.MouseMoveEvent, new MouseEventHandler(diagramView_MouseMove), true);
            this.View.AddHandler(Control.MouseUpEvent, new MouseButtonEventHandler(diagramView_MouseUp), true);
            this.View.AddHandler(Control.MouseDownEvent, new MouseButtonEventHandler(diagramView_MouseDown), true);
            this.Unloaded += new RoutedEventHandler(ConnectionAdorner_Unloaded);

            this.Handlers.AddRange(new IConnectionHandler[] { new NoCursorHandler(), new SelectFirstNodeHandler(), new ExitConnectionHandler() });   


            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this.View);
            layer.Add(this);


        }

        
        private Cursor _oldCursor;

      

        private void InvokeHandler(Action<IConnectionHandler, ConnectionArgs> action, MouseEventArgs e)
        {
            ConnectionArgs args = new ConnectionArgs() { Owner = this, Handled = false, MouseEventArgs = e };
            foreach (IConnectionHandler handler in this._handlers.ToArray())
            {
                action(handler, args);

                if (args.Handled)
                {
                    break;
                }
            }

            if (args.Cursor != null)
            {
                this.View.Cursor = args.Cursor;
            }
        }

        void diagramView_MouseEnter(object sender, MouseEventArgs e)
        {
            InvokeHandler((h, args) => { h.MouseEnter(args); }, e); 
        }

        void diagramView_MouseLeave(object sender, MouseEventArgs e)
        {
            InvokeHandler((h, args) => { h.MouseLeave(args); }, e); 
        }

        void diagramView_MouseMove(object sender, MouseEventArgs e)
        {

            InvokeHandler((h, args) => { h.MouseMove(args); }, e); 
        }

        void diagramView_MouseUp(object sender, MouseEventArgs e)
        {
            InvokeHandler((h, args) => { h.MouseUp(args); }, e); 
        }

        void diagramView_MouseDown(object sender, MouseEventArgs e)
        {
            InvokeHandler((h, args) => { h.MouseDown(args); }, e); 
        }


        private List<IConnectionHandler> _handlers = new List<IConnectionHandler>();
        internal List<IConnectionHandler> Handlers
        {
            get
            {
                return _handlers;
            }
        }

        public Node FirstNode { get; set; }

        public Node SecondNode { get; set; } 

        public DiagramView View { get; set; }

        private Point? _firstPoint;

        public Point? FirstPoint
        {
            get { return _firstPoint; }
            set { _firstPoint = value; }
        }

        private Point? _secondPoint = null;
        public Point? SecondPoint
        {
            get
            {
                return _secondPoint;
            }
            set
            {
                this._secondPoint = value;
                this.InvalidateVisual();
            }
        }


        private Pen _pen;

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (this.FirstPoint != null && this.SecondPoint != null)
            {
                drawingContext.DrawLine(this._pen, (Point)FirstPoint, (Point)SecondPoint);
            }
        }

        public IServiceProvider Site { get; set; }


        void ConnectionAdorner_Unloaded(object sender, RoutedEventArgs e)
        {
            this.View.RemoveHandler(Control.MouseEnterEvent, new MouseEventHandler(diagramView_MouseEnter));
            this.View.RemoveHandler(Control.MouseLeaveEvent, new MouseEventHandler(diagramView_MouseLeave));
            this.View.RemoveHandler(Control.MouseMoveEvent, new MouseEventHandler(diagramView_MouseMove));
            this.View.RemoveHandler(Control.MouseUpEvent, new MouseButtonEventHandler(diagramView_MouseUp));
        }

        public void OnExit()
        {
            if (this.View.IsMouseCaptured)
            {
                this.View.ReleaseMouseCapture();
            }
            this.View.ForceCursor = false;
            this.View.Cursor = this._oldCursor;
            
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this.View);
            layer.Remove(this);
            if (this.Exit != null)
            {
                this.Exit(this, EventArgs.Empty);
            }
        }

        public event EventHandler Exit;


        public event EventHandler CreatConnection;

        protected internal virtual void OnCreatConnection(EventArgs e)
        {
            if (this.CreatConnection != null)
            {
                this.CreatConnection(this, e);
            }
        }

        public event EventHandler<ValidatNodeArgs> ValidateFirstNode;

        protected internal virtual void OnValidateFirstNode(ValidatNodeArgs e)
        {
            if (this.ValidateFirstNode != null)
            {
                this.ValidateFirstNode(this, e);
            }
        }

        public event EventHandler<ValidatNodeArgs> ValidateSecondNode;

        protected internal virtual void OnValidateSecondNode(ValidatNodeArgs e)
        {
            if (this.ValidateSecondNode != null)
            {
                this.ValidateSecondNode(this, e);
            }
        }



       
    }
}
