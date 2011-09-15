using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel.Design;
using Fantasy.BusinessEngine;
using System.Collections;
using Fantasy.Adaption;
using System.Reflection;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassNodeDeleteHandler : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            ISelectionService selectionService = this.Site.GetRequiredService<ISelectionService>();

            bool rs = false;

            Model.ClassGlyph glyph = (Model.ClassGlyph)parameter;
            if (!glyph.IsShortCut)
            {

                ICollection selected = selectionService.GetSelectedComponents();
             
                if (selected.Count > 0)
                {
                    rs = true;
                    foreach (object o in selectionService.GetSelectedComponents())
                    {
                        if (!(o is Model.MemberNode) || ((Model.MemberNode)o).IsSystem || ((Model.MemberNode)o).IsInherited)
                        {
                            rs = false;
                            break;
                        }
                    }
                }
            }

            return rs;

        }

        event EventHandler ICommand.CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            ISelectionService selectionService = this.Site.GetRequiredService<ISelectionService>();

            var selected = selectionService.GetSelectedComponents().Cast<object>();

            Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();

            foreach (Model.MemberNode o in selected)
            {
                if (o is Model.PropertyNode)
                {
                    BusinessProperty prop = ((Model.PropertyNode)o).Entity;
                   
                    prop.Class.Properties.Remove(prop);
                    prop.Class = null;
                }
                else if (o is Model.LeftRoleNode || o is Model.RightRoleNode)
                {
                    BusinessAssociation association = Invoker.Invoke<BusinessAssociation>(o, "Entity");
                    if (association.Package != null)
                    {
                        association.Package.Associations.Remove(association);
                        association.Package = null;
                    }
                    if (association.LeftClass != null)
                    {
                        association.LeftClass.LeftAssociations.Remove(association);
                        association.LeftClass = null;
                    }
                    if (association.RightClass != null)
                    {
                        association.RightClass.RightAssociations.Remove(association);
                        association.RightClass = null;
                    }

                    var query = from glyph in diagram.Associations where glyph.Entity == association select glyph;

                    foreach (Model.AssociationGlyph glyph in query.ToArray())
                    {
                        diagram.Associations.Remove(glyph);
                    }
                    diagram.DeletingEntities.Add(association);

                }
            }
        }

        #endregion
    }
}
