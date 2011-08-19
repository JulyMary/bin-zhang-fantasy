using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using be = Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    public class BusinessClassModel : NotifyPropertyChangedObject, IObjectWithSite
    {

        public IServiceProvider Site { get; set; }

        private be.BusinessClass _entity;

        public be.BusinessClass Entity
        {
            get { return _entity; }
            set
            {
                if (_entity != value)
                {
                    _entity = value;
                    this.OnPropertyChanged("Entity");
                }
            }
        }

        private bool _isExpanded = true;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
            }
        }

        private bool _isPropertiesExpanded = true;

        public bool IsPropertiesExpanded
        {
            get { return _isPropertiesExpanded; }
            set
            {
                if (_isPropertiesExpanded != value)
                {
                    _isPropertiesExpanded = value;
                    this.OnPropertyChanged("IsPropertiesExpanded");
                }
            }
        }


        private bool _isRelationsExpanded = true;

        public bool IsRelationsExpanded
        {
            get { return _isRelationsExpanded; }
            set
            {
                if (_isRelationsExpanded != value)
                {
                    _isRelationsExpanded = value;
                    this.OnPropertyChanged("IsRelationsExpanded");
                }
            }
        }

        private bool _isShortCut;

        public bool IsShortCut
        {
            get { return _isShortCut; }
            set
            {
                if (_isShortCut != value)
                {
                    _isShortCut = value;
                    this.OnPropertyChanged("IsShortCut");
                }
            }
        }
       
    }
}
