﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;
using Syncfusion.Windows.Diagram;


namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramDeleteHandler : ObjectWithSite, ICommand
    {
       

        public object Execute(object args)
        {
            ISelectionService svc = this.Site.GetRequiredService<ISelectionService>();
            object[] selected = svc.GetSelectedComponents().Cast<object>().ToArray();

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

            DiagramView view = this.Site.GetRequiredService<DiagramView>();
            view.SelectionList.Clear();

            if (deleteObjects)
            {
                DoDeleteObjects(selected);
            }
            else
            {
                DoDeleteSymbols(selected);
            }

            return null;


            
        }

        private void DoDeleteSymbols(object[] selected)
        {
            Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
           

           

            foreach (object o in selected)
            {

                if (o is Model.ClassGlyph)
                {
                    Model.ClassGlyph cls = (Model.ClassGlyph)o;


                    var query = from inheritance in diagram.Inheritances
                                where inheritance.BaseClass == cls || inheritance.DerivedClass == cls
                                select inheritance;
                    foreach (Model.InheritanceGlyph inheritance in query.ToArray())
                    {
                        diagram.Inheritances.Remove(inheritance);
                    }

                    diagram.Classes.Remove((Model.ClassGlyph)o);

                }
                else if (o is Model.EnumGlyph)
                {
                    diagram.Enums.Remove((Model.EnumGlyph)o);
                }
                else if (o is Model.InheritanceGlyph)
                {
                    diagram.Inheritances.Remove((Model.InheritanceGlyph)o);
                }
               
            }

        }

        private void DoDeleteObjects(object[] selected)
        {
            Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
            ISelectionService ss = this.Site.GetRequiredService<ISelectionService>();
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            IEditingService editing = this.Site.GetRequiredService<IEditingService>();

           

            DoDeleteSymbols(selected);

            BusinessClass rootClass = es.GetRootClass();
            foreach (object o in selected)
            {

                if (o is Model.ClassGlyph)
                {
                   
                    BusinessClass cls = (BusinessClass)((Model.ClassGlyph)o).Entity;
                    if (!diagram.DeletingEntities.Contains(cls))
                    {
                        cls.Package.Classes.Remove(cls);
                        cls.ParentClass.ChildClasses.Remove(cls);
                        //cls.Package = null;
                        diagram.DeletingEntities.Add(cls);
                        editing.CloseView(cls, true);
                    }
                }
                else if (o is Model.InheritanceGlyph)
                {
                    Model.InheritanceGlyph inheritance = (Model.InheritanceGlyph)o;
                    BusinessClass cls = inheritance.DerivedClass.Entity;
                    
                    if (!diagram.DeletingEntities.Contains(cls))
                    {
                        cls.ParentClass.ChildClasses.Remove(cls);
                        cls.ParentClass = rootClass;
                        rootClass.ChildClasses.Add(cls);
                    }
                }
                else if (o is Model.EnumGlyph)
                {

                    BusinessEnum @enum = (BusinessEnum)((Model.EnumGlyph)o).Entity;
                    if (!diagram.DeletingEntities.Contains(@enum))
                    {
                        @enum.Package.Enums.Remove(@enum);
                        //@enum.Package = null;
                        diagram.DeletingEntities.Add(@enum);
                        editing.CloseView(@enum, true);
                    }
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
                else if (o is Model.InheritanceGlyph)
                {
                    Model.InheritanceGlyph inheritance = (Model.InheritanceGlyph)o;
                    if (inheritance.DerivedClass.Entity.EntityState != EntityState.New)
                    {
                        return false;
                    }
                }
                else if (o is Model.EnumGlyph)
                {
                    Model.EnumGlyph node = (Model.EnumGlyph)o;
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

        private bool IsDeletable(BusinessEnum @enum, List<object> deletable, object[] selected)
        {
            if (deletable.IndexOf(@enum) >= 0)
            {
                return true;
            }


            if (@enum.IsSystem)
            {
                return false;
            }


            Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();


            if (diagram.Entity.Package != @enum.Package)
            {
                return false;
            }

            var hasNode = from node in diagram.Enums where node.Entity == @enum select node;

            if (!hasNode.Any())
            {
                return false;
            }


            var hasUnselectedNode = from node in diagram.Enums where node.Entity == @enum  && Array.IndexOf(selected, node) < 0 select node;

            if (hasUnselectedNode.Any())
            {
                return false;
            }

            deletable.Add(@enum);
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