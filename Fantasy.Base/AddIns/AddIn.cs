using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;

namespace Fantasy.AddIns
{
    [ContentProperty("Extensions")]
    public class AddIn : DependencyObject
    {
        public AddIn()
        {
            this.SetValue(ResourcesProperty, new ResourceDictionary());
            this.SetValue(ExtensionsProperty, new List<Extension>());
            this.SetValue(ImportProperty, new List<Assembly>());
        }
       
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public string Author
        {
            get { return (string)GetValue(AuthorProperty); }
            set { SetValue(AuthorProperty, value); }
        }



        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public string Version
        {
            get { return (string)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

       // private IList<Assembly> _import = new List<Assembly>();
        public IList<Assembly> Import
        {
            get { return (IList<Assembly>)GetValue(ImportProperty.DependencyProperty); }
        }
      

        public IList<Extension> Extensions
        {
            get { return (IList<Extension>)GetValue(ExtensionsProperty.DependencyProperty); }
        }

        // Using a DependencyProperty as the backing store for Resources.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey ResourcesProperty =
            DependencyProperty.RegisterReadOnly("Resources", typeof(ResourceDictionary), typeof(AddIn), new PropertyMetadata(new ResourceDictionary()));

        // Using a DependencyProperty as the backing store for Extensions.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey ExtensionsProperty =
            DependencyProperty.RegisterReadOnly("Extensions", typeof(IList<Extension>), typeof(AddIn), new PropertyMetadata(new List<Extension>()));

        public static readonly DependencyPropertyKey ImportProperty =
    DependencyProperty.RegisterReadOnly("Import", typeof(IList<Assembly>), typeof(AddIn), new PropertyMetadata(new List<Assembly>()));


        // Using a DependencyProperty as the backing store for Version.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VersionProperty =
            DependencyProperty.Register("Version", typeof(string), typeof(AddIn), new PropertyMetadata(null));



        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(AddIn), new PropertyMetadata(null));



        // Using a DependencyProperty as the backing store for Url.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(AddIn), new PropertyMetadata(null));



        // Using a DependencyProperty as the backing store for Author.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthorProperty =
            DependencyProperty.Register("Author", typeof(string), typeof(AddIn), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(AddIn), new PropertyMetadata(null));


      
    }
}
