using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Collections.ObjectModel;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{

    [XSerializable("association", NamespaceUri = Consts.ClassDiagramNamespace)]
    public class AssociationGlyph : ClassDiagramGlyph, IConnectGlyph
    {
        public AssociationGlyph()
        {
            this._assocationListener = new WeakEventListener(this.EntityStateChanged);
            this.IntermediatePoints = new ObservableCollection<Point>();
        }

        [XAttribute("left")]
        private Guid _leftGlyphId;

        public Guid LeftGlyphId
        {
            get { return _leftGlyphId; }
            set
            {
                if (_leftGlyphId != value)
                {
                    _leftGlyphId = value;
                    this.OnPropertyChanged("LeftGlyphId");
                }
            }
        }

        [XAttribute("right")]
        private Guid _rightGlyphId;

        public Guid RightGlyphId
        {
            get { return _rightGlyphId; }
            set
            {
                if (_rightGlyphId != value)
                {
                    _rightGlyphId = value;
                    this.OnPropertyChanged("RightGlyphId");
                }
            }
        }


        private bool? _isShortCut;

        public bool IsShortCut
        {
            get
            {
                if (_isShortCut == null)
                {
                    _isShortCut = this.Entity.Package != this.Diagram.Entity.Package;
                }
                return (bool)_isShortCut;
            }
        }
        

        private ClassGlyph _leftClass;

        public ClassGlyph LeftClass
        {
            get { return _leftClass; }
            set { _leftClass = value; }
        }

        private ClassGlyph _rightClass;

        public ClassGlyph RightClass
        {
            get { return _rightClass; }
            set { _rightClass = value; }
        }

        [XAttribute("association")]
        private Guid _associationId;

        public Guid AssociationId
        {
            get { return _associationId; }
            set
            {
                if (_associationId != value)
                {
                    _associationId = value;
                    this.OnPropertyChanged("AssociationId");
                }
            }
        }

        private BusinessAssociation _entity;

        public BusinessAssociation Entity
        {
            get { return _entity; }
            set
            {
                _entity = value;
                if (value != null)
                {
                    this.AttatchEntity(_entity);
                }
            }
        }

        private bool EntityStateChanged(Type managerType, object sender, EventArgs e)
        {
            if (((IEntity)sender).EntityState != EntityState.Clean)
            {
                this.EditingState = EditingState.Dirty;
            }
            return true;
        }

        private WeakEventListener _assocationListener;

        private void AttatchEntity(BusinessAssociation entity)
        {
            EditingState state = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
            EntityStateChangedEventManager.AddListener(this.Entity, this._assocationListener);
            this.EditingState = state;
        }

        private static string[] NoneSerializeProperties = new string[] {
            "EditingState"
        };

        protected override void OnPropertyChanged(string propertyName)
        {

            base.OnPropertyChanged(propertyName);
            if (Array.IndexOf(NoneSerializeProperties, propertyName) < 0)
            {
                this.EditingState = EditingState.Dirty;
            }
        }

        public override void SaveEntity()
        {
            
                ISession session = this.Site.GetRequiredService<IEntityService>().DefaultSession;

                session.BeginUpdate();
                try
                {
                    session.SaveOrUpdate(this.Entity);
                    session.EndUpdate(true);
                    base.SaveEntity();
                }
                catch
                {
                    session.EndUpdate(false);
                    throw;
                }
            
        }


        [XArray,
        XArrayItem(Name = "point", Type = typeof(Point))]
        public ObservableCollection<Point> IntermediatePoints { get; private set; }
        
    }
}
