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
    public class InheritanceGlyph : ClassDiagramGlyph
    {
        public InheritanceGlyph()
        {
            this.IntermediatePoints = new ObservableCollection<Point>();
        }


        [XAttribute("child")]
        private Guid _childGlyphId;

        public Guid ChildGlyphId
        {
            get { return _childGlyphId; }
            set
            {
                if (_childGlyphId != value)
                {
                    _childGlyphId = value;
                    this.OnPropertyChanged("ChildGlyphId");
                }
            }
        }

        [XAttribute("parent")]
        private Guid _parentGlyphId;

        public Guid ParentGlyphId
        {
            get { return _parentGlyphId; }
            set
            {
                if (_parentGlyphId != value)
                {
                    _parentGlyphId = value;
                    this.OnPropertyChanged("ParentGlyphId");
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

        private ClassGlyph _childClass;

        public ClassGlyph ChildClass
        {
            get { return _childClass; }
            set
            {
                if (_childClass != value)
                {
                    _childClass = value;
                   
                }
            }
        }

        private ClassGlyph _parentClass;

        public ClassGlyph ParentClass
        {
            get { return _parentClass; }
            set
            {
                if (_parentClass != value)
                {
                    _parentClass = value;
                   
                }
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
