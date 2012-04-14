﻿using System;
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
            
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
            es.BeginUpdate();
            try
            {
                es.SaveOrUpdate(this.Entity);
                es.EndUpdate(true);
                base.SaveEntity();
            }
            catch
            {
                es.EndUpdate(false);
                throw;
            }
            
        }



        

        [XArray,
        XArrayItem(Name = "point", Type = typeof(Point))]
        private Point[]_persistedIntermediatePoints = new Point[0];

        private ObservableCollection<Point> _intermediatePoints;
        public ObservableCollection<Point> IntermediatePoints
        {
            get
            {
                if (this._intermediatePoints == null)
                {
                    this._intermediatePoints = new ObservableCollection<Point>(_persistedIntermediatePoints);
                    this._intermediatePoints.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(IntermediatePoints_CollectionChanged);
                }
                return _intermediatePoints;
            }
        }


        void IntermediatePoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this._persistedIntermediatePoints = this._intermediatePoints.ToArray();
            this.EditingState = Studio.EditingState.Dirty;

        }
        
        
    }
}
