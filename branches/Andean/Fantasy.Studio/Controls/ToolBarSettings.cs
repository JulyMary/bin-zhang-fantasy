using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using System.Xml.Serialization;

namespace Fantasy.Studio.Controls
{
    public class ToolBarSettings : NotifyPropertyChangedObject
    {
        public ToolBarSettings()
        {
           
        }

        private List<ToolBarSetting> _toolBars = new List<ToolBarSetting>();

        [XmlArray,
        XmlArrayItem(Type = typeof(ToolBarSetting), ElementName = "ToolBar")]
        public List<ToolBarSetting> ToolBars
        {
            get { return _toolBars; }
          
        }

        public ToolBarSetting GetSetting(string id)
        {
            ToolBarSetting rs = this.ToolBars.Find(t => t.ID == id);
            if (rs == null)
            {
                rs = new ToolBarSetting() { ID = id, Band = this.ToolBars.Count, BandIndex = 0, Visible = true, Parent=this };
                this.ToolBars.Add(rs);
            }
            return rs;
        }

        private bool _smallIcon = true;

        public bool SmallIcon
        {
            get { return _smallIcon; }
            set
            {
                if (_smallIcon != value)
                {
                    _smallIcon = value;
                    this.OnPropertyChanged("SmallIcon");
                }
            }
        }

        private bool _showText;

        public bool ShowText
        {
            get { return _showText; }
            set
            {
                if (_showText != value)
                {
                    _showText = value;
                    this.OnPropertyChanged("ShowText");
                }
            }
        }

    }

    public class ToolBarSetting : NotifyPropertyChangedObject
    {
        public string ID { get; set; }

        public int Band { get; set; }

        public int BandIndex { get; set; }

        private bool _visible;

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        [XmlIgnore]
        public ToolBarSettings Parent { get; internal set; }

    }
}
