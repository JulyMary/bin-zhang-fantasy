using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Syncfusion.Windows.Shared.Controls
{
    public class DragDecorator : Control
    {
        public DragDecorator()
        {
            DefaultStyleKey = typeof(DragDecorator);
        }

        private Border backBorder;

        private Path movePath;

        private Path copyPath;

        private Path impossiblePath;

        private TextBlock descriptionText;

        public string DropDescription
        {
            get { return (string)GetValue(DropDescriptionProperty); }
            set { SetValue(DropDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropDescriptionProperty =
            DependencyProperty.Register("DropDescription", typeof(string), typeof(DragDecorator), new PropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            backBorder = GetTemplateChild("PART_DragDescription") as Border;
            movePath = GetTemplateChild("MovePath") as Path;
            impossiblePath = GetTemplateChild("DragImpossiblePath") as Path;
            descriptionText = GetTemplateChild("DescriptionText") as TextBlock;
            copyPath = GetTemplateChild("CopyPath") as Path;
            base.OnApplyTemplate();
        }

        internal Border BackBorder
        {
            get
            {
                return backBorder;
            }
        }

        internal Path MovePath
        {
            get
            {
                return movePath;
            }
        }

        internal Path ImpossiblePath
        {
            get
            {
                return impossiblePath;
            }
        }

        internal TextBlock Descriptiontext
        {
            get
            {
                return descriptionText;
            }
        }

        internal Path CopyPath
        {
            get
            {
                return copyPath;
            }
        }
    }
}
