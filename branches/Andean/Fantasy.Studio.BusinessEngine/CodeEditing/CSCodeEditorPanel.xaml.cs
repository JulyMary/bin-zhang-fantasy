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
    public partial class CSCodeEditorPanel : UserControl, IObjectWithSite,
    {
        public CSCodeEditorPanel()
        {
            InitializeComponent();
            this.TextDocument = new TextDocument();
        }

        public TextDocument TextDocument
        {
            get { return (TextDocument)GetValue(TextDocumentProperty); }
            private set { SetValue(TextDocumentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextDocument.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextDocumentProperty =
            DependencyProperty.Register("TextDocument", typeof(TextDocument), typeof(CSCodeEditorPanel), new UIPropertyMetadata(null));
    }
}
