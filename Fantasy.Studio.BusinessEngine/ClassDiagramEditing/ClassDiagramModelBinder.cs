using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sf=Syncfusion.Windows.Diagram;
using System.ComponentModel.Design;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramModelBinder : IDisposable
    {

        private Model.ClassDiagram _model;
        private sf.DiagramModel _view;
       

        public ClassDiagramModelBinder(Model.ClassDiagram model, sf.DiagramModel view)
        {
            this._model = model;
            this._view = view;
            
            foreach (Model.ClassNode node in this._model.Classes)
            {
                this.AddNode(node);
            }

            this._model.Classes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Classes_CollectionChanged);

        }

        void SelectionService_SelectionChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void Classes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        foreach (Model.ClassNode node in e.NewItems)
                        {
                            this.AddNode(node);
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        foreach (Model.ClassNode node in e.OldItems)
                        {
                            this.RemoveNode(node);
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    {
                       
                        foreach (Model.ClassNode node in e.OldItems)
                        {
                            this.RemoveNode(node);
                        }
                        foreach (Model.ClassNode node in e.NewItems)
                        {
                            this.AddNode(node);
                        }
                       
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }


        private void RemoveNode(Model.ClassNode node)
        {
            Shapes.ClassNode shape = this._view.Nodes.Cast<Shapes.ClassNode>().SingleOrDefault(s => s.DataContext == node);
            if (shape != null)
            {
                this._view.Nodes.Remove(shape);
            }
        }

        private void AddNode(Model.ClassNode node)
        {
            Shapes.ClassNode shape = new Shapes.ClassNode();
            shape.DataContext = node;

            this._view.Nodes.Add(shape);
        }

        #region IDisposable Members

        public void Dispose()
        {
            this._model.Classes.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Classes_CollectionChanged);

        }

        #endregion
    }
}
