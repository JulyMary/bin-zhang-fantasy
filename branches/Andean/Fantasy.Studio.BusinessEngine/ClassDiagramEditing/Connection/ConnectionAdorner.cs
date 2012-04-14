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
using System.Collections.ObjectModel;

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
            this.View.AddHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(diagramView_MouseLeftButtonUp), true);
            this.View.AddHandler(Control.MouseLeftButtonDownEvent, new MouseButtonEventHandler(diagramView_MouseLeftButtonDown), true);
            this.View.AddHandler(Control.MouseRightButtonUpEvent, new MouseButtonEventHandler(diagramView_MouseRightButtonUp), true);
            this.View.AddHandler(Control.MouseRightButtonDownEvent, new MouseButtonEventHandler(diagramView_MouseRightButtonDown), true);

            this.Unloaded += new RoutedEventHandler(ConnectionAdorner_Unloaded);

            this.Handlers.AddRange(new IConnectionHandler[] { new NoCursorHandler(), new SelectFirstNodeHandler(), new ExitConnectionHandler() });   


            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this.View);
            layer.Add(this);

            this.IntermediatePoints = new ObservableCollection<Point>();
            this.IntermediatePoints.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PathChanged);


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

        void diagramView_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            InvokeHandler((h, args) => { h.MouseLeftButtonUp(args); }, e); 
        }

        void diagramView_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            InvokeHandler((h, args) => { h.MouseLeftButtonDown(args); }, e); 
        }


        void diagramView_MouseRightButtonUp(object sender, MouseEventArgs e)
        {
            InvokeHandler((h, args) => { h.MouseRightButtonUp(args); }, e);
        }

        void diagramView_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            InvokeHandler((h, args) => { h.MouseRightButtonDown(args); }, e);
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

       

         void PathChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.InvalidateVisual();
        }


        public ObservableCollection<Point> IntermediatePoints { get; private set; }

        public Point? StartPoint { get; set; }

        private Point? _currentPoint;

        public Point? CurrentPoint
        {
            get { return _currentPoint; }
            set 
            {
                if (_currentPoint != value)
                {
                    _currentPoint = value;
                    this.InvalidateVisual();
                }
            }
        }



        private Pen _pen;

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
           

            List<Point> points = new List<Point>();
            if (this.StartPoint != null)
            {
                points.Add((Point)this.StartPoint);
            }
            points.AddRange(this.IntermediatePoints);

            if (this.CurrentPoint != null)
            {
                points.Add((Point)this.CurrentPoint);
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                drawingContext.DrawLine(this._pen, points[i], points[i + 1]);
            }
           
           
        }

        public IServiceProvider Site { get; set; }


        void ConnectionAdorner_Unloaded(object sender, RoutedEventArgs e)
        {
            this.View.RemoveHandler(Control.MouseEnterEvent, new MouseEventHandler(diagramView_MouseEnter));
            this.View.RemoveHandler(Control.MouseLeaveEvent, new MouseEventHandler(diagramView_MouseLeave));
            this.View.RemoveHandler(Control.MouseMoveEvent, new MouseEventHandler(diagramView_MouseMove));
            this.View.RemoveHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(diagramView_MouseLeftButtonUp));
            this.View.RemoveHandler(Control.MouseLeftButtonDownEvent, new MouseButtonEventHandler(diagramView_MouseLeftButtonDown));
            this.View.RemoveHandler(Control.MouseRightButtonUpEvent, new MouseButtonEventHandler(diagramView_MouseRightButtonUp));
            this.View.RemoveHandler(Control.MouseRightButtonDownEvent, new MouseButtonEventHandler(diagramView_MouseRightButtonDown));

        }

        public void Exit()
        {
            if (this.View.IsMouseCaptured)
            {
                this.View.ReleaseMouseCapture();
            }
            this.View.ForceCursor = false;
            this.View.Cursor = this._oldCursor;
            
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this.View);
            layer.Remove(this);

           
            if (this.Exited != null)
            {
                this.Exited(this, EventArgs.Empty);
            }
            this.IsExited = true;
        }


        public bool IsExited { get; private set; }


      

        public event EventHandler Exited;


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
