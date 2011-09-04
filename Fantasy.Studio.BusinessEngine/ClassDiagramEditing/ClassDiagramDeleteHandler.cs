using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;


namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramDeleteHandler : ObjectWithSite, ICommand
    {
       

        public object Execute(object args)
        {
            bool deleteObjects = CanDeleteObjects();

            if (deleteObjects)
            {
                ConfirmDeletionDialog dialog = new ConfirmDeletionDialog();

                if ((bool)dialog.ShowDialog())
                {
                    deleteObjects = dialog.IsDeletingObject;
                }
                else
                {
                    return null;
                }

            }

            if (deleteObjects)
            {
                DoDeleteObjects();
            }
            else
            {
                DoDeleteSymbols();
            }

            return null;


            
        }

        private void DoDeleteSymbols()
        {
            Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
            ISelectionService svc = this.Site.GetRequiredService<ISelectionService>();

            object[] selected = svc.GetSelectedComponents().Cast<object>().ToArray();

            foreach (object o in selected)
            {

                if (o is Model.ClassGlyph)
                {
                    diagram.Classes.Remove((Model.ClassGlyph)o);
                }
               
            }

        }

        private void DoDeleteObjects()
        {
            Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
            ISelectionService ss = this.Site.GetRequiredService<ISelectionService>();
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            IEditingService editing = this.Site.GetRequiredService<IEditingService>();

            object[] selected = ss.GetSelectedComponents().Cast<object>().ToArray();

            DoDeleteSymbols();
           

            foreach (object o in selected)
            {

                if (o is Model.ClassGlyph)
                {
                    BusinessClass cls = (BusinessClass)((Model.ClassGlyph)o).Entity;
                    cls.Package.Classes.Remove(cls);
                    cls.ParentClass.ChildClasses.Remove(cls);
                    cls.Package = null;
                    diagram.DeletingEntities.Add(cls);
                    editing.CloseView(cls, true);

                }

            }

        }


        private bool CanDeleteObjects()
        {
            List<object> deletable = new List<object>();
            ISelectionService svc = this.Site.GetRequiredService<ISelectionService>();
            
            object[] selected = svc.GetSelectedComponents().Cast<object>().ToArray();
           
            foreach (object o in selected)
            {

                if (o is Model.ClassGlyph)
                {
                    Model.ClassGlyph node = (Model.ClassGlyph)o;
                    if (node.IsShortCut)
                    {
                        return false;
                    }
                    else if(!IsDeletable(node.Entity, deletable, selected))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }


            return true;

        }

        private bool IsDeletable(BusinessClass @class, List<object> deletable, object[] selected)
        {
            
           
           
            if (deletable.IndexOf(@class) >= 0)
            {
                return true;
            }


            if (@class.IsSystem)
            {
                return false;
            }


            Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();

           
            if (diagram.Entity.Package != @class.Package)
            {
                return false;
            }

            var hasNode = from node in diagram.Classes where node.Entity == @class select node;

            if (!hasNode.Any())
            {
                return false;
            }

           
            var hasUnselectedNode = from node in diagram.Classes where node.Entity == @class && Array.IndexOf(selected, node) < 0 select node;

            if (hasUnselectedNode.Any())
            {
                return false;
            }


            foreach (BusinessClass child in @class.ChildClasses)
            {
                if (IsDeletable(child, deletable, selected))
                {
                    return false;
                }
            }

            deletable.Add(@class);
            return true;

        }

      
    }
}
