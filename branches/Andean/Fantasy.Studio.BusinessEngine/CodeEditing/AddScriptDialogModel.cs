using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    class AddScriptDialogModel : NotifyPropertyChangedObject, IObjectWithSite
    {
        public AddScriptDialogModel(IServiceProvider site, BusinessPackage package)
        {
            this.Site = site;
            ScriptTemplateRepository repo = this.Site.GetRequiredService<ScriptTemplateRepository>();
            this.Items = repo.Templates;
            this._package = package;

        }

        private BusinessPackage _package;

        public IList<ScriptTemplate> Items { get; set; }

        public IServiceProvider Site { get; set; }


        private ScriptTemplate _selectedItem;

        public ScriptTemplate SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    this.OnPropertyChanged("SelectedItem");
                    if (value != null)
                    {
                        string ext = LongPath.GetExtension(this._selectedItem.DefaultFileName);
                        string newName;
                        if (!string.IsNullOrEmpty(this.Name))
                        {
                            
                            newName = LongPath.GetFileNameWithoutExtension(this.Name);
                        }
                        else
                        {
                            newName = LongPath.GetFileNameWithoutExtension(this._selectedItem.DefaultFileName);
                        }
                        this.Name = GetName(newName, ext);
                    }



                    
                }
            }
        }


        private string GetName(string defaultName, string ext)
        {
            for (int i = 0; ; i++)
            {
                string rs = i == 0 ? defaultName + "." + ext : string.Format("{0}{1}.{2}", defaultName, i, ext);
                if (!this._package.Scripts.Any(s => string.Compare(rs, s.Name, true) == 0))
                {
                    return rs;
                }

            }

        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;

                    this.OnPropertyChanged("Name");
                    
                }
            }
        }


        private void CheckIsValid()
        {
            this.IsValid = this.SelectedItem != null 
                &&!String.IsNullOrWhiteSpace(_name)
                        && _name.IndexOfAny(InvalidChars) < 0 &&
                        !this._package.Scripts.Any(s => string.Equals(_name, s.Name, StringComparison.OrdinalIgnoreCase)); 
        }


        private static char[] InvalidChars = new char[] {'\\', '/', '*', '?', '<', '>', '|'};

        private bool _isValid;

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    this.OnPropertyChanged("IsValid");
                }
            }
        }


    }
}
