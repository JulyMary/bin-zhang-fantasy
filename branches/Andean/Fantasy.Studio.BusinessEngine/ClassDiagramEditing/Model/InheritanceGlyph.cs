using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.XSerialization;
using System.Collections.ObjectModel;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    [XSerializable("inheritance", NamespaceUri= Consts.ClassDiagramNamespace)]
    public class InheritanceGlyph : ClassDiagramGlyph, Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model.IConnectGlyph
    {
        public InheritanceGlyph()
        {
            this.IntermediatePoints = new ObservableCollection<Point>();
        }


        [XAttribute("derived")]
        private Guid _derivedGlyphId;

        public Guid DerivedGlyphId
        {
            get { return _derivedGlyphId; }
            set
            {
                if (_derivedGlyphId != value)
                {
                    _derivedGlyphId = value;
                    this.OnPropertyChanged("DerivedGlyphId");
                }
            }
        }

        [XAttribute("base")]
        private Guid _baseGlyphId;

        public Guid BaseGlyphId
        {
            get { return _baseGlyphId; }
            set
            {
                if (_baseGlyphId != value)
                {
                    _baseGlyphId = value;
                    this.OnPropertyChanged("BaseGlyphId");
                }
            }
        }


        public string BaseTypeName
        {
            get
            {
                return this.BaseClass != null ? this.BaseClass.Entity.FullName : null; 
            }
        }

        public string DerivedTypeName
        {
            get
            {
                return this.DerivedClass != null ? this.DerivedClass.Entity.FullName : null;
            }
        }

        private ClassGlyph _derivedClass;

        public ClassGlyph DerivedClass
        {
            get { return _derivedClass; }
            set
            {
                if (_derivedClass != value)
                {
                    _derivedClass = value;

                }
            }
        }

        private ClassGlyph _baseClass;

        public ClassGlyph BaseClass
        {
            get { return _baseClass; }
            set
            {
                if (_baseClass != value)
                {
                    _baseClass = value;

                }
            }
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
            base.SaveEntity();
        }


        [XArray, 
        XArrayItem(Name = "point", Type= typeof(Point))]
        public ObservableCollection<Point> IntermediatePoints { get; private set; }



       
    }
}
