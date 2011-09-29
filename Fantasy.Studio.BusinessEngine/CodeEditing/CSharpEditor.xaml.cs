using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Document;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    /// <summary>
    /// Interaction logic for CSCodeEditorPanel.xaml
    /// </summary>
    public partial class CSharpEditor : UserControl, IObjectWithSite
    {
        public CSharpEditor()
        {
            InitializeComponent();
            this.TextDocument = new TextDocument();
        }

        public IServiceProvider Site { get; set; } 

        public TextDocument TextDocument
        {
            get { return (TextDocument)GetValue(TextDocumentPropertyKey.DependencyProperty); }
            private set { SetValue(TextDocumentPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for TextDocument.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey TextDocumentPropertyKey =
            DependencyProperty.RegisterReadOnly("TextDocument", typeof(TextDocument), typeof(CSharpEditor), new UIPropertyMetadata(null));



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CSharpEditor), new UIPropertyMetadata(false));


    }
}
