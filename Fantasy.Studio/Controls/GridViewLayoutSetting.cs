using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Controls;
using System.Windows;
using System.Collections;

namespace Fantasy.Studio.Controls
{
    public class GridViewLayoutSetting
    {

        public GridViewLayoutSetting()
        {
            this.Columns = new List<GridViewColumnLayoutSetting>();
        }

        [XmlArray("columns"),
        XmlArrayItem("column", Type = typeof(GridViewColumnLayoutSetting))]
        public List<GridViewColumnLayoutSetting> Columns { get;  set; }

        public void LoadLayout(GridView view)
        {

            List<GridViewColumn> viewColumns = new List<GridViewColumn>(view.Columns);
            viewColumns.Sort((x, y) =>
            {
                int ix = this.Columns.FindIndex(c => c.Name == (string)x.GetValue(GridViewLayout.ColumnIdProperty));
                if (ix < 0)
                {
                    ix = Int32.MaxValue - ( view.Columns.Count  - view.Columns.IndexOf(x));
                }
                int iy = this.Columns.FindIndex(c => c.Name == (string)y.GetValue(GridViewLayout.ColumnIdProperty));
                if (iy < 0)
                {
                    iy = Int32.MaxValue - (view.Columns.Count - view.Columns.IndexOf(y));
                }
                return Comparer.Default.Compare(ix, iy);
            });

            for (int i = 0; i < view.Columns.Count; i++)
            {
                GridViewColumn column = viewColumns[i];
                int oldIndex = view.Columns.IndexOf(column);
                view.Columns.Move(oldIndex, i);
                GridViewColumnLayoutSetting setting = this.Columns.Find(c => c.Name == (string)column.GetValue(GridViewLayout.ColumnIdProperty));
                if (setting != null)
                {
                    column.Width = setting.Width;
                }
            }
        }

        public void SaveLayout(GridView view)
        {
            this.Columns.Clear();
            foreach (GridViewColumn column in view.Columns)
            {
                GridViewColumnLayoutSetting setting = new GridViewColumnLayoutSetting()
                {
                    Name = (string)column.GetValue(GridViewLayout.ColumnIdProperty),
                    Width = column.Width
                };
                this.Columns.Add(setting);
            }
        }

    }

    public static class GridViewLayout
    {
        public static string GetColumnId(DependencyObject column)
        {
            return (string)column.GetValue(ColumnIdProperty);
        }

        public static void SetColumnId(DependencyObject column, string value)
        {
            column.SetValue(ColumnIdProperty, value);
        }

        public static readonly DependencyProperty ColumnIdProperty = DependencyProperty.RegisterAttached("ColumnId", typeof(string), typeof(GridViewLayout), new PropertyMetadata(ColumnIdPropertyChanged));

        private static void ColumnIdPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            sender.SetValue(ColumnIdProperty, e.NewValue);
        }
    }
}
