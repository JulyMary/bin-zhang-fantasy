using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Fantasy.Collections;
using Fantasy.BusinessEngine;
using System.Windows.Media;
using System.ComponentModel;
using Fantasy.BusinessEngine.EntityExtensions;
using Fantasy.Windows;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Fantasy.Studio.BusinessEngine.ExtensionEditing
{
    public class EntityExtensionEditorModel : NotifyPropertyChangedObject
    {
        public EntityExtensionEditorModel(ExtensionData data, IServiceProvider site)
        {
            this.ExtensionData = data;
            this.Extensions = new ObservableAdapterCollection<EntityExtensionModel>(data.Extensions.ToFiltered(x =>
                {
                    BrowsableAttribute ba = x.GetType().GetCustomAttribute<BrowsableAttribute>(required:false);
                    return ba != null ? ba.Browsable : true;
                }),
                x => new EntityExtensionModel((IEntityExtension)x, site));
        }


        public ExtensionData ExtensionData { get; private set; }


        public IEnumerable<EntityExtensionModel> Extensions { get; private set; }

        private EntityExtensionModel _primarySelected;

        public EntityExtensionModel PrimarySelected
        {
            get { return _primarySelected; }
            set
            {
                if (_primarySelected != value)
                {
                    _primarySelected = value;
                    this.OnPropertyChanged("PrimarySelected");
                    if (_primarySelected != null && _primarySelected.Editor != null)
                    {
                        this.EditorVisibility = Visibility.Visible;
                    }
                    else
                    {
                        this.EditorVisibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private Visibility _editorVisibility = Visibility.Collapsed;

        public Visibility EditorVisibility
        {
            get { return _editorVisibility; }
            set
            {
                if (_editorVisibility != value)
                {
                    _editorVisibility = value;
                    this.OnPropertyChanged("EditorVisibility");
                }
            }
        }

        private ObservableCollection<IEntityExtension> _selected = new ObservableCollection<IEntityExtension>();

        public IList<IEntityExtension> Selected
        {
            get { return _selected; }
        }
       
    }

    public class EntityExtensionModel
    {
        public IEntityExtension Extension { get; private set; }

        public EntityExtensionModel(IEntityExtension extension, IServiceProvider site)
        {
            this._site = site;
            this.Extension = extension;

            CategoryAttribute ca = this.Extension.GetType().GetCustomAttribute<CategoryAttribute>(required: false);
            this.Category = ca != null ? ca.Category : WellknownCategoryNames.Misc;

        }

        private IServiceProvider _site;

        private static readonly ImageSource _defaultIcon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/extension.png", UriKind.RelativeOrAbsolute));
        private ImageSource _icon;

        public string Category { get; private set; }

        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    Fantasy.ComponentModel.IconAttribute attr = this.Extension.GetType().GetCustomAttribute<Fantasy.ComponentModel.IconAttribute>(required: false);
                    if (attr != null)
                    {
                        System.Drawing.Image image = attr.Icon;
                        _icon = (ImageSource)(new ImageToBitmapImageConverter()).Convert(image, typeof(ImageSource), null, CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        _icon = _defaultIcon;
                    }
                }
                return _icon;
            }
        }

        private bool _isEditorCreated = false;
        private UIElement _editor;
        public UIElement Editor 
        {
            get
            {
                if (!_isEditorCreated)
                {
                    _isEditorCreated = true;
                    Fantasy.ComponentModel.EditorAttribute attr = this.Extension.GetType().GetCustomAttribute<Fantasy.ComponentModel.EditorAttribute>(required: false);
                    if (attr != null)
                    {
                        _editor = (UIElement)Activator.CreateInstance(attr.EditorType);
                        IObjectWithSite ows = _editor as IObjectWithSite;
                        if (ows != null)
                        {
                            ows.Site = this._site; 
                        }
                        ((IEntityExtensionEditor)_editor).Load(this.Extension);
                    }
                }
                return _editor;

            }

        }


    }


}
